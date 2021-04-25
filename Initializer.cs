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
	// Здесь хранятся методы, которые выводят что-либо на экран 		*В РАЗРАБОТКЕ*
	partial class MainForm
	{
		// Лейбл "удалить"
		internal static Label labDelete = Core.CreateLab(panWeekMain, 490, 5, 300, 40, 16);
		// Кнопка "Выйти"
		internal static Label labExit = Core.CreateLab(panWeekMain, 1200, 5, 70, 20, 10);
		// Отображение различных элементов
		internal void InitializeElements()
		{
			labDelete.Text = "удалить";
			labDelete.BackColor = Color.FromArgb(245, 162, 142);
			labDelete.Visible = false;
			labExit.Text = "Выйти";
			labExit.BackColor = Color.FromArgb(129, 202, 228);
			labExit.MouseClick += (MouseClick_labExit);
			labExit.MouseEnter += (MouseEnter_labExit);
			labExit.MouseLeave += (MouseLeave_labExit);
		}
		// Событие нажатия на кнопку "Выйти"
		internal void MouseClick_labExit(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				panRegMain.Visible = true;
				panWeekMain.Visible = false;
			}
		}
		
		// Событие наведения на кнопку "Выйти"
		internal void MouseEnter_labExit(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 208); }
		internal void MouseLeave_labExit(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 202, 228); }
	}
}




