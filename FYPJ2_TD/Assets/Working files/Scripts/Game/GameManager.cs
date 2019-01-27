using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] FirebaseAuth fbAuth;
    [SerializeField] ManagerStats managerStats;
    [SerializeField] int level;

    // Update is called once per frame
    void Update ()
    {
        WinCondition();
        LoseCondition();
    }


    void SetStars()
    {
        if (managerStats.GetLife() >= 7)
            fbAuth.UpdateLevelStars(level, 3);
        else if (managerStats.GetLife() >= 4)
            fbAuth.UpdateLevelStars(level, 2);
        else if (managerStats.GetLife() > 0)
            fbAuth.UpdateLevelStars(level, 1);
    }

    void WinCondition()
    {
        if (managerStats.GetCurrWave() >= managerStats.GetTotalWave())
        {
            StartCoroutine(Win());
        }
    }

    public IEnumerator Win()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(2);
        fbAuth.UpdateLevelCleared(level, 1);
        SetStars();
        if (Application.CanStreamedLevelBeLoaded("MenuScene"))
        {
            Debug.Log("back to menu");
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
    }

    void LoseCondition()
    {
        if (managerStats.GetLife() <= 0)
            StartCoroutine(Lose());
    }

    public IEnumerator Lose()
    {
        yield return new WaitForSeconds(2);
        if (Application.CanStreamedLevelBeLoaded("MenuScene"))
        {
            Debug.Log("back to menu");
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
    }

    //test win
    public void WinBtn()
    {
        StartCoroutine(Win());
    }
}
