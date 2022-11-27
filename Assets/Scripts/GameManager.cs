using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Base board;

    public Vector3 Dragoffset;

    bool isSelected;
    Camera camera;
    Vector3 originalTransform = Vector3.zero;
    public bool isDragging;
    Wall dragObject;
    // Start is called before the first frame update
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDragInput();
        CheckPlayerMovements();
    }

    private void CheckDragInput()
    {

        if (Input.touchCount < 1)
        {
            isDragging = false;
            return;
        }

        Touch touch = Input.touches[0];
        Vector3 touchPos = touch.position;
        
        Vector3 pos;
        if (touch.phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(touchPos);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Interactor"))
                {
                    dragObject = hit.transform.GetComponent<Wall>();
                    originalTransform = dragObject.transform.position;
                    dragObject.isDragging = true;
                    isDragging = true;
                }
            }
        }
        if (isDragging && touch.phase == TouchPhase.Moved)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
             {
                pos = hit.point;
                dragObject.transform.position = new Vector3(pos.x, dragObject.transform.position.y, pos.z)+ Dragoffset;
            }
            
        }
        if (isDragging && (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended))
        {
            isDragging = false;
            dragObject.placeWall(originalTransform);
            dragObject = null;
        }
    }

    private void CheckPlayerMovements()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                if (isSelected)
                {
                    Tile tile = objectHit.GetComponent<Tile>();
                    if (tile != null)
                    {
                        if (tile.isValid)
                        {
                            board.MovePlayer(tile.x, tile.y);
                            isSelected = false;
                        }
                    }
                }
                else
                {
                    Player player = objectHit.GetComponent<Player>();
                    if (player != null)
                    {
                        board.showNeighbours(player.x, player.y);
                        //player.transform.Translate(Vector3.up * 1f);
                        isSelected = true;
                    }
                }

            }
        }
    }
}
