using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Game
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int AllocConsole();

        public bool Up, Down;
        Task t;
        Point playerpos;
        Point enemypos;
        bool bounce;
        bool sideWaysDown;
        private bool sideWaysUp;
        private bool colision;

        public Form1()
        {
            InitializeComponent();

            colision = sideWaysUp = sideWaysDown = bounce = Up = Down = false;

            playerpos = player.Location;
            enemypos = enemy.Location;
            t = Task.Run(() => GameLoop());

            Console.WriteLine("test");
        }

        private void GameLoop()
        {
            while (true)
            {
                DetectColision();
                Movement();

                Thread.Sleep(5);
            }
        }

        private void DetectColision()
        {
            if (ball.Location.X < player.Location.X + player.Width && ball.Location.X + ball.Width > player.Location.X + 2 && ball.Location.Y + ball.Height > player.Location.Y && player.Location.Y + player.Height > ball.Location.Y)
            {
                bounce = true;
                colision = true;

                if (Up)
                {
                    sideWaysDown = false;
                    sideWaysUp = true;
                }

                if (Down)
                {
                    sideWaysDown = true;
                    sideWaysUp = false;
                }

            }
            else if (ball.Location.X + ball.Width > enemy.Location.X && ball.Location.X + ball.Width < enemy.Location.X + 2 && ball.Location.Y + ball.Height > enemy.Location.Y && enemy.Location.Y + enemy.Height > ball.Location.Y)
            {
                bounce = false;
                colision = true;

                if (UpEn)
                {
                    sideWaysDown = false;
                    sideWaysUp = true;
                }
                
                if(DownEn)
                {
                    sideWaysDown = true;
                    sideWaysUp = false;
                }

            }
            else
            {
                colision = false;
            }

            if (ball.Location.Y <= 0 || ball.Location.Y >= 500)
            {
                y = -y;
            }
        }

        delegate void SetTextCallback(Form f, PictureBox ctrl, Point position);
        public static void SetPos(Form form, PictureBox ctrl, Point position)
        {
            if (ctrl.InvokeRequired)
            {
                SetTextCallback p = new SetTextCallback(SetPos);
                try
                {
                    form.Invoke(p, new object[] { form, ctrl, position });
                }
                catch (Exception)
                {

                }
            }
            else
            {
                ctrl.Location = position;
            }
        }

        private void Movement()
        {
            if (Up)
            {
                playerpos.Y--;
            }

            if (Down)
            {
                playerpos.Y++;
            }

            if (UpEn)
            {
                enemypos.Y--;
            }

            if (DownEn)
            {
                enemypos.Y++;
            }

            SetPos(this, player, playerpos);
            SetPos(this, enemy, enemypos);
            MoveBall();
        }

        int y = 0;
        private bool DownEn;
        private bool UpEn;

        private void MoveBall()
        {
            Point p;
            if (colision)
            {
                if (sideWaysUp)
                {
                    y = -1;
                }
                else if (sideWaysDown)
                {
                    y = 1;
                }
            }

            if (bounce)
            {
                p = new Point(ball.Location.X + 1, ball.Location.Y + y);
            }
            else
            {
                p = new Point(ball.Location.X - 1, ball.Location.Y + y);
            }



            SetPos(this, ball, p);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {


        }

        private void Painting(object sender, PaintEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Movement_Trigger(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.W:
                    Up = true;
                    break;
                case Keys.S:
                    Down = true;
                    break;
                case Keys.Up:
                    UpEn = true;
                    break;
                case Keys.Down:
                    DownEn = true;
                    break;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Movement_Cancel(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.W:
                    Up = false;
                    break;
                case Keys.S:
                    Down = false;
                    break;
                case Keys.Up:
                    UpEn = false;
                    break;
                case Keys.Down:
                    DownEn = false;
                    break;
            }
        }
    }
}
