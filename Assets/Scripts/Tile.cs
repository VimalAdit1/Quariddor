using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y,index;

    public Vector3 originalTransform;

    [SerializeField]
    Material neighbour;
    [SerializeField]
    Material baseColor;

    public int VERTICAL_WALL_INDEX = 3;
    public int HORIZONTAL_WALL_INDEX = 0;

    [SerializeField]
    Transform[] wallAnchors;

    [SerializeField]
    Vector3 wallOffset;

    [SerializeField]
    Wall wallPrefab;

    [SerializeField]
    List<Wall> walls;

    public bool isValid;
    public Base board;

    public Tile left;
    public Tile right;
    public Tile top;
    public Tile bottom;
    

    //A* varaibles
    public int f;
    public int g;
    public int h;
    public Tile previosTile;

    MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        isValid = false;
        walls = new List<Wall>();
        SpawnWalls();
        originalTransform = this.transform.position;
    }

    public Base getBoard()
    {
        return board;
    }
    private void SpawnWalls()
    {
        int[] anchors = new int[4];
        bool isVertical=false;
        int i = 0;
        if (y!=0&&x!=8)
        {
            anchors[i] =VERTICAL_WALL_INDEX;
            i++;
        }
        if (x!=8&&y!=8)
        {
            anchors[i] = HORIZONTAL_WALL_INDEX;
            i++;
        }
        for(int j = 0; j <i;j++)
        {
            int anchorIndex = anchors[j];
            Transform anchor = wallAnchors[anchorIndex];
            if(anchorIndex == VERTICAL_WALL_INDEX)
            {
                isVertical = true;
            }
            else
            {
                isVertical = false;
            }
            if (anchor != null)
            {
                Wall wall = GameObject.Instantiate(wallPrefab, anchor.transform.localPosition + wallOffset, Quaternion.identity);
                wall.transform.parent = anchor;
                wall.transform.localPosition = Vector3.zero + wallOffset;
                wall.transform.localRotation = Quaternion.EulerAngles(0, 0, 0);
                wall.isVertical = isVertical;
                wall.parentTile = this;
                wall.wallIndex = anchorIndex;
                walls.Add(wall);
            }
        }
        
    }

    internal void PlaceWall(bool isVertical)
    {
        
       board.PlaceWall(isVertical, index);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Select()
    {
        renderer.material = neighbour;
        isValid = true;
    }

    internal void Reset()
    {
        renderer.material = baseColor;
        isValid = false;
    }

    internal void disableWall(bool isVertical)
    {
       
       foreach(Wall wall in walls)
        {
            if(wall != null&&wall.isVertical==isVertical)
            {
                wall.gameObject.SetActive(false);
                Debug.Log("Disabling wall " + wall.name);
            }
        }
    }

    public void CalculateFCost()
    {
        f = g + h;
    }
}
