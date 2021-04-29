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
		static Form _form;
		// Панель недели
		static Panel panWeekMain = Core.CreatePan(0, 0, 1280, 720);
		// Определяем года
		static Сalendar.Year[] year =
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
			_form = Core.CreateWindow(this, 10, 10, 1280, 720, "My-Affair");
			// Включаем возможность отслеживания нажатия клавиш
			_form.KeyPreview = true;
			// Цвет формы (скрыта)
			_form.BackColor = Color.FromArgb(255, 216, 177);
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

			panWeekMain.MouseClick += (MouseClick_Outside); // Для заполнения задачи
		}

		// Коллекция для хранения дней недели
		static internal List<Day> days = new List<Day>();
		// Просто массив с днями недели
		internal string[] wn = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
		Dictionary<string, int> dayWeek = new Dictionary<string, int>();
		// Пустаяя панелька
		static internal Label labVoid;
		// Метод создающий дни недели, которые отображаются на экран, размер панельки дня 175x400 px
		internal void InitializeWeek()
		{
			//***																														// ТЕСТ
			year[1].listMonth[2].listDay[24].cases.Add(new Сalendar.Case("Тест", "11:11", "Да да я"));
			year[1].listMonth[3].listDay[23].cases.Add(new Сalendar.Case("Позавтракать", "09:00", "Чтобы вырасти, нужно хорошо питаться"));
			year[1].listMonth[3].listDay[23].cases.Add(new Сalendar.Case("Поужинать", "12:00", "Чтобы вырасти, нужно хорошо питаться x2"));
			year[1].listMonth[3].listDay[25].cases.Add(new Сalendar.Case("Пара", "09:45-11:15", "Пара по 1С"));
			year[1].listMonth[3].listDay[26].cases.Add(new Сalendar.Case("Пара", "13:40-15:10", "Па-ма-ги-те"));
			year[1].listMonth[3].listDay[29].cases.Add(new Сalendar.Case("Пара", "13:40-15:10", "Памагите"));
			//***

			for (int i = 0; i < 7; i++)
				dayWeek.Add(wn[i], i);
			// Определяем переменную для определения первого дня недели
			int dayBegin = dayWeek[DateTime.Now.DayOfWeek.ToString()] + 1;

			for (int i = 0; i < 7; i++)                                                             // Нижний код в какой нибуть метод лучше
			{
				int numYear = 1;
				int numDay = DateTime.Now.Day - dayBegin + i;
				int mon = 1;

				// Если в момент обхода обращаемся к прошлому месяцу	(29 30 *1* 2 3 4 5)
				if (numDay < 0)
				{
					mon = 2;
					numDay = year[numYear].listMonth[DateTime.Now.Month - mon].listDay.Count() - dayBegin + i;
				}
				// Если в момент обхода обращаемся к следующему месяцу	(26 27 *28* 29 30 1 2)
				else if (year[numYear].listMonth[DateTime.Now.Month - mon].listDay.Count() <= numDay)
				{
					mon = 0;
					numDay = i - dayBegin - 1;
				}
				// Если в момент обхода обращаемся к прошлому году
				if (DateTime.Now.Month - mon < 0)                                       // Не проверено	***
				{

				}
				// Если в момент обхода обращаемся к следующему году
				else if (year[numYear].listMonth.Count() <= DateTime.Now.Month - mon)   // Не проверено
				{
					mon = 12;
					numYear = 2;
					numDay = i - dayBegin - 2;
				}

				days.Add(new Day(wn[i] + " - " + (numDay + 1), Core.CreatePan(panWeekMain, 15 + i * 180, 50, 175, 600)));   // Переход с месяцем !!!
				days[i].panDay.TabIndex = i;

				// Заполняем дни недели на экране задачами из классов
				foreach (Сalendar.Case cs in year[numYear].listMonth[DateTime.Now.Month - mon].listDay[numDay].cases)   // С месяцем проблеммы тоже !!!
				{
					days[i].panCase.Add(days[i].Copy_Case(days[i].panDay, 3, days[i].posBot, cs.nameCase, cs.lastTime, cs.description));
					days[i].posBot += days[i].panCase[0].Height + 3;        // Изменить индекс (0)	!!!
				}
			}
			// Пустой лейбл
			labVoid = Core.CreateLab(days[0].panDay, 3, 28, 170, 30, 10);
			labVoid.BackColor = Color.FromArgb(201, 201, 201);
			labVoid.Visible = false;
		}

		// Метод перерисовывающий дни недели
		internal void DrawWeek(int dayNow, int monthNow, int weekDay, bool v)
		{
			// Определяем переменную для определения первого дня недели
			int dayBegin = weekDay + 1;

			for (int i = 0; i < 7; i++)                                                             // Нижний код в какой нибуть метод лучше
			{
				int numYear = 1;
				int numDay = dayNow - dayBegin + i;
				int mon = 1;

				// Если в момент обхода обращаемся к прошлому месяцу	(29 30 *1* 2 3 4 5)
				if (numDay < 0)
				{
					mon = 2;
					numDay = year[numYear].listMonth[monthNow - mon].listDay.Count() - dayBegin + i;
				}
				// Если в момент обхода обращаемся к следующему месяцу	(26 27 *28* 29 30 1 2)
				else if (year[numYear].listMonth[monthNow - mon].listDay.Count() <= numDay)
				{
					mon = 0;
					numDay = i - dayBegin - 1;
				}
				// Если в момент обхода обращаемся к прошлому году
				if (monthNow - mon < 0)                                     // Не проверено	***
				{

				}
				// Если в момент обхода обращаемся к следующему году
				else if (year[numYear].listMonth.Count() <= monthNow - mon) // Не проверено
				{
					mon = 12;
					numYear = 2;
					numDay = i - dayBegin - 2;
				}

				// Меняем номер дня
				days[i].labDay.Text = wn[i] + " - " + (numDay + 1);

				// Заполняем дни недели на экране задачами из классов
				foreach (Сalendar.Case cs in year[numYear].listMonth[monthNow - mon].listDay[numDay].cases) // С месяцем проблеммы тоже !!!
				{
					days[i].panCase.Add(days[i].Copy_Case(days[i].panDay, 3, days[i].posBot, cs.nameCase, cs.lastTime, cs.description));
					days[i].posBot += days[i].panCase[0].Height + 3;        // Изменить индекс (0)	!!!
				}
			}
		}



		// Коллекция лейблов для календаря
		List<Label> kry = new List<Label>();

		List<Button> listButton = new List<Button> { };
		// Вывод календаря
		internal void InitializeWeekCal()
		{
			int xButton = 100;
			int yButton = 100;

			for (int i = 0; i < 12; i++)
			{
				Button button = new Button();
				listButton.Add(button);
				button.Name = "apex" + i;

				button.Size = new Size(40, 25);
				button.Left = xButton;
				button.Top = yButton;
				button.Text = "" + i;
				button.BringToFront();
				panRegMain.Controls.Add(button);

				button.Click += new EventHandler(ButtonClickPrintMonth);
				xButton += 40;
				if (i == 3 || i == 7)
				{
					yButton += 25;
					xButton = 100;
				}
			}


		}
		private void ButtonClickPrintMonth(object sender, EventArgs e)
		{

			int j = 0;

			for (int i = 0; i < 42; i++)
			{
				if (i % 7 == 0 && i != 0)
					j++;

				kry.Add(Core.CreateLab(panRegMain, 50 + i * 55 - j * 385, 50 + j * 55, 50, 50, 18));
				kry[i].BackColor = Color.FromArgb(201, 201, 201);

				//	kry[i].Text = i + "";
			}

			// Определяем переменную для определения первого дня недели
			int dayBegin = dayWeek[DateTime.Now.DayOfWeek.ToString()];
			for (int i = 0; i < dayBegin; i++)
			{
				kry[i].Name = (sender as Button).Text;
				kry[i].Text = (year[1].listMonth[DateTime.Now.Month].listDay.Count() - dayBegin + i + 1) + "";
				kry[i].Click += new EventHandler(ButtonClickPrintWeeek);
			}
			for (int i = dayBegin; i < year[1].listMonth[DateTime.Now.Month - 1].listDay.Count() + dayBegin; i++)
			{

				kry[i].Name = (sender as Button).Text;
				kry[i].BackColor = Color.FromArgb(128, 128, 0);
				kry[i].Text = (i - dayBegin + 1) + "";
				kry[i].Click += new EventHandler(ButtonClickPrintWeeek);
				// Выделяем сегодняшний день
				if (i == DateTime.Now.Day - 1 + dayBegin)
				{
					kry[i].BackColor = Color.FromArgb(128, 0, 128);
				}
			}

			for (int i = year[1].listMonth[DateTime.Now.Month].listDay.Count() + dayBegin - 1; i < 42; i++)
			{
				kry[i].Name = (sender as Button).Text;
				kry[i].Text = (i - year[1].listMonth[DateTime.Now.Month].listDay.Count() - dayBegin + 2) + "";
				kry[i].Click += new EventHandler(ButtonClickPrintWeeek);
			}
			foreach (Button element in listButton)
			{
				panRegMain.Controls.Remove(element);
			}

		}
		private void ButtonClickPrintWeeek(object sender, EventArgs e)
		{
			panWeekMain.Visible = true;
			panRegMain.Visible = false;

			dlaNikitki(int.Parse((sender as Label).Text), int.Parse((sender as Label).Name));


		}
		private void dlaNikitki(int day, int month)
		{

		}
	}
}




