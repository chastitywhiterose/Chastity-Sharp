using System;  
using System.Windows.Forms;  
using System.Drawing;  
using System.Drawing.Drawing2D;  
public class Hatchbru: Form {  
    public Hatchbru() {  
        this.Text = "Using Solid Brushes";  
        this.Paint += new PaintEventHandler(Fill_Graph);  
    }  
    public void Fill_Graph(object sender, PaintEventArgs e) {  
        Graphics g = e.Graphics;  
        //Creates a Hatch Style,Brush and fills the rectangle  
        /*Various HatchStyle values are DiagonalCross,ForwardDiagonal, 
        Horizontal, Vertical, Solid etc. */  
        HatchStyle hs = HatchStyle.Cross;  
        HatchBrush sb = new HatchBrush(hs, Color.Blue, Color.Red);  
        g.FillRectangle(sb, 50, 50, 150, 150);  
    }  
    public static void Main() {  
        Application.Run(new Hatchbru());  
    }  
    // End of class  
}  
//csc 5.cs && 5
