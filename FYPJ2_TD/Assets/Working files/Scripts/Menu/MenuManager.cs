using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    [SerializeField] FirebaseAuth fbAuth;
    [SerializeField] TextMeshProUGUI gemUGUI;
    [SerializeField] int gems = 0;
    [SerializeField] List<GameObject> bannerList = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(UpdateMenu());
        StartCoroutine(UpdateBanners());
    }

    public IEnumerator UpdateBanners()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(2);

        bannerList[0].SetActive(true);
        bool levelOneCheck = fbAuth.FetchLevelCheck(1);
        if (levelOneCheck)
            bannerList[1].SetActive(true);
    }

    public IEnumerator UpdateMenu()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(2);
        gems = fbAuth.FetchGems();
        gemUGUI.text = gems.ToString();
    }

    public void SignoutBtn()
    {
        StartCoroutine(Signout());
    }

    public IEnumerator Signout()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(2);
        fbAuth.SignoutUser();
        if (Application.CanStreamedLevelBeLoaded("LoginScene"))
        {
            Debug.Log("exit to login scene");
            SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
        }
    }
}
