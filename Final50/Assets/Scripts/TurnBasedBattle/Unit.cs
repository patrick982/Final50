using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;
    public int maxHP;
    public int currentHP;
    public int xpToGive;

    [System.NonSerialized] public int playerDamage = Player.instance.damage;
    [System.NonSerialized] public int playerMaxHP = Player.instance.maxHp;
    [System.NonSerialized] public int playerCurrenHP = Player.instance.curHp;
    [System.NonSerialized] public int playerUnitLevel = Player.instance.curLevel;


    public bool EnemyTakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        else
            return false;

    }
    public bool PlayerTakeDamage(int dmg)
    {
        playerCurrenHP -= dmg;

        if (playerCurrenHP <= 0)
            return true;
        else
            return false;

    }

    public IEnumerator PlayerAttackMove()
    {
        transform.Translate(1, 0, 0);
        yield return new WaitForSeconds(0.5f);
        transform.Translate(-1, 0, 0);
    }

    public IEnumerator EnemyAttackMove()
    {
        transform.Translate(-1, 0, 0);
        yield return new WaitForSeconds(0.5f);
        transform.Translate(1, 0, 0);
    }

}
