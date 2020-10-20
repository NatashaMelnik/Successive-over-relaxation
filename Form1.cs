using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lr2_successiveOverRelaxation
{
    public partial class Form1 : Form
    {
        Graphics gr;
        double eps = 1;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                 | ControlStyles.AllPaintingInWmPaint
                 | ControlStyles.UserPaint,
                 true);

            UpdateStyles();

            MainMenu();
        }

        private void MainMenu()
        {
            gr = this.CreateGraphics();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int n = 3;
            double[,] A = { { 3, -1, -1.5 }, { 2, -1, 3 }, { 1, -1, -0.5 } };
            double[] b = { 1, 3.5, 0.75 };
            double[,] Al = { { 3, -1, -1.5 }, { 2, -1, 3 }, { 1, -1, -0.5 }  };
            double[] bl = { 1, 1, 1 };

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Al[i, j] = -(A[i, j] / A[i, i]);
                }
                bl[i] = b[i] / A[i, i];
            }

            double[] x = { 0, 0, 0 };
            double[] Rn = { 0, 0, 0 };
            double[] xAnswers = { 0, 0, 0};

            for (int i = 0; i < n; i++)
            {
                Rn[i] = bl[i] - x[i];
            }

            //do
            //{
            //    int iteration = 0;
            //    xAnswers = Relaxation(Rn, xAnswers, Al, iteration);

            //    isAllready(xAnswers);
            //}
            //while (!isAllready(xAnswers));

            for (int j = 0; j < 10; j++)
            {
                int iteration = 0;
                xAnswers = Relaxation(Rn, xAnswers, Al, iteration);

                for (int i = 0; i < n; i++)
                {
                    Rn[i] = bl[i] - xAnswers[i] + (xAnswers[i]*Al[i, 0] + xAnswers[i] * Al[i, 1] + xAnswers[i] * Al[i, 2]);
                }
            }

            MessageBox.Show($"X1 = {xAnswers[0]} \nX2 = {xAnswers[1]} \nX3 = {xAnswers[2]}");
        }

        private double[] Relaxation(double[] Rn, double[] xAnswers, double[,] A, int iteration)
        {
            double siX = getMaxAbsRn(Rn);
            int siXIndex = Rn.ToList().IndexOf(siX);

            double[] Rprom = { Rn[0] + A[0, iteration]*siX, Rn[1] + A[1, iteration] * siX, Rn[2] + A[2, iteration] * siX };
            Rprom[siXIndex] = 0;
            //MessageBox.Show($"{Rprom[0]} {Rprom[1]} {Rprom[2]}");

            iteration = iteration + 1;

            xAnswers = xAnswerSummer(xAnswers, siX, siXIndex);
            //MessageBox.Show($"{xAnswers[0]} {xAnswers[1]} {xAnswers[2]}");

            if (iteration == 3)
            {
                return xAnswers;
            }
            else
            {
                 return Relaxation(Rprom, xAnswers, A, iteration);
            }
        }

        private double getMaxAbsRn(double[] Rn)
        {
            double max = 0;
            for (int i = 0; i < Rn.Length; i++)
            {
                if (Math.Abs(Rn[i]) > Math.Abs(max))
                    max = Rn[i];
            }

            return max;
        }

        private double[] xAnswerSummer(double[] xAnswer, double sX, int index)
        {
            xAnswer[index] = xAnswer[index] + sX;
            return xAnswer;
        }

        private bool isAllready(double[] a)
        {
            if (Math.Abs(a[0]) <= eps && Math.Abs(a[1]) <= eps && Math.Abs(a[2]) <= eps)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}