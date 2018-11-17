using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlot : Entity {
    //click on plot, toggle between showing build canvas
    //click on any button in build canvas - instantiates a building, setactive=false this plot
    
    [SerializeField] private float f_buildTimer;
    [SerializeField] private float f_lumberDur, f_arrowDur;
    [SerializeField] private bool b_built;
    private GameObject plotCanvas,buildCanvas;
    private Button plotButton;
    [SerializeField] GameObject archerPrefab;

    public enum BuildingType
    {
        Plot= 0,
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
        f_lumberDur = 4.0f;
        f_arrowDur = 4.0f;
        b_built = false;
        
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
    }

    private void Update () {

        Build();
    }

    private void Build()
    {
        if (f_buildTimer > 0.0f)
        {
            f_buildTimer -= 1.0f * Time.deltaTime;
            Debug.Log("building!: " + f_buildTimer);

            if (f_buildTimer <= 0.0f)
            {
                switch (buildingType)
                {
                    case BuildingType.Lumberyard:
                        //instantiate a lumberyard at this plot's position
                        Debug.Log("built lumberyard");
                        break;
                    case BuildingType.ArcherTower:
                        if (archerPrefab != null)
                        {
                            Instantiate(archerPrefab, transform.position, transform.rotation);
                            Debug.Log("built archer tower");
                        }
                        else
                            Debug.Log("empty archer tower prefab");
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
        if (!b_built)
        {
            b_built = true;
            f_buildTimer = f_lumberDur;
            buildingType = BuildingType.Lumberyard;
            //reduce resources from resource manager, 
        }
    }
    public void BuildArrowTower()
    {
        if (!b_built)
        {
            b_built = true;
            f_buildTimer = 5.0f;
            buildingType = BuildingType.ArcherTower;
            //reduce resources from resource manager, 
        }
        //instantiate a arrow tower at this position
        //make instantiated object know its parent, and when its destroy make it setactive=true its parent (its respective plot)
    }
    public void BuildFootmanTower()
    {
        //instantiate a footman tower at this position
        //make instantiated object know its parent, and when its destroy make it setactive=true its parent (its respective plot)
    }
}
