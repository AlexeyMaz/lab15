using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using System.Collections.Generic;

namespace lab15
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Graph g = new Graph();
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
            List<Tuple<int, int>> bridges = g.BridgesList();
            Assert.AreEqual(bridges[0].Item1, 2);
            Assert.AreEqual(bridges[0].Item2, 4);
            Assert.AreEqual(bridges[1].Item1, 0);
            Assert.AreEqual(bridges[1].Item2, 6);
        }
    }
}