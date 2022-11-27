using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    List<CinemachineVirtualCamera> cameras;

    [SerializeField]
    Wall verticalWall;

    [SerializeField]
    Wall horizontalWall;

    [SerializeField]
    List<Vector3> DragOffsets;

    // Start is called before the first frame update
    int activeCamera = 0;
    int noOfCameras;
    private Vector2 fingerUp;
    private Vector2 fingerDown;
    [SerializeField]
    GameManager gameManager;
    public float SWIPE_THRESHOLD = 20f;

    void Start()
    {
        noOfCameras = cameras.Count;
        UpdateActiveCamera(activeCamera);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isDragging)
        {
            DetectSwipeInput();
        }
    }

    void UpdateActiveCamera(int camera)
    {
        for(int i=0;i<noOfCameras;i++)
        {
            cameras[i].Priority = 0;
            if(i==camera)
            {
                cameras[activeCamera].Priority = 1;
            }
        }
        if(activeCamera%2==0)
        {
            horizontalWall.isVertical = false;  
        }
        else
        {
            horizontalWall.isVertical = true;
        }

        verticalWall.isVertical = !horizontalWall.isVertical;
        gameManager.Dragoffset = DragOffsets[activeCamera];
    }
  

    void DetectSwipeInput()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
        }
    }
    void checkSwipe()
    {
        float vertical = Mathf.Abs(fingerDown.y - fingerUp.y);
        float horizontal = Mathf.Abs(fingerDown.x - fingerUp.x);


        if (horizontal > SWIPE_THRESHOLD && horizontal > vertical)
        {
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }
    }


    void OnSwipeLeft()
    {
        activeCamera++;
        if (activeCamera > noOfCameras)
        {
            activeCamera = 0;
        }
        UpdateActiveCamera(activeCamera);
    }

    void OnSwipeRight()
    {
        activeCamera--;
        if (activeCamera < 0)
        {
            activeCamera = noOfCameras;
        }
        UpdateActiveCamera(activeCamera);
    }
}
