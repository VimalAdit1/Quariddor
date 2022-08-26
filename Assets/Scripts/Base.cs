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

    Tile[,] tiles;

    Player player;

    [SerializeField]
    private int width, height;
    // Start is called before the first frame update
    void Start()
    {
        tiles = new Tile[width, height];
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
        for (int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                Tile tile = GameObject.Instantiate(tilePrefab, new Vector3(i,this.transform.position.y, j), Quaternion.identity);
                tile.name = "Tile" + i + j;
                tiles[i,j] = tile;
                tile.x = i;
                tile.y = j;
            }
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
        if (x < width-1 )
            tiles[x + 1, y].Select();
        if (x > 0)
            tiles[x - 1, y].Select();
        if (y < height-1)
            tiles[x, y+1].Select();
        if (y > 0)
            tiles[x, y-1].Select();

    }

    private void resetMaterials()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tiles[i, j].Reset();
            }
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
