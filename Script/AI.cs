using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float WalkSpeed;
    public float boundary;
    [SerializeField] float onSight;
    [SerializeField] float timeForMove;
    [SerializeField] int damageByEnemy;
    [SerializeField] int bonusHealth;
    [Range(0,100)] public int enemyHealth;

    Animator animator;
    AudioPlayer audioPlayer;
    Vector3 dist;
    PlayerController player;

    public bool enemyIsAlive = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyIsAlive == false) return;
        StartCoroutine(MoveTowards());
        Attack();
        Dead();
    }

    IEnumerator MoveTowards()
    {
        yield return new WaitForSecondsRealtime(timeForMove);

        Vector3 goal = new(target.transform.position.x, target.transform.position.y);
        Vector3 startPos = new(transform.position.x, transform.position.y);
        dist = goal - startPos;
        
        if(dist.magnitude < onSight && dist.magnitude > boundary)
        {
            transform.position += Time.deltaTime * WalkSpeed * dist.normalized;

            Flipped();

            animator.SetBool("isWalk", true);
        }

        else if(dist.magnitude > onSight || dist.magnitude <= boundary)
        {
            animator.SetBool("isWalk",false);
        }
    }

    private void Flipped()
    {
        if (dist.x > 0)
        {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (dist.x < 0)
        {
            transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void Attack()
    {
        if(dist.magnitude <= boundary && player.PlayerisAlive == true)
        {
            animator.SetTrigger("isAttack");
        }
        else if(dist.magnitude > boundary || player.PlayerisAlive == false)
        {
            animator.ResetTrigger("isAttack");
        }
    }

    public void playAttackAudio()
    {
        audioPlayer.SwordAudio();
        if(dist.magnitude <= boundary && player.PlayerisAlive == true)
        {
            player.Hit();
            player.PlayerHealth -= damageByEnemy;
        }
    }

    public void HitEnemy()
    {
        if(enemyIsAlive == false){return;}
        if(dist.magnitude <= boundary-0.5f && enemyIsAlive == true)
        {
            animator.SetTrigger("isHit");
            enemyHealth -= player.damageByPlayer;
        }
    }

    void Dead()
    {
        if(enemyHealth <= 0)
        {
            enemyIsAlive = false;
            animator.SetTrigger("isDead");
            player.PlayerHealth += bonusHealth;
        }
    }
}
