   M S B A M L   `   `   `  ���� B  >Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Manager.PagesManager.Pages  )    #Manager.Pages.ServiceManagementPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     b ^SharpVectors.Converters.Wpf, Version=1.7.7.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/  'localclr-namespace:Manager.Pages    N� Title$  ServiceManagementPage��.��� -    � RowDefinitions �� $��85 ��� $��1 ��� $��* �.+�(  "SharpVectors.Converters.SvgViewbox  $��18��$��20��$
��Left=�$��	24,34,0,0q�   Source$/ )/Resources/icon/ic_service_management.svg?�$	��Top=��� $��서비스 관리��$	��Top=�   Color_Black1#���   $��19(�$8��2/Manager;component/Resources/font/#esamanru Medium)�$��	54,32,0,0q�$
��Left=�]� $��1��  Color_White2#���  � $��2�� �� $
��auto ��� $��* �.+��� $����해당 메뉴는 서울북부지원교육청 서비스에 대한 설정을 변경 할 수 있습니다. 주의! 해당 설정을 잘못 변경하면 일반 사용자 클라이언트가 서비스를 이용하지 못할 수도 있습니다. 따라서 해당 설정은 관리자 이외에는 수정을 하지않는 것을 권고드립니다.��$	��Top=�#���   $��13(�$��WrapWithOverflow=�$7��1/Manager;component/Resources/font/#esamanru Light)�$��
20,15,20,0q�� $��1��.+�  $��18��$��18��$
��Left=�$��	21,21,0,0q�$  /Resources/icon/ic_key.svg?�$	��Top=��� $��	인증키��$��13(�
���   �$��	47,23,0,0q�$
��Wrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$
��Left=��� -    ��Name$ 	accessKey��$��23��$��
122,18,0,0q� �� TextWrapping$ NoWrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$��12(�$��	0,0,0,1.5q�  MaterialDesignThemeForBlue � 	Resources# [ 
���   � �� SelectionBrush �#1C7BFF$
��Left=�$	��237���� -    ��Name$ serviceAccessComboBox��$��
122,90,0,0q�$	��Top=�$%��/Resources/font/#esamanru Light)�$��11(�$	��120��$��	0,0,0,1.5q�#���   #���   # [  �� 
IsReadOnly . $
��Left=�.@��� $��접근 허용�����	#FF636363�� $��접근 거부�����	#FF636363�� $��서비스 접근 여부��$��13(�
���   �$��	21,95,0,0q�$
��Wrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$
��Left=�$��96���� -   	 ��Name$	 
saveButton��  Color_White1#���  # [ $��저장��$��
21,172,0,0q�$	��Top=�$	��110��$��30��$%��/Resources/font/#esamanru Light)�$��13(�  Color_Blue1#���  #���  $
��Left=��� $��클라이언트 버전��$��13(�
���   �$��	21,60,0,0q�$
��Wrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$
��Left=�$��91���� -   $ clientVersion��$��23��$��
122,55,0,0q�$ NoWrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$��12(�$��	0,0,0,1.5q�# [ 
���   � �#1C7BFF$
��Left=�$	��237��