using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField]
    private Tile tilePrefab;

    [SerializeField]
    private Player playerPrefab;

    Tile[] tiles;

    Player player;

    TileGraph tileGraph;

    [SerializeField]
    private int width, height;
    // Start is called before the first frame update
    void Start()
    {
        tiles = new Tile[width* height];
        tileGraph = new TileGraph();
        GenerateBoard();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        player = GameObject.Instantiate(playerPrefab, new Vector3(0, this.playerPrefab.yOffset, (width / 2)), Quaternion.identity);
        player.x = 0;
        player.y = width / 2;
    }

    public TileGraph getTileGraph()
    {
        return tileGraph;
    }
    public Player getPlayer()
    {
        return player;
    }
    private void GenerateBoard()
    {
        int index = 0;
        for (int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                Tile tile = GameObject.Instantiate(tilePrefab, new Vector3(i,this.transform.position.y, j), Quaternion.identity);
                tile.name = "Tile" + i + j;
                tile.index = index;
                tile.board = this;
                AddNeighbours(index,i,j);
                tiles[index] = tile;
                index++;
                tile.x = i;
                tile.y = j;
            }
        }
        UpdateNeighboursForTiles();
        //tileGraph.PrintNeighbours();
    }

    private void UpdateNeighboursForTiles()
    {
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                Debug.Log(i + " " + j);
                int index = GetTileIdFromCoordinates(i, j);
                Tile tile = tiles[index];
                if (i < width - 1)
                    tile.top = tiles[GetTileIdFromCoordinates(i + 1, j)];
                if (i > 0)
                    tile.bottom = tiles[GetTileIdFromCoordinates(i - 1, j)];
                if (j < height - 1)
                    tile.left = tiles[GetTileIdFromCoordinates(i, j+1)];
                if (j > 0)
                    tile.right = tiles[GetTileIdFromCoordinates(i, j - 1)];
                tiles[index] = tile;
            }
        }
    }

    private void AddNeighbours(int index, int x,int y)
    {
        if (x < width - 1)
            tileGraph.addEdge(index, GetTileIdFromCoordinates(x + 1, y));
        if (x > 0)
            tileGraph.addEdge(index, GetTileIdFromCoordinates(x - 1, y));
        if (y < height - 1)
            tileGraph.addEdge(index, GetTileIdFromCoordinates(x, y + 1));
        if (y > 0)
            tileGraph.addEdge(index, GetTileIdFromCoordinates(x, y - 1));
    }

    private int GetTileIdFromCoordinates(int i, int j)
    {
        return (i * width + j);
    }
    
    internal void PlaceWall(bool isVertical, int index)
    {
        Tile tile = tiles[index];
        tileGraph=RemoveNeighbours(isVertical, index,tileGraph);
        if(isVertical)
        {
            if(tile.top!=null)
            {
                tile.top.disableWall(isVertical);

            }
            if (tile.bottom != null)
            {
                tile.bottom.disableWall(isVertical);
            }
            if (tile.right != null)
            {
                tile.right.disableWall(!isVertical);          
            }

            // Remove top right
        }
        else
        {
            if (tile.right != null)
            {
                tile.right.disableWall(isVertical);
            }
            if (tile.left != null)
            {
                tile.left.disableWall(isVertical);
                tile.left.disableWall(!isVertical);
            }

            //Remove left's top
        }
    }

    internal TileGraph RemoveNeighbours(bool isVertical,int index, TileGraph tileGraphCopy)
    {
        Tile tile = tiles[index];
        if (isVertical)
        {
            if (tile.top != null)
            {
                Tile top = tile.top;
                if (top.right != null)
                {
                    tileGraphCopy.removeEdge(top.index, top.right.index);
                }
            }
            if(tile.right != null)
            {
                tileGraphCopy.removeEdge(index, tile.right.index);
            }
        }
        else
        {
            if (tile.left != null)
            {
                Tile left = tile.left;
                if (left.top != null)
                {
                    tileGraphCopy.removeEdge(left.index, left.top.index);
                }
            }
            if (tile.top != null)
            {
                tileGraphCopy.removeEdge(index, tile.top.index);
            }
        }
        return tileGraphCopy;
    }

    internal void MovePlayer(int x, int y)
    {
        player.transform.position = new Vector3(x,player.yOffset,y);
        player.x = x;
        player.y = y;
        resetMaterials();
    }

    internal void showNeighbours(int x, int y)
    {
        resetMaterials();
        //pathExists(x, y);
        int index = GetTileIdFromCoordinates(x, y);
        List<int> neighbours = tileGraph.getNeighbours(index);
        foreach(int neighbour in neighbours)
        {
            tiles[neighbour].Select();
        }
    }

    private void resetMaterials()
    {
        foreach (Tile tile in tiles)
        {
            tile.Reset();
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool pathExists(int x, int y,TileGraph graph)
    {
        int index = GetTileIdFromCoordinates(x, y);
        foreach (Tile tile in tiles)
        {
            if (tile.index != index)
            {
                tile.g = int.MaxValue;
            }
            else
            {
                tile.g = 0;
            }    
            tile.h = width-1 - tile.x;
            tile.previosTile = null;
            tile.CalculateFCost();
        }
        Debug.Log("Initialized");
        
        List<int> searched = new List<int>();
        List<int> toSearch = new List<int>();
        toSearch.Add(index);
        Tile previousTile = null;
        while (toSearch.Count > 0)
        {
            int currentIndex = getOptimalTile(toSearch);
            Debug.Log("Searching Node"+ currentIndex);
            toSearch.Remove(currentIndex);
            Tile currentTile = tiles[currentIndex];
            if (currentTile.h <= 0)
            {
                currentTile.previosTile = previousTile;
                ColorPath(currentTile);
                return true;
            }
            else if (!searched.Contains(currentIndex))
            {
                List<int> neighbours = graph.getNeighbours(currentIndex);
                foreach(int neighbour in neighbours)
                {
                    Tile neighbourTile = tiles[neighbour];
                    neighbourTile.g = currentTile.g + 1;
                    //neighbourTile.previosTile = currentTile;
                    neighbourTile.CalculateFCost();
                    if(!searched.Contains(neighbour)&&!toSearch.Contains(neighbour))
                    {
                        toSearch.Add(neighbour);
                    }
                }
            searched.Add(currentIndex);
             currentTile.previosTile = previousTile;
             previousTile = currentTile;
            }
            
        }
        return false;
    }

    private void ColorPath(Tile tile)
    {
        while (tile != null)
        {
            tile.Select();
            tile = tile.previosTile; 
        }
    }

    public int getOptimalTile(List<int> toSearch)
    {
        int minF = int.MaxValue;
        int minIndex = 0;
        foreach(int tile in toSearch)
        {
            if(tiles[tile].f<minF)
            {
                minIndex = tile;
                minF = tiles[tile].f;
            }
        }
        Debug.Log("Optimal Tile"+minIndex);
        return minIndex;
    }

}
