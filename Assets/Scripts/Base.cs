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

    internal void UpdateGraph(bool isHorizontal, int index)
    {
        Tile tile = tiles[index];
        if(isHorizontal)
        {
            if(tile.left!=null)
            {
                tile.left.disableWall(isHorizontal);
                tile.left.disableWall(!isHorizontal);
            }
            if (tile.right != null)
            {
                tile.right.disableWall(isHorizontal);
            }
        }
        else
        {
             //Bottom and top tiles veertical
             //Right and top rights horizontal
        }
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
    
}
