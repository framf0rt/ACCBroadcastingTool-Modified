   M S B A M L   `   `   `  ����N  JACCBroadcastingTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:ksBroadcastingTestClient.Broadcasting%ksBroadcastingTestClient.Broadcasting  5      ;    5ksBroadcastingTestClient.Broadcasting.TrackCameraView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Elocal3clr-namespace:ksBroadcastingTestClient.Broadcasting     5      -   5        $��TrackCamControl��5	      .��5
      ��   �� Orientation$  
Horizontal=�6   .+�5   
   �� ��6   �� *
CameraSets+��5      �� 5      .R�5      �� $  
Horizontal=�6!   5      ��5      �� 5      .R�5      �� $  Vertical=�6!   .+�5      �� $��1q�6!   
���   ���6I   �� *CamsetBackground+._�5      �� $��70��6p    �� TextAlignment$ Center=�6{   $
��Bold"�6�   $��5,2,5,2q�6�   ��6(   �� *CameraSetName+��6G   �� *CamsetForeground+6�   5      �� 5      ��6'   �� *	Cameras+��5      �� 5   "   .R�5   &   �� $  Vertical=�61   5    #   ��5#      �� 5$   "   .R�5%   &   �� $��1q�6-   
���   ���6U   �� *CameraBackground+ J� InputBindings 5&   *   d� 5'   .    d� Gesture$ 	LeftClicka�6;    d� MouseAction$ 	LeftClicke�6�    �� Command 6O   ��  �� ElementName$ TrackCamControl�� �� Path$, &DataContext.RequestCameraSwitchCommand� �� CommandParameter 6�   �� 6�   ._�5)   *   �� $��90��6x   $ Center=�6�   $��5,2,5,2q�6�   ��64   �� *
CameraName+��6P   �� *CameraForeground+6�   5*   '   5,   #   5.      50      52      54      �� 58   
   $  Vertical=�6   $	��150��6,   $
��Left=�68   $	��Top=�6S   $��10, 0, 0, 0q�6k   .+�5:      �� $��1q�6   
���   �
���   �._�5;      �� $��HudPages��6   
��� ���$ Center=�6@   $
��Bold"�6W   $��2,2,2,2q�6i   5<      �� 5?      ��6   �� *
HudPages+��5@      �� 5A      .R�5B      �� $  Vertical=�6%   5C      ��5F      �� 5G      .R�5H      �� $��1q�6!   
���   ���6I   �� *HudPageBackground+ 5I      d� 5J   "   $ LeftDoubleClicka�6/    6I   �� $ TrackCamControl��$) #DataContext.RequestHudSwitchCommand� 6�   �� ._�5L      �� $��90��6j   $ Center=�6u   $��5,2,5,2q�6�   ��6(   �� *	Caption+��6A   �� *HudPageForeground+6�   5M      5O      5Q      5S      5T      5U      