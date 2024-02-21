using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projecTilleSpeed;
    [SerializeField] int projectileDamage;

    AILongRangedEnemy source;
    Rigidbody2D rb2d;
    PlayerController player;

    float xSpeed;

    public void Initialize(AILongRangedEnemy source)
    {
        this.source = source;
        xSpeed = source.transform.localScale.x * projecTilleSpeed;
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector2(xSpeed,0);
        transform.localScale = new Vector3(transform.localScale.x * -Mathf.Sign(source.transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(rb2d.IsTouchingLayers(LayerMask.GetMask("Character")))
        {
            Destroy(gameObject);
            player.Hit();
            player.PlayerHealth -= projectileDamage;
        }
    }
}
