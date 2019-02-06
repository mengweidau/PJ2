using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour {
    [SerializeField] FirebaseAuth fbAuth;
    [SerializeField] GameObject shopUI;


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleShop()
    {
        shopUI.SetActive(!shopUI.activeSelf);
    }
}
