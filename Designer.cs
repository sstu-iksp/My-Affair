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
		// Текущий год
		static Сalendar.Year year = new Сalendar.Year(Convert.ToInt32(DateTime.Now.ToString("yyyy")));
		
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
			// Включаем возможность отслеживания нажатия клавиш
			form.KeyPreview = true;
			
			// Отображаем главную панель
			panWeekMain.Visible = true;
			Controls.Add(panWeekMain);
			
			// Вывод дней недели
			InitializeWeek();
			
			InitializeReg();
			InitializeButtons();
			
			panWeekMain.MouseMove += (MouseMove_pmp);	// Зачем?
			
			panWeekMain.MouseClick += (MouseClick_Outside);	// Для заполнения задачи
		}
		
		// Коллекция для хранения дней недели
		static internal List<Day> days = new List<Day>();
		// Просто массив с днями недели
		internal string[] wn = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
		Dictionary<string, int> dayWeek = new Dictionary<string, int>();
		
		// Метод создающий дни недели, которые отображаются на экран, размер панельки дня 175x400 px
		internal void InitializeWeek()
		{
			//***																														// ТЕСТ
			year.listMonth[2].listDay[24].cases.Add(new Сalendar.Case("Тест", "11:11", "Да да я"));
			year.listMonth[3].listDay[23].cases.Add(new Сalendar.Case("Позавтракать", "9:00", "Чтобы вырасти, нужно хорошо питаться"));
			year.listMonth[3].listDay[23].cases.Add(new Сalendar.Case("Поужинать", "12:00", "Чтобы вырасти, нужно хорошо питаться x2"));
			year.listMonth[3].listDay[25].cases.Add(new Сalendar.Case("Пара", "9:45-11:15", "Пара по 1С"));
			year.listMonth[3].listDay[26].cases.Add(new Сalendar.Case("Пара", "13:40-15:10", "Памагите"));
			//***
			
			// Определяем день недели
			for(int i = 0; i < 7; i++)
				dayWeek.Add(wn[i], i);
			int dayBegin = dayWeek[DateTime.Now.DayOfWeek.ToString()];
			
			for(int i = 0; i < 7; i++)
			{
				days.Add(new Day(wn[i] + " - " + (DateTime.Now.Day - dayBegin + i), Core.CreatePan(panWeekMain, 15 + i * 180, 50, 175, 400)));	// Переход с месяцем !!!
				days[i].panDay.TabIndex = i;
				// События привязанные к панели дня
	//			days[i].panDay.MouseMove += (MouseMove_pmp);	// Прокрутка задач дня левой кнопкой мыши	***
	//			days[i].panDay.MouseWheel += (MouseWheel_pmp);	// Прокрутка задач дня колесиком мыши		***
				
				days[i].labAddCase.MouseEnter += (MouseEnter_labAddCase);	// Наведение			***
				days[i].labAddCase.MouseLeave += (MouseLeave_labAddCase);	// Наведение			***
				
				days[i].panDay.MouseClick += (MouseClick_Outside);			// Для заполнения задачи	#
				days[i].labDay.MouseClick += (MouseClick_Outside);			// Для заполнения задачи	#
				days[i].labAddCase.MouseClick += (MouseClick_Outside);		// Для заполнения задачи	#
				
				// Заполняем дни недели на экране задачами из классов
				foreach(Сalendar.Case cs in year.listMonth[DateTime.Now.Month - 1].listDay[DateTime.Now.Day - dayBegin + i + 1].cases)	// С месяцем проблеммы тоже !!!
				{
					days[i].panCase.Add(days[i].Copy_Case(days[i].panDay, 3, days[i].posBot, cs.nameCase, cs.lastTime, cs.description));
					days[i].posBot += days[i].panCase[0].Height + 3;		// Изменить индекс (0)	!!!
				}
			}
		}
		
		
		

	}
}




