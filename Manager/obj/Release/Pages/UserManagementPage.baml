   M S B A M L   `   `   `  ���� B  >Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Manager.PagesManager.Pages  &     Manager.Pages.UserManagementPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     _ [MaterialDesignThemes.Wpf, Version=4.4.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  b ^SharpVectors.Converters.Wpf, Version=1.7.7.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/  'localclr-namespace:Manager.Pages  .���   � ColumnDefinitions  �� $��* ��� $
��auto �.+�� -    �Name$ userInfoPanel��$y�	Collapsed=�$��0,0,-2,0q�$��1��$	��250��$��Right=�   Color_Blue2#���   .+��� .���  � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��* ��� $
��auto �.+��� $��사용자 정보��$	��Top=�  Color_White1#���  $��15(�$8��2/Manager;component/Resources/font/#esamanru Medium)�$��	20,35,0,0q�$
��Left=��� -   $��	0,10,10,0q�$��25��$��Right=�$	��Top=�$��25��.���� .+��� $��14��$��14��$��Center=�$��0q�$��Center=���/  )SharpVectors.Converters.SvgImageExtension  *)'/Resources/icon/ic_cancel_for_white.svg+����  �� 
TargetType" ��.���  �� Property � �� �� Value$ Right=���  � ��$ 40���� '  !System.Windows.Shell.WindowChrome  �@ IsHitTestVisibleInChrome . ��  � ��  Color_Black1# �  ��  � ��$ 0q���  � ��
 ���� ��  � �� ��  �� 
TargetType" ��.R��� #��z��._��� $��Center=�$��Center=�(  "SharpVectors.Converters.SvgViewbox  $��13��$��18��$
��Left=�$��	15,19,0,0q�   Source$" /Resources/icon/ic_email.svg?�$	��Top=�$��1���� $��사용자 이메일��$	��Top=�#���  $��12(�$7��1/Manager;component/Resources/font/#esamanru Light)�$��	40,19,0,0q�$
��Left=�$��1���� -    ��Name$ userInfoForEmail��$
��Auto��$��
15,40,15,0q�	 �� TextWrapping$
	 Wrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$��11(�  MaterialDesignTheme
 � 	Resources#
 [   Color_White2 �� SelectionBrush# �  #���  #���   �� 
CaretBrush# �  $��1��  $��18��$��18��$
��Left=�$��	15,15,0,0q�$# /Resources/icon/ic_office.svg?�$	��Top=�$��2���� $��학원/교습소 이름��$	��Top=�#���  $��12(�$7��1/Manager;component/Resources/font/#esamanru Light)�$��	39,18,0,0q�$
��Left=�$��2���� -   $ userInfoForOfficeName��$
��Auto��$��
15,40,15,0q�$
	 Wrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$��11(�#
 [ # �  #���  #���  # �  $��2�� �� 
IsReadOnly .   $��18��$��18��$
��Left=�$��	14,15,0,0q�$! /Resources/icon/ic_user.svg?�$	��Top=�$��3���� $��설립자 이름��$	��Top=�#���  $��12(�$7��1/Manager;component/Resources/font/#esamanru Light)�$��	38,18,0,0q�$
��Left=�$��3���� -   $ userInfoForOfficeFounder��$
��Auto��$��
14,40,15,0q�$
	 Wrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$��11(�#
 [ # �  #���  #���  # �  $��3�� .   $��18��$��18��$
��Left=�$��	14,15,0,0q�$) #/Resources/icon/ic_registration.svg?�$	��Top=�$��4���� $��등록번호��$	��Top=�#���  $��12(�$7��1/Manager;component/Resources/font/#esamanru Light)�$��	38,18,0,0q�$
��Left=�$��4���� -   $ userInfoForOfficeNumber��$
��Auto��$��
14,40,15,0q�$
	 Wrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$��11(�#
 [ # �  #���  #���  # �  $��4���� $��최근 로그인��$	��Top=�#���  $��11(�$7��1/Manager;component/Resources/font/#esamanru Light)�$��	16,15,0,0q�$
��Left=�$��5���� -    ��Name$ userInfoForDate��$	��Top=�#���  $��10(�$7��1/Manager;component/Resources/font/#esamanru Light)�$��	77,16,0,0q�$
��Left=�$��5��� -   $ warningPanel��$��6��$��15,10,15,40q�$��Bottom=� �� $��* ��� $��* �.+�  $��18��$��18��$
��Left=�$��0,0,0,5q�$$ /Resources/icon/ic_warning.svg?�$	��Top=��� #���  $��10(�$
��Wrap=�$��경고!��$7��1/Manager;component/Resources/font/#esamanru Light)�$��	25,3,0,10q��� -	   $ userInfoForErrorLog��$��Bottom=�#���  $��10(�$
��Wrap=�$7��1/Manager;component/Resources/font/#esamanru Light)�$��0q�$��1��� $��6��$��Bottom=�$��35��$��0,0,0,0q�  �� $��* ��� $��* �.+��� -
   $
��Auto��$��0q�.����  �� Orientation$ 
Horizontal=�.+��� $��14��$��14��$��Center=�$��0,0,10,0q�$��Center=���  */Resources/icon/ic_save.svg+�� $��저장��$��Center=�#���  $��12(�$7��1/Manager;component/Resources/font/#esamanru Light)�$��Center=����� " ��.���  � ��$% /Resources/font/#esamanru Light)���  � ��$ Right=���   �@ IsHitTestVisibleInChrome . ��  � ��# �  ��  � ��$ 0q���  � ��  Color_Blue3# �  ��  � �� �� " ��.R��� #��z��._��� $��Center=�$��Center=� �� Triggers P�  P� Property �@J�IsMouseOver P� Value . . ���  � ��  Color_Blue1# �  P�  � �� . . ���  � ��# �   �� -   $
��Auto��$��1��.���� $ 
Horizontal=�.+��� $��14��$��14��$��Center=�$��0,0,10,0q�$��Center=���  */Resources/icon/ic_delete.svg+�� $��사용자 삭제��$��Center=�#���  $��12(�$7��1/Manager;component/Resources/font/#esamanru Light)�$��Center=����� " ��.���  � ��$% /Resources/font/#esamanru Light)���  � ��$ Right=���   �@ IsHitTestVisibleInChrome . ��  � ��# �  ��  � ��$ 0q���  � ��# �  ��  � �� �� " ��.R��� #��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ���  � ��# �  P�  � �� . . ���  � ��# �   � -    �� $��85 ��� $��* ��� $
��auto �.+�  $��25��$��25��$
��Left=�$��	25,30,0,0q�$, &/Resources/icon/ic_user_management.svg?�$	��Top=��� $��사용자 관리��$	��Top=�#���  $��19(�$8��2/Manager;component/Resources/font/#esamanru Medium)�$��	55,32,0,0q�$
��Left=�  $��18��$��18��$��Right=�$��
0,36,373,0q�$# /Resources/icon/ic_search.svg?�$	��Top=��� -   $ 
searchUser��$��23��$��0,33,240,29q�$	 NoWrap=�$��Center=�$%��/Resources/font/#esamanru Light)�$��11(�$��	0,0,0,1.5q�  MaterialDesignThemeForBlue#
 [ # �  $��Right=�$	��125��#���  �� -   $��searchTypeComboBox��$��0,34,128,29q�$��Center=�$%��/Resources/font/#esamanru Light)�$��11(�$	��103��$��	0,0,0,1.5q�#���  #���  #
 [  �� 
IsReadOnly . $��Right=� �� SelectedIndex$ 0��.@��� $��사용자 이메일�����	#FF636363�� $��학원/교습소 이름�����	#FF636363�� $��설립자 이름�����	#FF636363�� $��등록번호�����	#FF636363�� $��최근 로그인 날짜�����	#FF636363�� -   $��sortComboBox��$��
0,34,15,29q�$��Center=�$%��/Resources/font/#esamanru Light)�$��11(�$	��104��$��	0,0,0,1.5q�#���  #���  #
 [  . $��Right=�.@��� $��오름차순 정렬�����	#FF636363�� $��내림차순 정렬�����	#FF636363�� -    ��Name$ userListView��#
 [ $��1��$��2�� �� View �� .P��� $��사용자 이메일�� �� Width$
 auto�� �� DisplayMemberBinding �� *email+�� $��학원/교습소 이름��$
 auto�� �� *	regName+�� $��설립자 이름��$
 auto�� �� *
regFounder+�� $��등록번호��$
 auto�� �� *regNum+�� $��최근 로그인 날짜��$
 auto�� �� *	loginDate+� -    �Name$ searchTaskProgressbae��$y�	Collapsed=����	#FF0074FF$��2��$��5����Z� ��Z� 