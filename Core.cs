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
	// Методы для создания разных объектов Control
	static class Core					
	{
		// Создание формы
		internal static Form CreateWindow(Form form, int left, int top, int width, int height, string caption)
		{
			form.Left = left;
			form.Top = top;
			form.ClientSize = new Size(width, height);
			form.Text = caption;
			
			return form;
		}
		// Создание панели
		internal static Panel CreatePan(int left, int top, int width, int height)
		{
			Panel pan = new Panel();
			pan.Left = left;
			pan.Top = top;
			pan.Width = width;
			pan.Height = height;
			pan.BackgroundImageLayout = ImageLayout.Stretch;
			
			pan.Visible = false;
			
			return pan;
		}
		// Создание панели с возможностью добавить цвет
		internal static Panel CreatePan(int left, int top, int width, int height, Color color)
		{
			Panel pan = new Panel();
			pan.Left = left;
			pan.Top = top;
			pan.Width = width;
			pan.Height = height;
			pan.BackgroundImageLayout = ImageLayout.Stretch;
			pan.BackColor = color;
			
			pan.Visible = false;
			
			return pan;
		}
		// Создание панели привязанной к другой панели
		internal static Panel CreatePan(Panel panel, int left, int top, int width, int height)
		{
			Panel pan = new Panel();
			pan.Left = left;
			pan.Top = top;
			pan.Width = width;
			pan.Height = height;
			pan.BackgroundImageLayout = ImageLayout.Stretch;
			pan.Parent = panel;
			pan.BackColor = Color.Transparent;
			
			pan.Visible = false;
			
			panel.Controls.Add(pan);
			return pan;
		}
		// Создание панели привязанной к другой панели с возможностью добавить цвет
		internal static Panel CreatePan(Panel panel, int left, int top, int width, int height, Color color)
		{
			Panel pan = new Panel();
			pan.Left = left;
			pan.Top = top;
			pan.Width = width;
			pan.Height = height;
			pan.BackgroundImageLayout = ImageLayout.Stretch;
			pan.Parent = panel;
			pan.BackColor = color;
			
			pan.Visible = false;
			
			panel.Controls.Add(pan);
			return pan;
		}
		// Создание PictureBox
		internal static PictureBox CreatePB(Panel pan, int left, int top, int width, int height)
		{
			PictureBox pb = new PictureBox();
			
			pb.Left = left;
			pb.Top = top;
			pb.Width = width;
			pb.Height = height;
			pb.SizeMode = PictureBoxSizeMode.StretchImage;
			
			pan.Controls.Add(pb);
			return pb;
		}
		// Создание PictureBox с прозрачным фоном
		internal static PictureBox CreatePB(Panel pan, int left, int top, int width, int height, bool b)
		{
			PictureBox pb = new PictureBox();
			pb.Left = left;
			pb.Top = top;
			pb.Width = width;
			pb.Height = height;
			pb.SizeMode = PictureBoxSizeMode.StretchImage;
			pb.Parent = pan;
			pb.BackColor = Color.Transparent;
			
			pan.Controls.Add(pb);
			return pb;
		}
		// Создание лейбла
		internal static Label CreateLab(Panel pan, int left, int top, int width, int height, float f)
		{
			Label lab = new Label();
			
			lab.Left = left;
			lab.Top = top;
			lab.Width = width;
			lab.Height = height;
			lab.ForeColor = Color.FromArgb(255, 255, 255);
			lab.TextAlign = ContentAlignment.MiddleCenter;
			lab.Font = new Font("Microsoft Sans Serif", f, FontStyle.Bold, GraphicsUnit.Point, 204);
			
			pan.Controls.Add(lab);
			return lab;
		}
		// Создание лейбла с текстом
		internal static Label CreateLab(Panel pan, int left, int top, int width, int height, float f, string text)
		{
			Label lab = new Label();
			
			lab.Left = left;
			lab.Top = top;
			lab.Width = width;
			lab.Height = height;
			lab.ForeColor = Color.FromArgb(255, 255, 255);
			lab.TextAlign = ContentAlignment.MiddleCenter;
			lab.Font = new Font("Microsoft Sans Serif", f, FontStyle.Bold, GraphicsUnit.Point, 204);
			lab.Text = text;
			
			pan.Controls.Add(lab);
			return lab;
		}
		// Создание лейбла с текстом и возможностью выбрать цвет
		internal static Label CreateLab(Panel pan, int left, int top, int width, int height, float f, string text, Color color)
		{
			Label lab = new Label();
			
			lab.Left = left;
			lab.Top = top;
			lab.Width = width;
			lab.Height = height;
			lab.ForeColor = Color.FromArgb(255, 255, 255);
			lab.TextAlign = ContentAlignment.MiddleCenter;
			lab.Font = new Font("Microsoft Sans Serif", f, FontStyle.Bold, GraphicsUnit.Point, 204);
			lab.BackColor = color;
			lab.Text = text;
			
			pan.Controls.Add(lab);
			return lab;
		}
		// Создание лейбла с возможностью выбрать цвет
		internal static Label CreateLab(Panel pan, int left, int top, int width, int height, float f, Color color)
		{
			Label lab = new Label();
			
			lab.Left = left;
			lab.Top = top;
			lab.Width = width;
			lab.Height = height;
			lab.ForeColor = Color.FromArgb(255, 255, 255);
			lab.TextAlign = ContentAlignment.MiddleCenter;
			lab.Font = new Font("Microsoft Sans Serif", f, FontStyle.Bold, GraphicsUnit.Point, 204);
			lab.BackColor = color;
			
			pan.Controls.Add(lab);
			return lab;
		}
		// Создание лейбла с прозрачным фоном
		internal static Label CreateLab(Panel pan, int left, int top, int width, int height, bool b, float f)
		{
			Label lab = new Label();
			
			lab.Left = left;
			lab.Top = top;
			lab.Width = width;
			lab.Height = height;
			lab.ForeColor = Color.FromArgb(255, 255, 255);
			lab.TextAlign = ContentAlignment.MiddleCenter;
			lab.Font = new Font("Microsoft Sans Serif", f, FontStyle.Bold, GraphicsUnit.Point, 204);
			lab.Parent = pan;
			lab.BackColor = Color.Transparent;
			
			pan.Controls.Add(lab);			
			return lab;
		}
		// Создание текстбокса
		internal static TextBox CreateBox(Panel pan, int left, int top, int width, int height, float f)
		{
			TextBox box = new TextBox();
			
			box.Left = left;
			box.Top = top;
			box.Width = width;
			box.Height = height;
			box.ForeColor = Color.FromArgb(0, 0, 0);
			box.Font = new Font("Microsoft Sans Serif", f, FontStyle.Bold, GraphicsUnit.Point, 204);
			
			pan.Controls.Add(box);			
			return box;
		}
		// Создание текстбокса в несколько строк
		internal static TextBox CreateBox(Panel pan, int left, int top, int width, int height, float f, bool mult)
		{
			TextBox box = new TextBox();
			
			box.Left = left;
			box.Top = top;
			box.Width = width;
			box.Height = height;
			box.ForeColor = Color.FromArgb(0, 0, 0);
			box.Font = new Font("Microsoft Sans Serif", f, FontStyle.Bold, GraphicsUnit.Point, 204);
			box.Multiline = mult;
			
			pan.Controls.Add(box);			
			return box;
		}
		// Создание маскедтекстбокса , который позволяет задать формат ввода (маску)
		internal static MaskedTextBox CreateMasBox(Panel pan, int left, int top, int width, int height, float f, bool mult)
		{
			MaskedTextBox box = new MaskedTextBox();
			
			box.Left = left;
			box.Top = top;
			box.Width = width;
			box.Height = height;
			box.ForeColor = Color.FromArgb(0, 0, 0);
			box.Font = new Font("Microsoft Sans Serif", f, FontStyle.Bold, GraphicsUnit.Point, 204);
			box.Multiline = mult;
			
			pan.Controls.Add(box);			
			return box;
		}
		// Метод позволяющий добавлять события к элементу
		internal static void EventAdd(object element, MouseEventHandler mouseClick, EventHandler mouseEnter, EventHandler mouseLeave)
		{
			if (element as Label != null)
			{
				(element as Label).MouseClick += mouseClick;
				(element as Label).MouseEnter += mouseEnter;
				(element as Label).MouseLeave += mouseLeave;				
			}
			else if (element as Panel != null)
			{
				(element as Panel).MouseClick += mouseClick;
				(element as Panel).MouseEnter += mouseEnter;
				(element as Panel).MouseLeave += mouseLeave;		
			}
		}
		// Метод позволяющий добавлять события к элементу
		internal static void EventAdde(object element, MouseEventHandler mouseMove, MouseEventHandler mouseDown, MouseEventHandler mouseUp)
		{
			if (element as Label != null)
			{
				(element as Label).MouseMove += mouseMove;
				(element as Label).MouseDown += mouseDown;
				(element as Label).MouseUp += mouseUp;				
			}
			else if (element as Panel != null)
			{
				(element as Panel).MouseMove += mouseMove;
				(element as Panel).MouseDown += mouseDown;
				(element as Panel).MouseUp += mouseUp;	
			}
		}
		// Метод скрывающий или показывающий элементы коллекции
		internal static void VisibleList(List<Label> list, bool b)
		{
			foreach (Label element in list)
				element.Visible = b;
		}
	}	
}




