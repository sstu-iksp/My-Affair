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
		// Созжание лейбла
		internal static Label CreateLab(Panel pan, int left, int top, int width, int height, float f)
		{
			Label lab = new Label();
			
			lab.Left = left;
			lab.Top = top;
			lab.Width = width;
			lab.Height = height;
			lab.ForeColor = Color.White;
			lab.TextAlign = ContentAlignment.MiddleCenter;
			lab.Font = new Font("Microsoft Sans Serif", f, FontStyle.Bold, GraphicsUnit.Point, 204);
			
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
			lab.ForeColor = Color.White;
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
			box.ForeColor = Color.Black;
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
			box.ForeColor = Color.Black;
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
			box.ForeColor = Color.Black;
			box.Font = new Font("Microsoft Sans Serif", f, FontStyle.Bold, GraphicsUnit.Point, 204);
			box.Multiline = mult;
			
			pan.Controls.Add(box);			
			return box;
		}
	}	
}




