using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDialogueTrigger : MonoBehaviour
{

    public string[] newLines;
    public string nameText;

    private bool canInteract;
    private bool alreadyTalked = false;
   

    public KeyCode interact;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canInteract && Input.GetKeyDown(interact) && !DialogueManager.instance.dialogueBox.activeInHierarchy && !alreadyTalked)
        {
            DialogueManager.instance.ActivateDialogue(newLines, nameText);
            alreadyTalked = true;
            
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(alreadyTalked)
                SceneManager.LoadScene("Menu");

        }
    }
}
