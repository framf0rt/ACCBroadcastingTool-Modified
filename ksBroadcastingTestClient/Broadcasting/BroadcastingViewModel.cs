using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ksBroadcastingNetwork;
using ksBroadcastingNetwork.Structs;
using log4net;

namespace ksBroadcastingTestClient.Broadcasting
{
    public class BroadcastingViewModel : KSObservableObject
    {
        public ObservableCollection<CarViewModel> Cars { get; } = new ObservableCollection<CarViewModel>();
        public TrackViewModel TrackVM { get => Get<TrackViewModel>(); private set => Set(value); }
        public KSRelayCommand RequestFocusedCarCommand { get; }

        private List<BroadcastingNetworkProtocol> _clients = new List<BroadcastingNetworkProtocol>();

        public BroadcastingViewModel()
        {
            RequestFocusedCarCommand = new KSRelayCommand(RequestFocusedCar);
        }

        public int focusedIndex = 0;
        public string activeCamera;
        public string activeCameraSet;
        public Dictionary<string, List<string>> cameras = new Dictionary<string, List<string>>();
        private static readonly ILog log = LogManager.GetLogger(typeof(BroadcastingViewModel));


        private void RequestFocusedCar(object obj)
        {
            var car = obj as CarViewModel;
            if (car != null)
            {
                foreach (var client in _clients)
                {
                    // mssing readonly check, will skip this as the ACC client has to handle this as well
                    client.SetFocus(Convert.ToUInt16(car.CarIndex));
                    focusedIndex = car.CarIndex;
                }
            }
        }
        public void RequestFocusedCar(int value)
        {
            foreach (var client in _clients)
            {
                // mssing readonly check, will skip this as the ACC client has to handle this as well
                focusedIndex += value;
                if (focusedIndex < 0)
                {
                    focusedIndex = 0;
                }
                client.SetFocus(Convert.ToUInt16(focusedIndex));
            }

        }

        private void RequestHudPageChange(string requestedHudPage)
        {
            foreach (var client in _clients)
            {
                // mssing readonly check, will skip this as the ACC client has to handle this as well
                client.RequestHUDPage(requestedHudPage);
            }
        }


        private void RequestCameraChange(string camSet, string camera)
        {
            if (string.IsNullOrWhiteSpace(camSet))
            {
                camSet = activeCameraSet;
            }
            if (string.IsNullOrWhiteSpace(camera))
            {
                camera = activeCamera;
            }
            foreach (var client in _clients)
            {
                // mssing readonly check, will skip this as the ACC client has to handle this as well
                client.SetCamera(camSet, camera);
            }
            activeCamera = camera;
            activeCameraSet = camSet;
        }

        public void CameraChange()
        {
            try
            {
                cameras.TryGetValue(activeCameraSet, out var cams);

                var c = cams.IndexOf(activeCamera);

                if (c + 1 == cams.Count())
                {
                    c = 0;
                }
                else
                {
                    c += 1;
                }

                RequestCameraChange(activeCameraSet, cams[c]);
            }
            catch (Exception e)
            {
                log.Error($"{e.Message} {e.StackTrace} {e.Data} {e.InnerException}");
            }
        }

        public void CameraSetChange()
        {
            try
            {
                var camSet = cameras.Select(x => x.Key).ToList();

                var c = camSet.IndexOf(activeCameraSet);

                if (c + 1 == camSet.Count())
                {
                    c = 0;
                }
                else
                {
                    c += 1;
                }
                cameras.TryGetValue(camSet[c], out var cams);
                RequestCameraChange(camSet[c], cams.FirstOrDefault());
            }
            catch (Exception e)
            {
                log.Error($"{e.Message} {e.StackTrace} {e.Data} {e.InnerException}");
            }
        }

        internal void RegisterNewClient(ACCUdpRemoteClient newClient)
        {
            if (newClient.MsRealtimeUpdateInterval > 0)
            {
                // This client will send realtime updates, we should listen
                newClient.MessageHandler.OnTrackDataUpdate += MessageHandler_OnTrackDataUpdate;
                newClient.MessageHandler.OnEntrylistUpdate += MessageHandler_OnEntrylistUpdate;
                newClient.MessageHandler.OnRealtimeUpdate += MessageHandler_OnRealtimeUpdate;
                newClient.MessageHandler.OnRealtimeCarUpdate += MessageHandler_OnRealtimeCarUpdate;
            }

            _clients.Add(newClient.MessageHandler);
        }

        private void MessageHandler_OnTrackDataUpdate(string sender, TrackData trackUpdate)
        {
            if (TrackVM?.TrackId != trackUpdate.TrackId)
            {
                if (TrackVM != null)
                {
                    TrackVM.OnRequestCameraChange -= RequestCameraChange;
                    TrackVM.OnRequestHudPageChange -= RequestHudPageChange;
                }

                cameras = trackUpdate.CameraSets;
                cameras.Remove("setVR");    
                TrackVM = new TrackViewModel(trackUpdate.TrackId, trackUpdate.TrackName, trackUpdate.TrackMeters);
                TrackVM.OnRequestCameraChange += RequestCameraChange;
                TrackVM.OnRequestHudPageChange += RequestHudPageChange;
            }


            // The track cams may update in between
            TrackVM.Update(trackUpdate);
        }

        private void MessageHandler_OnEntrylistUpdate(string sender, CarInfo carUpdate)
        {
            CarViewModel vm = Cars.SingleOrDefault(x => x.CarIndex == carUpdate.CarIndex);
            if (vm == null)
            {
                vm = new CarViewModel(carUpdate.CarIndex)
                {
                    CarClass = carUpdate.CarClass
                };

                Cars.Add(vm);
            }

            vm.Update(carUpdate);
        }

        private void MessageHandler_OnRealtimeUpdate(string sender, RealtimeUpdate update)
        {
            if (TrackVM != null)
                TrackVM.Update(update);

            foreach (var carVM in Cars)
            {
                carVM.SetFocused(update.FocusedCarIndex);
                focusedIndex = update.FocusedCarIndex;
            }

            activeCamera = update.ActiveCamera;
            activeCameraSet = update.ActiveCameraSet;



            try
            {
                if (TrackVM?.TrackMeters > 0)
                {
                    var sortedCars = Cars.OrderBy(x => x.SplinePosition).ToArray();
                    for (int i = 1; i < sortedCars.Length; i++)
                    {
                        var carAhead = sortedCars[i - 1];
                        var carBehind = sortedCars[i];
                        var splineDistance = Math.Abs(carAhead.SplinePosition - carBehind.SplinePosition);
                        while (splineDistance > 1f)
                            splineDistance -= 1f;

                        carBehind.GapFrontMeters = splineDistance * TrackVM.TrackMeters;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            if(update.SessionType == RaceSessionType.Race && update.Phase == SessionPhase.PreSession)
            {
                foreach (var item in Cars)
                {
                    item.ResetSession();
                }
            }

        }

        private void MessageHandler_OnRealtimeCarUpdate(string sender, RealtimeCarUpdate carUpdate)
        {
            var vm = Cars.FirstOrDefault(x => x.CarIndex == carUpdate.CarIndex);
            if (vm == null)
            {
                // Oh, we don't have this car yet. In this implementation, the Network protocol will take care of this
                // so hopefully we will display this car in the next cycles
                return;
            }

            vm.Update(carUpdate);
        }



    }
}
