using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
    RaycastHit hit;
    public Image fireCoolDownimg;
    public Image freezeCoolDownimg;

    public GameObject firePrefab;
    public GameObject freezePrefab;
    GameObject clone;
    GameObject target;

    public bool spellFireSelect, spellFreezeSelect = false;
    bool fireSpellUsed, freezeSpellUsed = false;
    bool isFireCooldown = false;
    bool isFreezeCooldown = false;
    bool startFreeze = false;

    float fireSpellcooldown = 20;
    float freezeSpellcooldown = 10;
    float freezeCd = 3;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (spellFireSelect == true)
            FireSpell();
        if (spellFreezeSelect == true)
            FreezeSpell();

        if (isFireCooldown == true)
        {
            fireCoolDownimg.fillAmount += 1 / fireSpellcooldown * Time.deltaTime;
            if (fireCoolDownimg.fillAmount >= 1)
            {
                fireCoolDownimg.fillAmount = 0;
                isFireCooldown = false;
                fireSpellUsed = false;
            }
        }
        if (isFreezeCooldown == true)
        {
            freezeCoolDownimg.fillAmount += 1 / freezeSpellcooldown * Time.deltaTime;
            if (freezeCoolDownimg.fillAmount >= 1)
            {
                freezeCoolDownimg.fillAmount = 0;
                isFreezeCooldown = false;
                freezeSpellUsed = false;
            }
        }

        if (startFreeze)
        {
            freezeCd -= Time.deltaTime;
            if (freezeCd <= 0)
            {
                if (target.name.Equals("Skeleton(Clone)"))
                {
                    startFreeze = false;
                    freezeCd = 3;
                    target.GetComponent<EnemySkeleton>().SetMoveSpeed(1);
                }
                else if (target.name.Equals("Troll(Clone)"))
                {
                    startFreeze = false;
                    freezeCd = 3;
                    target.GetComponent<EnemyTroll>().SetMoveSpeed(0.5f);
                }
            }
        }
    }

    #region FireSpell
    public void SummonFireSpell()
    {
        spellFireSelect = true;
        isFireCooldown = true;
    }

    void FireSpell()
    {
        if (spellFireSelect == true && fireSpellUsed == false)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    //Generate the spell
                    fireSpellUsed = true;
                    clone = Instantiate(firePrefab, hit.transform.position, Quaternion.identity);
                    target = hit.transform.gameObject;
                    //Decrease the full health value of the enemy, 1hitko
                    if (target.name.Equals("Skeleton(Clone)"))
                    {
                        target.GetComponent<EnemySkeleton>().DecreaseHealth(target.GetComponent<EnemySkeleton>().GetHealth());
                    }
                    else if (target.name.Equals("Troll(Clone)"))
                    {
                        target.GetComponent<EnemyTroll>().DecreaseHealth(target.GetComponent<EnemyTroll>().GetHealth());
                    }
                }
                else
                {
                    fireSpellUsed = false;
                }
            }
            Destroy(clone, 1);
        }
    }
    #endregion

    #region FreezeSpell
    public void SummonFreezeSpell()
    {
        spellFreezeSelect = true;
        isFreezeCooldown = true;
    }

    void FreezeSpell()
    {
        if (spellFreezeSelect == true && freezeSpellUsed == false)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    //Generate the spell
                    freezeSpellUsed = true;
                    clone = Instantiate(freezePrefab, hit.transform.position, Quaternion.identity);
                    target = hit.transform.gameObject;
                    //Decrease the full health value of the enemy, 1hitko
                    if (target.name.Equals("Skeleton(Clone)"))
                    {
                        startFreeze = true;
                        target.GetComponent<EnemySkeleton>().SetMoveSpeed(0);
                    }
                    else if (target.name.Equals("Troll(Clone)"))
                    {
                        startFreeze = true;
                        target.GetComponent<EnemyTroll>().SetMoveSpeed(0);
                    }
                }
                else
                {
                    freezeSpellUsed = false;
                }
            }
            Destroy(clone, 1);
        }
    }
    #endregion
}
