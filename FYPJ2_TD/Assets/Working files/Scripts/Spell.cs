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
    GameObject targets;
    [SerializeField] List<GameObject> target = new List<GameObject>();

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

        if (spellFireSelect && !fireSpellUsed)
            FireSpell();
        if (spellFreezeSelect && !freezeSpellUsed)
            FreezeSpell();

        if (isFireCooldown)
        {
            fireCoolDownimg.fillAmount += 1 / fireSpellcooldown * Time.deltaTime;
            if (fireCoolDownimg.fillAmount >= 1)
            {
                fireCoolDownimg.fillAmount = 0;
                isFireCooldown = false;
                fireSpellUsed = false;
                spellFireSelect = false;
            }
        }
        if (isFreezeCooldown)
        {
            freezeCoolDownimg.fillAmount += 1 / freezeSpellcooldown * Time.deltaTime;
            if (freezeCoolDownimg.fillAmount >= 1)
            {
                freezeCoolDownimg.fillAmount = 0;
                isFreezeCooldown = false;
                freezeSpellUsed = false;
                spellFreezeSelect = false;
            }
        }

        if (startFreeze)
        {
            freezeCd -= Time.deltaTime;
            if (freezeCd <= 0)
            {
                startFreeze = false;
                freezeCd = 3;
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] == null)
                        target.Remove(target[i]);
                    else if (target[i].name.Equals("Skeleton(Clone)"))
                    {
                        target[i].GetComponent<EnemySkeleton>().SetMoveSpeed(0.65f);
                        target.Remove(target[i]);
                    }
                    else if (target[i].name.Equals("Troll(Clone)"))
                    {
                        target[i].GetComponent<EnemyTroll>().SetMoveSpeed(0.5f);
                        target.Remove(target[i]);
                    }
                }
            }
        }
    }

    #region FireSpell
    public void SummonFireSpell()
    {
        Debug.Log("Fire spell button selected");
        spellFireSelect = true;
    }

    void FireSpell()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    isFireCooldown = true;
                    fireSpellUsed = true;
                    clone = Instantiate(firePrefab, hit.transform.position, Quaternion.identity);
                    targets = (hit.transform.gameObject);
                    if (targets.name.Equals("Skeleton(Clone)"))
                    {
                        targets.GetComponent<EnemySkeleton>().DecreaseHealth(targets.GetComponent<EnemySkeleton>().GetHealth());
                    }
                    if (targets.name.Equals("Troll(Clone)"))
                    {
                        targets.GetComponent<EnemyTroll>().DecreaseHealth(targets.GetComponent<EnemyTroll>().GetHealth());
                    }
                }
            }
        }
        Destroy(clone, 1);
    }
    #endregion

    #region FreezeSpell
    public void SummonFreezeSpell()
    {
        Debug.Log("Freeze spell button selected");
        spellFreezeSelect = true;
    }

    void FreezeSpell()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    isFreezeCooldown = true;
                    freezeSpellUsed = true;
                    startFreeze = true;
                    clone = Instantiate(freezePrefab, hit.transform.position, Quaternion.identity);
                    target.Add(hit.transform.gameObject);
                    for (int i = 0; i < target.Count; i++)
                    {
                        if (target[i].name.Equals("Skeleton(Clone)"))
                        {
                            target[i].GetComponent<EnemySkeleton>().SetMoveSpeed(0);
                        }
                        else if (target[i].name.Equals("Troll(Clone)"))
                        {
                            target[i].GetComponent<EnemyTroll>().SetMoveSpeed(0);
                        }
                    }
                }
            }
        }
        Destroy(clone, 1);
    }
    #endregion
}
