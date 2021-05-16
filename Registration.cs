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
	// Форма регистрации 							*В РАЗРАБОТКЕ*
	
	partial class MainForm
	{
		// Панель регистрации
		internal static Panel panRegMain = Core.CreatePan(0, 0, 1280, 720, Color.FromArgb(255, 216, 177));
		// Панель  формы регистрации
		internal static Panel panReg = Core.CreatePan(panRegMain, 400, 150, 480, 420, Color.FromArgb(129, 212, 238));
		// Надпись "Добро пожаловать"
		internal Label labReg = Core.CreateLab(panReg, 5, 5, 470, 40, 16, "Добро пожаловать", Color.FromArgb(129, 212, 238));
		// Логин и Пароль
		internal Label labRegLog = Core.CreateLab(panReg, 5, 100, 165, 20, 12, "Логин");
		internal Label labRegPar = Core.CreateLab(panReg, 5, 200, 165, 20, 12, "Пароль");
		// Ввод для логина и пароля
		internal TextBox boxRegLog = Core.CreateBox(panReg, 5, 125, 165, 20, 12);
		internal TextBox boxRegPar = Core.CreateBox(panReg, 5, 225, 165, 20, 12);
		// Кнопка "Войти"
		internal Label labRegEnter = Core.CreateLab(panReg, 355, 375, 120, 40, 12, "Войти", Color.FromArgb(129, 222, 238));
		// Метод для инициализаии регистрации
		internal void InitializeReg()
		{
			Controls.Add(panRegMain);
			
			panRegMain.Visible = true;
			panWeekMain.Visible = false;
			
			panReg.Visible = true;
			
			boxRegPar.PasswordChar = '*';
			
			labRegEnter.MouseClick += (MouseClick_labRegEnter);
			labRegEnter.MouseEnter += (MouseEnter_labRegEnter);
			labRegEnter.MouseLeave += (MouseLeave_labRegEnter);
		}
		
		// Событие кнопки "Войти", которое должно проверять введенные поля и сверять данные с базой данных	*В РАЗРАБОТКЕ*
		internal void MouseClick_labRegEnter(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				panWeekMain.Visible = true;
				panRegMain.Visible = false;
			}
		}
		
		// Событие наведения на кнопку "Войти"
		internal void MouseEnter_labRegEnter(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 208); }
		internal void MouseLeave_labRegEnter(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 222, 228); }
		
	}
}
