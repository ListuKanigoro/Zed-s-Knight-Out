using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Sword Swing")]
    [SerializeField] AudioClip swordswing;
    [SerializeField] [Range(0,1)] float volume;

    public void SwordAudio()
    {
        if(swordswing != null)
        {
            AudioSource.PlayClipAtPoint(swordswing, Camera.main.transform.position, volume);
        }
    }
}
