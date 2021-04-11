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
		// Главная панель
		static Panel panMidPanel = Core.CreatePan(0, 0, 1280, 720);
		
		internal void InitializeComponent()
		{
			AutoScaleDimensions = new SizeF(8F, 16F);
			AutoScaleMode = AutoScaleMode.Font;
			// Делает границу фиксированной
			FormBorderStyle = FormBorderStyle.FixedSingle;
			// Блокировка полноэкранного режима
			MaximizeBox = false;
			// Метод создающий окно
			Core.CreateWindow(this, 10, 10, 1280, 720, "My-Affair");
			
			// Отображаем главную панель
			panMidPanel.Visible = true;
			Controls.Add(panMidPanel);
			
			// Вывод дней недели
			InitializeW();
			// Вывод тестовой задачи
			InitializeCase();
			
			// Вывод дней недели
	//		InitializeWeek();	// (тест)
		}
		
		internal List<Сalendar.Day> days = new List<Сalendar.Day>();
		
		// Метод создающий дни недели, которые отображаются на экран, размер дня 175x400 px
		internal void InitializeW()
		{
			for(int i = 0; i < 7; i++)
			{
				days.Add(new Сalendar.Day("Monday", Core.CreatePan(panMidPanel, 15 + i * 180, 100, 175, 400)));
				
				// События привязанные к панели дня
				days[i].panDay.MouseMove += (MouseMove_pmp);	// Прокрутка задач дня левой кнопкой мыши
				days[i].panDay.MouseWheel += (MouseWheel_pmp);	// Прокрутка задач дня колесиком мыши
			}
		}
		
		
		
		// Переменная для проверки активности панели
		bool act;
		// Переменная для запоминания активной панели
		Panel actP;
		
		// Переменные для запоминания начальных координат панели (для теста)
		int actX;
		int actY;
		
		// Переменные для запоминания начальных координат на панели (для теста)
		int startX;
		int startY;
		
		// Событие зажатия кнопки мыши на панели, отвечает за присвоение к активной панели, которая используется далее
		internal void MouseDown_Case(object sender, MouseEventArgs e)
		{
			// Проверяем нажатие на кнопку
			if (e.Button == MouseButtons.Left)
			{
				startX = e.X;
				startY = e.Y;
				// Запоминаем панель и ее координаты, в условии проверяется нажатие на саму панель или ее составляющих
				if ((sender as Panel) != null) 	actP = (sender as Panel);
				else if ((sender as Label) != null)	actP = (Panel)(sender as Label).Parent;
				
				actX = actP.Left;
				actY = actP.Top;
				// Переносим на передний план
				actP.BringToFront();
				// Показываем что панель активна
				act = true;
			}
		}
		
		// Событие срабатывающие во время отпускания кнопки, отвечает за конечное расположение панели
		internal void MouseUp_Case(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// После отпускания кнопки мыши проверяем местоположение панели относительно дней
				for (int i = 0; i < 7; i++)
				{
					if ((actP.Left + actP.Width/2 > days[i].panDay.Left) && (actP.Left + actP.Width/2 < days[i].panDay.Left + days[i].panDay.Width)
					   && (actP.Top + actP.Height/2 > days[i].panDay.Top) && (actP.Top + actP.Height/2 < days[i].panDay.Top + days[i].panDay.Height))
					{
						days[i].panDay.Controls.Add(Copy_Case(days[i].panDay, 3, 28 + (days[i].panDay.Controls.Count - 1) * 105));
						
						break;
					}
				}
				actP.Left = actX;
				actP.Top = actY;
				act = false;
			}
		}
		
		// Событие перетаскивания панели в котором происходит изменение координат относительно движения курсора
		internal void MouseMove_Case(object sender, MouseEventArgs e)
		{
			if (act)
			{
				actP.Left = actP.Left + e.X - startX;
				actP.Top = actP.Top + e.Y - startY;
			}
		}
		
		int panNum;
		List<Panel> panCase = new List<Panel>();
		
		internal Panel Copy_Case(Panel pan, int x, int y)
		{
			//weekday
			panCase.Add(Core.CreatePan(pan, x, y, 170, 100));
			Label labCaseNameT = Core.CreateLab(panCase[panNum], 5, 5, 105, 20, 11);
			Label labCaseTimeT = Core.CreateLab(panCase[panNum], 110, 5, 55, 20, 11);
			Label labCaseDescT = Core.CreateLab(panCase[panNum], 5, 26, 160, 70, 9);
			PictureBox pbTest = Core.CreatePB(panCase[panNum], 1, 1, 1, 1);
			
			panCase[panNum].BackColor = Color.FromArgb(133, 238, 186);
			
			labCaseNameT.BackColor = Color.FromArgb(133, 248, 186);
			labCaseTimeT.BackColor = Color.FromArgb(133, 238, 176);
			labCaseDescT.BackColor = Color.FromArgb(133, 228, 166);
			
			labCaseNameT.TextAlign = ContentAlignment.MiddleLeft;
			labCaseTimeT.TextAlign = ContentAlignment.MiddleRight;
			labCaseDescT.TextAlign = ContentAlignment.TopLeft;
			
			labCaseNameT.Text = "Name";
			labCaseTimeT.Text = "Time";
			labCaseDescT.Text = "Description\nyep";
			
			panCase[panNum].Visible = true;
			
			// Присваиваем события для панели и ее составляющих
			panCase[panNum].MouseMove += (MouseMove_Case);
			labCaseNameT.MouseMove += (MouseMove_Case);
			labCaseTimeT.MouseMove += (MouseMove_Case);
			labCaseDescT.MouseMove += (MouseMove_Case);
			panCase[panNum].MouseMove += (MouseDown_Case);
			labCaseNameT.MouseDown += (MouseDown_Case);
			labCaseTimeT.MouseDown += (MouseDown_Case);
			labCaseDescT.MouseDown += (MouseDown_Case);
			panCase[panNum].MouseUp += (MouseUp_Case);
			labCaseNameT.MouseUp += (MouseUp_Case);
			labCaseTimeT.MouseUp += (MouseUp_Case);
			labCaseDescT.MouseUp += (MouseUp_Case);
			
			return panCase[panNum++];
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
                foreach (Control ctrl in (sender as Panel).Controls)
                    ctrl.Location = new Point(ctrl.Location.X, ctrl.Location.Y + d.Y);
            }            
        }
        
        // Прокрутка дня колесиком мыши
		internal void MouseWheel_pmp(object sender, MouseEventArgs e)
		{
			// '20' - скорость прокрутки, чем больше значение тем медленнее
			foreach (Control ctrl in (sender as Panel).Controls)
            		ctrl.Location = new Point(ctrl.Location.X, ctrl.Location.Y - e.Delta / 20);
		}
	}
}




