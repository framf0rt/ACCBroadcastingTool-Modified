using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ksBroadcastingNetwork;
using ksBroadcastingNetwork.Structs;
using log4net;
using log4net.Config;

namespace ksBroadcastingTestClient.Broadcasting
{
    public class ReplayControlViewModel : KSObservableObject
    {
        public KSRelayCommand PlayLiveReplay { get; }
        public int LiveReplaySecondsBack { get => Get<int>(); private set => Set(value); }
        public int LiveReplaySecondsPlaytime { get => Get<int>(); private set => Set(value); }
        public int CurrentSessionTime { get => Get<int>(); private set => Set(value); }

        private List<ACCUdpRemoteClient> _clients = new List<ACCUdpRemoteClient>();
        public ObservableCollection<BroadcastingEventViewModel> BroadcastingEvents { get; } = new ObservableCollection<BroadcastingEventViewModel>();

        private static readonly ILog log = LogManager.GetLogger(typeof(ReplayControlViewModel));

        public ReplayControlViewModel()
        {
            PlayLiveReplay = new KSRelayCommand(OnLiveReplay);
            LiveReplaySecondsBack = 120;
            LiveReplaySecondsPlaytime = 10;
        }

        private void OnStopReplay()
        {
 
        }


        private void OnHighlightReplay(BroadcastingEvent evt, float requestedStartTime, float durationSeconds)
        {
            try
            {
                foreach (var client in _clients)
                {
                    client.MessageHandler.RequestInstantReplay(requestedStartTime, durationSeconds * 1000.0f, evt.CarId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void OnLiveReplay(object obj)
        {
            try
            {
             
                var secondsBack = Convert.ToInt32(obj);
                var requestedStartTime = CurrentSessionTime - (secondsBack * 1000);

                if (secondsBack == 0)
                {
                    foreach (var client in _clients)
                    {
                        client.MessageHandler.RequestInstantReplay(CurrentSessionTime, 0);
                    }
                }
                

                foreach (var client in _clients)
                {
                    client.MessageHandler.RequestInstantReplay(requestedStartTime, secondsBack * 1000.0f);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        internal void RegisterNewClient(ACCUdpRemoteClient newClient)
        {
            newClient.MessageHandler.OnRealtimeUpdate += MessageHandler_OnRealtimeUpdate;
            newClient.MessageHandler.OnBroadcastingEvent += MessageHandler_OnBroadcastingEvent;
            _clients.Add(newClient);
        }

        private void MessageHandler_OnRealtimeUpdate(string sender, ksBroadcastingNetwork.Structs.RealtimeUpdate update)
        {
         
            CurrentSessionTime = Convert.ToInt32(update.SessionTime.TotalMilliseconds);
        }

        private void MessageHandler_OnBroadcastingEvent(string sender, BroadcastingEvent evt)
        {
            if (evt.Type == BroadcastingCarEventType.LapCompleted)
            {
                return;
            }
            BroadcastingEvents.Insert(0, new BroadcastingEventViewModel(evt, OnHighlightReplay));
            string s = $"Type: {evt.Type} Car number: {evt.CarData.RaceNumber} Time: {evt.TimeMs}";
            log.Info("Type: " + evt.Type + " " + evt.CarId);
            while (BroadcastingEvents.Count > 30)
            {
                BroadcastingEvents.Remove(BroadcastingEvents.Last());
            }
            //if (evt.Type == BroadcastingCarEventType.Accident)
            //{
            //    BroadcastingEvents.Insert(0, new BroadcastingEventViewModel(evt, OnHighlightReplay));
            //    string s = $"Type: {evt.Type} Car number: {evt.CarData.RaceNumber} Time: {evt.TimeMs}";
            //    log.Info("Type: " + evt.Type + " " + evt.CarId);
            //    while (BroadcastingEvents.Count > 30)
            //    {
            //        BroadcastingEvents.Remove(BroadcastingEvents.Last());
            //    }
            //}
        }
    }
}
