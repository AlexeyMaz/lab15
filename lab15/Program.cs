using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace lab15
{
    public static class Match
    {
        public static double degtorad(double deg)
        {
            return deg * Math.PI / 180;
        }
        public static double radtodeg(double rad)
        {
            return rad / Math.PI * 180;
        }
        public static double lengthdir_x(double len, double dir)
        {
            return len * Math.Cos(degtorad(dir));
        }
        public static double lengthdir_y(double len, double dir)
        {
            return len * Math.Sin(degtorad(dir)) * (-1);
        }
        public static double point_direction(int x1, int y1, int x2, int y2)
        {
            return 180 - radtodeg(Math.Atan2(y1 - y2, x1 - x2));
        }
        public static double point_distance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
    }
    public class Graph
    {
        public class Node
        {
            public int id;
            public int active;
            public int x;
            public int y;
            public string name;
            public List<int> edges;
            public void AddEdge(int id)
            {
                if (!edges.Contains(id)) edges.Add(id);
            }
            public void RemoveEdge(int id)
            {
                edges.Remove(id);
            }
        }
        public List<Node> nodes = new List<Node>();
        private int maxid = 0;
        public int x = 0;
        public int y = 0;
        public int sz = 32;
        public int time = 0;
        public void BridgesSearch(ref List<Tuple<int, int>> bridges, int u, bool[] visited, int[] disc, int[] low, int[] parent)
        {
            visited[u] = true;
            disc[u] = low[u] = ++time;
            foreach (int i in nodes[u].edges)
            {
                int v = i;
                if (!visited[v])
                {
                    parent[v] = u;
                    BridgesSearch(ref bridges, v, visited, disc, low, parent);
                    low[u] = Math.Min(low[u], low[v]);
                    if (low[v] > disc[u])
                    {
                        if (bridges.Count != 0)
                        {
                            for (int j = 0; j < bridges.Count; j++)
                                if ((bridges[j].Item1 != u && bridges[j].Item2 != v)
                                    && (bridges[j].Item1 != v && bridges[j].Item2 != u))
                                {
                                    bridges.Add(new Tuple<int, int>(u, v));
                                    break;
                                }
                        }
                        else bridges.Add(new Tuple<int, int>(u, v));
                    }
                }
                else if (v != parent[u])
                    low[u] = Math.Min(low[u], disc[v]);
            }
        }
        public List<Tuple<int, int>> BridgesList()
        {
            List<Tuple<int, int>> bridges = new List<Tuple<int, int>>();
            int n = nodes.Count; bool[] visited = new bool[n];
            int[] disc = new int[n]; int[] low = new int[n]; int[] parent = new int[n];
            for (int i = 0; i < n; i++)
            {
                parent[i] = -1;
                visited[i] = false;
            }
            for (int i = 0; i < n; i++)
                if (!visited[i])
                    BridgesSearch(ref bridges, i, visited, disc, low, parent);
            int k = 0;
            k++;
            return bridges;
        }
        public void AddNode()
        {
            bool find = false;
            int id = 0;
            for (int i = 0; i < maxid; i++)
            {
                bool exist = false;
                foreach (Node nd in nodes)
                {
                    if (nd.id == i)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                {
                    id = i;
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                id = maxid;
                maxid++;
            }
            Node n = new Node();
            n.id = id;
            n.active = 0;
            n.x = x;
            n.y = y;
            n.name = id.ToString();
            n.edges = new List<int>();
            nodes.Add(n);
            nodes.Sort((x, y) => x.id.CompareTo(y.id));
        }
        public void RemoveNode(int id)
        {
            Node n = null;
            foreach (Node nd in nodes)
            {
                nd.edges.Remove(id);
                if (nd.id == id)
                    n = nd;
            }
            nodes.Remove(n);
        }
        public void LoadNode(int id, int x, int y, List<int> e)
        {
            Node n = new Node();
            if (maxid <= id)
                maxid = id + 1;
            n.id = id;
            n.active = 0;
            n.x = x;
            n.y = y;
            n.name = id.ToString();
            n.edges = e;
            nodes.Add(n);
            nodes.Sort((xx, yy) => xx.id.CompareTo(yy.id));
        }
    }
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}