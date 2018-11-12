using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlot : Entity {
    private enum PlotState
    {
        PLOT = 0,
        LUMBER,
        MELEE, 
        RANGE, 
        NUM_OF_STATE
    };
    private PlotState plotState = PlotState.PLOT;

    [SerializeField] private float buildTime, buildCountdown;

    public BuildingPlot()
    {
        s_name = "Plot";
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
