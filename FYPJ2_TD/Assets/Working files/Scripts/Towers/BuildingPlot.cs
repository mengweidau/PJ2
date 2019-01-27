using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlot : Entity
{
    [SerializeField] private float f_buildTimer;
    [SerializeField] private float f_lumberDur = 2, f_arrowDur = 2;
    [SerializeField] private int i_lumberyardGCost = 100;
    [SerializeField] private int i_archerGCost = 150, i_archerLCost = 50;
    [SerializeField] public bool b_built;
    
    //prefabs
    [SerializeField] GameObject archerPrefab;
    [SerializeField] GameObject lumberPrefab;

    //ui related
    private GameObject plotCanvas, buildCanvas;
    private Button plotButton;

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
        f_buildTimer = 0.0f;
        b_built = false;

        //initialize PlotCanvas from its child
        if (transform.Find("PlotCanvas"))
        {
            plotCanvas = transform.Find("PlotCanvas").gameObject;
            plotButton = plotCanvas.transform.Find("PlotButton").GetComponent<Button>();
        }

        //initialize BuildCanvas from its child
        if (transform.Find("BuildCanvas"))
        {
            buildCanvas = transform.Find("BuildCanvas").gameObject;
            buildCanvas.SetActive(false);
        }
        managerStats = GameObject.Find("Canvas").GetComponent<ManagerStats>();
    }

    private void Update()
    {

        Build();
    }

    private void Build()
    {
        if (f_buildTimer > 0.0f)
        {
            f_buildTimer -= 1.0f * Time.deltaTime;
            //Debug.Log("building!: " + f_buildTimer);
            //to add - building progress bar

            if (f_buildTimer <= 0.0f)
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
                gameObject.SetActive(false);
            }
        }
    }

    public void PlotReturn()
    {
        gameObject.SetActive(true);
        buildCanvas.SetActive(false);
        plotButton.interactable = true;
    }

    public void SetBuildingType(BuildingType type)
    {
        buildingType = type;
    }

    /*Buttons Funcs
     *description: funcs to be attached onto buttons 
     */
    public void SelectedPlot()
    {
        buildCanvas.SetActive(true);
        plotButton.interactable = false;
    }

    public void CancelBuild()
    {
        buildCanvas.SetActive(false);
        plotButton.interactable = true;
    }

    public void BuildLumberyard()
    {
        if (!b_built && managerStats.GetGold() >= i_lumberyardGCost)
        {
            b_built = true;
            f_buildTimer = f_lumberDur;
            buildingType = BuildingType.Lumberyard;

            //decrease amount of resources
            managerStats.MinusGold(i_lumberyardGCost);

            buildCanvas.SetActive(false);
        }
    }
    public void BuildArrowTower()
    {
        if (!b_built && managerStats.GetGold() >= i_archerGCost && managerStats.GetLumber() >= i_archerLCost)
        {
            b_built = true;
            f_buildTimer = f_arrowDur;
            buildingType = BuildingType.ArcherTower;

            //decrease amount of resources
            managerStats.MinusGold(i_archerGCost);
            managerStats.MinusLumber(i_archerLCost);
            
            buildCanvas.SetActive(false);
        }
    }
    public void BuildFootmanTower()
    {
        //instantiate a footman tower at this position
        //make instantiated object know its parent, and when its destroy make it setactive=true its parent (its respective plot)
    }

    public int GetLumberyardGCost() { return i_lumberyardGCost; }
    
    public int GetArrowtowerGCost() { return i_archerGCost; }

    public int GetArrowtowerLCost() { return i_archerLCost; }
}
