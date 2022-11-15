using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    Material hoverMaterial;

    [SerializeField]
    Material normalMaterial;
    // Start is called before the first frame update

    [SerializeField]
    bool isVisible;

    [SerializeField]
    public bool isVertical;

    public bool isDragging;
    public Tile parentTile;
    public int wallIndex;

    MeshRenderer renderer;
    Wall target;
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.enabled = isVisible;
        if(!isVisible)
        {
            renderer.material = hoverMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void placeWall(Vector3 position)
    {
        if(target!=null)
        {
            target.isVisible = true;
            target.enableWall();
        }
        this.transform.position = position;
    }

    private void enableWall()
    {
        renderer.material = normalMaterial;
        parentTile.PlaceWall(isVertical);

    }
    private void OnTriggerEnter(Collider other)
    {
        CheckPaathExistsAndRenderWall(other);
    }
    private void OnTriggerStay(Collider other)
    {
        CheckPaathExistsAndRenderWall(other);
        
    }

    private void CheckPaathExistsAndRenderWall(Collider other)
    {
        if (!isVisible)
        {
            if (other.CompareTag("Wall"))
            {
                Wall wall = other.GetComponentInParent<Wall>();
                if (wall.isVertical == this.isVertical)
                {
                    Base board = this.parentTile.getBoard();
                    TileGraph tileGraphCopy= board.getTileGraph();
                    tileGraphCopy=board.RemoveNeighbours(this.isVertical, this.parentTile.index, tileGraphCopy);
                    Player player = board.getPlayer();
                    if (board.pathExists(player.x, player.y, tileGraphCopy))
                    {
                        wall.target = this;
                        renderer.enabled = true;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isVisible)
        {
            Wall wall = other.GetComponentInParent<Wall>();
            wall.target = null;
            renderer.enabled = isVisible;
        }
    }

}
