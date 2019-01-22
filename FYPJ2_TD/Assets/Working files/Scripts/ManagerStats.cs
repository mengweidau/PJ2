using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagerStats : MonoBehaviour
{

    public float globalLife;
    public float globalCurrency;
    public float globalLumber;

    [SerializeField] TextMeshProUGUI m_textMeshLife;
    [SerializeField] TextMeshProUGUI m_textMeshCurrency;
    [SerializeField] TextMeshProUGUI m_textMeshLumber;

    // Use this for initialization
    void Start()
    {
        globalLife = 10.0f;
        globalCurrency = 1000.0f;
        globalLumber = 200.0f;

        m_textMeshLife = transform.Find("TMP Life").GetComponent<TextMeshProUGUI>();
        m_textMeshCurrency = transform.Find("TMP Gold").GetComponent<TextMeshProUGUI>();
        m_textMeshLumber = transform.Find("TMP Lumber").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        m_textMeshLife.text = globalLife.ToString();
        m_textMeshCurrency.text = globalCurrency.ToString();
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

}
