using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTowerTap : MonoBehaviour {
    MeleeTower parentTower;
    GameObject parentRadius;
    GameObject parentCancelBtn;
    GameObject parentRelaypointBtn;

    public bool test = false;
	
	// Update is called once per frame
	void Update () {
        UnityCameraTap();
    }

    void UnityCameraTap()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
            Vector3 mousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

            Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
            Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

            Vector3 dir = mousePosF - mousePosN;
            Debug.DrawRay(mousePosN, dir, Color.green);

            RaycastHit hit;
            if (Physics.Raycast(mousePosN, dir, out hit))
            {
                if (parentTower != null && parentRadius != null &&
                    GameObject.ReferenceEquals(hit.transform.gameObject, parentRadius))
                {
                    parentTower.SetRallyPoint(new Vector3(hit.point.x, -5.0f, hit.point.z));
                }
            }
        }
    }

    public void SetParentTower(MeleeTower _tower)
    {
        parentTower = _tower;
    }

    public void SetParentRadius(GameObject _obj)
    {
        parentRadius = _obj;
    }

    public void SetParentRelay(GameObject _obj)
    {
        parentRelaypointBtn = _obj;
    }

    public void SetParentCancel(GameObject _obj)
    {
        parentCancelBtn = _obj;
    }
}
