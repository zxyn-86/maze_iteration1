using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace maze_iteration_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public static class vals // static class to hold "global" variables 
    {
            public static int w = 50;
            public static int rows = 600/w;
            public static int cols = 600/w;
            public static cell current;
            public static List<cell> grid = new List<cell>();
            public static Stack<cell> stack = new Stack<cell>();
            public static bool clicked;
    }
    public partial class MainWindow : Window
    {

        private gameloop Gameloop = null; // initialising a gameloop
        private Thread generationThread;  //backround thread for drawing

        public MainWindow()// here we render and load da window
        {
            InitializeComponent();

            setup();  // looping through making an array of cell objects
            Loaded += window_loaded;
           
           
           

        }

        private async void window_loaded(object sender, EventArgs e) // event handler that allows us to call the event game loop which calls on render which calls draw
        {
          
            Gameloop = new gameloop();
            Gameloop.start();
            generationThread = new Thread(GenerateMaze);
            generationThread.IsBackground = true;
            generationThread.Start();
          
            
        
        }
        private void generate_click(object sender, EventArgs e)
        {
            vals.clicked = true;
        }
        public void setup()
        {
            for (int i = 0; i < vals.cols; i++) // looping through making an array of cell objects
            {
                for (int j = 0; j < vals.rows; j++)  
                {
                    cell MazeCell = new cell(i,j);  

                    vals.grid.Add(MazeCell);
                }
            }
            vals.current = vals.grid[0]; //start point

            foreach (var cell in vals.grid)
            {
                if (cell.j - 1 < 0)
                {
                    cell.north = null;
                }
                else { cell.north = vals.grid[cell.index(cell.i, cell.j - 1)]; }  // i and j reflect vector notation as i am dealing with a translation

                if (cell.j + 1 >= vals.cols)
                {
                    cell.south = null;
                }
                else { cell.south = vals.grid[cell.index(cell.i, cell.j + 1)]; }

                if (cell.i + 1 >= vals.cols)
                {
                    cell.east = null;
                }
                else { cell.east = vals.grid[cell.index(cell.i + 1, cell.j)]; }

                if (cell.i - 1 < 0)
                {
                    cell.west = null;
                }
                else { cell.west = vals.grid[cell.index(cell.i - 1, cell.j)]; }
            }
        }

        private void GenerateMaze()
        {
            while (true)
            {
                Dispatcher.Invoke(() =>
                {
                    cell.carve(mycanvas); // carves out maze
                    InvalidateVisual();
                });
                Thread.Sleep(100); // Adjust sleep time to control generation speed
            }
        }




        protected override void OnRender(DrawingContext drawingContext) // for rendering and actually drawing to screen
        {
            base.OnRender(drawingContext);  

            if (Gameloop != null)
            {
                cell.carve(mycanvas); //carves out maze
            }
        }

        
    }



}

