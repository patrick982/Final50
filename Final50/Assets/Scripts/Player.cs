using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Instance
    public static Player instance;

    //Variables
    [Header ("Stats")]
    public float moveSpeed;
    private Vector2 facingDirection;
    public Vector2 vel;
    public int curHp;
    public int maxHp;
    public int damage; 

    [Header("Sprites")]
    public Sprite downSprite;
    public Sprite upSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    [Header("Combat")]
    public KeyCode attackKey;
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;
    private ParticleSystem hitEffect;

    [Header("Experience")]
    public int curLevel;
    public int curXp;
    public int xpToNextLevel;
    public float levelXpModifier;

    //Components
    private Rigidbody2D rig;
    private SpriteRenderer sr;
    public Animator animator;

    //UI
    private PlayerUI ui;
    public GameObject playerStats;

    //Interactable
    public float interactRange;

    //Inventory
    public List<string> inventory = new List<string>();

    //For when dialog is closed
    public bool canMove = false;

    //SFX
    public AudioSource hitSound;

    // Init only to this object related
    void Awake()
    {
        //get the components
        rig = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        hitEffect = GetComponentInChildren<ParticleSystem>();

        ui = FindObjectOfType<PlayerUI>();

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

    //Initialization
    void Start()
    {

        //initialize the UI elements
        ui.UpdateHealthBar();
        ui.UpdateXpBar();
        ui.UpdateLevelText();

    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Move();
        }
        else
        {
            rig.velocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
        }

        //check if attack key pressed
        if(Input.GetKeyDown(attackKey))
        {
            if (Time.time - lastAttackTime >= attackRate)
                if(canMove)
                {
                    Attack();
                }
        }

        //Call for Interactions
        CheckInteract();
    }

    void Move()
    {
        
        // horizontal and vertical movement
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        vel = new Vector2(x, y);

        //facing direction should always be the last one
        if(vel.magnitude != 0)
        {
            facingDirection = vel;
            
        }


        //update player facing direction and Animation
        UpdateSpriteAnimation(vel);

        //add the movment to the rigidbody of our player
        rig.velocity = vel * moveSpeed;
    }

    void UpdateSpriteAnimation(Vector2 vel)
    {
     
        animator.SetFloat("Horizontal", vel.x);
        animator.SetFloat("Vertical", vel.y);
        animator.SetFloat("Speed", vel.sqrMagnitude);


        //The idling facing direction
        if (facingDirection == Vector2.up)
        {
            animator.SetFloat("VerticalIdle", 1);
            animator.SetFloat("HorizontalIdle", 0);
        }
        else if (facingDirection == Vector2.down)
        {
            animator.SetFloat("VerticalIdle", -1);
            animator.SetFloat("HorizontalIdle", 0);
        }
        else if (facingDirection == Vector2.right)
        {
            animator.SetFloat("HorizontalIdle", 1);
            animator.SetFloat("VerticalIdle", 0);
        }
        else if (facingDirection == Vector2.left)
        {
            animator.SetFloat("HorizontalIdle", -1);
            animator.SetFloat("VerticalIdle", 0);
        }


    }

    void Attack()
    {
        lastAttackTime = Time.time;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, attackRange, 1 << 8);
        //Play Attack Animation
        animator.SetTrigger("Attack");
        hitSound.Play();

        if (hit.collider != null)
        {
            hit.collider.GetComponent<Enemy>().TakeDamage(damage);
            hitEffect.transform.position = hit.collider.transform.position;
            hitEffect.Play();
        }
    }

    public void TakeDamage(int damageTaken)
    {
        curHp -= damageTaken;

        if (curHp <= 0)
            Die();

        //deduct damage from health bar
        ui.UpdateHealthBar();
        
    }

    void Die()
    {
        SceneManager.LoadScene("GameOver");
    }

        
    public void AddXp(int xp)
    {
        curXp += xp;

        if (curXp >= xpToNextLevel)
            LevelUp();

        ui.UpdateXpBar();
    }

    public void AddHp(int hp)
    {
        curHp += hp;

        if (curHp >= maxHp)
            curHp = maxHp;
        ui.UpdateHealthBar();
    }

    void LevelUp()
    {
        curXp -= xpToNextLevel; //sets the current xp again to zero
        curLevel++;

        xpToNextLevel = Mathf.RoundToInt((float)xpToNextLevel * levelXpModifier);

        //update the ui text
        ui.UpdateLevelText();
        ui.UpdateXpBar();
    }

    void CheckInteract()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, interactRange, 1 << 9);

        if(hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            ui.SetInteractText(hit.collider.transform.position, interactable.interactDescription);

            if (Input.GetKeyDown(attackKey))
                interactable.Interact();
        }
        else
        {
            ui.DisableInteractText();
        }
    }

    public void AddItemToInventory(string item)
    {
        inventory.Add(item);
        ui.UpdateInventoryText();
    }

    public void UpdateUIAfterBattle()
    {
        ui.UpdateHealthBar();
        playerStats.SetActive(true);
    }
}
