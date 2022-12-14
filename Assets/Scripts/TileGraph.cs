using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGraph
{
    Dictionary<int, List<int>> edgeList;
    public TileGraph()
    {
        edgeList = new Dictionary<int, List<int>>();
    }

    public Dictionary<int, List<int>> getEdgeList()
    {
        return edgeList;
    }

    public TileGraph(Dictionary<int, List<int>> edgeList)
    {
        this.edgeList = new Dictionary<int, List<int>>();
        foreach(int key in edgeList.Keys)
        {
            this.edgeList.Add(key, new List<int>(edgeList[key]));
        }
    }
    public void addEdge(int tile, int neighbour)
    {
        if (edgeList.ContainsKey(tile))
        {
            edgeList[tile].Add(neighbour);
        }
        else
        {
            edgeList.Add(tile, new List<int>() { neighbour });
        }
    }
    public void removeEdge(int tile, int neighbour)
    {
        if (edgeList.ContainsKey(tile))
        {
            edgeList[tile].Remove(neighbour);
        }
        else
        {
            Debug.Log(neighbour + " not a neighbour of " + tile);
        }
        if (edgeList.ContainsKey(neighbour))
        {
            edgeList[neighbour].Remove(tile);
        }
        else
        {
            Debug.Log(neighbour + " not a neighbour of " + tile);
        }
    }
    public List<int> getNeighbours(int tile)
    {
        return edgeList.ContainsKey(tile) ? edgeList[tile] : null;
    }

    internal void PrintNeighbours()
    {
        foreach(int key in edgeList.Keys)
        {
            Debug.Log("Key " + key + "Value " + string.Join(", ", edgeList[key]));
        }
    }
}
