using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingPlot : Entity
{
    [SerializeField] int i_lumberyardGCost = 100;
    [SerializeField] int i_archerGCost = 150, i_archerLCost = 50;
    [SerializeField] float f_lumberDur = 2, f_arrowDur = 2;
    float f_buildTimer, f_buildDur;
    public bool b_built;
    
    //prefabs
    [SerializeField] GameObject archerPrefab;
    [SerializeField] GameObject lumberPrefab;

    //ui related
    GameObject plotCanvas, buildCanvas, progressCanvas;
    Button plotButton;
    Slider progressSlider;
    TextMeshProUGUI arw_goldText, arw_woodText, lyard_goldText;


    [SerializeField] ManagerStats managerStats;

    public enum BuildingType
    {
        Plot = 0,
        Lumberyard,
        ArcherTower,
        FootmanTower,
        BuildingType_num
    };
    private BuildingType buildingType = BuildingType.Plot;


    public BuildingPlot()
    {
        //wonder if there is any point in stats for plot 
        s_name = "Plot";
    }

    private void Awake()
    {
        InitCanvas();
        InitTextGUI();
        managerStats = GameObject.Find("Canvas").GetComponent<ManagerStats>();
        
        f_buildTimer = 0.0f;
        f_buildDur = 0.0f;
        b_built = false;
    }

    private void Start()
    {
        InitCostText();
    }

    private void Update()
    {
        Build();
    }

    private void Build()
    {
        if (f_buildDur > 0.0f)
        {
            f_buildTimer += 1.0f * Time.deltaTime;
            progressSlider.value = CalculateProgress();

            if (f_buildTimer >= f_buildDur)
                DetermineBuild();
        }
    }

    void DetermineBuild()
    {
        switch (buildingType)
        {
            case BuildingType.Lumberyard:
                if (lumberPrefab != null)
                {
                    GameObject go = Instantiate(lumberPrefab, transform.position, transform.rotation);
                    go.GetComponent<Lumberyard>().SetParentPlot(this);
                    Debug.Log("built lumber house");
                }
                break;
            case BuildingType.ArcherTower:
                if (archerPrefab != null)
                {
                    GameObject go = Instantiate(archerPrefab, transform.position, transform.rotation);
                    go.GetComponent<ArcherTower>().SetParentPlot(this);
                    Debug.Log("built archer tower");
                }
                break;
            default:
                Debug.Log("no building type was built");
                break;
        }

        f_buildTimer = 0.0f;
        f_buildDur = 0.0f;
        gameObject.SetActive(false);
    }

    private void InitCanvas()
    {
        if (transform.Find("PlotCanvas"))
        {
            plotCanvas = transform.Find("PlotCanvas").gameObject;
            plotButton = plotCanvas.transform.Find("PlotButton").GetComponent<Button>();
        }
        
        if (transform.Find("BuildCanvas"))
        {
            buildCanvas = transform.Find("BuildCanvas").gameObject;
            buildCanvas.SetActive(false);
        }

        if (transform.Find("ProgressCanvas"))
        {
            progressCanvas = transform.Find("ProgressCanvas").gameObject;
            progressSlider = progressCanvas.transform.Find("ProgressSlider").GetComponent<Slider>();
            progressCanvas.SetActive(false);
        }
    }

    private void InitTextGUI()
    {
        arw_goldText = buildCanvas.transform.Find("ArcherButton").transform.Find("goldText").GetComponent<TextMeshProUGUI>();
        arw_woodText = buildCanvas.transform.Find("ArcherButton").transform.Find("woodText").GetComponent<TextMeshProUGUI>();

        lyard_goldText = buildCanvas.transform.Find("LumberyardButton").transform.Find("goldText").GetComponent<TextMeshProUGUI>();
    }

    private void InitCostText()
    {
        arw_goldText.text = i_archerGCost.ToString();
        arw_woodText.text = i_archerLCost.ToString();

        lyard_goldText.text = i_lumberyardGCost.ToString();
    }

    public void PlotReturn()
    {
        gameObject.SetActive(true);
        buildCanvas.SetActive(false);
        progressCanvas.SetActive(false);
        plotButton.interactable = true;
    }

    public void SetBuildingType(BuildingType type)
    {
        buildingType = type;
    }
    
    public void SelectedPlotBtn()
    {
        buildCanvas.SetActive(true);
        plotButton.interactable = false;
    }

    public void CancelBuildBtn()
    {
        buildCanvas.SetActive(false);
        plotButton.interactable = true;
    }

    public void BuildLumberyardBtn()
    {
        if (!b_built && managerStats.GetGold() >= i_lumberyardGCost)
        {
            // set build type
            b_built = true;
            buildingType = BuildingType.Lumberyard;

            // set build duration
            f_buildDur = f_lumberDur;
            f_buildTimer = 0.0f;

            // decrease amount of resources
            managerStats.MinusGold(i_lumberyardGCost);

            buildCanvas.SetActive(false);
            progressCanvas.SetActive(true);
        }
    }
    public void BuildArrowTowerBtn()
    {
        if (!b_built && managerStats.GetGold() >= i_archerGCost && managerStats.GetLumber() >= i_archerLCost)
        {
            // set build type
            b_built = true;
            buildingType = BuildingType.ArcherTower;

            // set build duration
            f_buildDur = f_arrowDur;
            f_buildTimer = 0.0f;

            // decrease amount of resources
            managerStats.MinusGold(i_archerGCost);
            managerStats.MinusLumber(i_archerLCost);
            
            buildCanvas.SetActive(false);
            progressCanvas.SetActive(true);
        }
    }
    public void BuildFootmanTowerBtn()
    {
        //instantiate a footman tower at this position
        //make instantiated object know its parent, and when its destroy make it setactive=true its parent (its respective plot)
    }

    public int GetLumberyardGCost() { return i_lumberyardGCost; }
    
    public int GetArrowtowerGCost() { return i_archerGCost; }

    public int GetArrowtowerLCost() { return i_archerLCost; }

    float CalculateProgress(){ return f_buildTimer / f_buildDur; }
}
