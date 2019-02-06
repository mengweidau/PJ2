using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour {
    [SerializeField] FirebaseAuth fbAuth;
    [SerializeField] MenuManager menuManager;
    [SerializeField] GameObject shopUI;
    [SerializeField] TextMeshProUGUI gemUGUI, descriptionUGUI;
    [SerializeField] int gems = 0;

    [SerializeField] Image fireSpellSprite, freezeSpellSprite;

    int spellCost;
    bool selectedFireSpell = true;
    bool selectedFreezeSpell = false;

    void Start ()
    {
        StartCoroutine(InitialiseShop());
        StartCoroutine(UpdateGems());
        descriptionUGUI.text = "Fire Spell\n\nDoes fire dmg to an enemy";
    }

    public IEnumerator InitialiseShop()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(2);

        int fireUnlocked = fbAuth.FetchSpellStatus("fire");
        int freezeUnlocked = fbAuth.FetchSpellStatus("freeze");

        if (fireUnlocked == 1)
            fireSpellSprite.color = new Color(255, 255, 255, 255);
        if (freezeUnlocked == 1)
            freezeSpellSprite.color = new Color(255, 255, 255, 255);
    }

    public void BuySpell()
    {
        StartCoroutine(UpdateGems());
        
        if (selectedFireSpell)
            spellCost = 75;
        else if (selectedFreezeSpell)
            spellCost = 50;

        if (gems >= spellCost)
        {
            StartCoroutine(Buy());
        }
    }

    public IEnumerator UpdateGems()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(2);
        gems = fbAuth.FetchGems();
        gemUGUI.text = gems.ToString();
    }

    public IEnumerator Buy()
    {
        fbAuth.FetchSnapshot();
        yield return new WaitForSeconds(2);

        if (gems >= spellCost)
        {
            gems -= spellCost;
            fbAuth.UpdateGems(gems);
            gemUGUI.text = gems.ToString();
            UpdateSpells();
        }
    }

    void UpdateSpells()
    {
        if (selectedFireSpell)
        {
            fireSpellSprite.color = new Color(255, 255, 255, 255);
            fbAuth.UpdateSpells("fire", 1);
            selectedFireSpell = false;
        }
        else if (selectedFreezeSpell)
        {
            freezeSpellSprite.color = new Color(255, 255, 255, 255);
            fbAuth.UpdateSpells("freeze", 1);
            selectedFreezeSpell = false;
        }
    }

    public void ToggleShop()
    {
        shopUI.SetActive(!shopUI.activeSelf);

        if (!shopUI.activeSelf)
        {
            selectedFireSpell = true;
            selectedFreezeSpell = false;
            descriptionUGUI.text = "Fire Spell\n\nDoes fire dmg to an enemy";
            StartCoroutine(menuManager.UpdateMenu());
        }
    }

    public void FireSpellBtn()
    {
        selectedFireSpell = true;
        selectedFreezeSpell = false;

        descriptionUGUI.text = "Fire Spell\n\nDoes fire dmg to an enemy";
    }

    public void FreezeSpellBtn()
    {
        selectedFireSpell = false;
        selectedFreezeSpell = true;

        descriptionUGUI.text = "Freeze Spell\n\nStops an enemy from moving for a period of time";
    }
}
