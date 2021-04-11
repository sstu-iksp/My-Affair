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
		List<Panel> weekday = new List<Panel>();
		List<Label> weekdayName = new List<Label>();
		string[] wn = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
		
		// Метод отображающий дни недели на экран, (заменен)
		internal void InitializeWeek()
		{
			panMidPanel.Visible = true;
			Controls.Add(panMidPanel);
			
			for(int i = 0; i < 7; i++)
			{
				weekday.Add(Core.CreatePan(panMidPanel, 15 + i * 180, 100, 175, 400));
				weekdayName.Add(Core.CreateLab(weekday[i], 5, 5, 165, 20, 12));
				
				weekday[i].BackColor = Color.FromArgb(129, 212, 238);
				weekday[i].Visible = true;
				weekday[i].MouseMove += (MouseMove_pmp);	//************
				weekday[i].MouseWheel += (MouseWheel_pmp);	//************
				
				weekdayName[i].BackColor = Color.FromArgb(129, 222, 238);
				weekdayName[i].Text = wn[i];
				weekdayName[i].Visible = true;
			}
		}
		
		
		
		static Panel panCasePanel = Core.CreatePan(panMidPanel, 1100, 590, 170, 100);
		Label labCaseName = Core.CreateLab(panCasePanel, 5, 5, 105, 20, 11);
		Label labCaseTime = Core.CreateLab(panCasePanel, 110, 5, 55, 20, 11);
		Label labCaseDesc = Core.CreateLab(panCasePanel, 5, 26, 160, 70, 9);
		
		// Устанавливаем тестовую задачу
		internal void InitializeCase()
		{
			panCasePanel.BackColor = Color.FromArgb(133, 238, 186);
			
			labCaseName.BackColor = Color.FromArgb(133, 248, 186);
			labCaseTime.BackColor = Color.FromArgb(133, 238, 176);
			labCaseDesc.BackColor = Color.FromArgb(133, 228, 166);
			
			labCaseName.TextAlign = ContentAlignment.MiddleLeft;
			labCaseTime.TextAlign = ContentAlignment.MiddleRight;
			labCaseDesc.TextAlign = ContentAlignment.TopLeft;
			
			labCaseName.Text = "Name";
			labCaseTime.Text = "Time";
			labCaseDesc.Text = "Description\nyep";
			
			panCasePanel.Visible = true;
			
			// Присваиваем события для панели и ее составляющих
			panCasePanel.MouseMove += (MouseMove_Case);
			labCaseName.MouseMove += (MouseMove_Case);
			labCaseTime.MouseMove += (MouseMove_Case);
			labCaseDesc.MouseMove += (MouseMove_Case);
			panCasePanel.MouseMove += (MouseDown_Case);
			labCaseName.MouseDown += (MouseDown_Case);
			labCaseTime.MouseDown += (MouseDown_Case);
			labCaseDesc.MouseDown += (MouseDown_Case);
			panCasePanel.MouseUp += (MouseUp_Case);
			labCaseName.MouseUp += (MouseUp_Case);
			labCaseTime.MouseUp += (MouseUp_Case);
			labCaseDesc.MouseUp += (MouseUp_Case);
		}
	}
}




