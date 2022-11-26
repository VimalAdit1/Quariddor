using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    List<CinemachineVirtualCamera> cameras;
    // Start is called before the first frame update
    int activeCamera = 0;
    int noOfCameras;
    private Vector2 startPosition;
    private Vector2 endPosition;

    void Start()
    {
        noOfCameras = cameras.Count;
        UpdateActiveCamera(activeCamera);
    }

    // Update is called once per frame
    void Update()
    {
        GetSwipeInput();
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
    }
    void GetSwipeInput()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
                endPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
            {
                endPosition = touch.position;
                if ((endPosition - startPosition).magnitude > 20)
                {
                    DetectSwipe();
                }
            }
        }
    }
    void DetectSwipe()
    {
            Vector2 Distance = endPosition - startPosition;
                if (Distance.x > 0)
                {
                    activeCamera++;
                    if(activeCamera>noOfCameras)
                    {
                        activeCamera = 0;
                    }
                }
                else
                {
                    activeCamera--;
                    if (activeCamera < 0 )
                    {
                        activeCamera = noOfCameras;
                    }
                }
            UpdateActiveCamera(activeCamera);
        
    }
}
