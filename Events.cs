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
		// Переменная для запоминания активной панели
		static Panel actP;
		
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
			if (e.Button == MouseButtons.Left)
			{
				startX = e.X;
				startY = e.Y;
				// Запоминаем панель и ее координаты, в условии проверяется нажатие на саму панель или ее составляющих
				if ((sender as Panel) != null) 	actP = (sender as Panel);
				else if ((sender as Label) != null)	actP = (Panel)(sender as Label).Parent;
				
				actP.Parent.Controls.Remove(actP);		// ыыы
				form.Controls.Add(actP);				// (тест или нет)
				
				actX = actP.Left;
				actY = actP.Top;
				// Переносим на передний план
				actP.BringToFront();
				// Показываем что панель активна
				act = true;
			}
		}
		
		// Событие срабатывающие во время отпускания кнопки, отвечает за конечное расположение панели
		internal static void MouseUp_Case(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// После отпускания кнопки мыши проверяем местоположение панели относительно дней
				for (int i = 0; i < 7; i++)
				{
					if ((actP.Left + actP.Width/2 > days[i].panDay.Left) && (actP.Left + actP.Width/2 < days[i].panDay.Left + days[i].panDay.Width)
					   && (actP.Top + actP.Height/2 > days[i].panDay.Top) && (actP.Top + actP.Height/2 < days[i].panDay.Top + days[i].panDay.Height))
					{
						if (days[i].panCase.LastOrDefault() != null)			// ***
						{
							days[i].panDay.Controls.Add(Copy_Case(days[i].panDay, 3, days[i].panCase.LastOrDefault().Top + days[i].panCase.LastOrDefault().Height + 3));
						}
						else
						{
							days[i].panDay.Controls.Add(Copy_Case(days[i].panDay, 3, 28));
						}
						
						form.Controls.Remove(actP);	// (тест или нет)
						
						break;
					}
				}
				actP.Left = actX;
				actP.Top = actY;
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
                
                for(int i = 2; i < (sender as Panel).Controls.Count; i++)
                	(sender as Panel).Controls[i].Location = new Point((sender as Panel).Controls[i].Location.X, (sender as Panel).Controls[i].Location.Y + d.Y);
            }            
        }
		
		// Событие наведения на кнопку добавления новой задачи
		internal void MouseEnter_labAddCase(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 208); }
		internal void MouseLeave_labAddCase(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 228); }
	}
}




