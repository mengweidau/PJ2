using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    [SerializeField] bool lockZoom = false;
    [SerializeField] float minPanX = -1.0f, maxPanX = 1.0f;
    [SerializeField] float minPanY = -1.0f, maxPanY = 1.0f;
    [SerializeField] float minPanZ = -1.0f, maxPanZ = 1.0f;
    [SerializeField] float maxZoom = 60.0f; //float minZoom = 50.0f, 
    [SerializeField] float unityPanningSpeed = 1.0f;
    [SerializeField] float deltaMagnitudeDiff;
    [SerializeField] float mobileZoomSpeed = 1.0f;

    //mobile move variables
    public bool moved = false;
    private float timerChecked;
    [SerializeField] private float dragSpeed = 0.01f;

    void Start () {
        Camera.main.fieldOfView = maxZoom;
        timerChecked = 0.0f;
    }
	
	void Update () {
        PcControls();

//#if UNITY_ANDROID
//        MobileCameraControls();
//#endif

        //panning limit
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minPanX, maxPanX),
            Mathf.Clamp(transform.position.y, minPanY, maxPanY),
            Mathf.Clamp(transform.position.z, minPanZ, maxPanZ));
    }

    void PcControls()
    {
        //move up/down
        if (Input.GetKey(KeyCode.S))
            transform.Translate(-Vector3.up * unityPanningSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.up * unityPanningSpeed * Time.deltaTime);

        //move left/right
        if (Input.GetKey(KeyCode.A))
            transform.Translate(-Vector3.right * unityPanningSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * unityPanningSpeed * Time.deltaTime);

        //zoom controls
        if (!lockZoom)
        {
            if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus) && transform.position.y > minPanY)
            {
                //zoom in, decrease the minimum border
                transform.Translate(Vector3.forward * unityPanningSpeed * Time.deltaTime);
                minPanX -= unityPanningSpeed * Time.deltaTime;
                minPanZ -= unityPanningSpeed * Time.deltaTime * 0.5f;
                maxPanX += unityPanningSpeed * Time.deltaTime;
                maxPanZ += unityPanningSpeed * Time.deltaTime * 0.5f;
            }
            else if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus) && transform.position.y < maxPanY)
            {
                transform.Translate(-Vector3.forward * unityPanningSpeed * Time.deltaTime);
                minPanX += unityPanningSpeed * Time.deltaTime;
                minPanZ += unityPanningSpeed * Time.deltaTime * 0.5f;
                maxPanX -= unityPanningSpeed * Time.deltaTime;
                maxPanZ -= unityPanningSpeed * Time.deltaTime * 0.5f;
            }
        }
    }

    void MobileCameraControls()
    {
        //Camera zooming
        if (Input.touchCount > 1)
        {
            MobileZoom();
        }
        else if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                MobileDragging();
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                moved = false;
        }
    }

    void MobileDragging()
    {
        timerChecked += Time.deltaTime;
        if (timerChecked < .90f)//drag delay
            return;

        moved = true;
        Vector2 touchDeltaPos = Input.GetTouch(0).deltaPosition;
        transform.Translate(-touchDeltaPos.x * dragSpeed, -touchDeltaPos.y * dragSpeed, 0);
    }

    void MobileZoom()
    {
        if (!lockZoom)
        {
            //Debug.Log("zooming- mobile");
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMagnitude = (touchZero.position - touchOne.position).magnitude;

            deltaMagnitudeDiff = prevTouchDeltaMagnitude - touchDeltaMagnitude;

            if (deltaMagnitudeDiff > 0 && transform.position.y < maxPanY) //zoom out, increase pos.y
            {
                transform.Translate(-Vector3.forward * mobileZoomSpeed * Time.deltaTime);
                minPanX += mobileZoomSpeed * Time.deltaTime;
                minPanZ += mobileZoomSpeed * Time.deltaTime * 0.5f;
                maxPanX -= mobileZoomSpeed * Time.deltaTime;
                maxPanZ -= mobileZoomSpeed * Time.deltaTime * 0.5f;
            }
            else if (deltaMagnitudeDiff < 0 && transform.position.y > minPanY)//zoom in, decrease pos
            {
                transform.Translate(Vector3.forward * mobileZoomSpeed * Time.deltaTime);
                minPanX -= mobileZoomSpeed * Time.deltaTime;
                minPanZ -= mobileZoomSpeed * Time.deltaTime * 0.5f;
                maxPanX += mobileZoomSpeed * Time.deltaTime;
                maxPanZ += mobileZoomSpeed * Time.deltaTime * 0.5f;
            }
        }
    }
}
