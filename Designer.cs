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
		// Панель недели
		static Panel panWeekMain = Core.CreatePan(0, 0, 1280, 720);
		
		// Инициализация всех компонентов
		internal void InitializeComponent()
		{
			AutoScaleDimensions = new SizeF(8F, 16F);
			AutoScaleMode = AutoScaleMode.Font;
			// Делает границу фиксированной
			FormBorderStyle = FormBorderStyle.FixedSingle;
			// Блокировка полноэкранного режима
			MaximizeBox = false;
			// Создаем основную форму
			form = Core.CreateWindow(this, 10, 10, 1280, 720, "My-Affair");
			
			// Отображаем главную панель
			panWeekMain.Visible = true;
			Controls.Add(panWeekMain);
			
			// Вывод дней недели
			InitializeWeek();
			
			InitializeReg();
			InitializeButtons();
			
			panWeekMain.MouseMove += (MouseMove_pmp);	// Зачем?
		}
		
		// Коллекция для хранения дней недели
		static internal List<Day> days = new List<Day>();
		// Просто массив с днями недели
		string[] wn = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
		
		// Метод создающий дни недели, которые отображаются на экран, размер дня 175x400 px
		internal void InitializeWeek()
		{
			for(int i = 0; i < 7; i++)
			{
				days.Add(new Day(wn[i], Core.CreatePan(panWeekMain, 15 + i * 180, 50, 175, 400)));
				days[i].panDay.TabIndex = i;
				// События привязанные к панели дня
	//			days[i].panDay.MouseMove += (MouseMove_pmp);	// Прокрутка задач дня левой кнопкой мыши	***
	//			days[i].panDay.MouseWheel += (MouseWheel_pmp);	// Прокрутка задач дня колесиком мыши		***
				
				days[i].labAddCase.MouseEnter += (MouseEnter_labAddCase);	// Наведение			***
				days[i].labAddCase.MouseLeave += (MouseLeave_labAddCase);	// Наведение			***
				
			}
		}
		
		
		

	}
}




