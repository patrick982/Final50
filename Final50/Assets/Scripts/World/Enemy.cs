using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variables
    [Header("Stats")]
    public float moveSpeed;
    public int curHp;
    public int maxHp;

    [Header("Target")]
    public float chaseRange;
    public float attackRange;
    private Player player;

    [Header("Attack")]
    public int damage;
    public float attackRate;
    public float lastAttackTime;
    private ParticleSystem hitEffect;
    public int xpToGive;


    //Components
    private Rigidbody2D rig;

    //SFX
    public AudioSource hitSound;

    // Start is called before the first frame update
    void Awake()
    {
        //make the player object available in this script
        player = FindObjectOfType<Player>();

        //get the rigidbody
        rig = GetComponent<Rigidbody2D>();

        hitEffect = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float playerDist = Vector2.Distance(transform.position, player.transform.position);

        if(playerDist <= attackRange)
        {
            //code for attacking the player
            rig.velocity = Vector2.zero;

            if (Time.time - lastAttackTime >= attackRate)
                Attack();
        }
        else if(playerDist <= chaseRange)
        {
            Chase();
        }
        else
        {
            rig.velocity = Vector2.zero;
        }

    }

    void Chase()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;

        rig.velocity = dir * moveSpeed;
    }

    public void TakeDamage (int damageTaken)
    {
        curHp -= damageTaken;

        if (curHp <= 0)
            Die();
    }

    void Die()
    {
        //xp for the player
        player.AddXp(xpToGive);

        Destroy(gameObject);
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        hitSound.Play();
        player.TakeDamage(damage);


        //Particle system
        hitEffect.transform.position = player.transform.position;
        hitEffect.Play();
    }
}

