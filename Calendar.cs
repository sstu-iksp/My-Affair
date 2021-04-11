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
		internal class Day														// 		*В РАЗРАБОТКЕ*
		{
			internal string name;
			internal int panQ;
			internal Panel panDay;
			internal Label labDay;
			internal List<Panel> panCase = new List<Panel>();
			
			internal Day(string name, Panel panDay)
			{
				this.name = name;
				this.panDay = panDay;
				
				// Начальная настройка панели и надписи
				panDay.BackColor = Color.FromArgb(129, 212, 238);
				panDay.Visible = true;
				labDay = Core.CreateLab(this.panDay, 5, 5, 165, 20, 12);
				labDay.BackColor = Color.FromArgb(129, 222, 238);
				labDay.Text = this.name;
				labDay.Visible = true;
			}
			
			// Метод добавляющий новую задачу
			internal void caseAdd(Panel pan)
			{
				panCase.Add(pan);
				panQ++;
			}
			
			// Метод удаляющий задачу
			internal void caseRemove(Panel pan)
			{
				panCase.Remove(pan);
				panQ--;
			}
		}
	}
}




