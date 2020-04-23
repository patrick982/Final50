using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollowNoBounds : MonoBehaviour
{
    //Variables
    private GameObject target;

    void Start()
    {
        target = Player.instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //follow the target
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
    }
}
