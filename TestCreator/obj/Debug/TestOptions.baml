   M S B A M L   `   `   `  ����F  BTestCreator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TestCreatorTestCreator  5   	       TestCreator.TestOptions   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:TestCreator     5   	     � Title$  Настройки��5   	   $	��224��6   $	��350��6(   .��5	      � .+�5
   
   �� $"��Название теста:��6   $��	10,10,0,0q�6*   $	��Top=�6=   $
��Left=�6U   �� 5   
   $��111,15,10,0q�6X   $	��Top=�6m   ��6   ��  �� Path$ 
From.Title� �� UpdateSourceTrigger$ PropertyChanged=�6m   �� 5   
   $(��"Максмиальный балл:��6   $��	10,40,0,0q�6-   $	��Top=�6@   $
��Left=�6X   �� 5   
   $��141,45,148,0q�6\   $	��Top=�6r   ��6   �� $ From.MaxPoints�$ PropertyChanged=�6r   �� 5   
   $1��+Перетасовывать вопросы��6   $��	10,71,0,0q�6_   $	��Top=�6r   $
��Left=�6�    d� 	IsChecked 64   �� $ RandomQuestions�6�   �� 5   
   $�� ��6\   $��OK��6p   $��	0,0,10,10q�6   $��Right=�6�   $��Bottom=�6�   $��25��6�   $��70��6�   ��6   �� $ OptionsClose�$ PropertyChanged=�6�   �� 5   
   $!��На прохождение��6   $��	10,91,0,0q�6)   $	��Top=�6<   $
��Left=�6T   �� 5   
   $��
Часов��6   $��
51,117,0,0q�6    $	��Top=�64   $
��Left=�6L   �� 5   
   $��10,122,279,0q�6Y   $	��Top=�6o   ��6   �� $ 
From.Hours�$ PropertyChanged=�6o   �� 5   
   $��
Минут��6   $��157,117,0,0q�6    $	��Top=�65   $
��Left=�6M   �� 5   
   $��116,122,173,0q�6Z   $	��Top=�6q   ��6   �� $ From.Minutes�$ PropertyChanged=�6q   �� 5   
   $��Секунд��6   $��266,117,0,0q�6!   $	��Top=�66   $
��Left=�6N   �� 5   
   $��224,122,65,0q�6Z   $	��Top=�6p   ��6   �� $ From.Seconds�$ PropertyChanged=�6p   5      5      