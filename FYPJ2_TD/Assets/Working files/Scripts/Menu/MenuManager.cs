using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
