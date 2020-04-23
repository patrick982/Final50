using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public Text nameText;

    public GameObject dialogueBox;
    public GameObject nameBox;

    public string[] dialogueLines;

    public int currentLine;
    public KeyCode nextLine;

    public static DialogueManager instance;

        
    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text = dialogueLines[currentLine];

        //set instance to the loaded player to prevent multi-loading the player
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueBox.activeInHierarchy)
        {
            
            //Deactivate Player Movement
            Player.instance.canMove = false;

            if(Input.GetKeyDown(nextLine))
            {
                currentLine++;
                //deactivates the dialogue box at the end of what is to display
                if (currentLine >= dialogueLines.Length)
                {
                    dialogueBox.SetActive(false);
                    
                }
                else
                {
                    dialogueText.text = dialogueLines[currentLine];
                }
                
            }

        }
        else
        {
            Player.instance.canMove = true;
        }

    }

    public void ActivateDialogue(string[] newLines, string name)
    {
        dialogueLines = newLines;
        nameText.text = name;

        currentLine = 0;
        
        dialogueText.text = dialogueLines[currentLine];
        dialogueBox.SetActive(true);
        
    }

}
