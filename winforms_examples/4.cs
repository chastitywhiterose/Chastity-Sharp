using System;  
using System.Windows.Forms;  
using System.Drawing;  
public class Solidbru: Form {  
    public Solidbru() {  
        this.Text = "Using Solid Brushes";  
        this.Paint += new PaintEventHandler(Fill_Graph);  
    }  
    public void Fill_Graph(object sender, PaintEventArgs e) {  
        Graphics g = e.Graphics;  
        //Creates a SolidBrush and fills the rectangle  
        SolidBrush sb = new SolidBrush(Color.Pink);  
        g.FillRectangle(sb, 50, 50, 150, 150);  
    }  
    public static void Main() {  
        Application.Run(new Solidbru());  
    }  
    // End of class  
}  
//csc 4.cs && 4
