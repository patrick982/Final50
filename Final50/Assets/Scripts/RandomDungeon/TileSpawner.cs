using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    DungeonManager dManager;
    
    void Awake()
    {
        dManager = FindObjectOfType<DungeonManager>();
        //Instantiate the floor tile and save it AS game object in the editor
        GameObject goFloor = Instantiate(dManager.floorPrefab, transform.position, Quaternion.identity) as GameObject;
        goFloor.transform.SetParent(dManager.transform);

        //to get the floor boundaries of the dungeon
        if (transform.position.x > dManager.maxX)
            dManager.maxX = transform.position.x;
        if (transform.position.x < dManager.minX)
            dManager.minX = transform.position.x;
        if (transform.position.y > dManager.maxY)
            dManager.maxY = transform.position.y;
        if (transform.position.y < dManager.minY)
            dManager.minY = transform.position.y;
    }


    // Start is called before the first frame update
    void Start()
    {
        LayerMask envMask = LayerMask.GetMask("Wall", "Floor");
        Vector2 hitSize = Vector2.one * 0.8f;

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 targetPos = new Vector2(transform.position.x + x, transform.position.y + y);
                Collider2D hit = Physics2D.OverlapBox(targetPos, hitSize, 0, envMask);

                if(!hit)
                {
                    //Add a wall if nothing next hits
                    GameObject goWall = Instantiate(dManager.wallPrefab, targetPos, Quaternion.identity) as GameObject;
                    goWall.transform.SetParent(dManager.transform);
                }
            }
        }



        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }


}

