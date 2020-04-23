using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    //Variables
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI inventoryText;
    public Image healthBarFill;
    public Image xpBarFill;

    //can access stuff from the player script
    private Player player;

    //Interact Variables
    public TextMeshProUGUI interactText;

    void Start()
    {
       //
    }

    // Start is called before the first frame update
    void Awake()
    {
        //get the player object
        player = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLevelText()
    {
        levelText.text = "Lvl" + player.curLevel;
    }

    public void UpdateHealthBar()
    {
        healthBarFill.fillAmount = (float)player.curHp / (float)player.maxHp;
    }

    public void UpdateXpBar()
    {
        xpBarFill.fillAmount = (float)player.curXp / (float)player.xpToNextLevel;
    }

    //Interactable Text Functions

    public void SetInteractText(Vector2 pos, string text)
    {
        interactText.gameObject.SetActive(true);
        interactText.text = text;

        interactText.transform.position = Camera.main.WorldToScreenPoint(pos + Vector2.up);

    }

    public void DisableInteractText()
    {
        if (interactText.gameObject.activeInHierarchy)
            interactText.gameObject.SetActive(false);
    }

    public void UpdateInventoryText()
    {
        inventoryText.text ="";

        foreach(string item in player.inventory)
        {
            inventoryText.text += item + "\n";
        }
    }
}
