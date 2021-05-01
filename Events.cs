using System;
using System.Timers;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Construct
{
	// Здесь хранятся события, отвечающие за анимацию и ответ на действия пользователя 		*В РАЗРАБОТКЕ*
	partial class MainForm
	{
		// Переменная для проверки активности панели
		internal static bool act;
		// Перемнная для определения режима заполнения задачи
		internal static bool compl;
		// Переменная для запоминания активной панели
		internal static Panel actP;
		internal static Panel actP2;
		// Переменная для запоминания номера начального дня
		internal static int beginDay;
		// Переменная для запоминания номера начальной задачи
		internal static int beginCase;
		// Переменные для запоминания начальных координат панели (для теста)
		internal static int actX;
		internal static int actY;
		// Переменные для запоминания начальных координат на панели
		internal static int startX;
		internal static int startY;
		// Событие зажатия кнопки мыши на панели, отвечает за присвоение к активной панели, которая используется далее
		internal static void MouseDown_Case(object sender, MouseEventArgs e)
		{
			// Проверяем нажатие на кнопку
			if (e.Button == MouseButtons.Left && (sender as TextBox) == null && (sender as MaskedTextBox) == null && !compl)
			{
				// Отображаем кнопку "удалить"
				labDelete.Visible = true;
				// Запоминаем начальное положение курсора для перемещения задачи
				startX = e.X;
				startY = e.Y;
				// Запоминаем панель и ее координаты, в условии проверяется нажатие на саму панель или ее составляющих
				if ((sender as Panel) != null)		actP = (sender as Panel);
				else if ((sender as Label) != null)	actP = (Panel)(sender as Label).Parent;
				// Запоминаем номер начального дня
				beginDay = actP.Parent.TabIndex;
				// Удаляем задачу из класса календаря
				year[1].listMonth[days[beginDay].date.Month - 1].listDay[days[beginDay].date.Day - 1].cases.RemoveAt(days[beginDay].panCase.IndexOf(actP));		// <<< ###
				// Удаляем элемент из начальной колекции
				days[beginDay].panCase.Remove(actP);
				// Добавляем задачу на форму, для возможности перемещения вне начального дня
				form.Controls.Add(actP);
				// Сортируем список задач
				days[beginDay].PanCaseRedraw();
				// Запоминаем начальные координаты задачи (на данный момент не используются)
				actX = actP.Left;
				actY = actP.Top;
				// Переносим на передний план
				actP.BringToFront();
				// Показываем, что панель активна
				act = true;
			}
			if (e.Button == MouseButtons.Right && !compl && !act)
			{
				// Определяем панельку
				if ((sender as Panel) != null)		actP2 = (sender as Panel);
				else if ((sender as Label) != null)	actP2 = (Panel)(sender as Label).Parent;
				// Меняем местами Текстбоксы с Лейблами
				foreach (Control ctrl in actP2.Controls)
					if ((ctrl as TextBox) != null || (ctrl as MaskedTextBox) != null)
						ctrl.BringToFront();
				// Запоминаем индексы панелей дня и задачи для дальнейшей возможности к ним обратиться
				beginDay = actP2.Parent.TabIndex;
				beginCase = days[beginDay].panCase.IndexOf(actP2);
				// Объявляем о том, что происходит редактирование задачи
				compl = true;
			}
		}
		// Событие срабатывающие во время отпускания кнопки, отвечает за конечное расположение панели
		internal static void MouseUp_Case(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && act)
			{
				bool cancel = true;
				bool b = true;
				// Переменные для записи значений
				string name = "";
				string time = "";
				string desc = "";
				// Сначала считываем то, что записанно в лейблах путем перебора всех контролов на панели
				foreach (Control ctrl in actP.Controls)
				{
					if (ctrl.TabIndex == 0)			name = ctrl.Text;
					else if (ctrl.TabIndex == 1)	time = ctrl.Text;
					else if (ctrl.TabIndex == 2)	desc = ctrl.Text;
				}
				// После отпускания кнопки мыши проверяем местоположение панели относительно дней
				for (int i = 0; i < 7; i++)
				{
					if ((actP.Left + actP.Width/2 > days[i].panDay.Left) && (actP.Left + actP.Width/2 < days[i].panDay.Left + days[i].panDay.Width)
					   && (actP.Top + actP.Height/2 > days[i].panDay.Top) && (actP.Top + actP.Height/2 < days[i].panDay.Top + days[i].panDay.Height))
					{
						// Удаляем задачу с формы
						form.Controls.Remove(actP);
						// Добавляем задачу в нужный день
						days[i].panDay.Controls.Add(actP);
						
						for (int j = 0; j < days[i].panCase.Count(); j++)
						{
							if ((actP.Top + actP.Height/2 < days[i].panCase[j].Top + days[i].panCase[j].Height))
							{
								// Добавляем на нужную панельку, отображаем и меняем высоту
								days[i].panCase.Insert(j, actP);
								year[1].listMonth[days[i].date.Month - 1].listDay[days[i].date.Day - 1].cases.Insert(j, new Сalendar.Case(name, time, desc));		// <<< ###
								actP.Left = 3;
								actP.Top = labVoid.Top;
								b = false;
								break;
							}
						}
						if (b)
						{
							days[i].panCase.Insert(days[i].panCase.Count(), actP);
							year[1].listMonth[days[i].date.Month - 1].listDay[days[i].date.Day - 1].cases.Add(new Сalendar.Case(name, time, desc));		// <<< ###
							actP.Left = 3;
							actP.Top = days[i].posBot;
						}
						days[i].posBot += actP.Height + 3;
						// Изменяем координату последней задачи
						days[beginDay].posBot -= actP.Height + 3;
						cancel = false;
						// Перерисовываем измененный список задач
						days[i].posLab = 0;
						days[i].PanCaseRedraw();
						// Перерисовываем список задач начального дня
						days[beginDay].PanCaseRedraw();		// ***
						break;
					}
					if ((actP.Top + actP.Height/2 < days[i].panDay.Top) || true)	// 'Пуф'	!!! true в проверке !!!
					{
						form.Controls.Remove(actP);
						days[beginDay].panCase.Remove(actP);
						days[beginDay].posBot -= actP.Height + 3;
						cancel = false;
						days[beginDay].PanCaseRedraw();		// ***
					}
				}
				// Возвращаем задачу задачу обратно на начальный день если она не была перемещена	(выключено)
				if (cancel)
				{
					form.Controls.Remove(actP);
					days[beginDay].panDay.Controls.Add(actP);
					
					actP.Left = actX;
					actP.Top = actY;
				}
				// Скрываем кнопку "удалить"
				labDelete.Visible = false;
				// Скрываем пустой лейбл
				labVoid.Visible = false;
				act = false;
				// ***
				for (int i = 0; i < 7; i++)
					days[i].labAddCase.Text = days[i].panCase.Count() + " ";		// ------------------- DEBAG
				// ***
			}
		}
		// Событие перетаскивания панели в котором происходит изменение координат относительно движения курсора
		internal static void MouseMove_Case(object sender, MouseEventArgs e)
		{
			if (act)
			{
				int b = -1;
				// Меняем местоположение задачи
				actP.Left = actP.Left + e.X - startX;
				actP.Top = actP.Top + e.Y - startY;
				// Отображаем пустой лейбл в зависимости от положения задачи
				for (int i = 0; i < 7; i++)
				{
					if ((actP.Left + actP.Width/2 > days[i].panDay.Left) && (actP.Left + actP.Width/2 < days[i].panDay.Left + days[i].panDay.Width)
					   && (actP.Top + actP.Height/2 > days[i].panDay.Top) && (actP.Top + actP.Height/2 < days[i].panDay.Top + days[i].panDay.Height))
					{
						for (int j = 0; j < days[i].panCase.Count(); j++)
						{
							if ((actP.Top + actP.Height/2 < days[i].panCase[j].Top + days[i].panCase[j].Height))
							{
								if (labVoid.Top + labVoid.Height + 3 != days[i].panCase[j].Top)
								{
									// Добавляем на нужную панельку, отображаем и меняем высоту
									days[i].panDay.Controls.Add(labVoid);
									labVoid.Top = days[i].panCase[j].Top;
									days[i].posLab = labVoid.Top;
									labVoid.Visible = true;
								}
								b = i;
								break;
							}
						}
						if (b < 0)
						{
							// Добавляем на нужную панельку, отображаем и меняем высоту
							days[i].panDay.Controls.Add(labVoid);
							labVoid.Top = days[i].posBot;
							days[i].posLab = labVoid.Top;
							labVoid.Visible = true;
						}
						break;
					}
				}
				for (int i = 0; i < 7; i++)
				{
					// Сортировка всего ???
					if (b != i)
						days[i].posLab = 0;
					days[i].PanCaseRedraw();		// ***
				}
			}
		}
		// Метод для закрытия режима редактирования задачи
		internal static void MouseClick_Outside(object sender, MouseEventArgs e)
		{			
			if (e.Button == MouseButtons.Left && compl)
			{
				// Переменные для записи значений
				string name = "";
				string time = "";
				string desc = "";
				// Сначала считываем то, что записанно в Текстбоксах путем перебора всех контролов на панели
				foreach (Control ctrl in days[beginDay].panCase[beginCase].Controls)
				{
					if (ctrl.TabIndex == 3)			name = ctrl.Text;
					else if (ctrl.TabIndex == 4)	time = ctrl.Text;
					else if (ctrl.TabIndex == 5)	desc = ctrl.Text;
				}
				// После переписываем считанные данные в Лейблы, также путем перебора всех контролов (не круто, но работает)
				foreach (Control ctrl in days[beginDay].panCase[beginCase].Controls)
				{
					if (ctrl.TabIndex == 0)			ctrl.Text = name;
					else if (ctrl.TabIndex == 1)	ctrl.Text = time;
					else if (ctrl.TabIndex == 2)	ctrl.Text = desc;
				}
				// Далее меняем местами Лейблы с Текстбоксами
				foreach (Control ctrl in actP2.Controls)
					if ((ctrl as Label) != null) ctrl.BringToFront();
				// Меняем значения в классе календаря
				year[1].listMonth[days[beginDay].date.Month - 1].listDay[days[beginDay].date.Day - 1].cases[days[beginDay].panCase.IndexOf(actP2)].nameCase = name;		// <<< ###
				year[1].listMonth[days[beginDay].date.Month - 1].listDay[days[beginDay].date.Day - 1].cases[days[beginDay].panCase.IndexOf(actP2)].lastTime = time;
				year[1].listMonth[days[beginDay].date.Month - 1].listDay[days[beginDay].date.Day - 1].cases[days[beginDay].panCase.IndexOf(actP2)].description = desc;
				compl = false;
			}
		}
		// Событие наведения на кнопку добавления новой задачи
		internal static void MouseEnter_labAddCase(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 208); }
		internal static void MouseLeave_labAddCase(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 228); }
	}
}




