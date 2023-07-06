// mcs primes.cs && mono primes.exe
using System;
class chastity
{
 static void Main(string[] args)
 {
  int x,y;
  int length=1000000;
  byte[] a=new byte[length];
  //printf("2\n");
  Console.WriteLine(2);
  x=3;
  while(x<length)
  {
   Console.WriteLine(x);
   y=x;
   while(y<length)
   {
    a[y]=1;
    y+=x;
   }
   while(x<length && a[x]>0){x+=2;}
  }
 }
}


