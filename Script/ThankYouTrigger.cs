using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThankYouTrigger : MonoBehaviour
{
    [SerializeField] GameObject ThankYou;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Instantiate(ThankYou);
        }
    }
}
