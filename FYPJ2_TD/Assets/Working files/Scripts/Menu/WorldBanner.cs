using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldBanner : MonoBehaviour {
    [SerializeField] FirebaseAuth fbAuth;
    [SerializeField] GameObject[] stars;
    [SerializeField] int lvl;
    [SerializeField] int collectedStars = 0;


    private void Awake()
    {
        //set all stars to inactive
        for (int i = 0; i < 3; ++i)
            stars[i].SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(InitialiseBanner());
    }

    // Fetch data from snapshot and initialise with data
    public IEnumerator InitialiseBanner()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(3.0f);
        collectedStars = fbAuth.FetchStars(lvl);

        SetStarsActive(collectedStars);
        SetObjectActive();
    }

    // Determine number of stars to be active
    void SetStarsActive(int _numOfStars)
    {
        for (int i = 0; i < _numOfStars; ++i)
        {
            stars[i].SetActive(true);
        }
    }

    // Determine if object is to be active
    void SetObjectActive()
    {
        if (lvl == 1)
            gameObject.SetActive(true);
        else
        {
            //check if previous level has been cleared
            if (fbAuth.FetchLevelCheck(lvl - 1))
                gameObject.SetActive(true);
        }
    }

    public void ToLevelBtn()
    {
        if (Application.CanStreamedLevelBeLoaded("Level" + lvl))
        {
            Debug.Log("going to level" + lvl);
            SceneManager.LoadScene("Level" + lvl, LoadSceneMode.Single);
        }
    }

    public int GetLevel() { return lvl; }
}
