   M S B A M L   `   `   `  ����   MissingFiles V2     MissingFiles_V2.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -   $��400.319��$��824.227�����	#00FFFFFF  � 
ResizeMode$  NoResize=� � WindowStyle$
 None=�$y�Visible=� � WindowStartupLocation$ CenterScreen=� J� Opacity$ 1Q� � AllowsTransparency .  � SizeToContent$ WidthAndHeight=� � Icon$. (Nishad2m8-Hologram-Dock-My-documents.ico�� � Title$ Missing Files V2�� � 	Resources    NewButtonStyle	 �� 
TargetType
 �� Triggers �� Binding �� Value �� Property �� Value��%�   &	        �� "	 ��
 ��  �� *Keres+ true.a���  � {� .  ��  �� *Keres+	 false.a���  � {� .  � ContextMenu �� 
���   �
������$��1,1,1,1q�.@�m� -   $��Transparent�� m� IsCheckable .  m� 	IsChecked .  m� -   $��Close��.���� 
��� ���$��4,4,4,4q� �� CornerRadius$ 20,20,20,20��
�����f�._��� 
������ $��6,4,4,6q�.+�� $��
10,5,10,10q�$	��336�� � ColumnDefinitions �� $
��110* ��� $
��191* ��� $	��86* ��� $
��168* ��� $	��89* ��� $
��142* � � RowDefinitions �� $	��40* ��� $	��34* ��� $	��31* ��� $	��32* ��� $
��159* ��� $	��40* �.+��� $��Directory Path: ��$��Center=�$��Right=�$��16��$��	0,12,5,12q�$��81���� -    ��Name$ startdir��$��1��$��0��$��10,10q�$��4������  �� Path$ BaseDir� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� -   $��Browse��$��5��$��0��$��10,10q�#��[  �� $��	DB_host: ��$��0��$��1��$��Center=�$��Right=�$��16��$��5q�$��49���� $��1��$��1��$��5q����� $	 Srv�$ TwoWay=�$ PropertyChanged=��� $��DB Name ��$��2��$��1��$��Center=�$��Right=�$��16��$��5q�$��54���� $��3��$��1��$��5q����� $ DB_name�$ TwoWay=�$ PropertyChanged=��� $��Table name: ��$��1��$��Center=�$��Right=�$��16��$��0,12,4,6q�$��67��$��4���� $��5��$��1��$��5q����� $ DB_Table�$ TwoWay=�$ PropertyChanged=��� $��4��$��
10,10,10,0q�$��3������ *Errorlist_dir+$ OneWay=�$ PropertyChanged=��� $��4��$��
10,10,10,0q�$��3��$��3������ *Errorlist_db+$ OneWay=�$ PropertyChanged=��� $'��!Missing in folder (it is in DB): ��$��1��$��3��$��Center=�$��Right=�$��16��$��5q�$	��154���� $��Found files in folder: ��$��3��$��3��$��Center=�$��Right=�$��16��$��0,8,5,8q�$	��116���� $��2��$��3��$��Center=�$��center=�$��25��$��10q�$
��auto������ $ Missing�$ OneWay=�$ PropertyChanged=��� $��4��$��3��$��Center=�$��center=�$��25��$��10q�$
��auto������ $ Found�$ OneWay=�$ PropertyChanged=��� -   $��Start��$��5��$��10q�#��[  �� $��Rows in DB: ��$��2��$��5��$��Center=�$��Right=�$��16��$��
0,14,10,10q�$
��auto���� $��0��$��3��$��5��$��Center=�$
��Left=�$��28��$��5,5,0,0q�$
��auto��$��25���� $��Port number: ��$��0��$��2��$��Center=�$��Right=�$��16��$��0,8,5,7q�$
��auto���� $��1��$��2��$��5q����� $
 Port�$ TwoWay=�$ PropertyChanged=��� $��LogFile path: ��$��2��$��2��$��Center=�$��Right=�$��16��$��5q�$
��auto���� -   $ LogFile��$��3��$��2��$��5q�$��2������ *
LogFileDir+$ TwoWay=�$ PropertyChanged=��� -   $��Log File dir.��$��5��$��2��$��5q�#��[  � .+��� $;��5Using: mouse leftclick: move window; rightclick: menu��$��Center=�$
��Wrap=�$��21��$��Bottom=��� -	    ��Name$ elerut��$�� ��$��Center=�$��Right=�$
��auto��$��150,5q�$
��auto��$��2��$
��Wrap=�