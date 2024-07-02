using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace maze_iteration_1
{
    public  class cell
    {
        public int i;  //place holders for row and col 
        public int j;
        public bool visited = false; //flag for visited or not
        public bool[] walls = { true, true, true, true }; //an array holding a flag for each of the walls and control if theyre present or not
        private Random random; // random object
        List<cell> neighbours = new List<cell>(); //list of available cells to go to

        public cell north = null;   //neighbours of cell
        public cell east = null;
        public cell south = null;
        public cell west = null;


        public cell(int i, int j) //used i and j to mimick i and j vector notation
        {
            this.i = i;
            this.j = j;
            random = new Random(Guid.NewGuid().GetHashCode()); // allows me to generate unique random numbers for the check func
            
        }
        public static int index(int Row, int Col) // index function changes i and j to a index in a one d array
        {
            return Col + (Row * vals.cols);
        }
        public cell ReturnCell(cell mazecell)
        {
            if (mazecell.neighbours.Count > 0)
            {
                return mazecell.neighbours[random.Next(mazecell.neighbours.Count)];
            }
            else
            {
                return null;
            }
        }
        public cell check(cell mazecell) // checks if the cells up down left or right have been visited and if not adds to a list and returns a random cell in the list
        {
            //determines valid neighbour

            if (mazecell.north != null && !mazecell.north.visited)
            {
                mazecell.neighbours.Add(north);
            }
            if (east != null && !mazecell.east.visited)
            {
                mazecell.neighbours.Add(east);
            }
            if (south != null && !mazecell.south.visited)
            {
                mazecell.neighbours.Add(south);
            }
            if (west != null && !mazecell.west.visited)
            {
                mazecell.neighbours.Add(west);
            }

           return ReturnCell(mazecell);


        }

        public void highlight(Canvas mycanvas) //highlights current cell a diff colour
        {
            int x = vals.current.i * vals.w;
            int y = vals.current.j * vals.w;
            Rectangle rect = new Rectangle
            {
                Width = vals.w,
                Height = vals.w,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Fill = Brushes.DarkMagenta,



            };
            mycanvas.Children.Add(rect);
            Canvas.SetLeft(rect, (vals.current.i) * vals.w);
            Canvas.SetTop(rect, (vals.current.j) * vals.w);
        }
        public static void draw(cell mazecell, int w, Canvas mycanvas) // drawing walls and highlghting path taken
        {
            int x = mazecell.i * w;
            int y = mazecell.j * w;


            if (mazecell.walls[0])
            {
                Line line_top = new Line
                {
                    X1 = x,
                    Y1 = y,
                    X2 = x + w,
                    Y2 = y,
                    Stroke = Brushes.Purple,
                    StrokeThickness = 2
                };
                mycanvas.Children.Add(line_top);

            }
            if (mazecell.walls[1])
            {
                Line line_right = new Line
                {
                    X1 = x + w,
                    Y1 = y,
                    X2 = x + w,
                    Y2 = y + w,
                    Stroke = Brushes.Purple,
                    StrokeThickness = 2
                };
                mycanvas.Children.Add(line_right);

            }
            if (mazecell.walls[2])
            {
                Line line_bottom = new Line
                {
                    X1 = x + w,
                    Y1 = y + w,
                    X2 = x,
                    Y2 = y + w,
                    Stroke = Brushes.Purple,
                    StrokeThickness = 2
                };
                mycanvas.Children.Add(line_bottom);

            }
            if (mazecell.walls[3])
            {
                Line line_left = new Line
                {
                    X1 = x,
                    Y1 = y + w,
                    X2 = x,
                    Y2 = y,
                    Stroke = Brushes.Purple,
                    StrokeThickness = 2
                };
                mycanvas.Children.Add(line_left);

            }
            if (mazecell.visited == true)//highlights path taken
            {
                
                Rectangle rect = new Rectangle
                {
                    Width = vals.w,
                    Height = vals.w,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Fill = Brushes.LightGoldenrodYellow,



                };
                mycanvas.Children.Add(rect);
                Canvas.SetLeft(rect, (mazecell.i) * vals.w);
                Canvas.SetTop(rect, (mazecell.j) * vals.w);
            }

        }

        public static void carve(Canvas mycanvas) // func to draw walls and carve out the maze
        {
            for (int r = 0; r < vals.grid.Count; r++)
            {
                cell.draw(vals.grid[r], vals.w, mycanvas);
            }


            vals.current.visited = true;
            vals.current.highlight(mycanvas); // highlights the current cell

            cell next = vals.current.check(vals.current); //initialising the cell we go to next 


            if (next != null) // here we go to the next cell deleting wals and setting it to current
            {

                next.visited = true;
                cell newcell = new cell(vals.current.i, vals.current.j);
                newcell.north = vals.current.north;
                newcell.south = vals.current.south;
                newcell.east = vals.current.east;
                newcell.west = vals.current.west;
                
                vals.stack.Push(newcell);

                cell.remove(vals.current, next);
                Console.WriteLine("current: Row: " + vals.current.j + ", Col:" + vals.current.i + " : Stack count: " + vals.stack.Count);
                vals.current = next;

                Console.WriteLine("Row: " + next.j + ", Col:" + next.i + " : Stack count: " + vals.stack.Count);



            }
            else if (vals.stack.Count > 0 && next == null) //backtracks
            {

                vals.current = vals.stack.Pop();

                Console.WriteLine("has no neighbour" + " : Stack count: " + vals.stack.Count);

            }



        }


        public static void remove(cell a, cell b) // removes the walls between cells
        {
            int x = a.i - b.i;
            int y = a.j - b.j;

            if (x == 1)
            {
                a.walls[3] = false;
                b.walls[1] = false;
            }
            else if (x == -1)
            {
                a.walls[1] = false;
                b.walls[3] = false;
            }

            if (y == 1)
            {
                a.walls[0] = false;
                b.walls[2] = false;
            }
            else if (y == -1)
            {
                a.walls[2] = false;
                b.walls[0] = false;
            }
        }
    }
}
