   M S B A M L   `   `   `  ����F  BFootballApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:FootballApp.ControlsFootballApp.Controls  5   	       FootballApp.MainWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null;controls,http://metro.mahapps.com/winfx/xaml/controls  5   	   +uc"clr-namespace:FootballApp.Controls  5   	   Gsdk9http://schemas.microsoft.com/netfx/2009/xaml/presentation    5      $
��1080��5
      $	��640��5        � WindowState$  	Maximized=�5   	    � WindowStyle$
 None=�5   	    � Title$ 
LiveScores��5   	   (  "MahApps.Metro.Controls.MetroWindow   	TitleCaps .     AccentColorBrush   	GlowBrush# �   5   	    � Icon$" /Resources/football-2-48.ico��5   	    � DataContext 5   	   ��   mainViewModelLocator �� Source# [  �� Path$ MainViewModel�.��5      ]� .+�5   
   �� 
���   �$��0,1,0,0q�5      $��Bottom=�5      ._�5      �� ��5      �� 5      .R�5      � 	 � ColumnDefinitions	 5      �� 5   "   $
��1.5* �63   �� 5   "   $��7* �63   �� 5   "   $
��1.5* �63   5       5!      .@�5#      �� .��5$      �� 
 �� Orientation$
 
Horizontal=�6!   $��5,0,0,0q�5%   !   .+�5&      �� $��Matches Loaded: ��6$   �� 5'      ��6$   �� *
MatchCount+5(      5)      �� 5*      $��2��6    $��Right=�5+       .��5,      �� $��0,0,5,0q�6=   ��6    �� *TimeUpdated+6=   5-      5.      5/      � 51   
    � RowDefinitions 52   
   �� 53      $��* �6   	 55   
   �� 56      $	��.4* �6   $	��400��6+   6<   �� 57      $
��Auto �6   6.   �� 58      $��* �6   $	��600��6*   6;   .+�5;   
   �� $��1��6   $��4��6+   
���   �$��Stretch=�6H   $��Center=�6d   )   #FootballApp.Controls.CountryControl  5>   
   $��0��6   =  7MahApps.Metro.Controls.MetroAnimatedSingleRowTabControl  5A   
   $��2��5B   4   -  'MahApps.Metro.Controls.TabControlHelper   IsUnderlined .    
Transition$
 Down=�5D   4   $��0,50,0,0q�5E   4    �� SelectedIndex 5A   4   �� *
TabIndex+ �� Mode$ TwoWay=�.@�5G      �� $��Match��6   y�5H      �� *	MatchData+   BooleanToVisibilityConverter �� 	Converter# [  �� FallbackValue$ Hidden��.��5I      '   !FootballApp.Controls.EventControl  5J      �� 5L      $��	Standings��6   y�5M      �� *LeagueSelectedBool+# [ $ Hidden��.��5N      +   %FootballApp.Controls.StandingsControl  5O      �� 5P      ��6   �� *
TeamName+y�5Q      �� *TeamSelectedBool+# [ $ Hidden��.��5R      &    FootballApp.Controls.TeamControl  5S      5T      � 5W   
   $��3��6   y�5X      �� *LoadingData+# [ $ Hidden��.+�5Y      -   'FootballApp.Controls.LoadingOverlayView  5Z      5[      5\      5]      