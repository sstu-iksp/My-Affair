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
	// Здесь хранятся события, отвечающие за анимацию и ответ на действия пользователя 		*В РАЗРАБОТКЕ*

	partial class MainForm
	{
		// Переменная для проверки активности панели
		static bool _act;
		// Перемнная для определения режима заполнения задачи
		static bool _compl;
		// Переменная для запоминания активной панели
		static Panel _actP;
		static Panel _actP2;
		// Переменная для запоминания номера начального дня
		static int _beginDay;
		// Переменная для запоминания номера начальной задачи
		static int _beginCase;
		// Переменные для запоминания начальных координат панели (для теста)
		static int _actX;
		static int _actY;
		// Переменные для запоминания начальных координат на панели
		static int _startX;
		static int _startY;
		// Событие зажатия кнопки мыши на панели, отвечает за присвоение к активной панели, которая используется далее
		internal static void MouseDown_Case(object sender, MouseEventArgs e)
		{
			// Проверяем нажатие на кнопку
			if (e.Button == MouseButtons.Left && (sender as TextBox) == null && (sender as MaskedTextBox) == null && !_compl)
			{
				// Отображаем кнопку "удалить"
				labDelete.Visible = true;
				// Запоминаем начальное положение курсора для перемещения задачи
				_startX = e.X;
				_startY = e.Y;
				// Запоминаем панель и ее координаты, в условии проверяется нажатие на саму панель или ее составляющих
				if ((sender as Panel) != null) _actP = (sender as Panel);
				else if ((sender as Label) != null) _actP = (Panel)(sender as Label).Parent;
				// Запоминаем номер начального дня
				_beginDay = _actP.Parent.TabIndex;
				// Удаляем элемент из начальной колекции
				days[_beginDay].panCase.Remove(_actP);
				// Добавляем задачу на форму, для возможности перемещения вне начального дня
				_form.Controls.Add(_actP);
				// Сортируем список задач
				days[_beginDay].PanCaseRedraw();
				// Запоминаем начальные координаты задачи (на данный момент не используются)
				_actX = _actP.Left;
				_actY = _actP.Top;
				// Переносим на передний план
				_actP.BringToFront();
				// Показываем что панель активна
				_act = true;
			}
			if (e.Button == MouseButtons.Right && !_compl && !_act)
			{
				// Определяем панельку
				if ((sender as Panel) != null) _actP2 = (sender as Panel);
				else if ((sender as Label) != null) _actP2 = (Panel)(sender as Label).Parent;
				// Меняем местами Текстбоксы с Лейблами
				foreach (Control ctrl in _actP2.Controls)
					if ((ctrl as TextBox) != null || (ctrl as MaskedTextBox) != null)
						ctrl.BringToFront();
				// Запоминаем индексы панелей дня и задачи для дальнейшей возможности к ним обратиться
				_beginDay = _actP2.Parent.TabIndex;
				_beginCase = days[_beginDay].panCase.IndexOf(_actP2);
				// Объявляем о том, что происходит редактирование задачи
				_compl = true;
			}
		}
		// Событие срабатывающие во время отпускания кнопки, отвечает за конечное расположение панели
		internal static void MouseUp_Case(object sender, MouseEventArgs e)
		{
			bool cancel = true;
			bool b = true;

			if (e.Button == MouseButtons.Left && _act)
			{
				// После отпускания кнопки мыши проверяем местоположение панели относительно дней
				for (int i = 0; i < 7; i++)
				{
					if ((_actP.Left + _actP.Width / 2 > days[i].panDay.Left) && (_actP.Left + _actP.Width / 2 < days[i].panDay.Left + days[i].panDay.Width)
					   && (_actP.Top + _actP.Height / 2 > days[i].panDay.Top) && (_actP.Top + _actP.Height / 2 < days[i].panDay.Top + days[i].panDay.Height))
					{
						// Удаляем задачу из колекции начального дня
						_form.Controls.Remove(_actP);
						days[_beginDay].panCase.Remove(_actP);
						// Добавляем задачу в нужный день
						days[i].panDay.Controls.Add(_actP);

						for (int j = 0; j < days[i].panCase.Count(); j++)
						{
							if ((_actP.Top + _actP.Height / 2 < days[i].panCase[j].Top + days[i].panCase[j].Height))
							{
								// Добавляем на нужную панельку, отображаем и меняем высоту
								days[i].panCase.Insert(j, _actP);
								_actP.Left = 3;
								_actP.Top = labVoid.Top;
								b = false;
								break;
							}
						}
						if (b)
						{
							days[i].panCase.Insert(days[i].panCase.Count(), _actP);
							_actP.Left = 3;
							_actP.Top = days[i].posBot;
						}
						days[i].posBot += _actP.Height + 3;
						// Изменяем координату последней задачи
						days[_beginDay].posBot -= _actP.Height + 3;
						cancel = false;
						// Перерисовываем измененный список задач
						days[i].posLab = 0;
						days[i].PanCaseRedraw();
						// Перерисовываем список задач начального дня
						days[_beginDay].PanCaseRedraw();     // ***
						break;
					}
					if ((_actP.Top + _actP.Height / 2 < days[i].panDay.Top) || true)  // 'Пуф'	!!! true в проверке !!!
					{
						_form.Controls.Remove(_actP);
						days[_beginDay].panCase.Remove(_actP);
						days[_beginDay].posBot -= _actP.Height + 3;
						cancel = false;
						days[_beginDay].PanCaseRedraw();     // ***
					}
				}
				// Возвращаем задачу задачу обратно на начальный день если она не была перемещена	(выключено)
				if (cancel)
				{
					_form.Controls.Remove(_actP);
					days[_beginDay].panDay.Controls.Add(_actP);

					_actP.Left = _actX;
					_actP.Top = _actY;
				}
				// Скрываем кнопку "удалить"
				labDelete.Visible = false;
				// Скрываем пустой лейбл
				labVoid.Visible = false;
				_act = false;
				// ***
				for (int i = 0; i < 7; i++)
					days[i].labAddCase.Text = days[i].panCase.Count() + " ";        // ------------------- DEBAG
																					// ***
			}
		}
		// Событие перетаскивания панели в котором происходит изменение координат относительно движения курсора
		internal static void MouseMove_Case(object sender, MouseEventArgs e)
		{
			if (_act)
			{
				int b = -1;
				// Меняем местоположение задачи
				_actP.Left = _actP.Left + e.X - _startX;
				_actP.Top = _actP.Top + e.Y - _startY;
				// Отображаем пустой лейбл в зависимости от положения задачи
				for (int i = 0; i < 7; i++)
				{
					if ((_actP.Left + _actP.Width / 2 > days[i].panDay.Left) && (_actP.Left + _actP.Width / 2 < days[i].panDay.Left + days[i].panDay.Width)
					   && (_actP.Top + _actP.Height / 2 > days[i].panDay.Top) && (_actP.Top + _actP.Height / 2 < days[i].panDay.Top + days[i].panDay.Height))
					{
						for (int j = 0; j < days[i].panCase.Count(); j++)
						{
							if ((_actP.Top + _actP.Height / 2 < days[i].panCase[j].Top + days[i].panCase[j].Height))
							{
								if (labVoid.Top + labVoid.Height + 3 != days[i].panCase[j].Top)
								{
									// Добавляем на нужную панельку, отображаем и меняем высоту
									days[i].panDay.Controls.Add(labVoid);
									labVoid.Top = days[i].panCase[j].Top;
									days[i].posLab = labVoid.Top;
									labVoid.Visible = true;
								}
								b = i;
								break;
							}
						}
						if (b < 0)
						{
							// Добавляем на нужную панельку, отображаем и меняем высоту
							days[i].panDay.Controls.Add(labVoid);
							labVoid.Top = days[i].posBot;
							days[i].posLab = labVoid.Top;
							labVoid.Visible = true;
						}
						break;
					}
				}
				for (int i = 0; i < 7; i++)
				{
					// Сортировка всего ???
					if (b != i)
						days[i].posLab = 0;
					days[i].PanCaseRedraw();        // ***
				}
			}
		}
		// Метод для закрытия режима редактирования задачи
		internal static void MouseClick_Outside(object sender, MouseEventArgs e)
		{
			// Переменные для записи значений
			string name = "";
			string time = "";
			string desc = "";

			if (e.Button == MouseButtons.Left && _compl)
			{
				// Сначала считываем то, что записанно в Текстбоксах путем перебора всех контролов на панели
				foreach (Control ctrl in days[_beginDay].panCase[_beginCase].Controls)
				{
					if (ctrl.TabIndex == 3) name = ctrl.Text;
					else if (ctrl.TabIndex == 4) time = ctrl.Text;
					else if (ctrl.TabIndex == 5) desc = ctrl.Text;
				}
				// После переписываем считанные данные в Лейблы, также путем перебора всех контролов (не круто, но работает)
				foreach (Control ctrl in days[_beginDay].panCase[_beginCase].Controls)
				{
					if (ctrl.TabIndex == 0) ctrl.Text = name;
					else if (ctrl.TabIndex == 1) ctrl.Text = time;
					else if (ctrl.TabIndex == 2) ctrl.Text = desc;
				}
				// Далее меняем местами Лейблы с Текстбоксами
				foreach (Control ctrl in _actP2.Controls)
					if ((ctrl as Label) != null) ctrl.BringToFront();
				_compl = false;
			}
		}
		// Событие наведения на кнопку добавления новой задачи
		internal static void MouseEnter_labAddCase(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 208); }
		internal static void MouseLeave_labAddCase(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 228); }
	}
}




