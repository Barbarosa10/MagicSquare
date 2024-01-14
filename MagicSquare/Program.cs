using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms;


[assembly: InternalsVisibleTo("MagicSquareTest")]
namespace MagicSquare
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMagicSquare());
        }
    }

    public class MagicSquareSolver
    {
        public int[,] square { get; set; }
        private int size;
        private int targetSum;
        private int[] rowsSum;
        private int[] colSum;
        private int primDiagSum, secDiagSum;
        private List<int>[,] posSquare;


        public MagicSquareSolver(int size, int targetSum = 15)
        {
            this.size = size;
            this.square = new int[size, size];
            this.targetSum = targetSum; // Specific for 3x3 magic square
            this.posSquare = new List<int>[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    posSquare[i, j] = new List<int>(); 
                    for(int k=1; k<=size*size;k++)
                    {
                        posSquare[i, j].Add(k);
                    }
                }
            }

            this.rowsSum = new int[size];
            this.colSum = new int[size];
            primDiagSum = 0;
            secDiagSum = 0;
        }

        public void Solve()
        {
            if (PlaceNumber(0, 0, this.posSquare))
            {
                PrintSquare();
            }
            else
            {
                MessageBox.Show("No solution found.");
            }
        }

        public List<int>[,] DeepCopyDomains(List<int>[,] originalDomains)
        {
            List<int>[,] newDomains = new List<int>[size, size];
            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                    newDomains[i,j] = new List<int>(originalDomains[i,j]);
            }
            return newDomains;
        }

        public bool CheckIfDomainsVoid(List<int>[,] domain)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (domain[i, j].Count == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool PlaceNumber(int row, int col, List<int>[,] posSquare)
        {

            if (row == size)
            {
                return IsValid();
            }

            if(col == 0 && row > 0)
            {
                int s = 0;
                for(int i=0; i<size;i++)
                {
                    s += square[row - 1,i];
                }
                if (s != targetSum)
                    return false;
            }
            else if (row == size-1 && col > 0)
            {
                int s = 0;
                for (int i = 0; i < size; i++)
                {
                    s += square[i, col-1];
                }
                if (s != targetSum)
                    return false;
            }

            List<int>[,] savedPosSquare;

            int nextRow = col == size - 1 ? row + 1 : row;
            int nextCol = col == size - 1 ? 0 : col + 1;

            foreach(var num in posSquare[row,col])
            {
                square[row, col] = num;
                rowsSum[row] += num;
                colSum[col] += num;
                if(row == col)
                    primDiagSum += num;
                if (row + col == size - 1)
                    secDiagSum += num;  
                if (rowsSum[row] <= targetSum && colSum[col] <= targetSum && primDiagSum <= targetSum && secDiagSum <= targetSum)
                {
                    savedPosSquare = DeepCopyDomains(posSquare);
                    //Removing from all domains
                    for (int i = row; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (!(i == row && j == col))
                            {
                                savedPosSquare[i, j].Remove(num);
                            }
                        }
                    }
                    //Removing from column
                    for (int i=row+1;i<size;i++)
                    {
                            savedPosSquare[i, col].RemoveAll(x => x > (targetSum - colSum[col]));
                    }
                    //Remove from row
                    for (int i = col+1; i < size; i++)
                    {
                            savedPosSquare[row, i].RemoveAll(x => x > (targetSum - rowsSum[row]));
                    }
                    //Remove for primary diagonal
                    if(row == col)
                    {
                        for(int i=row+1;i<size;i++)
                        {
                                savedPosSquare[i, i].RemoveAll(x => x > (targetSum - primDiagSum));
                        }
                    }
                    //Remove for secondary
                    if(row + col == size-1)
                    {
                        for(int i=row+1;i<size;i++)
                        {
                                savedPosSquare[i,size-1-i].RemoveAll(x => x > (targetSum - secDiagSum));
                        }
                    }

                    if(!CheckIfDomainsVoid(savedPosSquare))
                        if (PlaceNumber(nextRow, nextCol, savedPosSquare))
                            return true;

                }

                square[row, col] = 0; // Backtrack
                rowsSum[row] -= num;
                colSum[col] -= num;
                if(row == col)
                    primDiagSum -= num;
                if (row + col == size - 1)
                    secDiagSum -= num;
            }

            return false;
        }

        public bool IsValid()
        {
            int sum;

            // Check rows and columns
            for (int i = 0; i < size; i++)
            {
                sum = 0;
                for (int j = 0; j < size; j++)
                    sum += square[i, j];
                if (sum != targetSum)
                    return false;

                sum = 0;
                for (int j = 0; j < size; j++)
                    sum += square[j, i];
                if (sum != targetSum)
                    return false;
            }

            // Check diagonals
            sum = 0;
            for (int i = 0; i < size; i++)
                sum += square[i, i];
            if (sum != targetSum)
                return false;

            sum = 0;
            for (int i = 0; i < size; i++)
                sum += square[i, size - 1 - i];
            if (sum != targetSum)
                return false;

            return true;
        }

        private void PrintSquare()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(square[i, j] + " ");
                }
                Console.WriteLine();
                
            }
        }

        public int[,] GetSolution()
        {
            return square;
        }
    }
}
