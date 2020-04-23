using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    //Battle Entities
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    //Position on screen
    public Transform playerPosition;
    public Transform enemyPosition;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    

    public BattleState state;
    public GameObject battleSound;


    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        
    }

    IEnumerator SetupBattle()
    {
        //Spawn Player and enemy, as child of BattleStation
        GameObject playerGO = Instantiate(playerPrefab, playerPosition);
        playerUnit = playerGO.GetComponent<Unit>();
        
        GameObject enemyGO = Instantiate(enemyPrefab, enemyPosition);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches";

        playerHUD.PlayerSetHUD(playerUnit);
        enemyHUD.EnemySetHUD(enemyUnit);

        //delay for 2 seconds and set battle state machine to player's turn
        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action: ";
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.EnemyTakeDamage(playerUnit.playerDamage);

        StartCoroutine(playerUnit.PlayerAttackMove());

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "your attack was successful";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
                    }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnAttackBtn()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());
    }


    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";
        yield return new WaitForSeconds(2f);
        /* 
         * to do: insert the attack animation (move the transform a bit
         * towards the player
         * later: add particle effects on top
         */
        //check if dead
        bool isDead = playerUnit.PlayerTakeDamage(enemyUnit.damage);

        StartCoroutine(enemyUnit.EnemyAttackMove());

        //update hud
        playerHUD.SetHP(playerUnit.playerCurrenHP);
        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            //sync stats from Battle to world character
            Player.instance.curHp = playerUnit.playerCurrenHP;
            Player.instance.AddXp(enemyUnit.xpToGive);
            Player.instance.UpdateUIAfterBattle();

            dialogueText.text = "you won the battle!";
            Destroy(gameObject);
            Destroy(battleSound);
            
              
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "you lost the battle...";
            //Switch to GameOver Scene - only option for now - restart the game!
            SceneManager.LoadScene("GameOver");
        }
    }
}
