using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] string[] lines;
    [SerializeField] float textSpeed;

    PlayerController player;

    int index;

    private void Start() 
    {
        player = FindObjectOfType<PlayerController>();

        textComponent.text = string.Empty;
        StartDialogue();
    }
    
    void Update()
    {
        DialogueStart();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void DialogueStart()
    {
        if(player.forTriggerDialogue == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void StartDialogue()
    {
        StartCoroutine(TypeLine());

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalIndex = SceneManager.sceneCountInBuildSettings;
        
        if(currentIndex+1 < totalIndex)
        {
            index = 0;
        }
        else if(currentIndex == totalIndex-1)
        {
            index = 2;
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
