   M S B A M L   `   `   `  ����  IQD_UI_Library'    !IQD_UI_Library.IQD_LoadingControl   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � WindowStyle$
  None=�5   	    � AllowsTransparency$
 True��5   	   
������  � Topmost$
 True��5
   	   $	��150��5   	   $	��150��6    � WindowStartupLocation$ CenterScreen=�5   
    � 
ResizeMode$ NoResize=�5   	    � 	Resources 5         RotateAnimation g� RepeatBehavior �� 
TargetName �� TargetProperty	 W� From	
 W� To g� Duration��%{  &	        �� 5   
   $ Forever��6-   .��5      W� $ Rotate1��6   $ Angle�6>   	 05      
 3606'   $ 0:0:10D�60   W� 5      $ Rotate2��6   $ Angle�6>   	 1205      
 4806)   $ 0:0:10D�62   W� 5      $ Rotate3��6   $ Angle�6>   	 2405      
 6006)   $ 0:0:10D�62   W� 5      $ Ellipse1��6   $ Opacity�6?   	 05      
 16'   $ 0:0:2D�6.   W� 5      $ Ellipse2��6   $ Opacity�6?   	 05      
 16'   $ 0:0:2D�6.   W� 5      $ Ellipse3��6   $ Opacity�6?   	 05      
 16'   $ 0:0:2D�6.   5       5!       � Triggers 5#      :� 5$   
    :� RoutedEvent$ Window.Loaded��6   .[�5%      �� #��[  6   5&      .��5)      � $��Center=�6   $��Center=�6&   .+�5*   
   �� $	��100��6   $	��100��6   .+�5,      A� -   5         A�Name$ Ellipse1��5,      $��30��6(   $��60��63    �� Left$ 35��6?   
 �� Top$ 20��6P   ���	#FF4CAF50z�5-      �� 5.      -   5        Rotate1�� �� Angle$ 0Q�5.   7    �� CenterX$ 15Q�6A    �� CenterY$ 30Q�6N   50      A� 53      -   5        $ Ellipse2��53      $��30��6(   $��60��63   $ 35��6?   $ 20��6P   ���	#FF03A9F4z�54      �� 55      -   5        Rotate2��$	 120Q�55   7   $ 15Q�6C   $ 30Q�6P   57      A� 5:      -   5        $ Ellipse3��5:      $��30��6(   $��60��63   $ 35��6?   $ 20��6P   ���	#FFFFC107z�5;      �� 5<      -   5        Rotate3��$	 240Q�5<   7   $ 15Q�6C   $ 30Q�6P   5>      5?      5@      5A      