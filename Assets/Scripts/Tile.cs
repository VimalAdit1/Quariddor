using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;

    [SerializeField]
    Material neighbour;
    [SerializeField]
    Material baseColor;

    [SerializeField]
    Transform[] wallAnchors;

    [SerializeField]
    Vector3 wallOffset;

    [SerializeField]
    Wall wallPrefab;

    public bool isValid;

    MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        isValid = false;
        SpawnWalls();
    }

    private void SpawnWalls()
    {
        int[] anchors = new int[4];
        bool isHorizontal=false;
        int i = 0;
        if (y!=0&&x!=8)
        {
            anchors[i] =3;
            i++;
        }
        if (x!=8&&y!=8)
        {
            anchors[i] = 0;
            i++;
        }
        for(int j = 0; j <i;j++)
        {
            int anchorIndex = anchors[j];
            Transform anchor = wallAnchors[anchorIndex];
            if(anchorIndex == 3)
            {
                isHorizontal = true;
            }
            else
            {
                isHorizontal = false;
            }
            if (anchor != null)
            {
                Wall wall = GameObject.Instantiate(wallPrefab, anchor.transform.localPosition + wallOffset, Quaternion.identity);
                wall.transform.parent = anchor;
                wall.transform.localPosition = Vector3.zero + wallOffset;
                wall.transform.localRotation = Quaternion.EulerAngles(0, 0, 0);
                wall.isHorizontal = isHorizontal;
            }
        }
        
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
}
