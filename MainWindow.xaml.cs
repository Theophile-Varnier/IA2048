using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace _2048
{

    public enum Move
    {
        TOP = 0,
        RIGHT,
        BOTTOM,
        LEFT
    }

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int size = 4;
        public Case[][] cases;
        public MainWindow()
        {
            cases = new Case[size][];
            for (int i = 0; i < size; i++)
            {
                cases[i] = new Case[size];
            }
            InitializeComponent();
            for (int i = 0; i < MainGrid.Children.Count; i++)
            {
                Case ca = MainGrid.Children[i] as Case;
                if (ca != null)
                {
                    cases[i / size][i % size] = ca;
                    cases[i / size][i % size].Init(0);
                }
            }
            Random initPos = new Random();
            int x1 = initPos.Next() % size;
            int y1 = initPos.Next() % size;
            int x2;
            int y2;
            cases[x1][y1].Init(2);
            do
            {
                x2 = initPos.Next() % size;
                y2 = initPos.Next() % size;
            } while (x2 == x1 && y2 == y1);
            cases[x2][y2].Init(2);
            /*while (!lost())
            {
                applyMove(findBestMove(cases, 1));
                System.Threading.Thread.Sleep(2000);
            }*/
        }

        private void Clear()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    cases[i][j].hasMovedThisRound = false;
                }
            }
        }

        private List<int[]> emptyCases(Case[][] currentStatus)
        {
            List<int[]> res = new List<int[]>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (currentStatus[i][j].val == 0)
                    {
                        int[] emptyCase = new int[] { i, j };
                        res.Add(emptyCase);
                    }
                }
            }
            return res;
        }

        public void generateAlea(Case[][] plateau, int[] pos)
        {
            plateau[pos[0]][pos[1]].Init(2);
        }

        private Case[][] Copy(Case[][] source)
        {
            Case[][] res = new Case[size][];
            for (int i = 0; i < size; i++)
            {
                res[i] = new Case[size];
                for (int j = 0; j < size; j++)
                {
                    res[i][j] = source[i][j];
                }
            }
            return res;
        }

        private int Max(Case[][] plateau)
        {
            int res = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (plateau[i][j].val > res)
                    {
                        res = plateau[i][j].val;
                    }
                }
            }
            return res;
        }

        public Move alphaBeta(Case[][] currentStatus, int prof)
        {
            Case[][] temp = Copy(currentStatus);
            Move res = Move.LEFT;
            int[] values = new int[size];
            return res;
        }

        public void findBestMove(Case[][] currentStatus, int prof)
        {
            Move res = alphaBeta(currentStatus, prof);
            Console.WriteLine(res);
            Key moveToApply;
            var routedEvent = Keyboard.KeyDownEvent;
            var target = Keyboard.FocusedElement;
            switch (res)
            {
                case Move.LEFT:
                    moveToApply = Key.Left;
                    break;
                case Move.RIGHT:
                    moveToApply = Key.Right;
                    break;
                case Move.TOP:
                    moveToApply = Key.Up;
                    break;
                case Move.BOTTOM:
                    moveToApply = Key.Down;
                    break;
                default:
                    moveToApply = Key.Left;
                    break;
            }
            target.RaiseEvent(
                new KeyEventArgs(
                    Keyboard.PrimaryDevice,
                    PresentationSource.FromVisual(this),
                    0,
                    moveToApply) { RoutedEvent = routedEvent });
            
        }

        private bool ColumnPlayable(int i, Move m)
        {
            switch (m)
            {
                case Move.TOP:
                    for (int x = 1; x < size; x++)
                    {
                        if (cases[x][i].val == 0)
                        {
                            continue;
                        }
                        if (cases[x-1][i].val == 0 || cases[x-1][i].val == cases[x][i].val)
                        {
                            return true;
                        }
                    }
                    break;
                case Move.BOTTOM:
                    for (int x = 0; x < 3; x++)
                    {
                        if (cases[x][i].val == 0)
                        {
                            continue;
                        }
                        if (cases[x + 1][i].val == 0 || cases[x + 1][i].val == cases[x][i].val)
                        {
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        public bool lost()
        {
            foreach (Move m in Enum.GetValues(typeof(Move)))
            {
                if (isPlayable(m))
                {
                    return false;
                }
            }
            Console.WriteLine("perdu");
            return true;
        }

        private bool RowPlayable(int i, Move m)
        {
            switch (m)
            {
                case Move.LEFT:
                    for (int x = 1; x < size; x++)
                    {
                        if (cases[i][x].val == 0)
                        {
                            continue;
                        }
                        if (cases[i][x-1].val == 0 || cases[i][x-1].val == cases[i][x].val)
                        {
                            return true;
                        }
                    }
                    break;
                case Move.RIGHT:
                    for (int x = 0; x < 3; x++)
                    {
                        if (cases[i][x].val == 0)
                        {
                            continue;
                        }
                        if (cases[i][x+1].val == 0 || cases[i][x+1].val == cases[i][x].val)
                        {
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        private bool isPlayable(Move m)
        {
            switch (m)
            {
                case Move.TOP:
                case Move.BOTTOM:
                    for (int i = 0; i < size; i++)
                    {
                        if (ColumnPlayable(i, m))
                        {
                            return true;
                        }
                    }
                    break;
                case Move.LEFT:
                case Move.RIGHT:
                    for (int i = 0; i < size; i++)
                    {
                        if (RowPlayable(i, m))
                        {
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        private int[] Next(int i, int j, Move m)
        {
            int offsetI = 0;
            int offsetJ = 0;
            switch (m)
            {
                case Move.TOP:
                    if (i < 3 && j <= 3)
                    {
                        offsetI = 1;
                        offsetJ = 0;
                    }
                    else if (i == 3 && j < 3)
                    {
                        offsetJ = 1;
                        offsetI = -i+1;
                    }
                    break;
                case Move.BOTTOM:
                    if (i > 0 && j <= 3)
                    {
                        offsetI = -1;
                        offsetJ = 0;
                    }
                    else if (i == 0 && j < 3)
                    {
                        offsetJ = 1;
                        offsetI = 2 - i;
                    }
                    break;
                case Move.LEFT:
                    if (j < 3 && i <= 3)
                    {
                        offsetI = 0;
                        offsetJ = 1;
                    }
                    else if (j == 3 && i < 3)
                    {
                        offsetJ = -j+1;
                        offsetI = 1;
                    }
                    break;
                case Move.RIGHT:
                    if (j > 0 && i <= 3)
                    {
                        offsetI = 0;
                        offsetJ = -1;
                    }
                    else if (j == 0 && i < 3)
                    {
                        offsetJ = 2 - j;
                        offsetI = 1;
                    }
                    break;
            }
            return new int[2] { i + offsetI, j + offsetJ };
        }

        public void applyMove(Move m)
        {
            if (!isPlayable(m))
            {
                return;
            }
            int[] indices;
            int[] temp;
            switch (m)
            {
                case Move.TOP:
                    indices = new int[] { 1, 0 };
                    break;
                case Move.BOTTOM:
                    indices = new int[] { 2, 0 };
                    break;
                case Move.LEFT:
                    indices = new int[] { 0, 1 };
                    break;
                case Move.RIGHT:
                    indices = new int[] { 0, 2 };
                    break;
                default:
                    indices = new int[] { 0, 0 };
                    break;
            }
            temp = new int[] { indices[0], indices[1] };
            do
            {
                tryToMove(indices[0], indices[1], m);
                temp[0] = indices[0];
                temp[1] = indices[1];
                indices = Next(indices[0], indices[1], m);
            }while (indices[0] != temp[0] || indices[1] != temp[1]);
            Random rand = new Random();
            /*do
            {
                indices[0] = rand.Next()%size;
                indices[1] = rand.Next()%size;
            } while (cases[indices[0]][indices[1]].val != 0);*/
            List<int[]> emptyCases = this.emptyCases(cases);
            int index = rand.Next() % emptyCases.Count();
            generateAlea(cases, emptyCases[index]);
            this.Clear();
        }

        private void tryToMove(int i, int j, Move m)
        {
            if (cases[i][j].val == 0)
            {
                return;
            }
            int variable;
            switch (m)
            {
                case Move.TOP:
                    variable = i-1;
                    while (variable > 0 && cases[variable][j].val == 0) 
                    {
                        variable--;
                    }
                    if (cases[variable][j].val == cases[i][j].val && !cases[variable][j].hasMovedThisRound)
                    {
                        cases[variable][j].Merge();
                        cases[variable][j].hasMovedThisRound = true;
                        cases[i][j].Init(0);
                    }
                    else if (variable == 0 && cases[variable][j].val == 0)
                    {
                        cases[variable][j].Init(cases[i][j].val);
                        cases[i][j].Init(0);
                    }
                    else if (variable != i - 1 || cases[variable][j].val == 0)
                    {
                        cases[variable + 1][j].Init(cases[i][j].val);
                        cases[i][j].Init(0);
                    }
                    break;
                case Move.BOTTOM:
                    variable = i + 1;
                    while (variable < 3 && cases[variable][j].val == 0)
                    {
                        variable++;
                    }
                    if (cases[variable][j].val == cases[i][j].val && !cases[variable][j].hasMovedThisRound)
                    {
                        cases[variable][j].Merge();
                        cases[variable][j].hasMovedThisRound = true;
                        cases[i][j].Init(0);
                    }
                    else if (variable == 3 && cases[variable][j].val == 0)
                    {
                        cases[variable][j].Init(cases[i][j].val);
                        cases[i][j].Init(0);
                    }
                    else if (variable != i + 1 || cases[variable][j].val == 0)
                    {
                        cases[variable-1][j].Init(cases[i][j].val);
                        cases[i][j].Init(0);
                    }
                    break;
                case Move.LEFT:
                    variable = j - 1;
                    while (variable > 0 && cases[i][variable].val == 0)
                    {
                        variable--;
                    }
                    if (cases[i][variable].val == cases[i][j].val && !cases[i][variable].hasMovedThisRound)
                    {
                        cases[i][variable].Merge();
                        cases[i][variable].hasMovedThisRound = true;
                        cases[i][j].Init(0);
                    }
                    else if (variable == 0 && cases[i][variable].val == 0)
                    {
                        cases[i][variable].Init(cases[i][j].val);
                        cases[i][j].Init(0);
                    }
                    else if (variable != j - 1 || cases[i][variable].val == 0)
                    {
                        cases[i][variable+1].Init(cases[i][j].val);
                        cases[i][j].Init(0);
                    }
                    break;
                case Move.RIGHT:
                    variable = j + 1;
                    while (variable < 3 && cases[i][variable].val == 0)
                    {
                        variable++;
                    }
                    if (cases[i][variable].val == cases[i][j].val && !cases[i][variable].hasMovedThisRound)
                    {
                        cases[i][variable].hasMovedThisRound = true;
                        cases[i][variable].Merge();
                        cases[i][j].Init(0);
                    }
                    else if (variable == 3 && cases[i][variable].val == 0)
                    {
                        cases[i][variable].Init(cases[i][j].val);
                        cases[i][j].Init(0);
                    }
                    else if (variable != j + 1 || cases[i][variable].val == 0   )
                    {
                        cases[i][variable-1].Init(cases[i][j].val);
                        cases[i][j].Init(0);
                    }
                    break;
            }
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    applyMove(Move.TOP);
                    break;
                case Key.Down:
                    applyMove(Move.BOTTOM);
                    break;
                case Key.Left:
                    applyMove(Move.LEFT);
                    break;
                case Key.Right:
                    applyMove(Move.RIGHT);
                    break;
            }
        }

        private void Window_Loaded_1(object sender, EventArgs e)
        {
            while (!lost())
            //for(int i = 0; i < size; i++)
            {
                findBestMove(cases, 6);
            }

        }
    }
}
