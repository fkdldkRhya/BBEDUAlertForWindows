   M S B A M L   `   `   `  ���� B  >Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Manager.PagesManager.Pages  #    Manager.Pages.MessageSendPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     _ [MaterialDesignThemes.Wpf, Version=4.4.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  b ^SharpVectors.Converters.Wpf, Version=1.7.7.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/  'localclr-namespace:Manager.Pages  .��� -     � RowDefinitions  �� $��85 ��� $��1 ��� $��* �.+�(  "SharpVectors.Converters.SvgViewbox  $��18��$��26��$
��Left=�$��	24,34,0,0q�   Source$) #/Resources/icon/ic_send_message.svg?�$	��Top=��� $	��Top=�   Color_Black1#���   $��19(�$&�� /Resources/font/#esamanru Medium)�$��	58,32,0,0q�$
��Left=��� $�공지사항 전송 ��]� $��1��  Color_White2#���  � $��2�� � ColumnDefinitions �� $	��250 ��� $��1 ��� $��* �.+��   �� $��70 ��� $��1 ��� $��* �.+�� .+��� $��12(�  Color_Gray4#���  $��	10,10,0,0q�$
��Wrap=�$	��Top=�$&�� /Resources/font/#esamanru Medium)�$
��Left=��� $�
메시지 �� �� $�전송 대상���� -   $��officeTypeComboBox��$��	10,0,0,10q�$��Bottom=�$%��/Resources/font/#esamanru Light)�$��11(�$	��113��$��	0,0,0,1.5q�#���   #���     MaterialDesignThemeForBlue � 	Resources# [  �� 
IsReadOnly . $
��Left=�$��28��.@��� $��학원�����	#FF636363�� $��	교습소�����	#FF636363�� -    ��Name$ allSelectCheckbox��$��전체선택��$%��/Resources/font/#esamanru Light)�$��12(�#���   # [ $��Right=�$��Center=����#1C7BFF$��
0,11,11,41q��� -    ��Name$ allunSelectButton��
�������$��	0,0,10,10q�$��Bottom=�$��25��$%��/Resources/font/#esamanru Light)�$��11(�$��선택해제�����#1C7BFF���#1C7BFF$��81��$��Right=�# [ ]� $��1��#���  �� -    ��Name$ officeSelectListView��$��2��# [ ���� .R�� $	��230�� �� $��* ��� $
��auto �  �� $��* ��� $��* �.+��� $
��Wrap=�$��26��$%��/Resources/font/#esamanru Light)�$��11(�$��0,0,5,0q�$��CharacterEllipsis=�$	��Top=����� *
officeName+���� *
txtColor+�� $
��Wrap=�$��26��$%��/Resources/font/#esamanru Light)�$��10(�  Color_Gray2#���  $��	0,3,4.7,0q�$��CharacterEllipsis=�$	��Top=�$��1������ *officeRegNum+]� $��1��#���  �� $��2��.���   �� $
��auto ��� $
��auto �.+�  $��16��$��24��$
��Left=�$" /Resources/icon/ic_title.svg?�$	��Top=�$��	14,67,0,0q��� $��15(�
���   �$��15,15,150,0q�$
��Wrap=�$	��Top=�$&�� /Resources/font/#esamanru Medium)��� $�
메시지 ���� $�내용���� -    ��Name$ messages��$
��Auto��$	��200��$��
15,95,49,0q�$	��Top=�$%��/Resources/font/#esamanru Light)�$��12(�
���   �	 �� SelectionBrush	 �#1C7BFF# [ �� -   $ messageSendButton��
�������$��
0,15,49,30q�$	��Top=�$��28��$%��/Resources/font/#esamanru Light)�$��11(�$��메시지 전송�����#1C7BFF���#1C7BFF$	��130��$��Right=�# [ $��1���� -   
 ��Name$
 title��$��23��$��
82,60,49,0q� �� TextWrapping$ NoWrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$��12(�
���   �	 �#1C7BFF# [ �� $��제목��$��13(�
���   �$��	45,66,0,0q�$
��Wrap=�$	��Top=�$%��/Resources/font/#esamanru Light)�$
��Left=�