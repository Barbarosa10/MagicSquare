using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MagicSquare;
using System.Collections.Generic;

namespace MagicSquareTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestIsValidFalse()
        {
            MagicSquareSolver s = new MagicSquareSolver(3, 15);
            s.square = new int[3, 3] {
                {1, 2, 3 },
                {4 ,5 ,6 },
                {7, 8, 9 }
                };
            Assert.IsTrue(!s.IsValid());
            
        }
        [TestMethod]
        public void TestIsValidTrue()
        {
            MagicSquareSolver s = new MagicSquareSolver(3, 15);
            s.square = new int[3, 3] {
                {2, 7, 6 },
                {9 ,5 ,1 },
                {4, 3, 8 }
                };
            Assert.IsTrue(s.IsValid());
        }

        [TestMethod]
        public void TestCheckDomainVoidTrue()
        {
            MagicSquareSolver s = new MagicSquareSolver(3, 15);
            List<int> [,] posSquare = new List<int>[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    posSquare[i, j] = new List<int>(); // Initialize each cell as a new list
                    for (int k = 1; k <= 3 * 3; k++)
                    {
                        posSquare[i, j].Add(k);
                    }
                    // Optionally add elements to the list here
                }
            }
            Assert.IsTrue(!s.CheckIfDomainsVoid(posSquare));
        }

        [TestMethod]
        public void TestCheckDomainFalse()
        {
            MagicSquareSolver s = new MagicSquareSolver(3, 15);
            List<int>[,] posSquare = new List<int>[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    posSquare[i, j] = new List<int>(); // Initialize each cell as a new list
                    // Optionally add elements to the list here
                }
            }
            Assert.IsTrue(s.CheckIfDomainsVoid(posSquare));
        }

        [TestMethod]
        public void TestDeepCopy()
        {
            MagicSquareSolver s = new MagicSquareSolver(3, 15);
            List<int>[,] posSquare = new List<int>[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    posSquare[i, j] = new List<int>(); // Initialize each cell as a new list
                    for (int k = 1; k <= 3 * 3; k++)
                    {
                        posSquare[i, j].Add(k);
                    }
                    // Optionally add elements to the list here
                }
            }
            List<int>[,] dom = s.DeepCopyDomains(posSquare);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < posSquare[i,j].Count; k++)
                    {
                        Assert.AreEqual(posSquare[i, j][k], dom[i, j][k]);
                    }
                    // Optionally add elements to the list here
                }
            }
        }


        [TestMethod]
        public void TestSolve3x3()
        {
            MagicSquareSolver s = new MagicSquareSolver(3, 15);
            s.Solve();
            Assert.IsTrue(s.IsValid());
        }

        [TestMethod]
        public void TestSolve4x4()
        {
            MagicSquareSolver s = new MagicSquareSolver(4, 34);
            s.Solve();
            Assert.IsTrue(s.IsValid());
        }

    }
}
