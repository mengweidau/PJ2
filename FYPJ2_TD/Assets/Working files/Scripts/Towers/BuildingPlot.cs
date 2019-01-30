using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingPlot : Entity
{
    [SerializeField] int lumyard_GCost = 100;
    [SerializeField] int archer_GCost = 150, archer_LCost = 50;
    [SerializeField] int melee_GCost = 150, melee_LCost = 100;
    [SerializeField] float lumber_BuildDur = 2.0f, arrow_BuildDur = 2.0f, melee_BuildDur = 2.0f;
    float buildTimer, buildDur;
    public bool b_built;
    
    //prefabs
    [SerializeField] GameObject archerPrefab;
    [SerializeField] GameObject lumberPrefab;
    [SerializeField] GameObject meleePrefab;

    //ui related
    GameObject plotCanvas, buildCanvas, progressCanvas;
    Button plotButton;
    Slider progressSlider;
    TextMeshProUGUI arw_goldText, arw_woodText, mle_goldText, mle_woodText, lyard_goldText;
    
    ManagerStats managerStats;

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
        
        buildTimer = 0.0f;
        buildDur = 0.0f;
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
        if (buildDur > 0.0f)
        {
            buildTimer += 1.0f * Time.deltaTime;
            progressSlider.value = CalculateProgress();

            if (buildTimer >= buildDur)
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
            case BuildingType.FootmanTower:
                if (meleePrefab != null)
                {
                    GameObject go = Instantiate(meleePrefab, transform.position, transform.rotation);
                    go.GetComponent<MeleeTower>().SetParentPlot(this);
                    Debug.Log("built melee tower");
                }
                break;
            default:
                Debug.Log("no building type was built");
                break;
        }

        buildTimer = 0.0f;
        buildDur = 0.0f;
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

        mle_goldText = buildCanvas.transform.Find("FootmanButton").transform.Find("goldText").GetComponent<TextMeshProUGUI>();
        mle_woodText = buildCanvas.transform.Find("FootmanButton").transform.Find("woodText").GetComponent<TextMeshProUGUI>();

        lyard_goldText = buildCanvas.transform.Find("LumberyardButton").transform.Find("goldText").GetComponent<TextMeshProUGUI>();
    }

    private void InitCostText()
    {
        arw_goldText.text = archer_GCost.ToString();
        arw_woodText.text = archer_LCost.ToString();

        mle_goldText.text = melee_GCost.ToString();
        mle_woodText.text = melee_LCost.ToString();
        
        lyard_goldText.text = lumyard_GCost.ToString();
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
        if (!b_built && managerStats.GetGold() >= lumyard_GCost)
        {
            // set build type
            b_built = true;
            buildingType = BuildingType.Lumberyard;

            // set build duration
            buildDur = lumber_BuildDur;
            buildTimer = 0.0f;

            // decrease amount of resources
            managerStats.MinusGold(lumyard_GCost);

            buildCanvas.SetActive(false);
            progressCanvas.SetActive(true);
        }
    }
    public void BuildArrowTowerBtn()
    {
        if (!b_built && managerStats.GetGold() >= archer_GCost && managerStats.GetLumber() >= archer_LCost)
        {
            // set build type
            b_built = true;
            buildingType = BuildingType.ArcherTower;

            // set build duration
            buildDur = arrow_BuildDur;
            buildTimer = 0.0f;

            // decrease amount of resources
            managerStats.MinusGold(archer_GCost);
            managerStats.MinusLumber(archer_LCost);
            
            buildCanvas.SetActive(false);
            progressCanvas.SetActive(true);
        }
    }
    public void BuildFootmanTowerBtn()
    {
        if (!b_built && managerStats.GetGold() >= melee_GCost && managerStats.GetLumber() >= melee_LCost)
        {
            // set build type
            b_built = true;
            buildingType = BuildingType.FootmanTower;

            // set build duration
            buildDur = melee_BuildDur;
            buildTimer = 0.0f;

            // decrease amount of resources
            managerStats.MinusGold(melee_GCost);
            managerStats.MinusLumber(melee_LCost);

            buildCanvas.SetActive(false);
            progressCanvas.SetActive(true);
        }
    }

    public int GetLumberyardGCost() { return lumyard_GCost; }
    
    public int GetArrowtowerGCost() { return archer_GCost; }

    public int GetArrowtowerLCost() { return archer_LCost; }

    float CalculateProgress(){ return buildTimer / buildDur; }
}
