   M S B A M L   `   `   `  ����  FootballApp)    #FootballApp.Controls.WelcomeControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � DataContext  5      ��    mainViewModelLocator �� Source# [   �� Path$ EventsViewModel�.��5	      � y�6   �� *NoMatchSelected+   BooleanToVisibilityConverter �� 	Converter# [  �� FallbackValue$ Hidden�� � ColumnDefinitions 5
   
   �� 5      $
��1.5* �6   �� 5      $��7* �6   �� 5      $
��1.5* �6    � RowDefinitions 5   
   �� 5      $
��1.5* �6   �� 5      $
��8.5* �6   .+�5   
   �� $	����6   $��0��5       �� TextAlignment$ Center=�5      $��72(�5        AccentColorBrush2#���  5        FontAwesome#���  5      �� 5   
   $��
LiveScores��6   $��1��5      $
��Bold"�5      $ Center=�5      $��72(�5      #���  5       �� 5!   
   $	����6   $��2��5"      $ Center=�5#      $��72(�5$      #���  5%      #���  5&      �� 5(   
   $��1��6   $��1��5)      $��	0,100,0,0q�5*      .+�5+      �� $��48(�5,      $��0,0,0,25q�5-      $ Center=�5.      #���  5/      ��5+      �� *
Greeting+5/      �� 50      $8��2Please select a game from the left to get started!��6   $��32(�51      $ Center=�52      $
��Wrap=�53      #���  54      55      56      57      