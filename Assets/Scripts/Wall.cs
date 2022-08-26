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
    public bool isHorizontal;

    public bool isDragging;
    MeshRenderer renderer;
    float lastUpdated;

    Wall target;
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.enabled = isVisible;
        if(!isVisible)
        {
            renderer.material = hoverMaterial;
        }
        lastUpdated = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {

        if(isDragging)
        {
            Debug.Log(target.name);
        }
       /* if (isVisible & isDragging)
        {
            if (Input.touchCount == 2&&lastUpdated-Time.deltaTime > 1)
            {
                if (isHorizontal)
                {
                    isHorizontal = false;
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
                else
                {
                    isHorizontal = true;
                    transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                }
                lastUpdated = Time.time;

            }
        }*/
    }

    public void placeWall(Vector3 position)
    {
        if(target!=null)
        {
            target.isVisible = true;
            target.setMaterial();
        }
        this.transform.position = position;
    }

    private void setMaterial()
    {
        renderer.material = normalMaterial;
    }

    private void OnCollisionEnter(Collision collision)
    {
       /*if(collision.collider.CompareTag("Wall"))
        {
            target = collision.collider.GetComponent<Wall>();
        }*/

    }
    private void OnCollisionExit(Collision collision)
    {
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isVisible)
        {
            if (other.CompareTag("Wall"))
            {
                Wall wall = other.GetComponent<Wall>();
                wall.target = this;
                if (wall.isHorizontal == this.isHorizontal)
                {
                    renderer.enabled = true;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isVisible)
        {
            if (other.CompareTag("Wall"))
            {
                Wall wall = other.GetComponent<Wall>();
                wall.target = this;
                if (wall.isHorizontal == this.isHorizontal)
                {
                    renderer.enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isVisible)
        {
            Wall wall = other.GetComponent<Wall>();
            //wall.target = null;
            renderer.enabled = isVisible;
        }
    }

}
