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
	static class Core					// Методы для создания разных объектов Control
	{
		internal static Form CreateWindow(Form form, int left, int top, int width, int height, string caption)
		{
			form.Left = left;
			form.Top = top;
			form.ClientSize = new Size(width, height);
			form.Text = caption;
			
			return form;
		}
		
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
	}	
}




