using System;
using System.Drawing;
class chastity
{
 static void Main(string[] args)
 {
  Bitmap image=new Bitmap(1920,1080);
  Console.WriteLine(image.ToString());
  Console.WriteLine("Width="+image.Width);
  Console.WriteLine("Height="+image.Height);
  Graphics g = Graphics.FromImage(image);

  Color[] c=new Color[2];
  c[0]=Color.FromArgb(0,0,0);
  c[1]=Color.FromArgb(255,255,255);

  int rectsize=image.Height/54;
  int index=0,index1;

  int y=0;
  while(y<image.Height)
  {
   index1=index;
   int x=0;
   while(x<image.Width)
   {
    SolidBrush brush = new SolidBrush(c[index]);
    g.FillRectangle(brush,x,y,rectsize,rectsize);
    index^=1;
    x+=rectsize;
   }
   index=index1^1;
   y+=rectsize;
  }

  image.Save("checker.png");

 }
}

/*
This is an important example for me. The checkerboard is my favorite pattern and so it is essential to figure out how to draw one when learning a new language. The .NET libraries include ways to create image files and draw to them. The following is my makefile I use which shows two ways to compile this either using the Mono C Sharp compiler or the Microsoft C Sharp compiler

C_Sharp_mono:
	mcs main.cs -pkg:dotnet && mono main.exe
C_Sharp_csc:
	csc main.cs && mono main.exe

*/


