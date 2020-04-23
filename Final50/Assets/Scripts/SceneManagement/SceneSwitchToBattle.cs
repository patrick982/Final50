using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchToBattle : MonoBehaviour
{
    public string sceneToLoad;

    public GameObject playerStats;

    public GameObject sceneSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Destroy(sceneSound);
            playerStats.SetActive(false);
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        }
        
    }
    
}
