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
	public static class Сalendar
	{
		// Класс имеет 00 поля и 00 метода, он нужен для создания дней, которые будут отображаться на экране просмотра недели
		// поля состоят из названия, кол-ва задач привязанных ко дню, панель дня, надпись названия, коллекция панелиц задач
		// методы позовляют добавить новую задачу и удалить задачу

		internal class Year
		{
			internal int yearInt;
			internal List<Month> listMonth = new List<Month>();

			internal Year(int year)
			{
				yearInt = year;
				for (int i = 0; i < 12; i++)
					listMonth.Add(new Month(i, yearInt));
			}

		}

		internal class Month
		{
			internal int monthInt;
			internal List<Day> listDay = new List<Day>();

			internal Month(int number, int year)
			{
				monthInt = number;

				for (int i = 0; i < DateTime.DaysInMonth(year, monthInt); i++)
					listDay.Add(new Day(i));
			}

		}

		internal class Day
		{
			internal int dayInt;
			internal List<Case> panCase = new List<Case>();

			internal Day(int day)
			{
				this.dayInt = day;
			}

		}

		internal class Case
		{
			internal string nameCase;
			internal string discription;
			internal string lastTime;
			internal string firstTime;

			internal Case(string nameCase, string discription, string lastTime, string firstTime)
			{
				this.nameCase = nameCase;
				this.discription = discription;
				this.lastTime = lastTime;
				this.firstTime = firstTime;
			}

		}



	}
}




