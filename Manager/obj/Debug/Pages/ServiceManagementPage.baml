   M S B A M L   `   `   `  ����B  >Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Manager.PagesManager.Pages  5	      )    #Manager.Pages.ServiceManagementPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      b ^SharpVectors.Converters.Wpf, Version=1.7.7.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/  5      'localclr-namespace:Manager.Pages  5	        N� Title$  ServiceManagementPage��5      .��5      � -   5         � RowDefinitions 5   
   �� 5      $��85 �6   �� 5      $��1 �6   �� 5      $��* �6   .+�5   
   (  "SharpVectors.Converters.SvgViewbox  $��18��5      $��20��5      $
��Left=�5      $��	24,34,0,0q�5         Source$/ )/Resources/icon/ic_service_management.svg?�5      $	��Top=�5      �� 5   
   $��서비스 관리��5      $	��Top=�5         Color_Black1#���   5       $��19(�5!      $8��2/Manager;component/Resources/font/#esamanru Medium)�5"      $��	54,32,0,0q�5#      $
��Left=�5$      ]� 5&   
   $��1��6     Color_White2#���  5'      � 5)   
   $��2��5*       5,      �� 5-      $
��auto �6    �� 5.      $��* �6    .+�51      �� $����해당 메뉴는 서울북부지원교육청 서비스에 대한 설정을 변경 할 수 있습니다. 주의! 해당 설정을 잘못 변경하면 일반 사용자 클라이언트가 서비스를 이용하지 못할 수도 있습니다. 따라서 해당 설정은 관리자 이외에는 수정을 하지않는 것을 권고드립니다.��52      $	��Top=�53      #���   54      $��13(�55      $��WrapWithOverflow=�56      $7��1/Manager;component/Resources/font/#esamanru Light)�57      $��
20,15,20,0q�58      � 5:      $��1��6   .+�5;        $��18��5<      $��18��5=      $
��Left=�5>      $��	21,21,0,0q�5?      $  /Resources/icon/ic_key.svg?�5@      $	��Top=�5A      �� 5C      $��	인증키��5D      $��13(�5E      
���   �$��	47,23,0,0q�5G      $
��Wrap=�5H      $	��Top=�5I      $%��/Resources/font/#esamanru Light)�5J      $
��Left=�5K      �� 5M      -   5         ��Name$ 	accessKey��5N      $��23��5O      $��
122,18,0,0q�5P       �� TextWrapping$ NoWrap=�5Q      $	��Top=�5R      $%��/Resources/font/#esamanru Light)�5S      $��12(�5T      $��	0,0,0,1.5q�5U   )     MaterialDesignThemeForBlue � 	Resources# [ 5V      
���   � �� SelectionBrush �#1C7BFF$
��Left=�5Y      $	��237��5Z      �� 5\      -   5         ��Name$ serviceAccessComboBox��5]      $��
122,90,0,0q�5^      $	��Top=�5_      $%��/Resources/font/#esamanru Light)�5`      $��11(�5a      $	��120��5b      $��	0,0,0,1.5q�5c      #���   5d      #���   5e      # [ 5f       �� 
IsReadOnly . $
��Left=�5h      .@�5k      �� $��접근 허용��6#   ���	#FF63636363   �� 5l      $��접근 거부��6#   ���	#FF63636363   5m      �� 5o      $��서비스 접근 여부��5p      $��13(�5q      
���   �$��	21,95,0,0q�5s      $
��Wrap=�5t      $	��Top=�5u      $%��/Resources/font/#esamanru Light)�5v      $
��Left=�5w      $��96��5x      �� 5z      -   5        	 ��Name$	 
saveButton��5{        Color_White1#���  5}      # [ 5~      $��저장��5�      $��
21,172,0,0q�5�      $	��Top=�5�      $	��110��5�      $��30��5�      $%��/Resources/font/#esamanru Light)�5�      $��13(�5�        Color_Blue1#���  5�      #���  5�      $
��Left=�5�      60   �� 5�      $��클라이언트 버전��5�      $��13(�5�      
���   �$��	21,60,0,0q�5�      $
��Wrap=�5�      $	��Top=�5�      $%��/Resources/font/#esamanru Light)�5�      $
��Left=�5�      $��91��60   �� 5�      -   5        $ clientVersion��5�      $��23��5�      $��
122,55,0,0q�5�      $ NoWrap=�5�      $	��Top=�5�      $%��/Resources/font/#esamanru Light)�5�      $��12(�5�      $��	0,0,0,1.5q�5�      # [ 5�      
���   � �#1C7BFF$
��Left=�5�      $	��237��5�      5�      5�      5�      5�      