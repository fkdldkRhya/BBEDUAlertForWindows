   M S B A M L   `   `   `  ���� B  >Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Manager.WindowsManager.Windows  N JRHYA.Network.Layout, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullV>clr-namespace:RHYA.Network.Layout;assembly=RHYA.Network.LayoutRHYA.Network.Layout      Manager.Windows.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:Manager.Windows     _ [MaterialDesignThemes.Wpf, Version=4.4.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  b ^SharpVectors.Converters.Wpf, Version=1.7.7.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/  O
RhyaLayout>clr-namespace:RHYA.Network.Layout;assembly=RHYA.Network.Layout  -     � Title$C  =서울특별시북부교육지원청 알리미 (관리자용)��$	��650��$
��1000��$	��450��$
��1000�� � WindowStartupLocation$ CenterScreen=�   CustomWindowTitleBarStyle#��[  .��� -    � RowDefinitions �� $��35 ��� $��55 ��� $��2 ��� $��* ��� $��20 �.+�]� ��.    Color_White1#���   � 	Resources  �� 
TargetType �� Property �� Value'  !System.Windows.Shell.WindowChrome  Color_Black1 �� 
TargetType �� Triggers	 P� Property
 P� Value  Color_White2  Color_White3��%�  '��       '�� �    �� " ��.���  � ��$) #/Resources/font/#Malgun Gothic Bold)���  � ��$
 Bold"���  � ��$ Right=���  � ��$ 40����   �@ IsHitTestVisibleInChrome . ��  � ��# �  ��  � ��$ 0q���  � ��
 ���� ��  � �� �� " ��.R��� #��z��._��� $��Center=�$��Center=� P� 	 �@J�IsMouseOver
 . . ���  � ��# �  P� 	 � ��
 . . ���  � ��# �  �� " ��.���  � ��$ 5,0,0,0q���  � ��$ Center=���  � ��$
 Left=���  � ��$&  /Resources/font/#esamanru Medium)���  � ��$
 Auto����  � ��
 �������  � ��$ 0q���  � ��
 ���� .+��� -   $��13(�.���� .+��� $��9��$��9��$��Center=�$��0q�$��Center=���/  )SharpVectors.Converters.SvgImageExtension  */Resources/icon/ic_cancel.svg+�� -   $��16(�.���� .+�(  "SharpVectors.Converters.SvgViewbox  -     Name$  buttonMaxAndMinimizedImage��$��9��$��9��$
��Left=�   Source$% /Resources/icon/ic_maximize.svg?�$��Center=��� -   $��13(�.���� .+��� $��11��$��11��$��Center=�$��0q�$��Center=���  *!/Resources/icon/ic_minimize.svg+�� $B��<서울특별시북부교육지원청 - 평생교육건강과��$��11(�#���  $
��Left=�]� $��2��#���  � $��1��.+��� $��40��$��40��$
��Left=�$��15,9,0,0q�$	��Top=���  *!/Resources/logo/logo_sbdooe.svg+�� $+��%서울북부교육지원청 알리미��$��63,6,0,0q�$	��Top=�$
��Left=�$��14(�#���  $&�� /Resources/font/#esamanru Medium)��� $5��/Seoul Bukbu District Office of Education Alerts��$��	65,23,0,0q�$	��Top=�$
��Left=�  Color_Gray2#���  $��10(�$%��/Resources/font/#esamanru Light)��� -    ��Name$ pVersion��$��	0,0,15,10q�$��Bottom=�$��Right=�#���  $��10(�$%��/Resources/font/#esamanru Light)� �� TextAlignment$ Right=�� $��3�� � ColumnDefinitions �� $	��200 ��� $��3 ��� $��* �.+�� $��0�� �� $��21 ��� $��3* ��� $��21 ��� $	��200 �.+�� $��0��  Color_Blue2#���  .+��� $��13��$��13��$��Right=�$��0,0,70,0q�$��Center=���  */Resources/icon/ic_menu.svg+�� $��관리 메뉴��$��Center=�$��Right=�#���  $��11(�$%��/Resources/font/#esamanru Light)�$��0,0,15,0q��� -    ��Name$ sideMenuListView��  MaterialDesignThemeForBlue# [ $��1������ .R���  �� Orientation$ 
Horizontal=�.+�  $��5,0,10,0q�$��25��$��16��$��Center=� (� Stretch$ Uniform=� �� *
svgImage+�� $��Center=�$
��Wrap=�$	��100��$8��2/Manager;component/Resources/font/#esamanru Medium)�$��12(����� *title+���� *color+� $��2��#���  .+��� $��13��$��13��$��Right=�$��	0,0,100,0q�$��Center=���  */Resources/icon/ic_sync.svg+�� $��서버 정보 동기화��$��Center=�$��Right=�#���  $��11(�$%��/Resources/font/#esamanru Light)�$��0,0,15,0q�� $��3��.+��� $��마지막 동기화��$	��Top=�$
��Left=�#���  $��12(�$%��/Resources/font/#esamanru Light)�$��	10,10,0,0q��� -	   $ syncTime��$	��Top=�  Color_Gray4#���  $��10(�$%��/Resources/font/#esamanru Light)�$��
13,34,13,0q��� -
    ��Name$ syncStartButton��#���  # [ $��	동기화��$��
15,62,15,0q�$	��Top=�$	��168��$��30��$%��/Resources/font/#esamanru Light)�$��13(� 	 Color_Blue1#��� 	 #��� 	 �� $ ��파일 데이터 동기화��$	��Top=�#���  $��11(�$%��/Resources/font/#esamanru Light)�$��
0,107,15,0q�$��Right=�  -   $ syncResultImage1��$��13��$��13��$
��Left=�$��15,106,0,81q�$" /Resources/icon/ic_check.svg?�$��Center=��� $+��%학원, 교습소 데이터 동기화��$	��Top=�#���  $��11(�$%��/Resources/font/#esamanru Light)�$��
0,127,15,0q�$��Right=�  -   $ syncResultImage2��$��13��$��13��$
��Left=�$��15,126,0,61q�$" /Resources/icon/ic_check.svg?�$��Center=��� $#��사용자 데이터 동기화��$	��Top=�#���  $��11(�$%��/Resources/font/#esamanru Light)�$��
0,145,15,0q�$��Right=�  -   $ syncResultImage3��$��13��$��13��$
��Left=�$��15,144,0,43q�$" /Resources/icon/ic_check.svg?�$��Center=��� $#��메시지 데이터 동기화��$	��Top=�#���  $��11(�$%��/Resources/font/#esamanru Light)�$��
0,165,15,0q�$��Right=�  -   $ syncResultImage4��$��12��$��13��$
��Left=�$��15,164,0,24q�$" /Resources/icon/ic_check.svg?�$��Center=�� -    �Name$ syncTaskProgressbar��$��3��$	��Top=�$��
17,92,17,0q����	#FF0074FF��Z� ��Z� ]� $��1��#���  � $��2��.+� � -   $��MyFrame��  � NavigationUIVisibility$ Hidden=�� -    �Name$ dialogYesOrNoGridPanel��$��4��$y�Hidden=�$��1�� �� $��* ��� $	��550 ��� $��* �.+��� $��3��
���   � J� Opacity$ 0Q�*  $RHYA.Network.Layout.DialogForYesOrNO  -     Name$ dialogYesOrNoPanel��$��Center=�$��1��$
��50,0q�� -   $ dialogOKGridPanel��$��4��$y�Hidden=�$��1�� �� $��* ��� $	��550 ��� $��* �.+��� $��3��
���   �$ 0Q�%  RHYA.Network.Layout.DialogForOK  -     Name$ dialogOKPanel��$��Center=�$��1��$
��50,0q�� -   $! dialogCreateOfficeGridPanel��$y�Hidden=�$��4��$��1�� �� $��* ��� $	��570 ��� $��* �.+��� $��3��
���   �$ 0Q�/  )RHYA.Network.Layout.DialogForOfficeCreate  -     Name$ dialogCreateOfficePanel��$��Center=�$��1��$
��50,0q�� -   $  dialogMessageInfoGridPanel��$y�Hidden=�$��4��$��1�� �� $��* ��� $	��600 ��� $��* �.+��� $��3��
���   �$ 0Q�.  (RHYA.Network.Layout.DialogForMessageInfo  -     Name$ dialogMessageInfoPanel��$��Center=�$��1��$
��50,0q�� -   $$ dialogMessageFileInfoGridPanel��$y�Hidden=�$��4��$��1�� �� $��* ��� $	��600 ��� $��* �.+��� $��3��
���   �$ 0Q�+  %RHYA.Network.Layout.DialogForFileList  -     Name$  dialogMessageFileInfoPanel��$��Center=�$��1��$
��50,0q�� -   $! loadingProgressbarGridPanel��$��4��$y�Hidden=�$��1�� �� $��* ��� $	��550 ��� $��* �.+��� $��3��
���   �$ 0Q�1	  +RHYA.Network.Layout.LoadingProgressbarPanel	  -    	 Name$ loadingProgressbarPanel��$��Center=�$��1��$
��50,0q�� $��4��#��� 	 .+��� $/��)© 2022 CHOI SI HUN. All rights reserved.��#���  $��9(�$%��/Resources/font/#esamanru Light)�$��0,0,10,0q�$ Right=�$��Center=�$��Right=��� $��항목 개수��$	��Top=�$
��Left=�#���  $��9(�$%��/Resources/font/#esamanru Light)�$��10,0,0,0q��� $��1��$��59,5,0,5q�$
��Left=�
��������� -    ��Name$ 	ItemCount��$	��Top=�$
��Left=�#���  $��9(�$%��/Resources/font/#esamanru Light)�$��68,0,0,0q�$��20��