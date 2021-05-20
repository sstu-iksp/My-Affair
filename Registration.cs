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
		// Панель входа и регистрации
		internal static Panel panRegMain = Core.CreatePan(0, 0, 1280, 720, Color.FromArgb(255, 216, 177));
		// Панель  формы входа
		internal static Panel panEnt = Core.CreatePan(panRegMain, 400, 150, 480, 420, Color.FromArgb(129, 212, 238));
		// Надпись "Добро пожаловать"
		internal Label labEnt = Core.CreateLab(panEnt, 5, 5, 470, 40, 16, "Добро пожаловать", Color.FromArgb(129, 212, 238));
		// Логин и Пароль
		internal Label labEntLog = Core.CreateLab(panEnt, 5, 100, 165, 20, 12, "Логин");
		internal Label labEntPar = Core.CreateLab(panEnt, 5, 200, 165, 20, 12, "Пароль");
		// Ввод для логина и пароля
		internal TextBox boxEntLog = Core.CreateBox(panEnt, 5, 125, 165, 20, 12);
		internal TextBox boxEntPar = Core.CreateBox(panEnt, 5, 225, 165, 20, 12);
		// Ошибки для логина, пароля и почты
		internal Label labEntLogErr = Core.CreateLab(panEnt, 175, 125, 250, 20, 10);
		internal Label labEntParErr = Core.CreateLab(panEnt, 175, 225, 250, 20, 10);
		// Кнопка "Войти"
		internal Label labEntEnter = Core.CreateLab(panEnt, 355, 375, 120, 40, 12, "Войти", Color.FromArgb(129, 222, 238));
		// Кнопка "Создать"
		internal Label labEntCreate = Core.CreateLab(panEnt, 5, 375, 120, 40, 12, "Создать", Color.FromArgb(129, 222, 238));
		
		// Метод для инициализаии регистрации
		internal void InitializeEnt()
		{
			Controls.Add(panRegMain);
			
			panRegMain.Visible = true;
			panWeekMain.Visible = false;
			
			panEnt.Visible = true;
			
			boxEntPar.PasswordChar = '*';
			
			labEntLogErr.Visible = false;
			labEntParErr.Visible = false;
			
			labEntLogErr.ForeColor = Color.Red;
			labEntParErr.ForeColor = Color.Red;
			
			labEntLogErr.TextAlign = ContentAlignment.MiddleLeft;
			labEntParErr.TextAlign = ContentAlignment.MiddleLeft;
			
			Core.EventAdd(labEntCreate, MouseClick_labEntCreate, MouseEnter_labEntEnter, MouseLeave_labEntEnter);
			Core.EventAdd(labEntEnter, MouseClick_labEntEnter, MouseEnter_labEntEnter, MouseLeave_labEntEnter);
			
			InitializeReg();
		}
		// Событие кнопки "Создать", которое должно проверять введенные поля и сверять данные с базой данных	*В РАЗРАБОТКЕ*
		internal void MouseClick_labEntCreate(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// Очищаем боксы
				boxEntLog.Text = "";
				boxEntPar.Text = "";
				
				labEntLogErr.Visible = false;
				labEntParErr.Visible = false;
				
				panReg.Visible = true;
				panEnt.Visible = false;
			}
		}
		
		// Событие кнопки "Войти", которое должно проверять введенные поля и сверять данные с базой данных
		internal void MouseClick_labEntEnter(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// Нули вход за администратора
				if (boxEntLog.Text != "0" && boxEntPar.Text != "0")
				{
					string login;
					string password;
					
					// Убираем лишние энтеры и пробелы
					login = boxEntLog.Text.Replace(" ", "");
					password = boxEntPar.Text.Replace(" ", "");
					// Меняем текст в боксах
					boxEntLog.Text = login;
					boxEntPar.Text = password;
					//Проверка логина
					if (login == "")
					{
						labEntLogErr.Visible = true;
						labEntLogErr.Text = "Заполните поле";
						return;
					}
					if (login.Length < 5)
					{
						labEntLogErr.Visible = true;
						labEntLogErr.Text = "Минимальная длина 5 символов";
						return;
					}
					labEntLogErr.Visible = false;
					
					//Проверка пароля
					if (password == "")
					{
						labEntParErr.Visible = true;
						labEntParErr.Text = "Заполните поле";
						return;
					}
					if (password.Length < 8)
					{
						labEntParErr.Visible = true;
						labEntParErr.Text = "Минимальная длина 8 символов";
						return;
					}
					labEntParErr.Visible = false;
					// Осуществляем проверку с базой данных
					int check = conn.CheckPass(login, password);
					if (check == 0)
					{
						labEntLogErr.Visible = true;
						labEntLogErr.Text = "Данный логин не существует";
						return;
					}
					if (check == 2)
					{
						labEntParErr.Visible = true;
						labEntParErr.Text = "Пароль неверный";
						return;
					}
					// Запоминаем ID пользователя и осуществляем чтение с базы данных
					conn.user = check;
					conn.Read(conn.user);
				}
				else
				{
					conn.user = 1;
					conn.Read(conn.user);
				}
				// Очищаем боксы
				boxEntLog.Text = "";
				boxEntPar.Text = "";
				
				panWeekMain.Visible = true;
				panRegMain.Visible = false;
				
				// Включаем таймер					// !!!***!!!
				timerDB.Enabled = true;
				// Отрисовываем неделю
				DrawWeek(ddd, mmm, false);
			}
		}
		
		// Событие наведения на кнопку "Войти"
		internal void MouseEnter_labEntEnter(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 208); }
		internal void MouseLeave_labEntEnter(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 222, 228); }
		
		
		
		// Панель  формы регистрации
		internal static Panel panReg = Core.CreatePan(panRegMain, 400, 150, 480, 420, Color.FromArgb(129, 212, 238));
		// Надпись "Добро пожаловать"
		internal Label labReg = Core.CreateLab(panReg, 5, 5, 470, 40, 16, "Регистрация", Color.FromArgb(129, 212, 238));
		// Логин, пароль и почта
		internal Label labRegLog = Core.CreateLab(panReg, 5, 100, 165, 20, 12, "Логин");
		internal Label labRegPar = Core.CreateLab(panReg, 5, 200, 165, 20, 12, "Пароль");
		internal Label labRegMail = Core.CreateLab(panReg, 5, 300, 165, 20, 12, "Почта");
		// Ввод для логина, пароля и почты
		internal TextBox boxRegLog = Core.CreateBox(panReg, 5, 125, 165, 20, 12);
		internal TextBox boxRegPar = Core.CreateBox(panReg, 5, 225, 165, 20, 12);
		internal TextBox boxRegMail = Core.CreateBox(panReg, 5, 325, 165, 20, 12);
		// Ошибки для логина, пароля и почты
		internal Label labRegLogErr = Core.CreateLab(panReg, 175, 125, 250, 20, 10);
		internal Label labRegParErr = Core.CreateLab(panReg, 175, 225, 250, 20, 10);
		internal Label labRegMailErr = Core.CreateLab(panReg, 175, 325, 250, 20, 10);
		// Кнопка "Создать"
		internal Label labRegCreate = Core.CreateLab(panReg, 355, 375, 120, 40, 12, "Создать", Color.FromArgb(129, 222, 238));
		// Кнопка "Войти"
		internal Label labRegEnter = Core.CreateLab(panReg, 5, 375, 120, 40, 12, "Войти", Color.FromArgb(129, 222, 238));
		// Метод для инициализаии регистрации
		internal void InitializeReg()
		{
			boxRegPar.PasswordChar = '*';
			
			labRegLogErr.Visible = false;
			labRegParErr.Visible = false;
			labRegMailErr.Visible = false;
			
			labRegLogErr.ForeColor = Color.Red;
			labRegParErr.ForeColor = Color.Red;
			labRegMailErr.ForeColor = Color.Red;
			
			labRegLogErr.TextAlign = ContentAlignment.MiddleLeft;
			labRegParErr.TextAlign = ContentAlignment.MiddleLeft;
			labRegMailErr.TextAlign = ContentAlignment.MiddleLeft;
			
			Core.EventAdd(labRegCreate, MouseClick_labRegCreate, MouseEnter_labRegEnter, MouseLeave_labRegEnter);
			Core.EventAdd(labRegEnter, MouseClick_labRegEnter, MouseEnter_labRegEnter, MouseLeave_labRegEnter);
		}
		// Событие кнопки "Создать", которое должно проверять введенные поля и сверять данные с базой данных
		internal void MouseClick_labRegCreate(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				string login;
				string password;
				string mail;
				
				// Убираем лишние энтеры и пробелы
				login = boxRegLog.Text.Replace(" ", "");
				password = boxRegPar.Text.Replace(" ", "");
				mail = boxRegMail.Text.Replace(" ", "");
				// Меняем текст в боксах
				boxRegLog.Text = login;
				boxRegPar.Text = password;
				boxRegMail.Text = mail;
				//Проверка логина
				if (login == "")
				{
					labRegLogErr.Visible = true;
					labRegLogErr.Text = "Заполните поле";
					return;
				}
				if (login.Length < 5)
				{
					labRegLogErr.Visible = true;
					labRegLogErr.Text = "Минимальная длина 5 символов";
					return;
				}
				labRegLogErr.Visible = false;
				
				//Проверка пароля
				if (password == "")
				{
					labRegParErr.Visible = true;
					labRegParErr.Text = "Заполните поле";
					return;
				}
				if (password.Length < 8)
				{
					labRegParErr.Visible = true;
					labRegParErr.Text = "Минимальная длина 8 символов";
					return;
				}
				labRegParErr.Visible = false;
				
				//Проверка почты
				if (mail == "")
				{
					labRegMailErr.Visible = true;
					labRegMailErr.Text = "Заполните поле";
					return;
				}
				if (mail.Length <= 5)
				{
					labRegMailErr.Visible = true;
					labRegMailErr.Text = "Неверная почта";
					return;
				}
				labRegMailErr.Visible = false;
				// Осуществляем проверку с базой данных
				int check = conn.Check(login, mail);
				if (check == 1)
				{
					labRegLogErr.Visible = true;
					labRegLogErr.Text = "Данный логин уже существует";
					return;
				}
				if (check == 2)
				{
					labRegMailErr.Visible = true;
					labRegMailErr.Text = "Данная почта уже существует";
					return;
				}
				
				// Записываем нового пользователя в бд
				conn.WriteUser(login, password, mail);
				
				// Очищаем боксы
				boxRegLog.Text = "";
				boxRegPar.Text = "";
				boxRegMail.Text = "";
				
				panEnt.Visible = true;
				panReg.Visible = false;
			}
		}
		// Событие кнопки "Войти"
		internal void MouseClick_labRegEnter(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// Очищаем боксы
				boxRegLog.Text = "";
				boxRegPar.Text = "";
				boxRegMail.Text = "";
				labRegLogErr.Visible = false;
				labRegParErr.Visible = false;
				labRegMailErr.Visible = false;
				
				panEnt.Visible = true;
				panReg.Visible = false;
			}
		}
		
		// Событие наведения на кнопку "Войти"
		internal void MouseEnter_labRegEnter(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 212, 208); }
		internal void MouseLeave_labRegEnter(object sender, EventArgs e) { (sender as Label).BackColor = Color.FromArgb(129, 222, 228); }
		
		
		
		
	}
}
