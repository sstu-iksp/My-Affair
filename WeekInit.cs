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
			// Переменная для запоминания даты
			internal DateTime date;
			// Координаты для прокрутки задач
			internal int posTop;
			internal int posBot;
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
				
				labAddCase.MouseEnter += (MouseEnter_labAddCase);	// Наведение			***
				labAddCase.MouseLeave += (MouseLeave_labAddCase);	// Наведение			***
				
				panDay.MouseClick += (MouseClick_Outside);			// Для заполнения задачи	#
				labDay.MouseClick += (MouseClick_Outside);			// Для заполнения задачи	#
				labAddCase.MouseClick += (MouseClick_Outside);		// Для заполнения задачи	#
				
				posTop = labDay.Height;
				posBot = labDay.Height;
			}
			// Метод добавляющий новую задачу
			internal void CaseAdd(Panel pan)
			{
				panCase.Add(pan);
				posBot += pan.Height + 3;
			}
			// Метод удаляющий задачу
			internal void CaseRemove(Panel pan)
			{
				panCase.Remove(pan);
				posBot -= pan.Height + 3;
			}
			// Событие нажатия на кнопку "Добавить (+)"
			internal void MouseClick_labAddCase(object sender, MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left && !compl)
				{
					//  Здесь будет метод, который создает пустую задачу с возможность ее заполнения
					CaseAdd(Copy_Case(panDay, 3, posBot, "NameCase", "00:00", "Description", Color.FromArgb(150, 0, 250), Color.FromArgb(255, 255, 255)));
					year[1].listMonth[date.Month - 1].listDay[date.Day - 1].cases.Add(new Сalendar.Case("NameCase", "00:00", "Description", Color.FromArgb(150, 0, 250), Color.FromArgb(255, 255, 255)));		// <<< ###
					// ***
					labAddCase.Text = panCase.Count() + " ";				// -------------------------------------------------- DEBAG
					// ***
				}
			}
			// Временный метод позволяющий копировать задачу, а точнее создать пустую
			internal Panel Copy_Case(Panel pan, int x, int y, string name, string time, string desc, Color colorCase, Color colorText)
			{
				Panel p = Core.CreatePan(pan, x, y, 170, 100, Color.Black);
				// Лейблы названия, времени и описания задачи
				Label labCaseNameT = Core.CreateLab(p, 1, 1, 115, 20, 11, name, colorCase);
				Label labCaseTimeT = Core.CreateLab(p, 117, 1, 52, 20, 10, time, colorCase);
				Label labCaseDescT = Core.CreateLab(p, 1, 22, 168, 77, 10, desc, colorCase);
				// Текстбоксы для редактирования названия, времени и описания задачи
				TextBox boxCaseNameT = Core.CreateBox(p, 1, 1, 115, 20, 10, true);
				MaskedTextBox boxCaseTimeT = Core.CreateMasBox(p, 117, 1, 52, 20, 8, true);	// Проблемы с размерами (Высота зависит от шрифта)
				TextBox boxCaseDescT = Core.CreateBox(p, 1, 22, 168, 77, 10, true);
				// Устанавливаем индексы для дальнейшего использования во время редактирования текста
				labCaseNameT.TabIndex = 0;
				labCaseTimeT.TabIndex = 1;
				labCaseDescT.TabIndex = 2;
				boxCaseNameT.TabIndex = 3;
				boxCaseTimeT.TabIndex = 4;
				boxCaseDescT.TabIndex = 5;
				
				boxCaseNameT.BackColor = colorCase;
				boxCaseNameT.ForeColor = colorText;
				boxCaseNameT.MaxLength = 12;
				boxCaseTimeT.BackColor = colorCase;
				boxCaseTimeT.ForeColor = colorText;
				boxCaseTimeT.Mask = "00:00";
				boxCaseDescT.BackColor = colorCase;
				boxCaseDescT.ForeColor = colorText;
				boxCaseDescT.MaxLength = 60;
				
				labCaseNameT.TextAlign = ContentAlignment.MiddleLeft;
			//	labCaseTimeT.TextAlign = ContentAlignment.MiddleRight;
				labCaseDescT.TextAlign = ContentAlignment.TopLeft;
				
				boxCaseNameT.Text = name;
				boxCaseTimeT.Text = time;
				boxCaseDescT.Text = desc;
				// Присваиваем события для панели и ее составляющих
				Core.EventAdde(p, MouseMove_Case, MouseDown_Case, MouseUp_Case);
				// События для возможности выбора задачи
				Core.EventAdde(labCaseNameT, MouseMove_Case, MouseDown_Case, MouseUp_Case);
				Core.EventAdde(labCaseTimeT, MouseMove_Case, MouseDown_Case, MouseUp_Case);
				Core.EventAdde(labCaseDescT, MouseMove_Case, MouseDown_Case, MouseUp_Case);
				// События для возможности выбора задачи
				boxCaseNameT.MouseDown += (MouseDown_Case);			// Зачем ???
				boxCaseTimeT.MouseDown += (MouseDown_Case);
				boxCaseDescT.MouseDown += (MouseDown_Case);
				// События для выхода из режима заполнения задачи
				p.MouseClick += (MouseClick_Outside);
				labCaseNameT.MouseClick += (MouseClick_Outside);
				labCaseTimeT.MouseClick += (MouseClick_Outside);
				labCaseDescT.MouseClick += (MouseClick_Outside);
				
				p.Visible = true;
				return p;
			}
			// Метод перерисовывающий весь список задач
			internal void PanCaseRedraw()
			{
				posBot = posTop;
				foreach (Panel pan in panCase)
				{
					if (posBot == posLab)
						posBot += labVoid.Height + 3;
					pan.Top = posBot;
					posBot += pan.Height + 3;
				}
			}
			// Прокрутка списка задач колесиком мыши
			internal void MouseWheel_Case(object sender, MouseEventArgs e)
			{
				// '20' - скорость прокрутки, чем больше значение тем медленнее скорость
				if (panCase.LastOrDefault() != null && !compl)
				{
					// Переменная для подсчета места занимаемого задачами
					int casesH = labDay.Height + labAddCase.Height;
					foreach (Panel pan in panCase)
						casesH += pan.Height + 3;
					// Ограничения на прокрутку вверх если достигли границы и прокрутку вниз, если кол-во задач не выходит за рамки
					if (0 < e.Delta && panCase[0].Top < labDay.Top + labDay.Height)
					{
						foreach (Panel pan in panCase)
							pan.Location = new Point(pan.Location.X, pan.Location.Y + e.Delta / 20);
					}
					else if (e.Delta < 0 && panDay.Height < casesH && labAddCase.Top < panCase.Last().Top + panCase.Last().Height)
					{
						foreach (Panel pan in panCase)
							pan.Location = new Point(pan.Location.X, pan.Location.Y + e.Delta / 20);
					}
					// Запоминаем нижнюю границу последней задачи
					posBot = panCase.LastOrDefault().Top + panCase.LastOrDefault().Height + 3;
				}
				// ***
				labAddCase.Text = panCase.Count() + " ";											// ------------------------------- DEBAG
				// ***
			}
			
		}//internal class Day	
	}//partial class MainForm
}//namespace Construct




