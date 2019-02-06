using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] FirebaseAuth fbAuth;
    [SerializeField] ManagerStats managerStats;
    [SerializeField] int level;
    [SerializeField] int stars;
    [SerializeField] GameObject fireSpell, freezeSpell;

    private void Start()
    {
        StartCoroutine(InitStars());
        StartCoroutine(InitSpells());
    }

    // Update is called once per frame
    void Update ()
    {
        WinCondition();
        LoseCondition();
    }

    public IEnumerator InitSpells()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(2);

        int fireUnlocked = fbAuth.FetchSpellStatus("fire");
        int freezeUnlocked = fbAuth.FetchSpellStatus("freeze");

        if (fireUnlocked == 1)
            fireSpell.SetActive(true);
        if (freezeUnlocked == 1)
            freezeSpell.SetActive(true);
    }

    public IEnumerator InitStars()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(2);
        stars = fbAuth.FetchStars(level);
    }

    void SetStars()
    {
        if (managerStats.GetLife() >= 7 && stars < 3)
        {
            fbAuth.UpdateLevelStars(level, 3);
            fbAuth.UpdateGems(100);
        }
        else if (managerStats.GetLife() >= 4 && stars < 2)
        {
            fbAuth.UpdateLevelStars(level, 2);
            fbAuth.UpdateGems(50);
        }
        else if (managerStats.GetLife() > 0 && stars <= 0)
        {
            fbAuth.UpdateLevelStars(level, 1);
            fbAuth.UpdateGems(25);
        }
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
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
    }

    //test win
    public void WinBtn()
    {
        StartCoroutine(Win());
    }
}
