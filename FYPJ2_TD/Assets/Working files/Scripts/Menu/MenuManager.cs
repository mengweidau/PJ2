﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    [SerializeField] FirebaseAuth fbAuth;

    private void Awake()
    {
        StartCoroutine(InitialiseMenu());
    }

    public IEnumerator InitialiseMenu()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(2);
        //fetch gem,initalise gem, display gem
    }

    public void SignoutBtn()
    {
        fbAuth.SignoutUser();
        if (Application.CanStreamedLevelBeLoaded("LoginScene"))
        {
            Debug.Log("exit to login scene");
            SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
        }
        
    }
}
