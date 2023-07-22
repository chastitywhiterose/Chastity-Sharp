using System;

/*
a structure definition containing the array for the tetris grid
it is written this way to match the C version of Chaste Tris
*/
struct tetris_grid
{
 public int[] array;
};


struct tetris_block
{
 public int[] array;
 public int color;
 public int spawn_x,spawn_y; /*where block spawns on entry*/
 public int x,y; /*current location of block*/
 public int width_used; /*width of block actually used*/
 public int id;
};


class chastity
{

//important variables for verifying moves or other things
static int move_id;
static int last_move_spin=0; /*was the last move a t spin?*/
static int last_move_fail;
static int moves=0;
static int moves_tried=0;
static byte[] move_log=new byte[1000000];
static int blocks_used=7;
static int lines_cleared=0,lines_cleared_last=0,lines_cleared_total=0;
static int empty_color=0x000000;
static int back_to_back=0;
static int score=0;
static int combo=0;

static int grid_width=10,grid_height=20;

static tetris_grid main_grid,temp_grid;

public static int max_block_width=4;
public static tetris_block main_block,hold_block,temp_block;

/*a function to initialize the static variables for the tetris game*/
static void init()
{
 Console.Write("Init Start\n");
 main_grid=new tetris_grid();
 main_grid.array=new int[200];
 //Console.WriteLine("main_grid.array.Length="+main_grid.array.Length);
 temp_grid=new tetris_grid();
 temp_grid.array=new int[200];
 //Console.WriteLine("temp_grid.array.Length="+temp_grid.array.Length);

 main_block=new tetris_block();
 main_block.array=new int[16];
 hold_block=new tetris_block();
 hold_block.array=new int[16];
 temp_block=new tetris_block();
 temp_block.array=new int[16];

 //Console.WriteLine("grid_width="+grid_width+" grid_height="+grid_height);

}



 static ConsoleKeyInfo cki;

 static void Main(string[] args)
 {
  init();
  //return;

  Console.Write("Main Start\n");

  spawn_block(); //spawn the first block before beginning game loop

  do
  {



   
   tetris_copy_temp();
   Console.Clear();
   tetris_print();


   cki=getkey();
   keyboard();
   Console.Write("Key="+cki.Key.ToString()+"\n");
  }while (cki.Key != ConsoleKey.Escape);

 }


static ConsoleKeyInfo getkey()
{
 ConsoleKeyInfo cki; 
 Console.TreatControlCAsInput = true; //Prevent example from ending if CTRL+C is pressed.
 cki = Console.ReadKey(true); //receive key from keyboard but do not echo it
 return cki;
}

static void keyboard()
{

 switch(cki.Key)
 {
  case ConsoleKey.Z:
   move_id='Z';
   block_rotate_left_basic();
  break;
  case ConsoleKey.X:
   move_id='X';
   block_rotate_right_basic();
  break;

    /*the main 4 directions*/

    case ConsoleKey.UpArrow:
    case ConsoleKey.W:
     move_id='W';
     tetris_move_up();
    break;

 case ConsoleKey.DownArrow:
 case ConsoleKey.S:
     move_id='S';
     tetris_move_down();
    break;
  
    case ConsoleKey.LeftArrow:
    case ConsoleKey.A:
     move_id='A';
    tetris_move_left();
    break;
    case ConsoleKey.RightArrow:
    case ConsoleKey.D:
     move_id='D';
     tetris_move_right();
    break;
  
 } 


}


/*this next section has the functions which move and test the block*/

/*this function controls whether or not the block index changes.*/
static void tetris_next_block()
{
 if(blocks_used==1){return;} /*do nothing if only one block type used*/
 /*optionally increment block type for different block next time.*/
 block_type++;  block_type%=blocks_used;
}

static void tetris_clear_lines()
{
 int x,y,xcount,x1,y1;

 lines_cleared=0;

 y=grid_height;
 while(y>0)
 {
  y-=1;

  xcount=0;
  x=0;
  while(x<grid_width)
  {
   if(main_grid.array[x+y*grid_width]!=empty_color){xcount++;}
   x+=1;
  }

  /*printf("row %d xcount %d\n",y,xcount);*/

  if(xcount==grid_width)
  {
   y1=y;

   /*printf("row %d line clear attempt.\n",y);*/

   x1=0;
   while(x1<grid_width)
   {
    main_grid.array[x1+y1*grid_width]=empty_color;
    x1++;
   }
   
  
   lines_cleared++;
  }

 }


 lines_cleared_total+=lines_cleared;

 if(lines_cleared!=0){combo++;}
 else{combo=0;}

 /*printf("combo: %d\n",combo);*/

 /*printf("this line clear: %d\n",lines_cleared);*/
 /*printf("total lines cleared: %d\n",lines_cleared_total);*/

 /*scoring section*/
 if(lines_cleared==1)
 {
  if(last_move_spin==1)
  {
   if(back_to_back>0){score+=1200;}
   else{score+=800;}
   back_to_back++;
  }
  else
  {
   score+=100;back_to_back=0;
  }
 }
 if(lines_cleared==2)
 {
  if(last_move_spin==1)
  {
   if(back_to_back>0){score+=1800;}
   else{score+=1200;}
   back_to_back++;
  }
  else
  {
   score+=300;back_to_back=0;
  }
 }
 if(lines_cleared==3)
 {
  if(last_move_spin==1)
  {
   if(back_to_back>0){score+=2400;}
   else{score+=1600;}
   back_to_back++;
  }
  else {score+=500;back_to_back=0;}
 }
 
 if(lines_cleared==4)
 {
  if(back_to_back>0){score+=1200;}
  else{score+=800;}
  back_to_back++;
 }

 if(lines_cleared!=0)
 {
  lines_cleared_last=lines_cleared;

 }

}


/*lines fall down to previously cleared line spaces*/

static void tetris_fall_lines()
{
 int x,y,xcount,y1;

/* printf("Time to make lines fall\n");*/

 y=grid_height;
 while(y>0)
 {
  y-=1;

  xcount=0;
  x=0;
  while(x<grid_width)
  {
   if(main_grid.array[x+y*grid_width]!=empty_color){xcount++;}
   x+=1;
  }

  /*printf("row %d xcount %d\n",y,xcount);*/

  if(xcount==0)
  {
   /* printf("row %d is empty\n",y);*/

   /*find first non empty row above empty row*/

   y1=y;
   while(y1>0)
   {
    y1--;
    xcount=0;
    x=0;
    while(x<grid_width)
    {
     if(main_grid.array[x+y1*grid_width]!=empty_color){xcount++;}
     x+=1;
    }
    if(xcount>0)
    {
     /*printf("row %d is not empty. Will copy to row %d.\n",y1,y);*/

     x=0;
     while(x<grid_width)
     {
      main_grid.array[x+y*grid_width]=main_grid.array[x+y1*grid_width];
      main_grid.array[x+y1*grid_width]=empty_color;
      x++;
     }
     break;
    }
   }

  }

 }

}


static void tetris_set_block()
{
 int x,y;


  /*draw block onto grid at it's current location*/
  y=0;
  while(y<max_block_width)
  {
   x=0;
   while(x<max_block_width)
   {
    if(main_block.array[x+y*max_block_width]!=0)
    {
     //main_grid.array[main_block.x+x+(main_block.y+y)*grid_width]=main_block.color;
     main_grid.array[main_block.x+x+(main_block.y+y)*grid_width]=main_block.id;
    }
    x+=1;
   }
   y+=1;
  }



 tetris_clear_lines();

 if(lines_cleared_last>0){tetris_fall_lines();}


 tetris_next_block();
 spawn_block();
}


/*all things about moving down*/
static void tetris_move_down()
{

 /*make backup of block location*/
 temp_block.x=main_block.x;
 temp_block.y=main_block.y;

 main_block.y+=1;

 last_move_fail=tetris_check_move();
 if(last_move_fail!=0)
 {
  /*restore backup of block location*/
  main_block.x=temp_block.x;
  main_block.y=temp_block.y;

  /*printf("Block is finished\n");*/
  tetris_set_block();
  move_log[moves]=(byte)move_id;
  moves++; /*moves normally wouldn't be incremented because move check fails but setting a block is actually a valid move.*/
 }
 else
 {
  /*move was successful*/
 }

 last_move_fail=0; /*because moving down is always a valid operation, the fail variable should be set to 0*/
}


/*all things about moving up*/
static void tetris_move_up()
{

 /*make backup of block location*/
 temp_block.x=main_block.x;
 temp_block.y=main_block.y;

 main_block.y-=1;
 last_move_fail=tetris_check_move();
 if(last_move_fail==0)
 {
  last_move_spin=0;
 }
 else
 {
  /*restore backup of block location*/
  main_block.x=temp_block.x;
  main_block.y=temp_block.y;
 }
}


/*all things about moving right*/
static void tetris_move_right()
{
 /*make backup of block location*/
 temp_block.x=main_block.x;
 temp_block.y=main_block.y;
 main_block.x+=1;
 last_move_fail=tetris_check_move();
 if(last_move_fail==0)
 {
  last_move_spin=0;
 }
 else
 {
  /*restore backup of block location*/
  main_block.x=temp_block.x;
  main_block.y=temp_block.y;
 }
}

/*all things about moving left*/
static void tetris_move_left()
{
 /*make backup of block location*/
 temp_block.x=main_block.x;
 temp_block.y=main_block.y;
 main_block.x-=1;
 last_move_fail=tetris_check_move();
 if(last_move_fail==0)
 {
  last_move_spin=0;
 }
 else
 {
  /*restore backup of block location*/
  main_block.x=temp_block.x;
  main_block.y=temp_block.y;
 }
}



static int pixel_on_grid(int x,int y)
{
 if(x<0){/*Console.WriteLine("Error: Negative X\n");*/return 1;}
 if(y<0){/*Console.WriteLine("Error: Negative Y\n");*/return 1;}
 if(x>=grid_width){/*Console.WriteLine("Error: X too high.\n");*/return 1;}
 if(y>=grid_height){/*Console.WriteLine("Error: Y too high.\n");*/return 1;}
 else{return main_grid.array[x+y*grid_width];}
}



/*
checks whether or not the block collides with anything on the current field
*/
static int tetris_check_move()
{
 int x,y;
 moves_tried++; /*move attempted*/

 y=0;
 while(y<max_block_width)
 {
  x=0;
  while(x<max_block_width)
  {
   if(main_block.array[x+y*max_block_width]!=0)
   {
    if( pixel_on_grid(main_block.x+x,main_block.y+y)!=0 )
    {
     //Console.WriteLine("Error: Block in Way on Move Check.\n");
     return 1; /*return failure*/
    }
   }
    x+=1;
  }
  y+=1;
 }
 
 move_log[moves]=(byte)move_id;
 moves++; /*move successful*/
 return 0;

}




/*
fancy right rotation system for T blocks only
does not actually rotate. Rather tries to move a T block into another valid spot and simulate SRS rules
*/
static void block_rotate_right_fancy_t()
{
 int x=0,y=0;

 if(main_block.id!='T')
 {
  Console.WriteLine("Block is not T. No action will be taken.");return;
 }

 x=main_block.x;
 y=main_block.y;


 main_block.x=x-1;
 main_block.y=y+1;
 last_move_fail=tetris_check_move();
 if(last_move_fail!=0)
 {
  /*Console.WriteLine("First fancy T Block spin attempt failed.");*/
  
  main_block.x=x-1;
  main_block.y=y+2;
  last_move_fail=tetris_check_move();
  if(last_move_fail!=0)
  {
   /*Console.WriteLine("Second fancy T Block spin attempt failed.");*/
  }

 }

}

/*basic (non SRS) rotation system*/
static void block_rotate_right_basic()
{
 int x=0,y=0,x1=0,y1=0;

 /*backup the block*/
 y=0;
 while(y<max_block_width)
 {
  x=0;
  while(x<max_block_width)
  {
   temp_block.array[x+y*max_block_width]=main_block.array[x+y*max_block_width];
   x+=1;
  }
  y+=1;
 }

 /*make backup of block location*/
 temp_block.x=main_block.x;
 temp_block.y=main_block.y;

 /*copy it from top to bottom to right to left(my own genius rotation trick)*/
 /*same as in the left rotation function but x,y and x1,y1 are swapped in the assignment*/

 x1=main_block.width_used;
 y=0;
 while(y<main_block.width_used)
 {
  x1--;
  y1=0;
  x=0;
  while(x<main_block.width_used)
  {
   main_block.array[x1+y1*max_block_width]=temp_block.array[x+y*max_block_width];
   x+=1;
   y1++;
  }
  y+=1;
 }

 /*if rotation caused collision, restore to the backup before rotate.*/
 last_move_fail=tetris_check_move();
 if(last_move_fail!=0)
 {
  /*if basic rotation failed, try fancier*/
  block_rotate_right_fancy_t();
 }
 if(last_move_fail!=0)
 {
  /*if it still failed, revert block to before rotation*/
 
  /*restore the block*/
  y=0;
  while(y<max_block_width)
  {
   x=0;
   while(x<max_block_width)
   {
    main_block.array[x+y*max_block_width]=temp_block.array[x+y*max_block_width];
    x+=1;
   }
   y+=1;
  }

  /*restore backup of block location*/
  main_block.x=temp_block.x;
  main_block.y=temp_block.y;

 }
 else
 {
  last_move_spin=1;

 }

}





/*
fancy left rotation system for T blocks only
does not actually rotate. Rather tries to move a T block into another valid spot and simulate SRS rules
*/
static void block_rotate_left_fancy_t()
{
 int x=0,y=0;

 if(main_block.id!='T')
 {
  Console.WriteLine("Block is not T. No action will be taken.");return;
 }
 
 x=main_block.x;
 y=main_block.y;


 main_block.x=x+1;
 main_block.y=y+1;
 last_move_fail=tetris_check_move();
 if(last_move_fail!=0)
 {
  /*printf("First fancy T Block spin attempt failed.");*/
  
  main_block.x=x+1;
  main_block.y=y+2;
  last_move_fail=tetris_check_move();
  if(last_move_fail!=0)
  {
   /*printf("Second fancy T Block spin attempt failed.");*/
  }

 }

}


/*basic (non SRS) rotation system*/
static void block_rotate_left_basic()
{
 int x=0,y=0,x1=0,y1=0;

 /*backup the block*/
 y=0;
 while(y<max_block_width)
 {
  x=0;
  while(x<max_block_width)
  {
   temp_block.array[x+y*max_block_width]=main_block.array[x+y*max_block_width];
   x+=1;
  }
  y+=1;
 }

 /*make backup of block location*/
 temp_block.x=main_block.x;
 temp_block.y=main_block.y;

 /*copy it from top to bottom to right to left(my own genius rotation trick)*/
/*same as in the right rotation function but x,y and x1,y1 are swapped in the assignment*/

 x1=main_block.width_used;
 y=0;
 while(y<main_block.width_used)
 {
  x1--;
  y1=0;
  x=0;
  while(x<main_block.width_used)
  {
   main_block.array[x+y*max_block_width]=temp_block.array[x1+y1*max_block_width];
   x+=1;
   y1++;
  }
  y+=1;
 }

 /*if rotation caused collision, restore to the backup before rotate.*/
 last_move_fail=tetris_check_move();
 if(last_move_fail!=0)
 {
  /*if basic rotation failed, try fancier*/
  block_rotate_left_fancy_t();
 }
 if(last_move_fail!=0)
 {
  /*if it still failed, revert block to before rotation*/

  /*restore the block*/
  y=0;
  while(y<max_block_width)
  {
   x=0;
   while(x<max_block_width)
   {
    main_block.array[x+y*max_block_width]=temp_block.array[x+y*max_block_width];
    x+=1;
   }
   y+=1;
  }

  /*restore backup of block location*/
  main_block.x=temp_block.x;
  main_block.y=temp_block.y;
 }
 else
 {
  last_move_spin=1;
 }

}






static void tetris_copy_temp()
{
 int x,y;

 /*make backup of entire grid*/
 y=0;
 while(y<grid_height)
 {
  x=0;
  while(x<grid_width)
  {
   temp_grid.array[x+y*grid_width]=main_grid.array[x+y*grid_width];
   x+=1;
  }
  y+=1;
 }

/*draw block onto temp grid at it's current location*/
  y=0;
  while(y<max_block_width)
  {
   x=0;
   while(x<max_block_width)
   {
    if(main_block.array[x+y*max_block_width]!=0)
    {
     if(temp_grid.array[main_block.x+x+(main_block.y+y)*grid_width]!=0)
     {
      Console.Write("Error: Block in Way\n");

      /*because a collision has occurred. We will restore everything back to the way it was before block was moved.*/

      break;
     }
     else
     {
      //temp_grid.array[main_block.x+x+(main_block.y+y)*grid_width]=main_block.color;
      temp_grid.array[main_block.x+x+(main_block.y+y)*grid_width]=main_block.id;
     }
    }
    x+=1;
   }
   y+=1;
  }


}

/*the function which displays the temp tetris grid to the console*/
static void tetris_print()
{
 int x,y;
 int v;
 y=0;
 while(y<grid_height)
 {
  Console.Write("[");
  x=0;
  while(x<grid_width)
  {
   v=temp_grid.array[x+y*grid_width];
   if(v==0){Console.Write(" ");}
   else
   {
    Console.Write((char)v);
   }
   Console.Write(" ");
   x+=1;
  }
  Console.WriteLine("]");
  y+=1;
 }
}

/*the following section has all the predefined arrays for the 7 types of tetris blocks*/

static int[] block_array_i=
{
 0,0,0,0,
 1,1,1,1,
 0,0,0,0,
 0,0,0,0,
};

static int[] block_array_t=
{
 0,1,0,0,
 1,1,1,0,
 0,0,0,0,
 0,0,0,0,
};

static int[] block_array_z=
{
 1,1,0,0,
 0,1,1,0,
 0,0,0,0,
 0,0,0,0,
};

static int[] block_array_j=
{
 1,0,0,0,
 1,1,1,0,
 0,0,0,0,
 0,0,0,0,
};

static int[] block_array_o=
{
 1,1,0,0,
 1,1,0,0,
 0,0,0,0,
 0,0,0,0,
};

static int[] block_array_l=
{
 0,0,1,0,
 1,1,1,0,
 0,0,0,0,
 0,0,0,0,
};

static int[] block_array_s=
{
 0,1,1,0,
 1,1,0,0,
 0,0,0,0,
 0,0,0,0,
};


static int block_type=0;

static void spawn_block()
{
 int x,y;
 int[] p;

 p=block_array_i; //assign to something just to avoid compiler error

 if(block_type==0)
 {
  p=block_array_i;
  main_block.color=0x00FFFF;
  main_block.width_used=4;
  main_block.id='I';
 }

 if(block_type==1)
 {
  p=block_array_t;
  main_block.color=0xFF00FF;
  main_block.width_used=3;
  main_block.id='T';
 }

 if(block_type==2)
 {
  p=block_array_z;
  main_block.color=0xFF0000;
  main_block.width_used=3;
  main_block.id='Z';
 }

 if(block_type==3)
 {
  p=block_array_j;
  main_block.color=0x0000FF;
  main_block.width_used=3;
  main_block.id='J';
 }

 if(block_type==4)
 {
  p=block_array_o;
  main_block.color=0xFFFF00;
  main_block.width_used=2;
  main_block.id='O';
 }

 if(block_type==5)
 {
  p=block_array_l;
  main_block.color=0xFF7F00;
  main_block.width_used=3;
  main_block.id='L';
 }

 if(block_type==6)
 {
  p=block_array_s;
  main_block.color=0x00FF00;
  main_block.width_used=3;
  main_block.id='S';
 }

 /*copy another new block array into the current one*/
 y=0;
 while(y<max_block_width)
 {
  x=0;
  while(x<max_block_width)
  {
   main_block.array[x+y*max_block_width]=p[x+y*max_block_width];
   x+=1;
  }
  y+=1;
 }

 main_block.x=(grid_width-main_block.width_used)/2;
 main_block.y=0;

 main_block.spawn_x=main_block.x;
 main_block.spawn_y=main_block.y;
}


}

/*
this example is only slightly modified from here:
https://learn.microsoft.com/en-us/dotnet/api/system.console.readkey?view=netcore-2.0
*/

