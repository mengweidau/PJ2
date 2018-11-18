using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {


    public Vector3 position;
    public float xSpeed, ySpeed;
    public float pinchSpeed;
    public float distance;
    public float minDistance, maxDistance;
    public float lastDist, curDist;
   

    Touch touchA, touchB;
    public Vector3 touchToWorldA, touchToWorldB;
    public Vector3 center;

    private float timerChecked;
    public bool moved = false;
    [SerializeField]
    private float minPanX = -100.0f, maxPanX = 100.0f;
    [SerializeField]
    private float minPanZ = -100.0f, maxPanZ = 100.0f;


    private void Awake()
    {
        distance = 15.0f;
        minDistance = 5.0f;
        maxDistance = 1000.0f;

        xSpeed = ySpeed = 1.0f;
        pinchSpeed = 1.0f;

        lastDist = curDist = 0.0f; 
    }

    private void Update()
    {        
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //MobileDragging();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended && !moved)
            {
                //MobileTap();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                moved = false;

            //float dragSpeed = 10;

            //Vector2 touchDeltaPos = Input.GetTouch(0).deltaPosition;
            //transform.Translate(-touchDeltaPos.x * dragSpeed, -touchDeltaPos.y * dragSpeed, 0);
        }
        //Two finger pinch to zoom in/out
        if (Input.touchCount > 1 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
        {
            Debug.Log("2 fingers");
            var touch1 = Input.touches[0];
            var touch2 = Input.touches[1];
            curDist = Vector2.Distance(touch1.position, touch2.position);
            if (curDist > lastDist)
            {
                distance -= Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition) * pinchSpeed / 10;
            }
            else
            {
                distance += Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition) * pinchSpeed / 10;
            }
            lastDist = curDist;
        }

        //minimum camera distance
        if (distance <= minDistance)
            distance = minDistance;
        
        //maximum camera distance
        if (distance >= maxDistance)
            distance = maxDistance;



        //this is what I have tried so far
        if (Input.touchCount > 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchA = Input.touches[0];
            touchB = Input.touches[1];

            Vector3 touchA_nearVec = new Vector3(touchA.position.x, touchA.position.y, Camera.main.nearClipPlane);
            Vector3 touchB_nearVec = new Vector3(touchB.position.x, touchB.position.y, Camera.main.nearClipPlane);
            touchToWorldA = Camera.main.ScreenToWorldPoint(touchA_nearVec);
            touchToWorldB = Camera.main.ScreenToWorldPoint(touchB_nearVec);

            center = (touchToWorldA + touchToWorldB / 2);

        }


        //Sets zoom

        //this was the original 
        // position = Vector3(0.0, 0.0, -distance);
        //this is the edit
        position = new Vector3(center.x, center.y, distance);

        //Applies rotation and position
        transform.position = position;
    }
}
