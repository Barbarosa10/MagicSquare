using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicSquare
{
    public partial class FormMagicSquare : Form
    {
        TextBox[,] square;
        public FormMagicSquare()
        {
            InitializeComponent();
            square = new TextBox[,] { { el11, el12, el13, el14 }, { el21, el22, el23, el24 }, { el31, el32, el33, el34}, { el41, el42, el43, el44 }};
        }

        private void Rezolvare_Click(object sender, EventArgs e)
        {
            int size = Convert.ToInt32(comboDimensiune.SelectedItem.ToString());
            int suma = Convert.ToInt32(textSuma.Text);
            MagicSquareSolver s = new MagicSquareSolver(size, suma);
            s.Solve();
            int[,] squareSolve = s.GetSolution();

            for(int i=0; i<size;i++)
            {
                for(int j=0; j<size; j++)
                {
                    square[i,j].Text = squareSolve[i,j].ToString();
                }
            }
        }
    }
}
