using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    [Header("Camera limit")]
    [SerializeField]
    private float minPanX = -1.0f;
    [SerializeField]
    private float maxPanX = 1.0f;
    [SerializeField]
    private float minPanY = 0.5f, maxPanY = 1.0f;
    [SerializeField]
    private float minPanZ = -1.0f, maxPanZ = 1.0f;
    //[SerializeField]
    private float minZoom = 50.0f, maxZoom = 60.0f;
    [SerializeField]
    private float unityPanningSpeed = 1.0f, unityZoomSpeed = 0.25f;
    
    void Start () {
        //transform.position = new Vector3(66.0f, 349.0f, -75.0f);
        //Camera.main.fieldOfView = maxZoom;
    }
	
	void Update () {
#if UNITY_EDITOR
        UnityCameraMovement();
#endif
    }

    void UnityCameraMovement()
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
        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus))
            Camera.main.fieldOfView -= unityZoomSpeed;
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
            Camera.main.fieldOfView += unityZoomSpeed;

        //zooming limit
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoom, maxZoom);

        //panning limit
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPanX, maxPanX), Mathf.Clamp(transform.position.y, minPanY, maxPanY), Mathf.Clamp(transform.position.z, minPanZ, maxPanZ));
    }
}
