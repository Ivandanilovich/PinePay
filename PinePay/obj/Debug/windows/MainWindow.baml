   M S B A M L   `   `   `  ����B  >PinePay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:PinePayPinePay  5   	       PinePay.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:PinePay     5   	     � Title$  PinePay��5   	   $	��500��6   $	��700��6&    � Icon$ ../favicon/favicon.ico��5	   	   ��5      �� 5   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ 	#FF0E0E0E��6   � 5      $ 	#FF3E3E3E��6    � Offset$ 1Q�6-   5      .��5      � .+�5   
   �  � ShowGridLines .   � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $	��140 �6#   �� 5      $	��160 �6#   �� 5      $	��140 �6   �� 5       $��* �6   .+�5#   
   �� -   5        	 ��Name$	 Login��5%      $��2��5#      $��2��5$      $��
Войти��5&      $��18(�5'      $��10q�5)      G� 5-   
   -   5        
 G�Name$
 Password��5/      $��2��5-      $��1��5.      ���	#FFDDDDDD$��18(�51      $��10q�52      �� 54   
   $!��Введите пароль��6   $��1��55      $��1��56      $��Center=�57      $��Bottom=�58      ���	#FFDDDDDD$��18(�5:      5<      �� 5>   
   -   5        $	 Import��5@      $��Right=�5>      $	��Top=�5?      $-��'Импортировать данные��5A      $��10q�5B      $	��230��5C      $��18(�5D      6   �� 5G   
   $
��Left=�6   $	��Top=�5H      $��PinePay��5I      $��25(�5J      $��10q�5K      ���	#FFDDDDDD5L      5M      5P      