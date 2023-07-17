using System;
class chastity
{
 static void Main(string[] args)
 {
  ConsoleKeyInfo cki; // Prevent example from ending if CTRL+C is pressed.
  Console.TreatControlCAsInput = true;
  Console.WriteLine("Press any combination of CTRL, ALT, and SHIFT, and a console key.");
  Console.WriteLine("Press the Escape (Esc) key to quit: \n");
  do
  {
   cki = Console.ReadKey(true); //receive key from keyboard but do not echo it
   Console.Write(" --- You pressed ");
   if((cki.Modifiers & ConsoleModifiers.Alt) != 0) Console.Write("ALT+");
   if((cki.Modifiers & ConsoleModifiers.Shift) != 0) Console.Write("SHIFT+");
   if((cki.Modifiers & ConsoleModifiers.Control) != 0) Console.Write("CTRL+");
   Console.WriteLine(cki.Key.ToString());
  }while (cki.Key != ConsoleKey.Escape);
 }
}

/*
this example is only slightly modified from here:
https://learn.microsoft.com/en-us/dotnet/api/system.console.readkey?view=netcore-2.0
*/

