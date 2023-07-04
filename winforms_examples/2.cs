using System;  
using System.Drawing;  
using System.Windows.Forms;  
public class Clr: Form {  
    Button b1 = new Button();  
    TextBox tb = new TextBox();  
    ColorDialog clg = new ColorDialog();  
    public Clr() {  
        b1.Click += new EventHandler(b1_click);  
        b1.Text = "OK";  
        tb.Location = new Point(50, 50);  
        this.Controls.Add(b1);  
        this.Controls.Add(tb);  
    }  
    public void b1_click(object sender, EventArgs e) {  
        clg.ShowDialog();  
        tb.BackColor = clg.Color;  
    }  
    public static void Main() {  
        Application.Run(new Clr());  
    }  
    // End of class  
} 
//csc 2.cs && 2