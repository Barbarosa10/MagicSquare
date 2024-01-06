using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private int[,] square;
        private int size;
        private int targetSum;
        private bool[] usedNumbers;
        private int[] rowsSum;
        private int[] colSum;
        private int primDiagSum, secDiagSum;
        private List<int>[,] posSquare;

        public MagicSquareSolver(int size, int targetSum = 15)
        {
            this.size = size;
            this.square = new int[size, size];
            this.targetSum = targetSum; // Specific for 3x3 magic square
            this.usedNumbers = new bool[targetSum+1]; // Track used numbers
            this.posSquare = new List<int>[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    posSquare[i, j] = new List<int>(); // Initialize each cell as a new list
                    for(int k=1; k<=size*size;k++)
                    {
                        posSquare[i, j].Add(k);
                    }
                                                           // Optionally add elements to the list here
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

        List<int>[,] DeepCopyDomains(List<int>[,] originalDomains)
        {
            List<int>[,] newDomains = new List<int>[size, size];
            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                    newDomains[i,j] = new List<int>(originalDomains[i,j]);
            }
            return newDomains;
        }

        private bool CheckIfDomainsVoid(List<int>[,] domain)
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
                    for(int i=0;i<size;i++)
                    {
                        for(int j=0;j<size;j++)
                        {
                            if (!(i == row && j == col))
                            {
                                savedPosSquare[i, j].Remove(num);
                            }
                        }
                    }
                    //Removing from column
                    for(int i=0;i<size;i++)
                    {
                        if (i != row)
                            savedPosSquare[i, col].RemoveAll(x => x >= (targetSum - num));
                    }
                    //Remove from row
                    for (int i = 0; i < size; i++)
                    {
                        if (i != col)
                            savedPosSquare[row, i].RemoveAll(x => x >= (targetSum - num));
                    }
                    //Remove for primary diagonal
                    if(row == col)
                    {
                        for(int i=0;i<size;i++)
                        {
                            if(i != row)
                                savedPosSquare[i, i].RemoveAll(x => x >= (targetSum - num));
                        }
                    }
                    //Remove for secondary
                    if(row + col == size-1)
                    {
                        for(int i=0;i<size;i++)
                        {
                            if(i != row)
                                savedPosSquare[i,size-1-i].RemoveAll(x => x >= (targetSum - num));
                        }
                    }

                    if(!CheckIfDomainsVoid(savedPosSquare))
                        if (PlaceNumber(nextRow, nextCol, savedPosSquare))
                            return true;
                    //Add for primary diagonal
                   /* if (row == col)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            if(i!= row)
                                posSquare[i, i].AddRange(Enumerable.Range(size * size - num, num));
                        }
                    }
                    //Add for secondary
                    if (row + col == size-1)
                    {
                        for(int i=0;i<size;i++)
                        {
                            if(i != row)
                                posSquare[i,size-1-i].AddRange(Enumerable.Range(size * size - num, num));
                        }
                    }
                    //Adding to column
                    for (int i = 0; i < size; i++)
                    {
                        if (i != col)
                            posSquare[row, i].AddRange(Enumerable.Range(size*size-num, num));
                    }
                    //Adding to row
                    for (int i = 0; i < size; i++)
                    {
                        if (i != row)
                            posSquare[i, col].AddRange(Enumerable.Range(size*size-num, num));
                    }
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (i != row && j != col)
                                posSquare[i, j].Add(num);
                        }
                    }*/

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

/*        private bool PlaceNumber(int row, int col)
        {
            if (row == size)
            {
                return IsValid();
            }

            int nextRow = col == size - 1 ? row + 1 : row;
            int nextCol = col == size - 1 ? 0 : col + 1;

            for (int num = 1; num <= size; num++)
            {
                if (!usedNumbers[num])
                {
                    square[row, col] = num;
                    rowsSum[row] += num;
                    colSum[col] += num;
                    if (row == col)
                        primDiagSum += num;
                    usedNumbers[num] = true;
                    if (rowsSum[row] <= targetSum && colSum[col] <= targetSum && primDiagSum <= targetSum)
                    {
                        if (PlaceNumber(nextRow, nextCol))
                        {
                            return true;
                        }
                    }

                    square[row, col] = 0; // Backtrack
                    rowsSum[row] -= num;
                    colSum[col] -= num;
                    if (row == col)
                        primDiagSum -= num;
                    usedNumbers[num] = false;
                }
            }

            return false;
        }*/

        private bool IsSumPotentialValid()
        {
            int sum;

            // Check rows and columns
            for (int i = 0; i < size; i++)
            {
                sum = 0;
                for (int j = 0; j < size; j++)
                    sum += square[i, j];
                if (sum > targetSum)
                    return false;

                sum = 0;
                for (int j = 0; j < size; j++)
                    sum += square[j, i];
                if (sum > targetSum)
                    return false;
            }

            // Check diagonals
            sum = 0;
            for (int i = 0; i < size; i++)
                sum += square[i, i];
            if (sum > targetSum)
                return false;

            sum = 0;
            for (int i = 0; i < size; i++)
                sum += square[i, size - 1 - i];
            if (sum > targetSum)
                return false;

            return true;
        }

        private bool IsValid()
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
