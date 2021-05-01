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
		internal static Form form;
		// Панель недели
		internal static Panel panWeekMain = Core.CreatePan(0, 0, 1280, 720);
		// Определяем года
		internal static Сalendar.Year[] year =
		{
			new Сalendar.Year(Convert.ToInt32(DateTime.Now.ToString("yyyy")) - 1),
			new Сalendar.Year(Convert.ToInt32(DateTime.Now.ToString("yyyy"))),
			new Сalendar.Year(Convert.ToInt32(DateTime.Now.ToString("yyyy")) + 1)
		};
		// Инициализация всех компонентов
		internal void InitializeComponent()
		{
			// Всякие настройки
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
			// Цвет формы (скрыта)
			form.BackColor = Color.FromArgb(255, 216, 177);
			// Цвет панельки недели
			panWeekMain.BackColor = Color.FromArgb(255, 216, 177);
			// Отображаем главную панель
			panWeekMain.Visible = true;
			Controls.Add(panWeekMain);
			// Вывод дней недели
			InitializeWeek();
			// Вывод регистрации/авторизации
			InitializeReg();
			// Ввод различных элементов
			InitializeElements();
			// Инициализация календаря
			InitializeCalendarView();
			// Событие для выхода из режима заполнения задачи
			panWeekMain.MouseClick += (MouseClick_Outside);
		}
		
		// Коллекция для хранения дней недели
		internal static List<Day> days = new List<Day>();
		// Просто массив с днями недели
		internal string[] wn = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
		internal Dictionary<string, int> dayWeek = new Dictionary<string, int>();
		// Пустаяя панелька
		internal static Label labVoid;
		// Метод создающий дни недели, которые отображаются на экран, размер панельки дня 175x400 px
		internal void InitializeWeek()
		{
			// Определяем начальные день и месяц
			ddd = DateTime.Now.Day;
			mmm = DateTime.Now.Month;
			
			// Здесь возможно будет чтение из базы данных
			//***																												// Временное заполнение классов
			year[1].listMonth[2].listDay[24].cases.Add(new Сalendar.Case("Тест", "11:11", "Да да я"));
			year[1].listMonth[3].listDay[23].cases.Add(new Сalendar.Case("Позавтракать", "09:00", "Чтобы вырасти, нужно хорошо питаться"));
			year[1].listMonth[3].listDay[23].cases.Add(new Сalendar.Case("Поужинать", "12:00", "Чтобы вырасти, нужно хорошо питаться x2"));
			year[1].listMonth[3].listDay[25].cases.Add(new Сalendar.Case("Пара", "09:45-11:15", "Пара по 1С"));
			year[1].listMonth[3].listDay[26].cases.Add(new Сalendar.Case("Пара", "13:40-15:10", "Памагите"));
			year[1].listMonth[3].listDay[26].cases.Add(new Сalendar.Case("Пара", "13:40-15:10", "222"));
			year[1].listMonth[3].listDay[26].cases.Add(new Сalendar.Case("Пара", "13:40-15:10", "333"));
			year[1].listMonth[3].listDay[29].cases.Add(new Сalendar.Case("Пара", "13:40-15:10", "Памагите"));
			//***
			
			for (int i = 0; i < 7; i++)
				dayWeek.Add(wn[i], i);
			// Определяем переменную для определения первого дня недели
			int dayBegin = dayWeek[DateTime.Now.DayOfWeek.ToString()] + 1;
			
			for (int i = 0; i < 7; i++)																// Нижний код в какой нибуть метод лучше
			{
				int numYear = 1;
				int numDay = DateTime.Now.Day - dayBegin + i;
				int mon = 1;
				// Если в момент обхода обращаемся к прошлому месяцу	(29 30 *1* 2 3 4 5)
				if (numDay < 0)
				{
					mon = 2;
					numDay = year[numYear].listMonth[DateTime.Now.Month - mon].listDay.Count() - dayBegin  + DateTime.Now.Day + i;
				}
				// Если в момент обхода обращаемся к следующему месяцу	(26 27 *28* 29 30 1 2)
				else if (year[numYear].listMonth[DateTime.Now.Month - mon].listDay.Count() <= numDay)
				{
					mon = 0;
					numDay = i - dayBegin;
				}
				// Если в момент обхода обращаемся к прошлому году
				if (DateTime.Now.Month - mon < 0)										// Не проверено	***
				{
					
				}
				// Если в момент обхода обращаемся к следующему году
				else if (year[numYear].listMonth.Count() <= DateTime.Now.Month - mon)	// Не проверено
				{
					mon = 12;
					numYear = 2;
					numDay = i - dayBegin - 2;
				}
				// Создаем панели дней
				days.Add(new Day(wn[i] + " - " + (numDay + 1), Core.CreatePan(panWeekMain, 15 + i * 180, 50, 175, 600)));
				days[i].panDay.TabIndex = i;
				days[i].date = new DateTime(year[numYear].yearInt, DateTime.Now.Month - mon + 1, numDay + 1);
				// Заполняем дни недели на экране задачами из классов
				foreach (Сalendar.Case cs in year[numYear].listMonth[DateTime.Now.Month - mon].listDay[numDay].cases)
				{
					days[i].panCase.Add(days[i].Copy_Case(days[i].panDay, 3, days[i].posBot, cs.nameCase, cs.lastTime, cs.description));
					days[i].posBot += days[i].panCase[0].Height + 3;		// Изменить индекс (0)	!!!
				}
				// Выделяем текущий день
				if (days[i].date.Year == DateTime.Now.Year && days[i].date.Month == DateTime.Now.Month && days[i].date.Day == DateTime.Now.Day)
					days[i].labDay.BackColor = Color.FromArgb(115, 160, 250);
			}
			// Пустой лейбл
			labVoid = Core.CreateLab(days[0].panDay, 3, 28, 170, 30, 10);
			labVoid.BackColor = Color.FromArgb(201, 201, 201);
			labVoid.Visible = false;
		}
		
		// Метод перерисовывающий дни недели
		internal void DrawWeek(int dayNow, int monthNow, int weekDay, bool v)
		{
			// Переменная для определения первого дня недели
			int dayBegin = weekDay + 1;
			
			for(int i = 0; i < 7; i++)																// Нижний код в какой нибуть метод лучше
			{
				// Сбрасываем цвет текущего дня
				days[i].labDay.BackColor = Color.FromArgb(129, 222, 238);
				int numYear = 1;
				int numDay = dayNow - dayBegin + i;
				int mon = 1;
				
				// Если в момент обхода обращаемся к прошлому месяцу	(29 30 *1* 2 3 4 5)
				if (numDay < 0)
				{
					mon = 2;
					numDay = year[numYear].listMonth[monthNow - mon].listDay.Count() - dayBegin + dayNow + i;
				}
				// Если в момент обхода обращаемся к следующему месяцу	(26 27 *28* 29 30 1 2)
				else if (year[numYear].listMonth[monthNow - mon].listDay.Count() <= numDay)
				{
					mon = 0;
					numDay = i - dayBegin;
				}
				// Если в момент обхода обращаемся к прошлому году
				if (monthNow - mon < 0)										// Не проверено	***
				{
					
				}
				// Если в момент обхода обращаемся к следующему году
				else if (year[numYear].listMonth.Count() <= monthNow - mon)	// Не проверено
				{
					mon = 12;
					numYear = 2;
					numDay = i - dayBegin - 2;
				}
				// Меняем номер дня
				days[i].labDay.Text = wn[i] + " - " + (numDay + 1);
				days[i].date = new DateTime(year[numYear].yearInt, monthNow - mon + 1, numDay + 1);
				// Заполняем дни недели на экране задачами из классов
				foreach (Сalendar.Case cs in year[numYear].listMonth[monthNow - mon].listDay[numDay].cases)	// С месяцем проблеммы тоже !!!
				{
					days[i].panCase.Add(days[i].Copy_Case(days[i].panDay, 3, days[i].posBot, cs.nameCase, cs.lastTime, cs.description));
					days[i].posBot += days[i].panCase[0].Height + 3;		// Изменить индекс (0)	!!!
				}
				// Выделяем текущий день
				if (days[i].date.Year == DateTime.Now.Year && days[i].date.Month == DateTime.Now.Month && days[i].date.Day == DateTime.Now.Day)
					days[i].labDay.BackColor = Color.FromArgb(115, 160, 250);
				
				days[i].PanCaseRedraw();
			}
		}
		
		// Панель календаря
		internal static Panel panCalendar = Core.CreatePan(panWeekMain, 150, 150, 600, 400);
		// Коллекции лейблов для календаря
		internal List<Label> viewMonths = new List<Label>();
		internal List<Label> viewDays = new List<Label>();
		// Вывод календаря
		internal void InitializeCalendarView()
		{
			panCalendar.BackColor = Color.FromArgb(0, 180, 140);
			// Отображаем месяца
			for (int i = 0, j = 0; i < 12; i++)
			{
				if (i % 4 == 0 && i != 0)	j++;
				viewMonths.Add(Core.CreateLab(panCalendar, 100 + i * 55 - j * 220, 100 + j * 55, 50, 50, 18));	// Panel другая !!!
				viewMonths[i].BackColor = Color.FromArgb(88, 123, 224);
				viewMonths[i].Text = (i + 1) + "";
				viewMonths[i].Name = "apex" + i;
				viewMonths[i].MouseClick += (ButtonClickPrintMonth);
				viewMonths[i].MouseEnter += (MouseEnter_viewMonths);
				viewMonths[i].MouseLeave += (MouseLeave_viewMonths);
			}
		}
		// кек
		internal void ButtonClickPrintMonth(object sender, MouseEventArgs e)
		{
			// Отображаем дни месяца
			for (int i = 0, j = 0; i < 42; i++)
			{
				if (i % 7 == 0 && i != 0)	j++;
				viewDays.Add(Core.CreateLab(panCalendar, 50 + i * 55 - j * 385, 50 + j * 55, 50, 50, 18));	// Panel другая !!!
			}
			// Переменная для определения первого дня недели
			int dayBegin = dayWeek[DateTime.Now.DayOfWeek.ToString()];
			// Дни предыдущего месяца
			for (int i = 0; i < dayBegin; i++)
			{
				viewDays[i].Name = (sender as Label).Text;
				viewDays[i].Text = (year[1].listMonth[DateTime.Now.Month].listDay.Count() - dayBegin + i + 1) + "";
				viewDays[i].BackColor = Color.FromArgb(100, 100, 100);
			}
			// Дни текущего месяца
			for (int i = dayBegin; i < year[1].listMonth[DateTime.Now.Month - 1].listDay.Count() + dayBegin; i++)
			{
				viewDays[i].Name = (sender as Label).Text;
				viewDays[i].Text = (i - dayBegin + 1) + "";
				viewDays[i].BackColor = Color.FromArgb(125, 81, 237);
				viewDays[i].MouseClick += (ButtonClickPrintWeeek);
				viewDays[i].MouseEnter += (MouseEnter_viewDays);
				viewDays[i].MouseLeave += (MouseLeave_viewDays);
				// Выделяем сегодняшний день
				if (i == DateTime.Now.Day - 1 + dayBegin)
				{
					viewDays[i].BackColor = Color.FromArgb(128, 0, 128);
					viewDays[i].MouseClick += (ButtonClickPrintWeeek);
					viewDays[i].MouseEnter += (MouseEnter_dayNow);
					viewDays[i].MouseLeave += (MouseLeave_dayNow);
				}
			}
			// Дни следующего месяца
			for (int i = year[1].listMonth[DateTime.Now.Month - 1].listDay.Count() + dayBegin; i < 42; i++)
			{
				viewDays[i].Name = (sender as Label).Text;
				viewDays[i].Text = (i - year[1].listMonth[DateTime.Now.Month - 1].listDay.Count() - dayBegin + 1) + "";
				viewDays[i].BackColor = Color.FromArgb(100, 100, 100);
				viewDays[i].MouseClick -= (ButtonClickPrintWeeek);
				viewDays[i].MouseEnter -= (MouseEnter_viewDays);
				viewDays[i].MouseLeave -= (MouseLeave_viewDays);
			}
			// Серьезно ???
			foreach (Label lab in viewMonths)
				lab.Visible = false;
		}
		// ага
		internal void ButtonClickPrintWeeek(object sender, MouseEventArgs e)
		{
			/*
			panWeekMain.Visible = true;
			panRegMain.Visible = false;
			*/
			dlaNikitki(int.Parse((sender as Label).Text), int.Parse((sender as Label).Name));
		}
		// ъуъ
		internal void dlaNikitki(int day, int month) { }
		
		// Событие наведения на месяца
		internal static void MouseEnter_viewMonths(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(84, 168, 247); }
		internal static void MouseLeave_viewMonths(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(88, 123, 224); }
		// Событие наведения на дни текущего месяца
		internal static void MouseEnter_viewDays(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(86, 64, 247); }
		internal static void MouseLeave_viewDays(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(125, 81, 237); }
		// Событие наведения на текущий день
		internal static void MouseEnter_dayNow(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(150, 0, 100); }
		internal static void MouseLeave_dayNow(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(128, 0, 128); }
		
	}
}




