   M S B A M L   `   `   `  ����F  BFootballApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:FootballApp.ClassesFootballApp.Classes  5      &     FootballApp.Controls.TeamControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      1	behaviors!clr-namespace:FootballApp.Classes  5        � DataContext  5	      ��    mainViewModelLocator �� Source# [   �� Path$ TeamViewModel� J� InputBindings 5      �� 5   
   
 �� Key$ Return��5       �� Command 5      �� *GameEnteredCommand+5       � 	Resources 5        GroupedFixtures �� IsLiveGroupingRequested �� Source	 �� GroupDescriptions2  ,System.Windows.Data.PropertyGroupDescription
   PropertyName  GroupedMatches  expanderHeader��%�  &	       &	 �     &	 4    �� 5   
    .  5      �� *UpcomingFixtureList+	 5        5      $

 date��6+     5      $
 competition.name��6+   5      �� 5   
    .  5      �� *PastMatchList+	 5        5      $

 date��6+     5       $
 competition_name��6+   5"      �� 5%   
   .R�5&      �� 
�������$��18(�5(      $
��Bold"�5)      $
��Wrap=�5*      $��Center=�5+      ��5&      �� 5+      5,      5-      .��5/      �  � RowDefinitions 50   
   �� 51      $
��Auto �6   �� 52      $��* �6   .+�55   
   �� .+�56      �� $	��100��57      $��	0,-25,0,0q�58      ��56      �� *
TeamLogo+58      �� 59       �� TextAlignment$ Center=�5:      $��20(�5;      $��10q�5<      $��SemiBold"�5=      ��59      �� *
TeamName+5=      �� 5?       �� Orientation$ 
Horizontal=�6   $��Center=�5@       � Cursor$
 Hand��5A      $��0,-5,0,5q�5B      y�5C      �� *CurrentlyLive+   BooleanToVisibilityConverter �� 	Converter# [  �� FallbackValue$ Hidden����5D      �� 5E       �� 
TargetType" ��6    .�5F      ��  �� Property �@J�Opacity �� Value$ 1Q�68    �� Triggers 5G      P� 5H   "    P� Property �@J�IsMouseOver P� Value . . �5I   &   ��  �@J�Opacity$	 0.7Q�6@   5J   #   5L      .+�5N      �� $��Live ��6    $��16(�5O       $
��Bold"�5P       
���  ��5Q       �� 5R      $	����6      FontAwesome#���  5S       $��10(�5T       $��0,8,0,0q�5U       
���  ��5V       .  (System.Windows.Interactivity.Interaction   Triggers 5W      /  )System.Windows.Interactivity.EventTrigger  5X         	EventName$ MouseLeftButtonUp��6)   .  (System.Windows.Interactivity.TriggerBase   Actions. 5Y      6  0System.Windows.Interactivity.InvokeCommandAction     Command 64   �� *LiveClickedCommand+5Z      5\      5^      � 5_      $��1��6    � ColumnDefinitions 5`      �� 5a      $��5* �6#   �� 5b      $��5* �6#   .+�5e      9� -   5         9�Name$ FixtureExpander��5e       9� 
IsExpanded . $��Upcoming Fixtures��5g      #��[ 5h      
���   �$��0,2,1,0q�5j      .��5k      �� -   5         ��Name$ FixtureList��5k      #  [ 5l      $��0q�5n      ��5m      ��  �� 
GroupStyle 5o      (  "System.Windows.Controls.GroupStyle  5p          ContainerStyle  5q      �� 5r   "   " ��6(   .�5s   &   ��  � �� 5t   *   �� 5u   .   ! �� 
TargetType"! ��6>   .R�5v   2   9�  . ��5w   6   �� 5x   :   $
��Bold"�5y   D   
�������$��16(�5{   D   ��5x   D   �� $
 Name�5{   D   ��5}   6   �� 5~   :   5�   3   5�   /   5�   '   5�   #   5�        5�        5�      �� 5�   "   " ��6(   .�5�   &   ��  � �� 5�   *   �� 5�   .   "! ��6>   .R�5�   2   9�  . ��5�   6   �� 5�   :   $��SemiBold"�5�   @   
���   �$��12(�5�   @   ��5�   D   �� $
 Name�5�   @   ��5�   6   �� 5�   :   " �� SelectionMode$" Single=�5�   ;   $��0q�5�   ;   ��5�   C   �� *Items+# �� SelectedItem# 5�   ;   �� $! DataContext.SelectedFixture�$ �� RelativeSource$ �� *FindAncestor+% �� AncestorType"% >�& �� Mode$& TwoWay=�'   	Behaviors' 5�   >   /   )FootballApp.Classes.ScrollParentWhenAtMax  5�   B    5�   >     5�   B   $  PreviewMouseLeftButtonDown��6Q   . 5�   F      6\   �� $% DataContext.MatchClickedCommand�$ �� *FindAncestor+"% >�5�   C     5�   B   $ MouseDoubleClick��6Q   . 5�   F      6\   �� $( "DataContext.FixtureSelectedCommand�$ �� *FindAncestor+"% >�5�   C   ��5�   >   �� 5�   B   .R�5�   F   �� 
���   �$	��1.5q�5�   E   ( �� CornerRadius$( 5��5�   E   ) �� Padding$
) 0,10q�5�   E   ._�5�   J   �  5�   N   �� 5�   R   $
��4.5* �6c   �� 5�   R   $��1* �6c   �� 5�   R   $
��4.5* �6c   .+�5�   N   � $	��200��6S   $	��5,0q�5�   K   .+�5�   R   �� $��SemiBold"�5�   T   $ Center=�5�   T   $��WrapWithOverflow=�5�   T   ��5�   \   �� *	home_name+5�   T   5�   O   �� 5�   N   $��1��6Y   .+�5�   R   �� $��SemiBold"�5�   T   $ Center=�5�   T   ��5�   \   �� *time+5�   T   5�   O   � 5�   N   $	��200��6S   $	��5,0q�5�   K   $��2��5�   K   .+�5�   R   �� $��SemiBold"�5�   T   $ Center=�5�   T   $��WrapWithOverflow=�5�   T   ��5�   \   �� *	away_name+5�   T   5�   O   5�   K   5�   G   5�   C   5�   ;   5�   3   5�   /   5�   '   5�   #   5�      5�      5�      9� 5�      $��Past Matches��5�      #��[ 5�      
���   �$��1,2,0,0q�5�      $��1��5�       5�      �� *
IsExpanded+* �� ElementName$* FixtureExpander��.��5�      �� -   5        $ 	MatchList��5�      #  [ 5�      $��0q�5�      ��5�      ��  5�        5�        5�      �� 5�   "   " ��6(   .�5�   &   ��  � �� 5�   *   �� 5�   .   "! ��6>   .R�5�   2   9�  . ��5�   6   �� 5�   :   $
��Bold"�5�   D   
�������$��16(�5�   D   ��5�   D   �� $
 Name�5�   D   ��5�   6   �� 5�   :   5�   3   5�   /   5�   '   5�   #   5�        5�        5�      �� 5�   "   " ��6(   .�5�   &   ��  � �� 5�   *   �� 5�   .   "! ��6>   .R�5�   2   9�  . ��5�   6   �� 5�   :   $��SemiBold"�5   @   
���   �$��12(�5  @   ��5�   D   �� $
 Name�5  @   ��5  6   �� 5  :   $" Single=�5  ;   $��0q�5  ;   ��5  C   �� *Items+# 5  ;   �� $ DataContext.SelectedMatch�$ �� *FindAncestor+"% >�$& TwoWay=�' 5	  >     5
  B    5  >     5  B   $  PreviewMouseLeftButtonDown��6Q   . 5  F      6\   �� $% DataContext.MatchClickedCommand�$ �� *FindAncestor+"% >�5  C     5  B   $ MouseDoubleClick��6Q   . 5  F      6\   �� $&  DataContext.MatchSelectedCommand�$ �� *FindAncestor+"% >�5  C   ��5  >   �� 5  B   .R�5  F   �� 
���   �$	��1.5q�5  E   $( 5��5  E   $
) 0,10q�5  E   ._�5  J   �  5  N   �� 5  R   $
��4.5* �6c   �� 5  R   $��1* �6c   �� 5  R   $
��4.5* �6c   .+�5   N   � $	��200��6S   $	��5,0q�5!  K   .+�5"  R   �� $��SemiBold"�5#  T   $ Center=�5$  T   $��WrapWithOverflow=�5%  T   ��5"  \   �� *	home_name+5%  T   5&  O   �� 5'  N   $��1��6Y   .+�5(  R   �� $��SemiBold"�5)  T   $ Center=�5*  T   ��5(  \   �� *score+5*  T   5+  O   � 5,  N   $	��200��6S   $	��5,0q�5-  K   $��2��5.  K   .+�5/  R   �� $��SemiBold"�50  T   $ Center=�51  T   $��WrapWithOverflow=�52  T   ��5/  \   �� *	away_name+52  T   53  O   54  K   55  G   56  C   58  ;   5:  3   5;  /   5=  '   5>  #   5@     5B     5C     5D     5F     5G     