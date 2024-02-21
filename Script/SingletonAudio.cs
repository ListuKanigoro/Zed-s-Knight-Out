using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonAudio : MonoBehaviour
{
    AudioSource audioSource;

    void Awake()
    {
        int numMusicPlays = FindObjectsOfType<SingletonAudio>().Length;

        if(numMusicPlays > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
        }
    }

}
