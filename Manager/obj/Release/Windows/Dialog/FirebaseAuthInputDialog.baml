   M S B A M L   `   `   `  ���� B  >Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Manager.WindowsManager.Windows  4    .Manager.Windows.Dialog.FirebaseAuthInputDialog   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     b ^SharpVectors.Converters.Wpf, Version=1.7.7.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/  )localclr-namespace:Manager.Windows  Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  _ [MaterialDesignThemes.Wpf, Version=4.4.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  $	��300��$	��450��  � Title$  관리자 인증�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � AllowsTransparency .  � WindowStyle$
 None=�   CustomWindowTitleBarStyle#��[  ��Z� .���  � RowDefinitions �� $��40 ��� $��* �.+��� 
�������$��0q�
�������$��5q�$��2�� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     ShadowDepth$ 0Q�   
BlurRadius$ 10Q�	   Color$	 Black��]� ��.  $��5,5,5,0q�
 � 	Resources
  �� 
TargetType �� Property �� Value��%�   '��       �� " ��.���  � ��$ 10,0,0,0q���  � ��$ Center=���  � ��$
 Left=���  � ��$&  /Resources/font/#esamanru Medium)���  � ��$
 Auto����  � ��
 �������  � ��$ 0q���  � ��
 ���� .+��� $4��.서울특별시북부교육지원청 알리미��$��12(�  Color_Black1#���  �� $��11(�  Color_Gray2#���  $��	20,20,0,0q�$��1��$%��/Resources/font/#esamanru Light)�$��WrapWithOverflow=�$	��Top=�$
��Left=�$	��335���� $G�A사용자가 관리자임을 인증해야 합니다. Firebase URL�� �� $� �� �� $���(Firebase Realtime Database)와 서버 접근 비밀번호를 입력해 주십시오.

해당 작업을 진행하지 않으면 해당 프로그램을 사용할 수 없습니다.���� $ ��Firebase Realtime Database��$��12(�#���  $��	20,97,0,0q�$��1��$%��/Resources/font/#esamanru Light)�$��NoWrap=�$	��Top=�$
��Left=�$	��165���� -    ��Name$ realtimeDatbaseURL��$��23��$��200,91,20,0q�$��1�� �� TextWrapping$ NoWrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$��12(�  MaterialDesignThemeForBlue#
 [   Color_Blue1 �� SelectionBrush# �  �� $��12(�$��Firebase Admin Password��#���  $��
20,131,0,0q�$��1��$%��/Resources/font/#esamanru Light)�$��NoWrap=�$	��Top=�$
��Left=�$	��165���� -   $ firebasePassword��$��23��$��200,125,20,0q�$��1��$ NoWrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$��12(�#
 [ # �  �� $��Right=�$	��Top=�$��60��$��62��$��	0,15,18,0q�$��1����/  )SharpVectors.Converters.SvgImageExtension  *!/Resources/logo/logo_sbdooe.svg+�� -   $��종료��$��Right=�$��	0,0,20,20q�$��1��$��Bottom=�$	��120��$��30��  Color_White1#���    MaterialDesignThemeForGrey#
 [ $%��/Resources/font/#esamanru Light)�$��13(��� -   #���  #
 [ $��확인��$��Right=�$��
0,0,160,20q�$��1��$��Bottom=�$	��120��$��30��$%��/Resources/font/#esamanru Light)�$��13(�#���  #���  �� -    ��Name$ result��$��11(�  
Color_Red1#���  $��20,165,20,0q�$��1��$%��/Resources/font/#esamanru Light)�$
��Wrap=�$	��Top=�