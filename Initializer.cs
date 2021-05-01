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
	// Здесь хранятся методы, которые выводят что-либо на экран 		*В РАЗРАБОТКЕ*
	partial class MainForm
	{
		// Лейбл "Календарь"
		internal static Label labCalendar = Core.CreateLab(panWeekMain, 15, 7, 175, 35, 10);
		// Лейбл "Назад"
		internal static Label labBackward = Core.CreateLab(panWeekMain, 15, panWeekMain.Height - 60, 355, 50, 18);
		// Лейбл "Вперед"
		internal static Label labForward = Core.CreateLab(panWeekMain, panWeekMain.Width - 365, panWeekMain.Height - 60, 355, 50, 18);
		// Лейбл "Удалить"
		internal static Label labDelete = Core.CreateLab(panWeekMain, 490, 5, 300, 40, 16);
		// Кнопка "Выйти"
		internal static Label labExit = Core.CreateLab(panWeekMain, 1200, 5, 70, 20, 10);
		// Отображение различных элементов
		internal void InitializeElements()
		{
			labCalendar.Text = "Календарь";
			labCalendar.BackColor = Color.FromArgb(150, 35, 255);
			labCalendar.MouseClick += (MouseClick_labCalendar);
			labCalendar.MouseEnter += (MouseEnter_labCalendar);
			labCalendar.MouseLeave += (MouseLeave_labCalendar);
			labBackward.Text = "<<<";
			labBackward.BackColor = Color.FromArgb(133, 238, 176);
			labBackward.MouseClick += (MouseClick_labBackard);
			labBackward.MouseEnter += (MouseEnter_labBF);
			labBackward.MouseLeave += (MouseLeave_labBF);
			labForward.Text = ">>>";
			labForward.BackColor = Color.FromArgb(133, 238, 176);
			labForward.MouseClick += (MouseClick_labForward);
			labForward.MouseEnter += (MouseEnter_labBF);
			labForward.MouseLeave += (MouseLeave_labBF);
			labDelete.Text = "удалить";
			labDelete.BackColor = Color.FromArgb(245, 162, 142);
			labDelete.Visible = false;
			labExit.Text = "Выйти";
			labExit.BackColor = Color.FromArgb(129, 202, 228);
			labExit.MouseClick += (MouseClick_labExit);
			labExit.MouseEnter += (MouseEnter_labExit);
			labExit.MouseLeave += (MouseLeave_labExit);
		}
		// Событие нажатия на кнопку "Календарь"
		internal void MouseClick_labCalendar(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)			panCalendar.Visible = true;
			else if (e.Button == MouseButtons.Right)	panCalendar.Visible = false;
		}
		// Переменные для запоминания текущего дня и месяца
		internal int ddd;
		internal int mmm;
		// Событие нажатия на кнопку "Назад"
		internal void MouseClick_labBackard(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// Очищаем контролы панели  и коллекцию задач
				for (int i = 0; i < 7; i++)
				{
					days[i].panCase.Clear();
					for (int j = days[i].panDay.Controls.Count - 1; 1 < j; j--)
						days[i].panDay.Controls.RemoveAt(j);
				}
				// Изменяем значение переменной в нужную сторону
				ddd -= 7;
				// Переходим к прошлому месяцу
				if (ddd <= 0)
				{
					mmm--;
					ddd = year[1].listMonth[mmm - 1].listDay.Count() + ddd;
				}
				// Определяем номер дня недели
				int dnw = dayWeek[new DateTime(year[1].yearInt, mmm, ddd).DayOfWeek.ToString()];
				
				labBackward.Text = mmm + " <<<";	// *** (не нужно) ***
				
				DrawWeek(ddd, mmm, dnw, false);
			}
		}
		// Событие нажатия на кнопку "Вперед"
		internal void MouseClick_labForward(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// Очищаем контролы панели  и коллекцию задач
				for (int i = 0; i < 7; i++)
				{
					days[i].panCase.Clear();
					for (int j = days[i].panDay.Controls.Count - 1; 1 < j; j--)
						days[i].panDay.Controls.RemoveAt(j);
				}
				// Изменяем значение переменной в нужную сторону
				ddd += 7;
				// Переходим к прошлому месяцу
				if (year[1].listMonth[mmm - 1].listDay.Count() < ddd)
				{
					ddd = ddd - year[1].listMonth[mmm - 1].listDay.Count();
					mmm++;
				}
				// Определяем номер дня недели
				int dnw = dayWeek[new DateTime(year[1].yearInt, mmm, ddd).DayOfWeek.ToString()];
				
				labForward.Text = ">>> " + mmm;		// *** (не нужно) ***
				
				DrawWeek(ddd, mmm, dnw, false);
			}
		}
		// Событие нажатия на кнопку "Выйти"
		internal void MouseClick_labExit(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// Здесь будет проверка на правельность введеннх данных
				panRegMain.Visible = true;
				panWeekMain.Visible = false;
			}
		}
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




