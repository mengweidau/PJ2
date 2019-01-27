using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lumberyard : Entity
{
    [SerializeField] private BuildingPlot parentPlot;
    [SerializeField] GameObject towerCanvas, upgradeCanvas;
    private Button towerButton;
    //[SerializeField] private float lumberCapacity;
    private float generateTimer;
    GameObject canvas;

    enum LumberyardLevel
    {
        Level1 = 0,
        Level2
    }
    LumberyardLevel level = LumberyardLevel.Level1;

    public Lumberyard()
    {
        f_health = 10.0f;
        s_name = "Lumberyard";
    }

    private void Awake()
    {
        generateTimer = 0.5f;
        if (transform.Find("LumberyardCanvas"))
        {
            towerCanvas = transform.Find("LumberyardCanvas").gameObject;
            towerButton = towerCanvas.transform.Find("LumberyardButton").GetComponent<Button>();
        }
        if (transform.Find("UpgradeCanvas"))
        {
            upgradeCanvas = transform.Find("UpgradeCanvas").gameObject;
            upgradeCanvas.SetActive(false);
        }
        canvas = GameObject.Find("Canvas");
    }

    private void Update()
    {
        GenerateLumbers();
    }

    public void SetParentPlot(BuildingPlot plot)
    {
        parentPlot = plot;
    }

    public void SellLumberyard()
    {
        parentPlot.PlotReturn();
        Destroy(gameObject);
        parentPlot.b_built = false;

        //determine return value
        int gold = parentPlot.GetLumberyardGCost();
        gold = (int)(gold * 0.25f);

        canvas.GetComponent<ManagerStats>().AddGold(gold);
    }

    public void SelectedLumberyard()
    {
        upgradeCanvas.SetActive(true);
        towerButton.interactable = false;
    }

    public void CancelUpgrade()
    {
        upgradeCanvas.SetActive(false);
        towerButton.interactable = true;
    }

    private void GenerateLumbers()
    {
        generateTimer -= Time.deltaTime;
        if (generateTimer <= 0)
        {
            canvas.GetComponent<ManagerStats>().AddLumber(1);
            generateTimer = 0.5f;
        }
    }
}
