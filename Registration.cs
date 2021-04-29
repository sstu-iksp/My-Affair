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
		static Panel panRegMain = Core.CreatePan(0, 0, 1280, 720);
		// Панель  формы регистрации
		static Panel panReg = Core.CreatePan(panRegMain, 400, 150, 480, 420);
		// Название
		Label labReg = Core.CreateLab(panReg, 5, 5, 470, 40, 16);
		// Логин и Пароль
		Label labRegLog = Core.CreateLab(panReg, 5, 100, 165, 20, 12);
		Label labRegPar = Core.CreateLab(panReg, 5, 200, 165, 20, 12);
		// Ввод для логина и пароля
		TextBox boxRegLog = Core.CreateBox(panReg, 5, 125, 165, 20, 12);
		TextBox boxRegPar = Core.CreateBox(panReg, 5, 225, 165, 20, 12);
		// Кнопка "Войти"
		Label labRegEnter = Core.CreateLab(panReg, 355, 375, 120, 40, 12);
		
		// Метод для инициализаии регистрации
		internal void InitializeReg()
		{
			Controls.Add(panRegMain);
			// Цвет панельки регистрации
			panRegMain.BackColor = Color.FromArgb(255, 216, 177);
			
			panRegMain.Visible = true;
			panWeekMain.Visible = false;
							
			panReg.BackColor = Color.FromArgb(129, 212, 238);
			panReg.Visible = true;
			
			labReg.BackColor = Color.FromArgb(129, 212, 238);
			labReg.Text = "Добро пожаловать";
			
			labRegLog.Text = "Логин";
			labRegPar.Text = "Пароль";
			
			boxRegPar.PasswordChar = '*';
			
			labRegEnter.BackColor = Color.FromArgb(129, 222, 238);
			labRegEnter.Text = "Войти";
			
			labRegEnter.MouseClick += (MouseClick_labRegEnter);
			labRegEnter.MouseEnter += (MouseEnter_labRegEnter);
			labRegEnter.MouseLeave += (MouseLeave_labRegEnter);
			
		//	InitializeWeekCal();	// *******
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
