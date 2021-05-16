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
		internal static Panel panWeekMain = Core.CreatePan(0, 0, 1280, 720, Color.FromArgb(255, 216, 177));
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
			// Создаем форму
			form = Core.CreateWindow(this, 10, 10, 1280, 720, "My-Affair");
			// Включаем возможность отслеживания нажатия клавиш
			form.KeyPreview = true;
			// Цвет формы (скрыта)
			form.BackColor = Color.FromArgb(255, 216, 177);
			// Отображаем главную панель
			panWeekMain.Visible = true;
			Controls.Add(panWeekMain);
			// Инициализация недели
			InitializeWeek();
			// Инициализация регистрации/авторизации
			InitializeReg();
			// Инициализация различных элементов
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
		// Пустая панелька
		internal static Label labVoid = Core.CreateLab(panWeekMain, 3, 28, 170, 30, 10, Color.FromArgb(201, 201, 201));
		// Метод создающий дни недели, которые отображаются на экран, размер панельки дня 175x600 px
		internal void InitializeWeek()
		{
			// Здесь возможно будет чтение из базы данных
			//***																												// Временное заполнение классов
			year[1].listMonth[2].listDay[24].cases.Add(new Сalendar.Case("Тест", "11:11", "Да да я", Color.BlueViolet, Color.White));
			year[1].listMonth[3].listDay[23].cases.Add(new Сalendar.Case("Позавтракать", "09:00", "Чтобы вырасти, нужно хорошо питаться", Color.BlueViolet, Color.White));
			year[1].listMonth[3].listDay[23].cases.Add(new Сalendar.Case("Поужинать", "12:00", "Чтобы вырасти, нужно хорошо питаться x2", Color.BlueViolet, Color.White));
			year[1].listMonth[3].listDay[25].cases.Add(new Сalendar.Case("Пара", "09:45-11:15", "Пара по 1С", Color.BlueViolet, Color.White));
			year[1].listMonth[3].listDay[26].cases.Add(new Сalendar.Case("Пара", "13:40-15:10", "Памагите", Color.BlueViolet, Color.White));
			year[1].listMonth[3].listDay[26].cases.Add(new Сalendar.Case("Пара", "13:40-15:10", "222", Color.BlueViolet, Color.White));
			year[1].listMonth[3].listDay[26].cases.Add(new Сalendar.Case("Пара", "13:40-15:10", "333", Color.BlueViolet, Color.White));
			year[1].listMonth[3].listDay[29].cases.Add(new Сalendar.Case("Пара", "13:40-15:10", "Памагите", Color.BlueViolet, Color.White));
			//***
			
			for (int i = 0; i < 7; i++)
				dayWeek.Add(wn[i], i);
			// Определяем начальные день и месяц
			ddd = DateTime.Now.Day - dayWeek[DateTime.Now.DayOfWeek.ToString()];
			mmm = DateTime.Now.Month;
			// Создаем панели дней
			for (int i = 0; i < 7; i++)
			{
				days.Add(new Day("", Core.CreatePan(panWeekMain, 15 + i * 180, 50, 175, 600)));
				days[i].panDay.TabIndex = i;
			}
			// Отрисовыаем неделю
			DrawWeek(ddd, mmm, false);
			// Скрываем пустой лейбл
			labVoid.Visible = false;
		}
		// Метод перерисовывающий дни недели
		internal void DrawWeek(int dayNow, int monthNow, bool v)
		{
			int numYear = 1;
			int mon = 1;
			int numDay = dayNow - 1;
			
			for(int i = 0; i < 7; i++, numDay++)
			{
				// Очищаем контролы панели  и коллекцию задач				
				days[i].panCase.Clear();
					for (int j = days[i].panDay.Controls.Count - 1; 1 < j; j--)
						days[i].panDay.Controls.RemoveAt(j);
				// Сбрасываем цвет текущего дня
				days[i].labDay.BackColor = Color.FromArgb(129, 222, 238);
				// Если в момент обхода обращаемся к следующему месяцу
				if (year[numYear].listMonth[monthNow - mon].listDay.Count() <= numDay)
				{
					mon = 0;
					numDay = 0;
				}
				// Если в момент обхода обращаемся к следующему году
				if (year[numYear].listMonth.Count() <= monthNow - mon)	// Не работает !!!***!!!
				{
					mon = 12;
					numYear = 2;
				}
				// Меняем номер дня
				days[i].labDay.Text = wn[i] + " - " + (numDay + 1);
				// Записываем дату в класс календаря
				days[i].date = new DateTime(year[numYear].yearInt, monthNow - mon + 1, numDay + 1);
				// Заполняем дни недели на экране задачами из классов
				foreach (Сalendar.Case cs in year[numYear].listMonth[monthNow - mon].listDay[numDay].cases)	// С месяцем проблеммы тоже !!!
				{
					days[i].panCase.Add(days[i].Copy_Case(days[i].panDay, 3, days[i].posBot, cs.nameCase, cs.lastTime, cs.description, cs.colorCase, cs.colorText));
					days[i].posBot += days[i].panCase[0].Height + 3;		// Изменить индекс (0)	!!!
				}
				// Выделяем текущий день
				if (days[i].date.Year == DateTime.Now.Year && days[i].date.Month == DateTime.Now.Month && days[i].date.Day == DateTime.Now.Day)
					days[i].labDay.BackColor = Color.FromArgb(115, 160, 250);
				
				days[i].PanCaseRedraw();
			}
		}
		
		// Панель календаря
		internal static Panel panCalendar = Core.CreatePan(panWeekMain, 150, 150, 800, 600, Color.FromArgb(0, 180, 140));
		// Коллекции лейблов для календаря
		internal List<Label> dayLabel = new List<Label>();
		internal List<Label> weekLabel = new List<Label>();
		internal List<Label> listMonthLabels = new List<Label>();
		internal Label monthHeaderLabel = new Label();
		internal Label monthBackwardLabel = new Label();
		internal Label monthForwardLabel = new Label();
		// Коллекции названий
		internal List<string> listNameWeek = new List<string> { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вc" };
		internal List<string> listNameMonthLong = new List<string> { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
		internal List<string> listNameMonthShort = new List<string> { "Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек" };
		// Инициализация состовляющих календаря
		internal void InitializeCalendarView()
		{
			// Временное предупреждение
			Label warning = Core.CreateLab(panCalendar, 0, 0, 800, 40, 12);	// warning
			warning.Text = "Перюключение между годами так-же как и крайние месяца еще не работают!!!";
			
			// Создаем лейблы месяцев
			for (int i = 0, j = 0; i < 12; i++)
			{
				if (i == 4 || i == 8)	j++;
				
				listMonthLabels.Add(Core.CreateLab(panCalendar, 50 + i * 55 - j * 4 * 55, 100 + j * 55, 50, 50, 13, listNameMonthShort[i] + "", Color.FromArgb(88, 123, 224)));
				listMonthLabels[i].TabIndex = i;
				listMonthLabels[i].BringToFront();
				listMonthLabels[i].MouseClick+= (MouseClick_labMonth);
				// Выделяем текущий месяц
				if (year[1].yearInt == DateTime.Now.Year && i + 1 == DateTime.Now.Month)
				{
					listMonthLabels[i].BackColor = Color.FromArgb(128, 0, 128);
					listMonthLabels[i].MouseEnter += (MouseEnter_dayNow);
					listMonthLabels[i].MouseLeave += (MouseLeave_dayNow);
					continue;
				}
				listMonthLabels[i].MouseEnter += (MouseEnter_viewMonths);
				listMonthLabels[i].MouseLeave += (MouseLeave_viewMonths);
			}
			// Кнопка переключения на предыдущий месяц
			monthBackwardLabel = Core.CreateLab(panCalendar, 50, 40, 50, 50, 18, "<", Color.FromArgb(88, 123, 224));
			Core.EventAdd(monthBackwardLabel, MouseClick_backwardMonth, MouseEnter_viewMonths, MouseLeave_viewMonths);
			monthBackwardLabel.Visible = false;
			// Создаем лейбл названия месяца
			monthHeaderLabel = Core.CreateLab(panCalendar, monthBackwardLabel.Left + monthBackwardLabel.Width + 5, monthBackwardLabel.Top, 270, 50, 18, Color.FromArgb(88, 123, 224));
			Core.EventAdd(monthHeaderLabel, MouseClick_viewMonth, MouseEnter_viewMonths, MouseLeave_viewMonths);
			monthHeaderLabel.Visible = false;
			// Кнопка переключения на следующий месяц
			monthForwardLabel = Core.CreateLab(panCalendar, monthHeaderLabel.Left + monthHeaderLabel.Width + 5, monthHeaderLabel.Top, 50, 50, 18, ">", Color.FromArgb(88, 123, 224));
			Core.EventAdd(monthForwardLabel, MouseClick_forwardMonth, MouseEnter_viewMonths, MouseLeave_viewMonths);
			monthForwardLabel.Visible = false;
			// Создаем лейблы названий дней
			for (int i = 0; i < 7; i++)
			{
				weekLabel.Add(Core.CreateLab(panCalendar, monthBackwardLabel.Left + i * 55, monthBackwardLabel.Top + monthBackwardLabel.Height + 5, 50, 50, 18, listNameWeek[i], Color.FromArgb(128, 128, 128)));
				weekLabel[i].ForeColor = Color.FromArgb(102, 0, 0);
				weekLabel[i].Visible = false;
			}
			// Создаем лейблы дней
			for (int i = 0, j = 0; i < 42; i++)
			{
				if (i % 7 == 0 && i != 0)
					j++;
				
				dayLabel.Add(Core.CreateLab(panCalendar, weekLabel[0].Left + i * 55 - j * 385, weekLabel[0].Top + weekLabel[0].Height + 5 + j * 55, 50, 50, 18, Color.FromArgb(201, 201, 201)));
				dayLabel[i].Visible = false;
			}
		}
		// Метод отображающий дни месяца
		internal void DrawMonth(int index)
		{
			// Отображаем название месяца
			monthHeaderLabel.Text = listNameMonthLong[index];
			// Отключаем ненужные события
			for (int i = 0 ; i < 42; i++)
			{
				dayLabel[i].MouseClick -= (MouseClick_PrintWeek);
				dayLabel[i].MouseEnter -= (MouseEnter_viewDays);
				dayLabel[i].MouseLeave -= (MouseLeave_viewDays);
				dayLabel[i].MouseEnter -= (MouseEnter_dayNow);
				dayLabel[i].MouseLeave -= (MouseLeave_dayNow);
			}
			
			int day;
			// Определяем начальный день с которого начнется отображение
			int dayBegin = year[1].listMonth[index - 1].listDay.Count();
			dayBegin -= dayWeek[new DateTime(year[1].yearInt, index + 1, 1).DayOfWeek.ToString()];
			// Изменяем начальный день если текущий месяц начинается с понедельника
			if (year[1].listMonth[index - 1].listDay.Count() <= dayBegin)
				dayBegin = year[1].listMonth[index - 1].listDay.Count() - 7;
			// Отображаем дни предыдущего месяца
			for (day = 0; dayBegin < year[1].listMonth[index - 1].listDay.Count(); day++, dayBegin++)
			{
				dayLabel[day].TabIndex = index;
				dayLabel[day].Text = (dayBegin + 1) + "";
				dayLabel[day].BackColor = Color.FromArgb(100, 100, 100);
			}
			// Отображаем дни текущего месяца
			for (dayBegin = 0; dayBegin < year[1].listMonth[index].listDay.Count(); day++, dayBegin++)
			{
				dayLabel[day].TabIndex = index;
				dayLabel[day].Text = (dayBegin + 1) + "";
				// Выделяем сегодняшний день
				if (year[1].yearInt == DateTime.Now.Year && index + 1 == DateTime.Now.Month && dayBegin + 1 == DateTime.Now.Day)
				{
					dayLabel[day].BackColor = Color.FromArgb(128, 0, 128);
					Core.EventAdd(dayLabel[day], MouseClick_PrintWeek, MouseEnter_dayNow, MouseLeave_dayNow);
					continue;
				}
				dayLabel[day].BackColor = Color.FromArgb(125, 81, 237);
				Core.EventAdd(dayLabel[day], MouseClick_PrintWeek, MouseEnter_viewDays, MouseLeave_viewDays);
			}
			// Отображаем дни следующего месяца
			for (dayBegin = 0; day < 42; day++, dayBegin++)
			{
				dayLabel[day].TabIndex = index;
				dayLabel[day].Text = (dayBegin + 1) + "";
				dayLabel[day].BackColor = Color.FromArgb(100, 100, 100);
			}
			// Скрываем месяца и отображаем дни
			Core.VisibleList(listMonthLabels, false);
			monthBackwardLabel.Visible = true;
			monthHeaderLabel.Visible = true;
			monthForwardLabel.Visible = true;
			Core.VisibleList(weekLabel, true);
			Core.VisibleList(dayLabel, true);
		}
		// Нажатие на месяц
		internal void MouseClick_labMonth(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				monthBackwardLabel.TabIndex = (sender as Label).TabIndex - 1;
				monthForwardLabel.TabIndex = (sender as Label).TabIndex + 1;
				DrawMonth((sender as Label).TabIndex);
			}
		}
		// Нажатие на предыдущий месяц
		internal void MouseClick_backwardMonth(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				monthHeaderLabel.TabIndex = (sender as Label).TabIndex;
				monthForwardLabel.TabIndex = (sender as Label).TabIndex + 1;
				monthBackwardLabel.TabIndex = (sender as Label).TabIndex - 1;
				DrawMonth(monthHeaderLabel.TabIndex);
			}
		}
		// Нажатие на слудующий месяц
		internal void MouseClick_forwardMonth(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				monthBackwardLabel.TabIndex = (sender as Label).TabIndex - 1;
				monthHeaderLabel.TabIndex = (sender as Label).TabIndex;
				monthForwardLabel.TabIndex = (sender as Label).TabIndex + 1;
				DrawMonth(monthHeaderLabel.TabIndex);
			}
		}
		// Нажатие на название месяца
		internal void MouseClick_viewMonth(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				monthBackwardLabel.Visible = false;
				monthHeaderLabel.Visible = false;
				monthForwardLabel.Visible = false;
				Core.VisibleList(dayLabel, false);
				Core.VisibleList(weekLabel, false);
				Core.VisibleList(listMonthLabels, true);
			}
		}
		// Нажатие на день
		internal void MouseClick_PrintWeek(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				panCalendar.Visible = false;
				// Определяем начальный день с которого начнется отображение
				int dayBegin = dayWeek[new DateTime(year[1].yearInt, ((sender as Label).TabIndex) + 1, int.Parse((sender as Label).Text)).DayOfWeek.ToString()];
				// Определяем начальные день и месяц
				ddd = int.Parse((sender as Label).Text) - dayBegin;
				mmm = ((sender as Label).TabIndex) + 1;
				// Переходим к прошлому месяцу
				if (ddd <= 0)
				{
					mmm--;
					ddd = year[1].listMonth[mmm - 1].listDay.Count() + ddd;
				}
				// Отрисовыаем неделю
				DrawWeek(ddd, mmm, false);		
			}
		}
		
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




