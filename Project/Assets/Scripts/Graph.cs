﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Graph<T>
    {

        public List<Node<T>> Nodes { get; private set; } = new List<Node<T>>();
        public List<Edge<T>> Edges { get; private set; } = new List<Edge<T>>();

        public Graph()
        {

        }

        public Node<T> AddNode(Node<T> node)
        {
            Nodes.Add(node);
            return node;
        }

        public Node<T> AddNode(T item)
        {
            return AddNode(new Node<T>(item));
        }

        public void RemoveNode(Node<T> node)
        {
            Nodes.Remove(node);
            foreach (var edge in node.Edges)
            {
                if (edge.Node1 == node)
                    edge.Node2.RemoveEdge(edge);
                else
                    edge.Node1.RemoveEdge(edge);
            }
        }

        public void RemoveNode(T item)
        {
            foreach (var node in Nodes)
            {
                if (node.Item.Equals(item))
                {
                    RemoveNode(node);
                }
            }
        }

        public void Bind(Node<T> node1, Node<T> node2)
        {
            Edge<T> edge = new Edge<T>(node1, node2);
            Edges.Add(edge);
            node1.AddEdge(edge);
            node2.AddEdge(edge);
        }

        public void Bind(T item1, T item2)
        {
            Bind(AddNode(item1), AddNode(item2));
        }

        public void Disbind(Node<T> node1, Node<T> node2)
        {
            if (node2.Edges.Count < node1.Edges.Count)
            {
                Node<T> temp = node1;
                node1 = node2;
                node2 = temp;
            }

            List<Edge<T>> toRemove = new List<Edge<T>>();
            foreach (var edge in node1.Edges)
            {
                if (edge.Node1.Equals(node2) || edge.Node2.Equals(node2))
                {
                    toRemove.Add(edge);
                    Edges.Remove(edge);
                    node2.RemoveEdge(edge);
                }
            }
            foreach (var edge in toRemove)
            {
                node1.RemoveEdge(edge);
            }
        }

        public void Disbind(T item1, T item2)
        {
            Node<T> node1 = null, node2 = null;
            foreach (var node in Nodes)
            {
                if (node.Item.Equals(item1))
                    node1 = node;
                if (node.Item.Equals(item2))
                    node2 = node;
            }

            Disbind(node1, node2);
        }

        public class Edge<T>
        {
            public Node<T> Node1 { get; internal set; }
            public Node<T> Node2 { get; internal set; }

            internal Edge(Node<T> node1, Node<T> node2)
            {
                Node1 = node1;
                Node2 = node2;
            }

        }

        public class Node<T>
        {
            public List<Edge<T>> Edges { get; private set; } = new List<Edge<T>>();

            public T Item { get; private set; }

            internal void AddEdge(Edge<T> edge)
            {
                Edges.Add(edge);
            }

            internal void RemoveEdge(Edge<T> edge)
            {
                Edges.Remove(edge);
            }

            internal Node(T item)
            {
                Item = item;
            }
        }

    }
}