   M S B A M L   `   `   `  ����B  >Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Manager.WindowsManager.Windows  5   	   4    .Manager.Windows.Dialog.FirebaseAuthInputDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   b ^SharpVectors.Converters.Wpf, Version=1.7.7.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/  5   	   )localclr-namespace:Manager.Windows  5   	   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5   	   _ [MaterialDesignThemes.Wpf, Version=4.4.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   $	��300��5   	   $	��450��6     � Title$  관리자 인증��5   	    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � AllowsTransparency .  � WindowStyle$
 None=�5   	      CustomWindowTitleBarStyle#��[  5   	   ��5   	   Z� .��5      �  � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   �� 
�������$��0q�5      
�������$��5q�5!      $��2��5"       J� Effect 5$      3  -System.Windows.Media.Effects.DropShadowEffect  5%         ShadowDepth$ 0Q�5&         
BlurRadius$ 10Q�5'      	   Color$	 Black��5(      5*      ]� 5,   
   ��.  $��5,5,5,0q�6*   
 � 	Resources
 5-       �� 
TargetType �� Property �� Value��%}  '��       �� 5/      " ��6   .�50      ��  � ��$ 10,0,0,0q�6/   �� 51       � ��$ Center=�6:   �� 52       � ��$
 Left=�67   �� 53       � ��$&  /Resources/font/#esamanru Medium)�63   �� 54       � ��$
 Auto��6.   �� 55       � ��
 �����63   �� 56       � ��$ 0q�68   �� 57       � ��
 ���� 63   58      59      .+�5;      �� $4��.서울특별시북부교육지원청 알리미��5<      $��12(�5=        Color_Black1#���  5>      5?      �� 5A   
   $��11(�5B        Color_Gray2#���  5C      $��	20,20,0,0q�5D      $��1��5E      $%��/Resources/font/#esamanru Light)�5F      $��WrapWithOverflow=�5G      $	��Top=�5H      $
��Left=�5I      $	��335��5K      �� 5L      $G�A사용자가 관리자임을 인증해야 합니다. Firebase URL��6    �� 5M      $� ��6    �� 5N      $���(Firebase Realtime Database)와 서버 접근 비밀번호를 입력해 주십시오.

해당 작업을 진행하지 않으면 해당 프로그램을 사용할 수 없습니다.��6   5O      �� 5Q   
   $ ��Firebase Realtime Database��5R      $��12(�5S      #���  5T      $��	20,97,0,0q�5U      $��1��5V      $%��/Resources/font/#esamanru Light)�5W      $��NoWrap=�5X      $	��Top=�5Y      $
��Left=�5Z      $	��165��5[      �� 5]   
   -   5         ��Name$ realtimeDatbaseURL��5^      $��23��5_      $��200,91,20,0q�5`      $��1��5a       �� TextWrapping$ NoWrap=�5b      $	��Top=�5c      $%��/Resources/font/#esamanru Light)�5d      $��12(�5e        MaterialDesignThemeForBlue#
 [ 5f        Color_Blue1 �� SelectionBrush# �  5g      �� 5i   
   $��12(�5j      $��Firebase Admin Password��5k      #���  5l      $��
20,131,0,0q�5m      $��1��5n      $%��/Resources/font/#esamanru Light)�5o      $��NoWrap=�5p      $	��Top=�5q      $
��Left=�5r      $	��165��5s      �� 5u   
   -   5        $ firebasePassword��5v      $��23��5w      $��200,125,20,0q�5x      $��1��5y      $ NoWrap=�5z      $	��Top=�5{      $%��/Resources/font/#esamanru Light)�5|      $��12(�5}      #
 [ 5~      # �  5      �� 5�   
   $��Right=�5�      $	��Top=�5�      $��60��5�      $��62��5�      $��	0,15,18,0q�5�      $��1��5�      ��5�      /  )SharpVectors.Converters.SvgImageExtension  *!/Resources/logo/logo_sbdooe.svg+5�      �� 5�   
   -   5        $��종료��5�      $��Right=�5�      $��	0,0,20,20q�5�      $��1��5�      $��Bottom=�5�      $	��120��5�      $��30��5�        Color_White1#���  5�        MaterialDesignThemeForGrey#
 [ 5�      $%��/Resources/font/#esamanru Light)�5�      $��13(�5�      �� 5�   
   -   5        #���  5�      #
 [ 5�      $��확인��5�      $��Right=�5�      $��
0,0,160,20q�5�      $��1��5�      $��Bottom=�5�      $	��120��5�      $��30��5�      $%��/Resources/font/#esamanru Light)�5�      $��13(�5�      #���  5�      #���  5�      �� 5�   
   -   5         ��Name$ result��5�      $��11(�5�        
Color_Red1#���  5�      $��20,165,20,0q�5�      $��1��5�      $%��/Resources/font/#esamanru Light)�5�      $
��Wrap=�5�      $	��Top=�5�      5�      5�      