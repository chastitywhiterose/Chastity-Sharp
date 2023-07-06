// mcs pow2.cs && mono pow2.exe
using System;
class chastity
{
 static void Main(string[] args)
 {
  int x,y,z; /*my favorite 3 variable names. Because I've played Minecraft too long!*/
  int length=1000; /*how many digits the array can use*/
  int length2=1; /*How many digits are currently used. This will increase over time!*/
  byte[] a=new byte[length];

  for(x=0;x<length;x++){a[x]=0;} a[0]=1; /*set up the array*/

  for(z=0;z<=256;z++)
  {
   Console.Write("2^"+z+"=");
   for(x=length2-1;x>=0;x--){Console.Write(a[x]);} Console.WriteLine();
   y=0;
   for(x=0;x<=length2;x++)
   {
    a[x]+=a[x]; /*Add this digit to itself!*/
    a[x]+=(byte)y;    /*Add the carry!*/
    if(a[x]>9){y=1;a[x]-=10;}else{y=0;}
   }
   if(a[length2]>0){length2++;}
  }
 }
}


