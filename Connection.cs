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
	public class Connection
	{
		public Connection()
		{
			
		}
		
		// Название базы данных
		internal String dbFileName;
		// Переменная для соединения
       	internal SQLiteConnection m_dbConn;
        // Переменная для команд
        internal SQLiteCommand m_sqlCmd;
        // Лейбл состояния соединения
        Label lbStatusText = new Label();
        // Для вывода таблицы
        DataGridView dgvViewer = new DataGridView();
		
        internal void Load()
        {
        	// Определяем соединение и команду
            m_dbConn = new SQLiteConnection();
            m_sqlCmd = new SQLiteCommand();
			// Задаем название и статус соединения
            dbFileName = "sample.sqlite";
            lbStatusText.Text = "Disconnected";     
            // Проверяем существует ли файл с базой данных
		    if (!File.Exists(dbFileName))
		        SQLiteConnection.CreateFile(dbFileName);
        }
        
        internal void Create()
		{
        	// Проверяем существует ли файл с базой данных
		    if (!File.Exists(dbFileName))
		        SQLiteConnection.CreateFile(dbFileName);
		    
		    try
		    {
		    	// Задаем соединение
		        m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
		        // Открываем соединение
		        m_dbConn.Open();
		        // Передаем подключение в команду
		        m_sqlCmd.Connection = m_dbConn;
				// Задаем текст запроса
		        m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Catalog (id INTEGER PRIMARY KEY AUTOINCREMENT, author TEXT, book TEXT)";
				// Выполняем запрос		        
		        m_sqlCmd.ExecuteNonQuery();
				// Показываем, что соединение установленно
		        lbStatusText.Text = "Connected";
		    }
		    catch (SQLiteException ex)
		    {
		        lbStatusText.Text = "Disconnected";
		        MessageBox.Show("Error: " + ex.Message);
		    }
		}
        
        internal void ReadAll()
		{
		    DataTable dTable = new DataTable();
		    String sqlQuery;
			// Проверяем наличие связи с бд
		    if (m_dbConn.State != ConnectionState.Open)
		    {
		        MessageBox.Show("Open connection with database");
		        return;
		    }
		    
		    try
		    {
		        sqlQuery = "SELECT * FROM Catalog";
		        SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
		        adapter.Fill(dTable);
		
		        if (dTable.Rows.Count > 0)
		        {
		            dgvViewer.Rows.Clear();
		
		            for (int i = 0; i < dTable.Rows.Count; i++)
		                dgvViewer.Rows.Add(dTable.Rows[i].ItemArray);
		        }
		        else
		            MessageBox.Show("Database is empty");
		    }
		    catch (SQLiteException ex)
		    {               
		        MessageBox.Show("Error: " + ex.Message);
		    }           
		}
        
        
        
	}
}




