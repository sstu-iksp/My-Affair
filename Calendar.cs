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
			internal Label labAddCase;
			internal List<Panel> panCase = new List<Panel>();
			
			internal Day(string name, Panel pan)
			{
				this.name = name;
				panDay = pan;
				
				// Начальная настройка панели и надписи
				panDay.BackColor = Color.FromArgb(129, 212, 238);
				panDay.Visible = true;
				labDay = Core.CreateLab(this.panDay, 5, 5, 165, 20, 12);
				labDay.BackColor = Color.FromArgb(129, 222, 238);
				labDay.Text = this.name;
				labAddCase  = Core.CreateLab(this.panDay, 5, panDay.Height - 32, panDay.Width - 10, 27, 18);
				labAddCase.BackColor = Color.FromArgb(129, 212, 228);
				labAddCase.Text = "+";
				labAddCase.TextAlign = ContentAlignment.MiddleCenter;
				
				labAddCase.MouseClick += (MouseClick_labAddCase);	// Нажатие			***
				
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
			
			// Событие нажатия на кнопку "Добавить (+)"
			internal void MouseClick_labAddCase(object sender, MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					if ((sender as Label) != null)
					{
						caseAdd(MainForm.Copy_Case(panDay, 3, 28 + panQ * 105));
					}
				}
			}
		}
	}
}




