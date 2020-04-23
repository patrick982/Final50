using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public GameObject[] randomEnemies;
    public GameObject floorPrefab, wallPrefab, tilePrefab, exitPrefab;
    public int totalFloorCount;
    [Range(0, 100)] public int enemySpawnRate;

    [HideInInspector] public float minX, maxX, minY, maxY;

    private List<Vector3> floorList = new List<Vector3>();

    LayerMask floorMask;
    

    void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        RandomWalker();
    }

    void Update()
    {
        if(Application.isEditor && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("RandomDungeon");
        }
    }

    void RandomWalker()
    {
        //Starting position of the virtual walker
        Vector3 curPos = Vector3.zero;

        //set initial floor tile
        floorList.Add(curPos);

        while (floorList.Count < totalFloorCount)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    curPos += Vector3.up;
                    break;
                case 1:
                    curPos += Vector3.right;
                    break;
                case 2:
                    curPos += Vector3.down;
                    break;
                case 3:
                    curPos += Vector3.left;
                    break;
            }
            bool inFloorList = false;
            for (int i= 0; i < floorList.Count; i++)
            {
                if (Vector3.Equals(curPos, floorList[i]))
                {
                    inFloorList = true;
                    break;
                }
            }
            if (inFloorList == false)
            {
                floorList.Add(curPos);
            }
        }

        for (int i = 0; i < floorList.Count; i++)
        {
            GameObject goTile = Instantiate(tilePrefab, floorList[i], Quaternion.identity) as GameObject;
            goTile.transform.SetParent(transform);
        }
        StartCoroutine(DungeonBuilt());
    }

    //Wait for dungeon is fully generated
    IEnumerator DungeonBuilt()
    {
        while(FindObjectsOfType<TileSpawner>().Length > 0)
        {
            yield return null;
        }
        //Place the exit
        ExitDoor();
        //Crawl through the dungeon boundaries
        for(int x = (int)minX - 2; x <= (int)maxX + 2; x++)
        {
            for (int y = (int)minY - 2; y <= (int)maxY + 2; y++)
            {
                Collider2D hitFloor = Physics2D.OverlapBox(new Vector2(x, y), Vector2.one * 0.8f, 0, floorMask);
                if(hitFloor)
                {
                    if(!Vector2.Equals(hitFloor.transform.position, floorList[floorList.Count -1]))
                    {
                        RandomEnemies(hitFloor);
                    }
                }
            }
        }
    }

    void ExitDoor()
    {
        Vector3 doorPos = floorList[floorList.Count - 1];
        GameObject goDoor = Instantiate(exitPrefab, doorPos, Quaternion.identity) as GameObject;
        goDoor.transform.SetParent(transform);
    }

    void RandomEnemies(Collider2D hitFloor)
    {
        int dice = Random.Range(0, 101);
        if(dice <= enemySpawnRate)
        {
            int enemyIndex = Random.Range(0, randomEnemies.Length);
            GameObject goEnemy = Instantiate(randomEnemies[enemyIndex], hitFloor.transform.position, Quaternion.identity) as GameObject;
            goEnemy.transform.SetParent(hitFloor.transform);
        }
    }
}
