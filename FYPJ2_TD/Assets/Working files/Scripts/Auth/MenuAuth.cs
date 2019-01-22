using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TEST SCRIPT
public class MenuAuth : MonoBehaviour {
    [SerializeField] FirebaseAuth fbAuth;
    public Text text;
    
    public void FetchUser()
    {
        Debug.Log("fetch button pressed");
        StartCoroutine(FetchData());
    }

    public IEnumerator FetchData()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(2);
        //text.text = "fetched: " + fbAuth.FetchLvl1CheckFromSnapshot();
    }

    public void UpdateLevel1Clear()
    {
        Debug.Log("Debug: pressed UpdateLevel1Clear");
        fbAuth.UpdateLevelCleared(1, 0);
    }
    public void UpdateLevel2Clear()
    {
        Debug.Log("Debug: pressed UpdateLevel1Clear");
        fbAuth.UpdateLevelCleared(2, 1);
    }

    public void UpdateLevel1Stars()
    {
        Debug.Log("Debug: pressed UpdateLevel1Clear");
        fbAuth.UpdateLevelStars(1, 0);
    }
    public void UpdateLevel2Stars()
    {
        Debug.Log("Debug: pressed UpdateLevel1Clear");
        fbAuth.UpdateLevelStars(2, 1);
    }


    public void UpdateGems()
    {
        Debug.Log("Debug: pressed UpdateGems");
        fbAuth.UpdateGems(10);
    }
}
