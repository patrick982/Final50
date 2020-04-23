using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    //Variables
    private GameObject target;

    public Tilemap map;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;
 
    void Start()
    {
        target = Player.instance.gameObject;

        bottomLeftLimit = map.localBounds.min;
        topRightLimit = map.localBounds.max;

    }

    // Update is called once per frame
    void Update()
    {
        //follow the target
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);

        //clamp the camera
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), -10);
    }
}
