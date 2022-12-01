using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Helpers;

namespace AdventOfCode
{
    public static class Day12
    {
        private const string StartName = "start";
        private const string EndName = "end";
        public static long GetP1() => FuncPart1(GetInput());
        public static long GetP2() => FuncPart2(GetInput());
        private static Graph GetInput()
        {
            var nodeNames = new Dictionary<string, Node>();
            foreach (var line in File.ReadAllLines(Program.InputFolderPath + "/Day12.txt"))
            {
                var nodes = line.Split('-');
                var node0 = new Node();
                var node1 = new Node();
                node0.name = nodes[0];
                node1.name = nodes[1];
                if (!nodeNames.ContainsKey(nodes[0]))
                {
                    nodeNames.Add(nodes[0], node0);
                }
                else
                {
                    node0 = nodeNames[nodes[0]];
                }
                if (!nodeNames.ContainsKey(nodes[1]))
                {
                    nodeNames.Add(nodes[1], node1);
                }
                else
                {
                    node1 = nodeNames[nodes[1]];
                }
                node0.edges.Add(node1);
                node1.edges.Add(node0);
            }
            nodeNames.Foreach(i =>
            {
                if (Char.IsLower(i.Key[0]))
                {
                    i.Value.SmallCave = true;
                }
                else
                {
                    i.Value.SmallCave = false;
                }
            });
            return new Graph(nodeNames[StartName], nodeNames[EndName]);
        }
        private static long FuncPart1(Graph input)
        {
            return GetAllPaths(input.Start, input.End, new List<Node>() { input.Start });
        }
        private static long GetAllPaths(Node current, Node end, List<Node> visited)
        {
            if (current == end)
            {
                return 1;
            }
            return current.edges.Sum(i =>
            {
                if (!visited.Contains(i))
                {
                    return GetAllPaths(i, end, i.SmallCave ? visited.ToList().ADD(i) : visited);
                }
                return 0;
            });
        }
        private static long FuncPart2(Graph input)
        {
            return GetAllPaths2(input.Start, input.End, new List<Node>() { input.Start }, false);
        }
        private static long GetAllPaths2(Node current, Node end, List<Node> visited, bool SmallCaveVisitedTwice)
        {
            if (current == end)
            {
                return 1;
            }
            return current.edges.Sum(i =>
            {
                if (!visited.Contains(i))
                {
                    return GetAllPaths2(i, end, i.SmallCave ? visited.ToList().ADD(i) : visited, SmallCaveVisitedTwice);
                }
                else if ((!SmallCaveVisitedTwice) && i != visited[0])
                {
                    return GetAllPaths2(i, end, visited, true);
                }
                return 0;
            });
        }
    }
}
internal class Graph
{
    public Node Start = null;
    public Node End = null;
    public Graph(Node start, Node end)
    {
        Start = start;
        End = end;
    }
}

internal class Node
{
    public string name = "";
    public bool SmallCave = false;
    public List<Node> edges = new List<Node>();
    public Node() { }
}