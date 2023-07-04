using System;  
using System.Windows.Forms;  
using System.Drawing;  
using System.Drawing.Drawing2D;  
public class Drawgra: Form {  
    public Drawgra() {  
        this.Text = "Illustrating DrawXXX() methods";  
        this.Size = new Size(450, 400);  
        this.Paint += new PaintEventHandler(Draw_Graphics);  
    }  
    public void Draw_Graphics(object sender, PaintEventArgs e) {  
        Graphics g = e.Graphics;  
        Pen penline = new Pen(Color.Red, 5);  
        Pen penellipse = new Pen(Color.Blue, 5);  
        Pen penpie = new Pen(Color.Tomato, 3);  
        Pen penpolygon = new Pen(Color.Maroon, 4);  
        /*DashStyle Enumeration values are Dash, 
        DashDot,DashDotDot,Dot,Solid etc*/  
        penline.DashStyle = DashStyle.Dash;  
        g.DrawLine(penline, 50, 50, 100, 200);  
        //Draws an Ellipse  
        penellipse.DashStyle = DashStyle.DashDotDot;  
        g.DrawEllipse(penellipse, 15, 15, 50, 50);  
        //Draws a Pie  
        penpie.DashStyle = DashStyle.Dot;  
        g.DrawPie(penpie, 90, 80, 140, 40, 120, 100);  
        //Draws a Polygon  
        g.DrawPolygon(penpolygon, new Point[] {  
            new Point(30, 140),  
                new Point(270, 250),  
                new Point(110, 240),  
                new Point(200, 170),  
                new Point(70, 350),  
                new Point(50, 200)  
        });  
    }  
    public static void Main() {  
        Application.Run(new Drawgra());  
    }  
    // End of class  
}  
//csc 3.cs && 3
