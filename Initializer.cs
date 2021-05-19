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
	partial class MainForm
	{
		// Лейбл для задачи "Откатить"
		internal static Label labBack = Core.CreateLab(panWeekMain, 220, 5, 30, 30, 7, "ctrl + z", Color.FromArgb(150, 75, 175));
		// Лейбл для задачи "Редактировать"
		internal static Label labCase1 = Core.CreateLab(panWeekMain, 0, 0, 100, 20, 8, "Редактировать", Color.FromArgb(150, 35, 255));
		// Лейбл для задачи "Изменить цвет"
		internal static Label labCase2 = Core.CreateLab(panWeekMain, 0, 25, 100, 20, 8, "Изменить цвет", Color.FromArgb(150, 35, 255));
		// Лейбл для задачи "Дублировать"
		internal static Label labCase4 = Core.CreateLab(panWeekMain, 0, 75, 100, 20, 8, "Дублировать", Color.FromArgb(150, 35, 255));
		// Лейбл для задачи "Приоритет"
		internal static Label labCase5 = Core.CreateLab(panWeekMain, 0, 100, 100, 20, 8, "Приоритет", Color.FromArgb(150, 35, 255));
		// Лейбл "Календарь"
		internal static Label labCalendar = Core.CreateLab(panWeekMain, 15, 7, 175, 35, 10, "Календарь", Color.FromArgb(150, 35, 255));
		// Лейбл "Назад"
		internal static Label labBackward = Core.CreateLab(panWeekMain, 15, panWeekMain.Height - 60, 355, 50, 18, "<<<", Color.FromArgb(133, 238, 176));
		// Лейбл "Вперед"
		internal static Label labForward = Core.CreateLab(panWeekMain, panWeekMain.Width - 365, panWeekMain.Height - 60, 355, 50, 18, ">>>", Color.FromArgb(133, 238, 176));
		// Лейбл "Удалить"
		internal static Label labDelete = Core.CreateLab(panWeekMain, 490, 5, 300, 40, 16, "удалить", Color.FromArgb(245, 162, 142));
		// Кнопка "Выйти"
		internal static Label labExit = Core.CreateLab(panWeekMain, 1200, 5, 70, 20, 10, "Выйти", Color.FromArgb(129, 202, 228));
		// Отображение различных элементов
		internal void InitializeElements()
		{
			Core.EventAdd(labCase1, MouseClick_labCase1, MouseEnter_labCase, MouseLeave_labCase);
			labCase1.Visible = false;
			Core.EventAdd(labCase2, MouseClick_labCase2, MouseEnter_labCase, MouseLeave_labCase);
			labCase2.Visible = false;
			Core.EventAdd(labCase4, MouseClick_labCase4, MouseEnter_labCase, MouseLeave_labCase);
			labCase4.Visible = false;
			Core.EventAdd(labCase5, MouseClick_labCase5, MouseEnter_labCase, MouseLeave_labCase);
			labCase5.Visible = false;
			
			Core.EventAdd(labCalendar, MouseClick_labCalendar, MouseEnter_labCalendar, MouseLeave_labCalendar);
			labCalendar.MouseClick += (MouseClick_Outside);				// ###
			Core.EventAdd(labBackward, MouseClick_labBackard, MouseEnter_labBF, MouseLeave_labBF);
			labBackward.MouseClick += (MouseClick_Outside);				// ###
			Core.EventAdd(labForward, MouseClick_labForward, MouseEnter_labBF, MouseLeave_labBF);
			labForward.MouseClick += (MouseClick_Outside);				// ###
			labDelete.Visible = false;
			Core.EventAdd(labExit, MouseClick_labExit, MouseEnter_labExit, MouseLeave_labExit);
			labExit.MouseClick += (MouseClick_Outside);					// ###
			
			labBack.MouseClick += (MouseClick_labBack);		// Тест
			
			labInit();
		}
		// Событие нажатия на кнопку "откатить"
		internal void MouseClick_labBack(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				
				DrawWeek(ddd, mmm, false);
			}
		}
		
		// Метод отображающий кнопки задачи
		internal static void caseViewBut(int left, int top, int width)
		{
			labCase1.Location = new Point(left + width + 5, top);
			labCase2.Location = new Point(left + width + 5, labCase1.Top + labCase1.Height + 5);
			labCase4.Location = new Point(left + width + 5, labCase2.Top + labCase2.Height + 5);
			labCase5.Location = new Point(left + width + 5, labCase4.Top + labCase4.Height + 5);
			labCase1.Visible = true;
			labCase2.Visible = true;
			labCase4.Visible = true;
			labCase5.Visible = true;
		}
		
		// Лейблы для выбора цвета
		internal static List<Label> labColor = new List<Label>();
		// Временный метод
		internal void labInit()
		{
			labSave.BringToFront();
			labSave.Visible = false;
			labSave.MouseClick += (MouseClick_labSave);
			labCancel.BringToFront();
			labCancel.Visible = false;
			labCancel.MouseClick += (MouseClick_labCancel);
			
			for (int i = 0; i < 8; i++)
			{
				labColor.Add(Core.CreateLab(panWeekMain, 0, 0, 20, 20, 10));
				labColor[i].BringToFront();
				labColor[i].Visible = false;
				labColor[i].MouseClick += (MouseClick_labColor);
			}
			// Цвета
			labColor[0].BackColor = Color.FromArgb(80, 220, 60);
			labColor[1].BackColor = Color.FromArgb(70, 250, 190);
			labColor[2].BackColor = Color.FromArgb(220, 40, 40);
			labColor[3].BackColor = Color.FromArgb(250, 220, 40);
			labColor[4].BackColor = Color.FromArgb(130, 130, 130);
			labColor[5].BackColor = Color.FromArgb(150, 0, 250);
			labColor[6].BackColor = Color.FromArgb(70, 60, 130);
			labColor[7].BackColor = Color.FromArgb(250, 115, 45);
		}
		// Лейбл для задачи "Сохранить"
		internal static Label labSave = Core.CreateLab(panWeekMain, 0, 0, 80, 30, 8, "Сохранить", Color.FromArgb(80, 220, 60));
		// Лейбл для задачи "Отменить"
		internal static Label labCancel = Core.CreateLab(panWeekMain, 0, 0, 80, 30, 8, "Отменить", Color.FromArgb(250, 50, 50));
		// Событие нажатия на кнопку "Редактировать"
		internal void MouseClick_labCase1(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Core.VisibleList(labColor, false);
				
				// Меняем местами Текстбоксы с Лейблами
				foreach (Control ctrl in actP2.Controls)
					if ((ctrl as TextBox) != null || (ctrl as MaskedTextBox) != null)
						ctrl.BringToFront();
				
				labSave.Left = actP2.Parent.Left + actP2.Left;
				labSave.Top = actP2.Parent.Top + actP2.Top + actP2.Height + 5;
				labSave.Visible = true;
				
				labCancel.Left = labSave.Left + labSave.Width + 10;
				labCancel.Top = actP2.Parent.Top + actP2.Top + actP2.Height + 5;
				labCancel.Visible = true;
			}
		}
		// Событие нажатия на кнопку "Сохранить"
		internal void MouseClick_labSave(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
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
				// Убираем лишние энтеры
				name = name.Replace("\r\n", " ");
				desc = desc.Replace("\r\n", " ");
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
				year[1].listMonth[days[beginDay].date.Month - 1].listDay[days[beginDay].date.Day - 1].cases[days[beginDay].panCase.IndexOf(actP2)].CaseRewrite(name, time, desc);
				labSave.Visible = false;
				labCancel.Visible = false;
			}
		}
		// Событие нажатия на кнопку "Отменить"
		internal void MouseClick_labCancel(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				string name = "";
				string time = "";
				string desc = "";
				foreach (Control ctrl in days[beginDay].panCase[beginCase].Controls)
				{
					if (ctrl.TabIndex == 0)			name = ctrl.Text;
					else if (ctrl.TabIndex == 1)	time = ctrl.Text;
					else if (ctrl.TabIndex == 2)	desc = ctrl.Text;
				}
				foreach (Control ctrl in days[beginDay].panCase[beginCase].Controls)
				{
					if (ctrl.TabIndex == 3)			ctrl.Text = name;
					else if (ctrl.TabIndex == 4)	ctrl.Text = time;
					else if (ctrl.TabIndex == 5)	ctrl.Text = desc;
				}
				foreach (Control ctrl in actP2.Controls)
					if ((ctrl as Label) != null) ctrl.BringToFront();
				labSave.Visible = false;
				labCancel.Visible = false;
			}
		}
		// Событие нажатия на кнопку "Изменить цвет"
		internal void MouseClick_labCase2(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				labSave.Visible = false;
				labCancel.Visible = false;
				
				// Отображаем кнопки
				if(beginDay != 6)
				{
					for (int i = 0; i < 8; i++)
					{
						labColor[i].Left = labCase1.Left + labCase1.Width + 5 + (i / 4) * (labColor[0].Width + 5);
						labColor[i].Top = labCase1.Top + (labCase1.Height + 5) * (i % 4);
						labColor[i].Visible = true;
					}
				}
				else
				{
					for (int i = 0; i < 8; i++)
					{
						labColor[i].Left = labCase1.Left - (labColor[0].Width + 5) * 2 + (i / 4) * (labColor[0].Width + 5);
						labColor[i].Top = labCase1.Top + (labCase1.Height + 5) * (i % 4);
						labColor[i].Visible = true;
					}
				}
				// Меняем местами Лейблы с Текстбоксами если были в режиме редактирования
				foreach (Control ctrl in actP2.Controls)
					if ((ctrl as Label) != null) ctrl.BringToFront();
			}
		}
		// Событие нажатия на кнопку цвета
		internal void MouseClick_labColor(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				foreach (Control ctrl in days[beginDay].panCase[beginCase].Controls)
					ctrl.BackColor = (sender as Label).BackColor;
				
				// Меняем значения в классе календаря
				year[1].listMonth[days[beginDay].date.Month - 1].listDay[days[beginDay].date.Day - 1].cases[days[beginDay].panCase.IndexOf(actP2)].colorCase = (sender as Label).BackColor;
				year[1].listMonth[days[beginDay].date.Month - 1].listDay[days[beginDay].date.Day - 1].cases[days[beginDay].panCase.IndexOf(actP2)].colorText = (sender as Label).ForeColor;
			}
		}
		// Событие нажатия на кнопку "Дублировать"
		internal void MouseClick_labCase4(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				string name = "";
				string time = "";
				string desc = "";
				Color colorCase = actP2.Controls[0].BackColor;
				Color colorText = actP2.Controls[0].ForeColor;
				foreach (Control ctrl in days[beginDay].panCase[beginCase].Controls)
				{
					if (ctrl.TabIndex == 0)			name = ctrl.Text;
					else if (ctrl.TabIndex == 1)	time = ctrl.Text;
					else if (ctrl.TabIndex == 2)	desc = ctrl.Text;
				}
				days[beginDay].CaseAdd(days[beginDay].Copy_Case(days[beginDay].panDay, 3, days[beginDay].posBot, name, time, desc, colorCase, colorText));
				year[1].listMonth[days[beginDay].date.Month - 1].listDay[days[beginDay].date.Day - 1].cases.Add(new Сalendar.Case(name, time, desc, colorCase, colorText));
			}
		}
		// Событие нажатия на кнопку "Приоритет"
		internal void MouseClick_labCase5(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				
			}
		}
		// Событие нажатия на кнопку "Календарь"
		internal void MouseClick_labCalendar(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && !compl)		panCalendar.Visible = true;
			else if (e.Button == MouseButtons.Right && !compl)	panCalendar.Visible = false;
		}
		// Переменные для запоминания текущего дня и месяца
		internal int ddd;
		internal int mmm;
		// Событие нажатия на кнопку "Назад"
		internal void MouseClick_labBackard(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && !compl)
			{
				// Изменяем значение переменной в нужную сторону
				ddd -= 7;
				// Переходим к прошлому месяцу
				if (ddd <= 0)
				{
					mmm--;
					ddd = year[1].listMonth[mmm - 1].listDay.Count() + ddd;
				}
				
				labBackward.Text = mmm + " <<<";	// *** (не нужно) ***
				
				DrawWeek(ddd, mmm, false);
			}
		}
		// Событие нажатия на кнопку "Вперед"
		internal void MouseClick_labForward(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && !compl)
			{
				// Изменяем значение переменной в нужную сторону
				ddd += 7;
				// Переходим к прошлому месяцу
				if (year[1].listMonth[mmm - 1].listDay.Count() < ddd)
				{
					ddd = ddd - year[1].listMonth[mmm - 1].listDay.Count();
					mmm++;
				}
				
				labForward.Text = ">>> " + mmm;		// *** (не нужно) ***
				
				DrawWeek(ddd, mmm, false);
			}
		}
		// Событие нажатия на кнопку "Выйти"
		internal void MouseClick_labExit(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && !compl)
			{
				// Прежде чем записывать, удаляем все задачи связанные с текущим пользователем
				conn.Delete(conn.user);
				for (int i = 0; i < year[1].listMonth.Count; i++)
					for (int j = 0; j < year[1].listMonth[i].listDay.Count; j++)
						foreach (Сalendar.Case cs in year[1].listMonth[i].listDay[j].cases)
							conn.WriteCase(cs, year[1].yearInt, i, j);
				// Выключаем таймер					// !!!***!!!
				timerDB.Enabled = false;
				// Очищаем данные вышедшего пользователя
				for (int i = 0; i < year[1].listMonth.Count; i++)
					for (int j = 0; j < year[1].listMonth[i].listDay.Count; j++)
						year[1].listMonth[i].listDay[j].cases.Clear();
				
				DrawWeek(ddd, mmm, false);
				
				conn.user = 0;
				
				// Здесь будет проверка на правельность введеннх данных
				panRegMain.Visible = true;
				panWeekMain.Visible = false;
			}
		}
		// Событие наведения на кнопки задачи
		internal void MouseEnter_labCase(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(180, 35, 225); }
		internal void MouseLeave_labCase(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(150, 35, 255); }
		// Событие наведения на кнопку "Календарь"
		internal void MouseEnter_labCalendar(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(180, 35, 225); }
		internal void MouseLeave_labCalendar(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(150, 35, 255); }
		// Событие наведения на кнопки "Назад" и "Вперед"
		internal void MouseEnter_labBF(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(113, 228, 156); }
		internal void MouseLeave_labBF(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(133, 238, 176); }
		// Событие наведения на кнопку "Выйти"
		internal void MouseEnter_labExit(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 208); }
		internal void MouseLeave_labExit(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 202, 228); }
		
		/*														Рамки
		 
		internal List<Label> kek = new List<Label>();
		
		//												   		  +
		
		for (int i = 0, j = 0; i < 42; i++)
		{
			if (i % 7 == 0 && i != 0)	j++;
			kek.Add(Core.CreateLab(panRegMain, 49 + i * 55 - j * 385, 49 + j * 55, 52, 52, 18));
			kek[i].BackColor = Color.FromArgb(0, 0, 0);
			kek[i].SendToBack();
		}
		
		*/
	}
}




