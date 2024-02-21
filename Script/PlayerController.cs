using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speedMove;
    [SerializeField] float jumpSpeed;
    [SerializeField] float LoadingNextLevel;
    [SerializeField] float LoadCurrentLevel;
    [SerializeField] float probabilityAttack;


    public int damageByPlayer;
    private float playerHealth = 100;
    public bool PlayerisAlive = true;
    public bool forTriggerDialogue = false;

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D charCollider;
    AudioPlayer audioplayer;
    AI[] enemies;
    AILongRangedEnemy[] longRangedEnemies;
    AIBoss[] bosses;
    Dialogue dialogue;

    void Awake()
    {
        audioplayer = FindObjectOfType<AudioPlayer>();
    }
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        charCollider = GetComponent<CapsuleCollider2D>();
        enemies = FindObjectsOfType<AI>();
        longRangedEnemies = FindObjectsOfType<AILongRangedEnemy>();
        bosses = FindObjectsOfType<AIBoss>();
        dialogue = FindObjectOfType<Dialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogue.gameObject.activeSelf == true) return;
        if(PlayerisAlive == false){return;}
        Run();
        FlippedCharacter();
        Dead();
        ApplicationQuit();
    }

    public float PlayerHealth
    {
        get { return playerHealth; }
        set { playerHealth = Mathf.Clamp(value, 0, 100); }
    }

    void OnMove(InputValue value)
    {
        if(dialogue.gameObject.activeSelf == true) return;
        if(PlayerisAlive == false){return;}
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(dialogue.gameObject.activeSelf == true) return;
        if(PlayerisAlive == false){return;}
        if(value.isPressed && charCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            rb2d.velocity += new Vector2(0, jumpSpeed);
            animator.SetTrigger("isJump");
        }
    }

    void OnFire(InputValue value)
    {
        if(dialogue.gameObject.activeSelf == true) return;
        if(PlayerisAlive == false){return;}
        audioplayer.SwordAudio();
        animator.SetTrigger("isAttack");

        foreach(AI enemy in enemies)
        {
            enemy.HitEnemy();
        }

        foreach(AILongRangedEnemy longRangedEnemy in longRangedEnemies)
        {
            longRangedEnemy.HitEnemy();
        }
        
        foreach(AIBoss boss in bosses)
        {
            float probability = UnityEngine.Random.value;

            if(probability < probabilityAttack)
            {
                boss.HitEnemy();
            }
        }
        
    }

    void Run()
    {
        if(charCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            Vector2 playerVelocity = new Vector2(moveInput.x * speedMove, rb2d.velocity.y);
            rb2d.velocity = playerVelocity;
            bool flipped = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
            animator.SetBool("isWalk", flipped && charCollider.IsTouchingLayers(LayerMask.GetMask("Platform")));
        }
    }

    void FlippedCharacter()
    {
        bool flipped = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        if(flipped)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x) * Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        }
    }

    void Dead()
    {
        if(PlayerHealth <= 0)
        {
            PlayerisAlive = false;
            animator.SetTrigger("isDead");
            StartCoroutine(RestartLevel());
        }
    }

    public void Hit()
    {
        if(PlayerisAlive == false){return;}
        animator.SetTrigger("isHit");
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSecondsRealtime(LoadCurrentLevel);
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("NextLevel"))
        {
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(LoadingNextLevel);
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalIndex = SceneManager.sceneCountInBuildSettings;
        if(currentIndex+1 < totalIndex)
        {
            SceneManager.LoadScene(currentIndex + 1);
        }
        else if(currentIndex == totalIndex-1)
        {
            SceneManager.LoadScene(0);
        }
    }

    void ApplicationQuit()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
