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
	// Костя, напиши комменты
	
	public static class Сalendar
	{
		internal class Year
		{
			internal int yearInt;
			internal List<Month> listMonth = new List<Month>();
			
			internal Year(int year)
			{
				yearInt = year;
				for (int i = 1; i <= 12; i++)
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
				{
					listDay.Add(new Day(i));
				}
			}
			
		}
		
		internal class Day : MainForm.IObserver
		{
			internal int dayInt;			
			internal List<Case> cases = new List<Case>();
			
			internal Day(int day)
			{
				this.dayInt = day;
			}
			
			public void Update(MainForm.Day subject)
			{
				cases.Clear();
				// Переменные для записи значений
				string name = "";
				string time = "";
				string desc = "";
				
				foreach (Panel pan in subject.panCase)
				{
					foreach (Control ctrl in pan.Controls)
					{
						if (ctrl.TabIndex == 0)			name = ctrl.Text;
						else if (ctrl.TabIndex == 1)	time = ctrl.Text;
						else if (ctrl.TabIndex == 2)	desc = ctrl.Text;
					}
					Color colorCase = pan.Controls[0].BackColor;
					Color colorText = pan.Controls[0].ForeColor;
					
					cases.Add(new Case(name, time, desc, colorCase, colorText));
				}
			}
			
		}
		
		internal class Case
		{
			internal string nameCase;
			internal string lastTime;
			internal string description;
			// Цвет задачи
			internal Color colorCase;
			// Цвет текста
			internal Color colorText;
			// Приоритет задачи
			internal bool priority;
			
			internal Case(string nameCase, string lastTime, string description, Color colorCase, Color colorText)
			{
				this.nameCase = nameCase;
				this.lastTime = lastTime;
				this.description  = description;
				this.colorCase = colorCase;
				this.colorText = colorText;
			}
			
			internal void CaseRewrite(string name, string time, string desc)
			{
				nameCase = name;
				lastTime = time;
				description  = desc;
			}
			
			internal void CasePriority(bool b)
			{
				priority = b;
			}
			
		}
		
	}
}




