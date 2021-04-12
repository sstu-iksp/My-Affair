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
		// Главная и единственная форма
		static Form form;
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
			form = Core.CreateWindow(this, 10, 10, 1280, 720, "My-Affair");
			
			// Отображаем главную панель
			panMidPanel.Visible = true;
			Controls.Add(panMidPanel);
			
			// Вывод дней недели
			InitializeW();
			// Вывод тестовой задачи
			InitializeCase();
			
			InitializeReg();
			
			// Вывод дней недели
	//		InitializeWeek();	// (тест)
		}
		
		// Коллекция для хранения дней недели
		static internal List<Сalendar.Day> days = new List<Сalendar.Day>();
		
		// Метод создающий дни недели, которые отображаются на экран, размер дня 175x400 px
		internal void InitializeW()
		{
			for(int i = 0; i < 7; i++)
			{
				days.Add(new Сalendar.Day(wn[i], Core.CreatePan(panMidPanel, 15 + i * 180, 100, 175, 400)));
				
				// События привязанные к панели дня
				days[i].panDay.MouseMove += (MouseMove_pmp);	// Прокрутка задач дня левой кнопкой мыши	***
	//			days[i].panDay.MouseWheel += (MouseWheel_pmp);	// Прокрутка задач дня колесиком мыши		***
				
				days[i].labAddCase.MouseEnter += (MouseEnter_labAddCase);	// Наведение			***
				days[i].labAddCase.MouseLeave += (MouseLeave_labAddCase);	// Наведение			***
				
			}
		}
		
		
		static int panNum;
		static List<Panel> panCase = new List<Panel>();
		
		// Временный метод позволяющий копировать задачу
		internal static Panel Copy_Case(Panel pan, int x, int y)
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
			
			panCase[panNum].Visible = true;
			
			return panCase[panNum++];
		}
		
		
		

	}
}




