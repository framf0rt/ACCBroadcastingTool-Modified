   M S B A M L   `   `   `  ����N  JACCBroadcastingTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:ksBroadcastingTestClient.Autopilot"ksBroadcastingTestClient.Autopilot  5      9    3ksBroadcastingTestClient.Autopilot.AutopilotCarView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:ksBroadcastingTestClient.Autopilot     5      .��5	      �   � ColumnDefinitions  5
   
   �� 5      ��6   �� *PressureWidth+�� 5      $��0.001* �6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      .+�5   
   � 
�������$��0��6G   $��2��6T    � Value 6   �� *
Pressure+6T   �� 5   
   $��3q�6    �� Orientation$ 
Horizontal=�6    $��2��69   $��2��6M   .+�5      �� $��30��6   $
��Bold"�6=   $
��Left=�6O   ��6#   �� *
Position+6O   �� 5      $��60��6    �� TextAlignment$ Center=�6T   $
��Left=�6k   ��6#   �� *
RaceNumber+ �� StringFormat$
 #{0}��6k   �� 5      $��80��6   $ Center=�6\   $
��Left=�6s   ��6#   �� *CurrentDriverName+$
 #{0}��6s   �� 5      $��30��6   $
��Bold"�6N   $
��Left=�6`   ��6#   �� *
Pressure+$ P1��6`   5      �� 5   
   $��1��6B   $��2��6O   ��6   �� *PressureCategories+��5      �� 5      .R�5       �� $ Vertical=�6!   5!      ��5$      �� 5%      .R�5&      �   5'      �� 5(      $��60 �6/   �� 5)      $��60 �6/   .+�5+      �� ��6$   �� *
Category+� 5,      $��1��6   .+�5-      �  � Minimum$ 0Q�6J    � Maximum$ 1Q�6V    6*   �� *WeightedValue+6V   �� 5.      
�������$ Center=�6T   ��6(   �� *Hint+6T   5/      50      51      53      55      56      