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
		// Класс отвечающий за ввод дней недели на экран
		internal class Day											// 		*В РАЗРАБОТКЕ*
		{
			// Координаты для прокрутки задач
			internal int posTop = 28;
			internal int posBot = 28;
			internal int posLab;
			// Панелька дня
			internal Panel panDay;
			// Название дня
			internal Label labDay;
			// Кнопка добавить
			internal Label labAddCase;
			// Коллекция с задачами
			internal List<Panel> panCase = new List<Panel>();
			
			internal Day(string name, Panel pan)
			{
				panDay = pan;
				// Начальная настройка панели и надписи
				panDay.BackColor = Color.FromArgb(129, 212, 238);
				panDay.Visible = true;
				// Название дня
				labDay = Core.CreateLab(this.panDay, 0, 0, panDay.Width, 28, 12);
				labDay.BackColor = Color.FromArgb(129, 222, 238);
				labDay.Text = name;
				// Кнопка добавить
				labAddCase = Core.CreateLab(this.panDay, 0, panDay.Height - 28, panDay.Width, 28, 18);
				labAddCase.BackColor = Color.FromArgb(129, 212, 228);
				labAddCase.Text = "+";
				labAddCase.TextAlign = ContentAlignment.MiddleCenter;
				// Нажатие на кнопку "Добавить"
				labAddCase.MouseClick += (MouseClick_labAddCase);
				// Прокрутка задач колесиком мыши
				panDay.MouseWheel += (MouseWheel_Case);
			}
			// Метод добавляющий новую задачу
			internal void caseAdd(Panel pan)
			{
				panCase.Add(pan);
				posBot += pan.Height + 3;
			}
			// Метод удаляющий задачу
			internal void caseRemove(Panel pan)
			{
				panCase.Remove(pan);
				posBot -= pan.Height + 3;
			}
			// Событие нажатия на кнопку "Добавить (+)"
			internal void MouseClick_labAddCase(object sender, MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					//  Здесь будет метод, который создает пустую задачу с возможность ее заполнения
					caseAdd(Copy_Case(panDay, 3, posBot, "NameTest", "18-00", "Des keku keku"));
					year.listMonth[0].listDay[0].cases.Add(new Сalendar.Case("NameTest", "18-00", "Des keku keku"));
					// ***
					labAddCase.Text = panCase.Count() + " ";				// -------------------------------------------------- DEBAG
					// ***
				}
			}
			// Временный метод позволяющий копировать задачу, а точнее создать пустую
			internal Panel Copy_Case(Panel pan, int x, int y, string name, string time, string desc)
			{
		//		panCase.Add(Core.CreatePan(pan, x, y, 170, 100));
				
				Panel p = Core.CreatePan(pan, x, y, 170, 100);	// ***
				
				Label labCaseNameT = Core.CreateLab(p, 5, 5, 105, 20, 11);
				Label labCaseTimeT = Core.CreateLab(p, 110, 5, 55, 20, 11);
				Label labCaseDescT = Core.CreateLab(p, 5, 26, 160, 70, 9);
				
				TextBox boxCaseNameT = Core.CreateBox(p, 5, 5, 105, 20, 10, true);
			//	TextBox boxCaseTimeT = Core.CreateBox(p, 110, 5, 55, 20, 11, true);
				MaskedTextBox boxCaseTimeT = Core.CreateMasBox(p, 110, 5, 55, 20, 8, true);	// Проблемы с размерами (Высота независима)
				TextBox boxCaseDescT = Core.CreateBox(p, 5, 26, 160, 70, 9, true);
				
				labCaseNameT.TabIndex = 0;
				labCaseTimeT.TabIndex = 1;
				labCaseDescT.TabIndex = 2;
				boxCaseNameT.TabIndex = 3;
				boxCaseTimeT.TabIndex = 4;
				boxCaseDescT.TabIndex = 5;
				
				boxCaseNameT.BackColor = Color.FromArgb(133, 238, 176);
				boxCaseNameT.MaxLength = 10;
				boxCaseTimeT.BackColor = Color.FromArgb(133, 238, 176);
				boxCaseTimeT.Mask = "00:00-00:00";
				boxCaseDescT.BackColor = Color.FromArgb(133, 238, 176);
				boxCaseDescT.MaxLength = 50;
				
				p.BackColor = Color.FromArgb(133, 238, 186);
				
				labCaseNameT.BackColor = Color.FromArgb(133, 248, 186);
				labCaseTimeT.BackColor = Color.FromArgb(133, 238, 176);
				labCaseDescT.BackColor = Color.FromArgb(133, 228, 166);
				boxCaseNameT.ForeColor = Color.White;
				boxCaseTimeT.ForeColor = Color.White;
				boxCaseDescT.ForeColor = Color.White;
				
				labCaseNameT.TextAlign = ContentAlignment.MiddleLeft;
				labCaseTimeT.TextAlign = ContentAlignment.MiddleRight;
				labCaseDescT.TextAlign = ContentAlignment.TopLeft;
				
				labCaseNameT.Text = name;
				labCaseTimeT.Text = time;
				labCaseDescT.Text = desc;
				boxCaseNameT.Text = name;
				boxCaseTimeT.Text = time;
				boxCaseDescT.Text = desc;
				
				// Присваиваем события для панели и ее составляющих
				p.MouseMove += (MouseMove_Case);
				p.MouseDown += (MouseDown_Case);
				p.MouseUp += (MouseUp_Case);
				
				labCaseNameT.MouseMove += (MouseMove_Case);
				labCaseNameT.MouseDown += (MouseDown_Case);
				labCaseNameT.MouseUp += (MouseUp_Case);
				
				labCaseTimeT.MouseMove += (MouseMove_Case);
				labCaseTimeT.MouseDown += (MouseDown_Case);
				labCaseTimeT.MouseUp += (MouseUp_Case);
				
				labCaseDescT.MouseMove += (MouseMove_Case);
				labCaseDescT.MouseDown += (MouseDown_Case);
				labCaseDescT.MouseUp += (MouseUp_Case);
				
				boxCaseNameT.MouseDown += (MouseDown_Case);
				boxCaseTimeT.MouseDown += (MouseDown_Case);
				boxCaseDescT.MouseDown += (MouseDown_Case);
				
				p.MouseClick += (MouseClick_Outside);				// Для заполнения задачи	#
				labCaseNameT.MouseClick += (MouseClick_Outside);	// Для заполнения задачи	#
				labCaseTimeT.MouseClick += (MouseClick_Outside);	// Для заполнения задачи	#
				labCaseDescT.MouseClick += (MouseClick_Outside);	// Для заполнения задачи	#
				
				p.Visible = true;
				
		//		panCase.Add(p);
		
				return p;
			}
			// Метод перерисовывающий весь список задач
			internal void panCaseRedraw()
			{
				posBot = posTop;
				foreach (Panel pan in panCase)
				{
					if (posBot == posLab)					// ***
						posBot += labVoid.Height + 3;
					pan.Top = posBot;
					posBot += pan.Height + 3;
				}
			}
			// Метод переписывающий задачу
			internal void panCaseRewrite()
			{
				
			}
			// Прокрутка списка задач колесиком мыши
			internal void MouseWheel_Case(object sender, MouseEventArgs e)
			{
				// '20' - скорость прокрутки, чем больше значение тем медленнее скорость
				if (panCase.LastOrDefault() != null)
				{
					int casesH = labDay.Height + labAddCase.Height;
					foreach (Panel pan in panCase)
						casesH += pan.Height + 3;
										
					// Ограничение на прокрутку вверх если достигли границы и прокрутку вниз, если кол-во задач не выходит за рамки
					if (0 < e.Delta && panCase[0].Top < labDay.Top + labDay.Height)
					{
						foreach (Panel pan in panCase)
							pan.Location = new Point(pan.Location.X, pan.Location.Y + e.Delta / 20);
					}
					else if (e.Delta < 0 && panDay.Height < casesH && labAddCase.Top < panCase.Last().Top + panCase.Last().Height)	// Нужно добавить ограничение на прокрутку вниз !!!***!!!
					{
						foreach (Panel pan in panCase)
							pan.Location = new Point(pan.Location.X, pan.Location.Y + e.Delta / 20);
					}
					// Запоминаем нижнюю границу последней задачи
					posBot = panCase.LastOrDefault().Top + panCase.LastOrDefault().Height + 3;
				}
				// ***
				labAddCase.Text = panCase.Count() + " ";		// ------------------------------- DEBAG
				// ***
			}
			
		}//internal class Day	
	}//partial class MainForm
}//namespace Construct




