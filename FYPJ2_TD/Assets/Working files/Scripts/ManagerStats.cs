using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Game Manager
/// Description: determines health, gold, lumber, if game is won or lost
/// </summary>
public class ManagerStats : MonoBehaviour
{
    [SerializeField] SpawnManager spawnManager;

    //values can be changed under inspector 
    //these are default values 
    [SerializeField] int globalLife = 10;
    [SerializeField] int globalGold = 500;
    [SerializeField] int globalLumber = 100;
    int totalWave = 0;

    //ugui can be assigned from inspector
    [SerializeField] TextMeshProUGUI m_textMeshLife;
    [SerializeField] TextMeshProUGUI m_textMeshCurrency;
    [SerializeField] TextMeshProUGUI m_textMeshLumber;
    [SerializeField] TextMeshProUGUI m_textMeshWave;

    private void Start()
    {
        if (spawnManager == null)
            m_textMeshWave.text = "no spawn manager assigned";
        else
            totalWave = spawnManager.GetTotalWave();
    }

    // Update is called once per frame
    void Update()
    {
        m_textMeshLife.text = globalLife.ToString();
        m_textMeshCurrency.text = globalGold.ToString();
        m_textMeshLumber.text = globalLumber.ToString();
        m_textMeshLumber.text = globalLumber.ToString();
        UpdateWaveText();

        if (globalLife <= 0)
        {
            //Change to gameover scene
        }
    }

    void UpdateWaveText()
    {
        if (spawnManager != null)
        {
            int currWave = spawnManager.GetCurrentWave() + 1;
            m_textMeshWave.text = "Wave: " + currWave + " / " + totalWave;
        }
    }

    public void SetLife(int life)
    {
        globalLife = life;

        //avoid displaying negative values in gui
        if (globalLife < 0) 
            globalLife = 0;
    }
    public int GetLife()
    {
        return globalLife;
    }

    public void AddLumber(int lumber)
    {
        globalLumber += lumber;

        //avoid unlimited value
        if (globalLumber > 9999)
            globalLumber = 9999;
    }
    public void MinusLumber(int lumber)
    {
        globalLumber -= lumber;

        //avoid displaying negative values in gui
        if (globalLumber < 0)
            globalLumber = 0;
    }

    public int GetLumber()
    {
        return globalLumber;
    }

    public void AddGold(int gold)
    {
        globalGold += gold;

        //avoid unlimited value
        if (globalGold > 9999)
            globalGold = 9999;
    }

    public void MinusGold(int gold)
    {
        globalGold -= gold;

        //avoid displaying negative values in gui
        if (globalGold < 0)
            globalGold = 0;
    }

    public int GetGold()
    {
        return globalGold;
    }
}
