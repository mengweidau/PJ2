using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
    RaycastHit hit;
    public Image coolDownimg;

    public GameObject spellPrefab;
    GameObject clone;
    GameObject target;

    public bool spellSelect = false;
    bool spellUsed = false;
    bool isCooldown = false;

    float cooldown = 20;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
            //isCooldown = true;

        if (spellSelect == true && spellUsed == false)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    //Generate the spell
                    spellUsed = true;
                    clone = Instantiate(spellPrefab, hit.transform.position, Quaternion.identity);
                    target = hit.transform.gameObject;
                    //Decrease the full health value of the enemy, 1hitko
                    if (target.name.Equals("Skeleton(Clone)"))
                        target.GetComponent<EnemySkeleton>().DecreaseHealth(target.GetComponent<EnemySkeleton>().GetHealth());
                    else if (target.name.Equals("Troll(Clone)"))
                        target.GetComponent<EnemyTroll>().DecreaseHealth(target.GetComponent<EnemyTroll>().GetHealth());
                }
                else
                    spellUsed = false;
            }
            Destroy(clone, 1);
        }

        if (isCooldown == true)
        {
            coolDownimg.fillAmount += 1 / cooldown * Time.deltaTime;
            if (coolDownimg.fillAmount >= 1)
            {
                coolDownimg.fillAmount = 0;
                isCooldown = false;
                spellUsed = false;
            }
        }
    }

    public void SummonSpell()
    {
        spellSelect = true;
        isCooldown = true;
    }
}
