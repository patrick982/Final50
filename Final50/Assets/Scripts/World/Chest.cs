using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    //Variables
    public int xpToGive;

    //Functions
    public void OpenChest()
    {
        FindObjectOfType<Player>().AddXp(xpToGive);
        Destroy(gameObject);
        
    }
}
