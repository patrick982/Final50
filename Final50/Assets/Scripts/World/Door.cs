using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //Variables
    public Transform doorEntryPosition;

    //Functions
    public void OpenDoor()
    {
        FindObjectOfType<Player>().transform.position = doorEntryPosition.position;
    }
}
