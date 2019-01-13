using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAuth : MonoBehaviour {
    [SerializeField] FirebaseAuth fbAuth;

    // Use this for initialization
    void Start () {
        fbAuth.GetAuth();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void WriteUser()
    {
        fbAuth.writeNewUser("test1",1);
    }

    public void FetchUser()
    {
        fbAuth.FetchUserData();
    }
}
