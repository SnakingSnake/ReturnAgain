using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject go_DialogueBar;
    [SerializeField] GameObject go_DialogueNameBar;
    [SerializeField] Text txt_Dialogue;
    [SerializeField] Text txt_Name;
    Dialogue[] dialogues;

    public static bool end_dialogue;
    bool isDialogue = false;
 bool isNext = false;
    [Header("텍스트 출력 딜레이")]
    [SerializeField] float textDelay;

    int lineCount = 0;
    int contextCount = 0;

    InteractionController theIC;

     void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
    }

     void Update()
    {
        if (isDialogue)
        {
            if (isNext)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
                {
                    isNext = false;
                    txt_Dialogue.text = "";
                   if (++contextCount < dialogues[lineCount].contexts.Length)
                    {
 StartCoroutine(Typewriter());
                    }
                    else
                    {
                        contextCount = 0;
                        if(++lineCount < dialogues.Length)
                        {
StartCoroutine(Typewriter());
                        }
                        else
                        {
                            EndDialogue();
                        }
                    }
                   
                }
            }
        }
    }


    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        isDialogue = true;
        txt_Dialogue.text = "";
        txt_Name.text = "";
        theIC.SettingUI(false);
        dialogues = p_dialogues;
        StartCoroutine(Typewriter());
        
    }

    void EndDialogue()
    {
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        theIC.SettingUI(true);
        SettingUI(false);
        if(end_dialogue==false)
        end_dialogue = true;
        
    }
    
   
    IEnumerator Typewriter()
    {
      SettingUI(true);
        string t_ReplaceText = dialogues[lineCount].contexts[contextCount];
        t_ReplaceText = t_ReplaceText.Replace("'", ",");
      
        txt_Name.text = dialogues[lineCount].name;
        for (int i = 0; i < t_ReplaceText.Length; i++)
        {
            txt_Dialogue.text += t_ReplaceText[i];
            yield return new WaitForSeconds(textDelay);
        }
        isNext = true;
        yield return null;
    }


    void SettingUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);
        go_DialogueNameBar.SetActive(p_flag);
    }

}
