using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI liveText;

    PlayerController player;
    
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        liveText.text = player.PlayerHealth.ToString();
    }
}
