using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchPosition : MonoBehaviour
{
    public string sceneToLoad;
    public Transform pos;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {

            SceneManager.LoadScene(sceneToLoad);
            Player.instance.transform.position = pos.position;
        }
        
    }
    
}
