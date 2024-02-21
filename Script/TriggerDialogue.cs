using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;

    BoxCollider2D triggerDialogue;
    PlayerController player;

    void Start()
    {
        triggerDialogue = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            player.forTriggerDialogue = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            player.forTriggerDialogue = false;
        }
    }
}
