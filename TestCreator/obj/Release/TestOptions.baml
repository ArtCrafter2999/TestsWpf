   M S B A M L   `   `   `  ���� F  BTestCreator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TestCreatorTestCreator      TestCreator.TestOptions   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:TestCreator       � Title$  Настройки��$	��224��$	��350��.��� .+��� $"��Название теста:��$��	10,10,0,0q�$	��Top=�$
��Left=��� $��111,15,10,0q�$	��Top=�����  �� Path$ 
From.Title� �� UpdateSourceTrigger$ PropertyChanged=��� $(��"Максмиальный балл:��$��	10,40,0,0q�$	��Top=�$
��Left=��� $��141,45,148,0q�$	��Top=����� $ From.MaxPoints�$ PropertyChanged=��� $1��+Перетасовывать вопросы��$��	10,71,0,0q�$	��Top=�$
��Left=� d� 	IsChecked �� $ RandomQuestions��� $�� ��$��OK��$��	0,0,10,10q�$��Right=�$��Bottom=�$��25��$��70������ $ OptionsClose�$ PropertyChanged=��� $!��На прохождение��$��	10,91,0,0q�$	��Top=�$
��Left=��� $��
Часов��$��
51,117,0,0q�$	��Top=�$
��Left=��� $��10,122,279,0q�$	��Top=����� $ 
From.Hours�$ PropertyChanged=��� $��
Минут��$��157,117,0,0q�$	��Top=�$
��Left=��� $��116,122,173,0q�$	��Top=����� $ From.Minutes�$ PropertyChanged=��� $��Секунд��$��266,117,0,0q�$	��Top=�$
��Left=��� $��224,122,65,0q�$	��Top=����� $ From.Seconds�$ PropertyChanged=�