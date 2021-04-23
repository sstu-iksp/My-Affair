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
		static bool act;
		// Перемнная для определения режима заполнения задачи
		static bool compl;
		// Переменная для запоминания активной панели
		static Panel actP;
		static Panel actP2;
		// Переменная для запоминания номера начального дня
		static int beginDay;
		// Переменная для запоминания номера начальной задачи
		static int beginCase;
		// Переменные для запоминания начальных координат панели (для теста)
		static int actX;
		static int actY;
		// Переменные для запоминания начальных координат на панели (для теста)
		static int startX;
		static int startY;
		
		// Событие зажатия кнопки мыши на панели, отвечает за присвоение к активной панели, которая используется далее
		internal static void MouseDown_Case(object sender, MouseEventArgs e)
		{
			// Проверяем нажатие на кнопку
			if (e.Button == MouseButtons.Left && (sender as TextBox) == null && (sender as MaskedTextBox) == null)
			{
				startX = e.X;
				startY = e.Y;
				// Запоминаем панель и ее координаты, в условии проверяется нажатие на саму панель или ее составляющих
				if ((sender as Panel) != null)		actP = (sender as Panel);
				else if ((sender as Label) != null)	actP = (Panel)(sender as Label).Parent;
				
				// Запоминаем номер начального дня
				beginDay = actP.Parent.TabIndex;
				
				// Изменяем координату последней задачи
				days[beginDay].posBot -= actP.Height + 3;
				
		//		actP.Parent.Controls.Remove(actP);						// Неизвестно, нужно или нет
				
				// Добавляем задачу на форму, для возможности перемещения вне начального дня
				form.Controls.Add(actP);
				
				actX = actP.Left;
				actY = actP.Top;
				// Переносим на передний план
				actP.BringToFront();
				// Показываем что панель активна
				act = true;
			}
			if (e.Button == MouseButtons.Right && !compl)
			{
				if ((sender as Panel) != null)		actP2 = (sender as Panel);
				else if ((sender as Label) != null)	actP2 = (Panel)(sender as Label).Parent;
				
				foreach (Control ctrl in actP2.Controls)
				{
					if ((ctrl as TextBox) != null || (ctrl as MaskedTextBox) != null) ctrl.BringToFront();
				}
				beginDay = actP2.Parent.TabIndex;
				beginCase = days[beginDay].panCase.IndexOf(actP2);
				
				compl = true;
			}
		}
		
		// Событие срабатывающие во время отпускания кнопки, отвечает за конечное расположение панели
		internal static void MouseUp_Case(object sender, MouseEventArgs e)
		{
			bool cancel = true;
			
			if (e.Button == MouseButtons.Left)
			{
				// После отпускания кнопки мыши проверяем местоположение панели относительно дней
				for (int i = 0; i < 7; i++)
				{
					if ((actP.Left + actP.Width/2 > days[i].panDay.Left) && (actP.Left + actP.Width/2 < days[i].panDay.Left + days[i].panDay.Width)
					   && (actP.Top + actP.Height/2 > days[i].panDay.Top) && (actP.Top + actP.Height/2 < days[i].panDay.Top + days[i].panDay.Height)
					  	&& i != beginDay)
					{
						/*
						if (days[i].panCase.LastOrDefault() != null)			// ***
						{
							//days[i].panDay.Controls.Add(days[i].Copy_Case(days[i].panDay, 3, days[i].panCase.LastOrDefault().Top + days[i].panCase.LastOrDefault().Height + 3));
							days[i].panCase.Add(days[i].Copy_Case(days[i].panDay, 3, days[i].panCase.LastOrDefault().Top + days[i].panCase.LastOrDefault().Height + 3));
						}
						else
						{
							//days[i].panDay.Controls.Add(days[i].Copy_Case(days[i].panDay, 3, 28));
							days[i].panCase.Add(days[i].Copy_Case(days[i].panDay, 3, 28));
						}
						*/
						form.Controls.Remove(actP);							// ДОБАВИТЬ КОММЕНТЫ !!!
						days[i].panDay.Controls.Add(actP);
						days[i].panCase.Add(actP);
						actP.Left = 3;
						actP.Top = days[i].posBot;
				//		days[i].panCase.Add(days[i].Copy_Case(days[i].panDay, 3, days[i].posBot));	// ---
						// Удаляем задачу из колекции начального дня
						days[beginDay].panCase.Remove(actP);
						days[i].posBot += actP.Height + 3;
				//		actP.Dispose();																// ---
					//	form.Controls.Remove(actP);	// (тест или нет)
						cancel = false;						
						// Перерисовываем список задач начального дня
						days[beginDay].panCaseRedraw();
						
						break;
					}
				}
				// Возвращаем задачу задачу обратно на начальный день если она не была перемещена
				if(cancel)
				{
					form.Controls.Remove(actP);
					days[beginDay].panDay.Controls.Add(actP);
					
					actP.Left = actX;
					actP.Top = actY;
					//actP.Dispose();	// че
				}
		//		actP.Dispose();
				act = false;
			}
		}
		
		// Событие перетаскивания панели в котором происходит изменение координат относительно движения курсора
		internal static void MouseMove_Case(object sender, MouseEventArgs e)
		{
			if (act)
			{
				actP.Left = actP.Left + e.X - startX;
				actP.Top = actP.Top + e.Y - startY;
			}
		}
		
		// Метод для закрытия режима редактирования задачи
		internal static void MouseClick_Outside(object sender, MouseEventArgs e)
		{
			string name = "";
			string time = "";
			string desc = "";
			
			if (e.Button == MouseButtons.Left && compl)
			{
				foreach (Control ctrl in days[beginDay].panCase[beginCase].Controls)
				{
					if (ctrl.TabIndex == 3)
					{
						name = ctrl.Text;
					}
					else if (ctrl.TabIndex == 4)
					{
						time = ctrl.Text;
					}
					else if (ctrl.TabIndex == 5)
					{
						desc = ctrl.Text;
					}
				}
				foreach (Control ctrl in days[beginDay].panCase[beginCase].Controls)
				{
					if (ctrl.TabIndex == 0)
					{
						ctrl.Text = name;
					}
					else if (ctrl.TabIndex == 1)
					{
						ctrl.Text = time;
					}
					else if (ctrl.TabIndex == 2)
					{
						ctrl.Text = desc;
					}
					
				}
				foreach (Control ctrl in actP2.Controls)
					if ((ctrl as Label) != null) ctrl.BringToFront();
				compl = false;
			}
		}
		
		
		internal Point prevMousePos;
		// Прокурутка дня левой кнопкой мыши (тест)
        internal void MouseMove_pmp(object sender, MouseEventArgs e)
        {
        	// Записываем в переменную координаты 
            var d = new Point(e.X - prevMousePos.X, e.Y - prevMousePos.Y);
            // Записываем в переменную текущее положение курсора
            prevMousePos = e.Location;
            //if((sender as Panel) != null) 	actP = (sender as Panel);
            if (e.Button == MouseButtons.Left)
            {
            	// Меняем положение всех контролов на панели относительно курсора
	//			foreach (Control ctrl in (sender as Panel).Controls)
	//				ctrl.Location = new Point(ctrl.Location.X, ctrl.Location.Y + d.Y);
                
                for(int i = 1; i < (sender as Panel).Controls.Count; i++)
                	(sender as Panel).Controls[i].Location = new Point((sender as Panel).Controls[i].Location.X + d.X, (sender as Panel).Controls[i].Location.Y + d.Y);
            }            
        }
		
		// Событие наведения на кнопку добавления новой задачи
		internal void MouseEnter_labAddCase(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 208); }
		internal void MouseLeave_labAddCase(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 228); }
	}
}




