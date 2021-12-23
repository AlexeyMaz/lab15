using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace lab15
{
    public partial class Form1 : Form
    {
        public Graph g = new Graph();
        public int drag = -1;
        public int drage = -1;
        public int dx1 = 0;
        public int dy1 = 0;
        public int dx2 = 0;
        public int dy2 = 0;
        public bool act = false;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.Width = this.Width - 16;
            pictureBox1.Height = this.Height - pictureBox1.Location.Y - 39;
            g.x = pictureBox1.Width / 2;
            g.y = pictureBox1.Height / 2;
            SolidBrush myBrush2 = new SolidBrush(Color.Gold);
            Bitmap buffer = new Bitmap(Width, Height);
            Graphics gfx = Graphics.FromImage(buffer);
            SolidBrush myBrush = new SolidBrush(Color.Black);
            Pen myPen = new Pen(Color.Black);
            Pen myPen2 = new Pen(Color.Black);

            gfx.Clear(Color.White);
            foreach (Graph.Node n in g.nodes)
            {
                foreach (int eg in n.edges)
                {
                    foreach (Graph.Node m in g.nodes)
                    {
                        if (m.id == eg)
                        {
                            double a = Match.point_direction(n.x, n.y, m.x, m.y);
                            double dist = Match.point_distance(n.x, n.y, m.x, m.y);
                            gfx.DrawLine(myPen2,
                                new Point(n.x + (int)Match.lengthdir_x(g.sz / 2, a), n.y + (int)Match.lengthdir_y(g.sz / 2, a)),
                                new Point(n.x + (int)Match.lengthdir_x(dist - (g.sz / 2), a),
                                n.y + (int)Match.lengthdir_y(dist - (g.sz / 2), a)));
                        }
                    }
                }
            }
            foreach (Graph.Node n in g.nodes)
            {
                myPen.Color = Color.Red;
                if (n.active == 0)
                    myBrush2.Color = Color.Gold;
                if (n.active == 1)
                    myBrush2.Color = Color.RoyalBlue;
                gfx.FillEllipse(myBrush2, new Rectangle(n.x - g.sz / 2, n.y - g.sz / 2, g.sz, g.sz));
                gfx.DrawEllipse(myPen, new Rectangle(n.x - g.sz / 2, n.y - g.sz / 2, g.sz, g.sz));
                gfx.DrawString(n.name, new Font("Arial", 20, FontStyle.Regular), myBrush, new PointF(n.x - g.sz / 5, n.y - 15));
            }
            if (drage != -1)
            {
                if (checkBox2.Checked)
                    myPen2.Color = Color.Red;

                double a1 = Match.point_direction(dx1, dy1, dx2, dy2);
                double dist1 = Match.point_distance(dx1, dy1, dx2, dy2);
                gfx.DrawLine(myPen2,
                    new Point(dx1 + (int)Match.lengthdir_x(g.sz / 2, a1), dy1 + (int)Match.lengthdir_y(g.sz / 2, a1)),
                    new Point(dx1 + (int)Match.lengthdir_x(dist1, a1), dy1 + (int)Match.lengthdir_y(dist1, a1)));
            }
            pictureBox1.Image = buffer;
            myBrush.Dispose();
            myPen.Dispose();
            myPen2.Dispose();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (g.nodes.Count == 0 || act)
                button3.Enabled = false;
            else
                button3.Enabled = true;

            g.sz = 80;
            for (int i = 0; i < g.nodes.Count; i++)
                for (int j = 0; j < g.nodes.Count; j++)
                    if (i != j)
                    {
                        double dist = Match.point_distance(g.nodes[i].x, g.nodes[i].y, g.nodes[j].x, g.nodes[j].y);
                        int sz_in = 10;
                        if (dist <= (g.sz + sz_in))
                        {
                            var rand = new Random();
                            if (g.nodes[i].x == g.nodes[j].x)
                            {
                                if (rand.Next(2) == 1)
                                    g.nodes[i].x += 1;
                                else
                                    g.nodes[i].x -= 1;
                            }
                            if (g.nodes[i].y == g.nodes[j].y)
                            {
                                if (rand.Next(2) == 1)
                                    g.nodes[i].y += 1;
                                else
                                    g.nodes[i].y -= 1;
                            }
                            if (g.nodes[i].x < g.nodes[j].x)
                                g.nodes[i].x -= (int)(g.sz + sz_in - dist);
                            else
                                g.nodes[i].x += (int)(g.sz + sz_in - dist);
                            if (g.nodes[i].y < g.nodes[j].y)
                                g.nodes[i].y -= (int)(g.sz + sz_in - dist);
                            else
                                g.nodes[i].y += (int)(g.sz + sz_in - dist);
                        }
                    }
            Refresh();
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag != -1)
                foreach (Graph.Node n in g.nodes)
                    if (drag == n.id)
                    {
                        n.x = e.X;
                        n.y = e.Y;
                        break;
                    }
            if (drage != -1)
                foreach (Graph.Node n in g.nodes)
                    if (drage == n.id)
                    {
                        dx1 = n.x;
                        dy1 = n.y;
                        dx2 = e.X;
                        dy2 = e.Y;
                        break;
                    }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drage = -1;
                if (drag == -1)
                    foreach (Graph.Node n in g.nodes)
                        if (Match.point_distance(n.x, n.y, e.X, e.Y) < g.sz / 2)
                        {
                            drag = n.id;
                            n.x = e.X;
                            n.y = e.Y;
                            break;
                        }
            }
            if (!act)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    drag = -1;
                    drage = -1;
                    foreach (Graph.Node n in g.nodes)
                    {
                        if (Match.point_distance(n.x, n.y, e.X, e.Y) < g.sz / 2)
                        {
                            g.RemoveNode(n.id);
                            break;
                        }
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    drag = -1;
                    dx1 = 0;
                    dy1 = 0;
                    dx2 = 0;
                    dy2 = 0;
                    foreach (Graph.Node n in g.nodes)
                    {
                        if (Match.point_distance(n.x, n.y, e.X, e.Y) < g.sz / 2)
                        {
                            drage = n.id;
                            dx1 = n.x;
                            dy1 = n.y;
                            dx2 = e.X;
                            dy2 = e.Y;
                            break;
                        }
                    }
                }
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                drag = -1;
            if (e.Button == MouseButtons.Right)
            {
                if (drage != -1)
                {
                    foreach (Graph.Node n in g.nodes)
                    {
                        if (Match.point_distance(n.x, n.y, e.X, e.Y) < g.sz / 2)
                        {
                            if (n.id != drage)
                            {
                                foreach (Graph.Node m in g.nodes)
                                {
                                    if (m.id == drage)
                                    {
                                        if (checkBox2.Checked)
                                        {
                                            m.RemoveEdge(n.id);
                                            n.RemoveEdge(m.id);
                                        }
                                        else
                                        {
                                            m.AddEdge(n.id);
                                            n.AddEdge(m.id);
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                drage = -1;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            g.AddNode();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            List<Tuple<int, int>> bridges = g.BridgesList();
            List<int> nodesInBridges = new List<int>();
            for (int i = 0; i < bridges.Count; i++)
            {
                if (!nodesInBridges.Contains(bridges[i].Item1)) nodesInBridges.Add(bridges[i].Item1);
                if (!nodesInBridges.Contains(bridges[i].Item2)) nodesInBridges.Add(bridges[i].Item2);
            }
            foreach (Graph.Node n in g.nodes)
                n.active = 0;
            for (int i = 0; i < nodesInBridges.Count; i++)
                foreach (Graph.Node n in g.nodes)
                    if (Convert.ToInt32(n.name) == nodesInBridges[i]) n.active = 1;
            textBox1.Text = "";
            if (bridges.Count == 0)
                textBox1.Text = "Мосты не найдены";
            else
            {
                textBox1.Text = "Мосты: " + Environment.NewLine;
                for (int i = 0; i < bridges.Count; i++)
                    textBox1.Text += bridges[i].Item1 + " " + bridges[i].Item2 + Environment.NewLine;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            using (FileStream writer = File.Create("g.txt"))
            {
                foreach (Graph.Node n in g.nodes)
                {
                    string S = "";
                    S += n.id.ToString() + ",";
                    S += n.x.ToString() + ",";
                    S += n.y.ToString() + ";";
                    if (n.edges.Count != 0)
                    {
                        foreach (int eg in n.edges)
                        {
                            S += eg.ToString() + ",";
                        }
                        S = S.Remove(S.Length - 1, 1);
                    }
                    S += "\n";
                    byte[] info = new UTF8Encoding(true).GetBytes(S);
                    writer.Write(info, 0, info.Length);
                }
                writer.SetLength(writer.Length - 1);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            g.nodes.Clear();
            using (StreamReader reader = new StreamReader("g.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string S = reader.ReadLine();
                    string[] SS = S.Split(';');
                    string[] ss1 = SS[0].Split(',');
                    List<int> id_x_y = new List<int>();
                    for (int i = 0; i < 3; i++)
                        id_x_y.Add(int.Parse(ss1[i]));
                    List<int> L = new List<int>();
                    if (SS[1] != "")
                    {
                        string[] SSE = SS[1].Split(',');
                        foreach (string eg in SSE)
                            L.Add(int.Parse(eg));
                    }
                    g.LoadNode(id_x_y[0], id_x_y[1], id_x_y[2], L);
                }
            }
        }
    }
}