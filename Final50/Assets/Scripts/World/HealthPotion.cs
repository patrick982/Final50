using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    //Variables
    public int hpToGive;

    //Functions
    public void DrinkPotion()
    {
        FindObjectOfType<Player>().AddHp(hpToGive);
        Destroy(gameObject);

    }
}
