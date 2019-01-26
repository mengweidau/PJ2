using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagerStats : MonoBehaviour
{

    public float globalLife;
    public float globalGold;
    public float globalLumber;

    [SerializeField] TextMeshProUGUI m_textMeshLife;
    [SerializeField] TextMeshProUGUI m_textMeshCurrency;
    [SerializeField] TextMeshProUGUI m_textMeshLumber;

    // Use this for initialization
    void Start()
    {
        globalLife = 10.0f;
        globalGold = 500.0f;
        globalLumber = 100.0f;

        m_textMeshLife = transform.Find("TMP Life").GetComponent<TextMeshProUGUI>();
        m_textMeshCurrency = transform.Find("TMP Gold").GetComponent<TextMeshProUGUI>();
        m_textMeshLumber = transform.Find("TMP Lumber").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        m_textMeshLife.text = globalLife.ToString();
        m_textMeshCurrency.text = globalGold.ToString();
        m_textMeshLumber.text = globalLumber.ToString();

        if (globalLife <= 0)
        {
            //Change to gameover scene
        }
    }

    public void SetLife(float life)
    {
        globalLife = life;
    }
    public float GetLife()
    {
        return globalLife;
    }

    public void AddLumber(float lumber)
    {
        globalLumber += lumber;
    }
    public void MinusLumber(float lumber)
    {
        globalLumber -= lumber;
    }

    public float GetLumber()
    {
        return globalLumber;
    }

    public void AddGold(float gold)
    {
        globalGold += gold;
    }

    public void MinusGold(float gold)
    {
        globalGold -= gold;
    }

    public float GetGold()
    {
        return globalGold;
    }
}
