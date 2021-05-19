using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace Construct
{
	// 																						В РАЗРАБОТКЕ !!!
	// Класс реализован по паттерну "Одиночка"
	sealed class Connection
	{
		// Название базы данных
		readonly string _dbFileName;
		// Переменная для соединения
		readonly SQLiteConnection _dbConn;
		// Переменная для команд
		readonly SQLiteCommand _sqlCmd;
		// Номер пользователя
		internal int user;
       	
		static Connection _instance;
		
		internal static Connection Instance
		{
			get
			{
				if (_instance == null)
					_instance = new Connection();
				return _instance; 
			}
		}
		
		Connection()
		{
			// Задаем название базы данных
			_dbFileName = "myaffair.sqlite";
		    // Задаем соединение
			_dbConn = new SQLiteConnection("Data Source=" + _dbFileName + ";Version=3;foreign keys=true;");
			// Определяем команду
			_sqlCmd = new SQLiteCommand();
			// Проверяем существует ли файл с базой данныx, и создаем в случае его отсутствия
			if (!File.Exists(_dbFileName))
				SQLiteConnection.CreateFile(_dbFileName);
			// Вызываем метод, который создаст начальные таблицы
			Create();
		}
		// Метод создающий таблицы
		void Create()
		{
			try
			{
				// Открываем соединение
				_dbConn.Open();
				// Передаем подключение в команду
				_sqlCmd.Connection = _dbConn;
				// Задаем текст запроса
				_sqlCmd.CommandText = 	"PRAGMA foreign_keys=on;" +
										"CREATE TABLE IF NOT EXISTS Cases(" +
										"idCase INTEGER PRIMARY KEY," +
										"nameCase TEXT," +
										"lastTime TEXT," +
										"description TEXT, " +
										"colorCase TEXT," +
										"colorText TEXT," +
										"priority INTEGER," +
										"date TEXT," +
										"userId INTEGER," +
										"FOREIGN KEY (userId) REFERENCES User(idUser));" +
										"CREATE TABLE IF NOT EXISTS User(" +
										"idUser INTEGER PRIMARY KEY," +
										"login TEXT," +
										"password TEXT," +
										"mail TEXT);";
				// Выполняем запрос
				_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}
		// Метод записывающий новую задачу в базу данных для вошедшего пользователя
		internal void WriteCase(Сalendar.Case cs, int year, int month, int day)
		{
			try
			{
				// Проверяем подключение к базе данных и открываем его в случае отсутсвия
				if (_dbConn.State != ConnectionState.Open)
					_dbConn.Open();
				
				string commandText;
				string colCase;
				string colText;
				string colDate;
				
				string[] str = cs.colorCase.ToString().Split(',');
				str[1] = str[1].Substring(str[1].IndexOf("=", StringComparison.Ordinal) + 1);
				str[2] = str[2].Substring(str[2].IndexOf("=", StringComparison.Ordinal) + 1);
				str[3] = str[3].Substring(str[3].IndexOf("=", StringComparison.Ordinal) + 1).TrimEnd(']');
				colCase = str[1] + ":" + str[2] + ":" + str[3];
				
				str = cs.colorText.ToString().Split(',');
				str[1] = str[1].Substring(str[1].IndexOf("=", StringComparison.Ordinal) + 1);
				str[2] = str[2].Substring(str[2].IndexOf("=", StringComparison.Ordinal) + 1);
				str[3] = str[3].Substring(str[3].IndexOf("=", StringComparison.Ordinal) + 1).TrimEnd(']');
				colText = str[1] + ":" + str[2] + ":" + str[3];
				
				colDate = year + ":" + month + ":" + day;
				
				commandText = "INSERT INTO Cases (idCase, nameCase, lastTime, description, colorCase, colorText, priority, date, userId) " +
				"VALUES (NULL, '" + cs.nameCase + "', '" + cs.lastTime + "', '" + cs.description + "', '" + colCase + "', '" + colText + "', " + 0 + ", '" + colDate + "', " + user + ")";
				
				// Задаем текст запроса
				_sqlCmd.CommandText = commandText;
				// Выполняем запрос
				_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{               
				MessageBox.Show("Error: " + ex.Message);
			}
		}
		// Метод записвающий нового пользователя в базу данных
		internal void WriteUser(string login, string password, string mail)
		{
			try
			{
				// Проверяем подключение к базе данных и открываем его в случае отсутсвия
				if (_dbConn.State != ConnectionState.Open)
					_dbConn.Open();
				
				// Задаем текст запроса
				_sqlCmd.CommandText = 	"INSERT INTO User (idUser, login, password, mail) " +
										"VALUES (NULL, '" + login + "', '" + password + "', '" + mail + "')";
				// Выполняем запрос
				_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{               
				MessageBox.Show("Error: " + ex.Message);
			}
		}
		// Метод проверяющий верность введенных данных ***, return: idUser - если данные почта или логин существуют и введенный пароль верен
		//															2 - если введенный пароль неверный
		internal int CheckPass(string login, string password)
		{
			try
			{
				// Проверяем подключение к базе данных и открываем его в случае отсутсвия
				if (_dbConn.State != ConnectionState.Open)
					_dbConn.Open();
				
				// Задаем текст запроса
				_sqlCmd.CommandText = 	"SELECT * FROM User";
				// Запускаем чтение данных
				using (SQLiteDataReader reader = _sqlCmd.ExecuteReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read()) 
						{
							var idUser = reader.GetValue(0);
							var loginR = reader.GetValue(1);
							var passwordR = reader.GetValue(2);
							var mailR = reader.GetValue(2);
							
							if (loginR.ToString() == login || mailR.ToString() == login)
							{
								if (passwordR.ToString() == password)
									return Convert.ToInt32(idUser);
								return 2;
							}
						}
					}
				}
				return 0;
			}
			catch (SQLiteException ex)
			{               
				MessageBox.Show("Error: " + ex.Message);
				return -1;
			}
		}
		// Метод проверяющий наличие существования логина и почты в базе данных, return: 0 - без совпадений, 1 - совпал логин, 2 - совпала почта
		internal int Check(string login, string mail)
		{
			try
			{
				// Проверяем подключение к базе данных и открываем его в случае отсутсвия
				if (_dbConn.State != ConnectionState.Open)
					_dbConn.Open();
				
				// Задаем текст запроса
				_sqlCmd.CommandText = 	"SELECT * FROM User";
				// Запускаем чтение данных
				using (SQLiteDataReader reader = _sqlCmd.ExecuteReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read()) 
						{
							var idUser = reader.GetValue(0);
							var loginR = reader.GetValue(1);
							var passwordR = reader.GetValue(2);
							var mailR = reader.GetValue(2);
							
							if (loginR.ToString() == login)
								return 1;
							if (mailR.ToString() == mail)
								return 2;
						}
					}
				}
				return 0;
			}
			catch (SQLiteException ex)
			{               
				MessageBox.Show("Error: " + ex.Message);
				return -1;
			}
		}
		// Метод считывающий задачи определенного пользователя
		internal void Read(int num)
		{
			try
			{
				// Проверяем подключение к базе данных и открываем его в случае отсутсвия
				if (_dbConn.State != ConnectionState.Open)
					_dbConn.Open();
				
				// Задаем текст запроса
				_sqlCmd.CommandText = 	"SELECT * FROM Cases WHERE userId='" + num + "'";
				// Запускаем чтение данных
				using (SQLiteDataReader reader = _sqlCmd.ExecuteReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read()) 
						{
							var idCase = reader.GetValue(0);
							var nameCase = reader.GetValue(1);
							var lastTime = reader.GetValue(2);
							var description = reader.GetValue(3);
							var colorCase = reader.GetValue(4);
							var colorText = reader.GetValue(5);
							var priority = reader.GetValue(6);
							var date = reader.GetValue(7);
							var userId = reader.GetValue(8);
							
							string[] colCase = colorCase.ToString().Split(':');
							string[] colText = colorText.ToString().Split(':');
							string[] colDate = date.ToString().Split(':');
							
							MainForm.year[1].listMonth[Convert.ToInt32(colDate[1])].listDay[Convert.ToInt32(colDate[2])].cases.Add(new Сalendar.Case(nameCase.ToString(), lastTime.ToString(), description.ToString(),
							                                                                      Color.FromArgb(Convert.ToInt32(colCase[0]), Convert.ToInt32(colCase[1]), Convert.ToInt32(colCase[2])),
							                                                                      Color.FromArgb(Convert.ToInt32(colText[0]), Convert.ToInt32(colText[1]), Convert.ToInt32(colText[2]))));
						}
					}
				}
			}
			catch (SQLiteException ex)
			{               
				MessageBox.Show("Error: " + ex.Message);
			}
		}
		// Метод удаляющий задачи определенного пользователя
		internal void Delete(int num)
		{
			try
			{
				// Проверяем подключение к базе данных и открываем его в случае отсутсвия
				if (_dbConn.State != ConnectionState.Open)
					_dbConn.Open();
				
				// Задаем текст запроса
				_sqlCmd.CommandText = "DELETE FROM Cases WHERE userId='" + num + "'";
				// Выполняем запрос
				_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{               
				MessageBox.Show("Error: " + ex.Message);
			}		
		}
		
	}
}




