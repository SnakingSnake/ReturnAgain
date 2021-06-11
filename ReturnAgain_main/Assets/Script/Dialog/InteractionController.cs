using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera cam;

    RaycastHit hitInfo;
    [SerializeField] GameObject go_NomalCrosshair;
    [SerializeField] GameObject go_InteractiveCrosshair;
    [SerializeField] GameObject go_Crosshair;
    [SerializeField] GameObject go_Cursor;
    bool isContact = false;
    public static bool isInteract = false;
    public GameObject sword; 

    [SerializeField] ParticleSystem ps_QuestionEffect;
    DialogueManager theDM;

    public void SettingUI(bool p_flag)
    {
        go_Crosshair.SetActive(p_flag);
        isInteract = !p_flag;
       
    }
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckObject();
        ClickLeftBtn();
        give_sword();
    }

    void give_sword()
    {
        if(DialogueManager.end_dialogue == true)
        {
            Instantiate(sword,new Vector3(-23,0,42), Quaternion.identity);
            DialogueManager.end_dialogue = false;
        }
    }

    void CheckObject()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            if (Physics.Raycast(cam.ScreenPointToRay(t_MousePos), out hitInfo, 1000))
            {
                Contact();
            }
            else
            {
                NotContact();
            }
        }
    }

    void Contact()
    {
        if (hitInfo.transform.CompareTag("Interaction"))
        {
            if (!isContact)
            {
                isContact = true;
                go_InteractiveCrosshair.SetActive(true);
                go_NomalCrosshair.SetActive(false);
            }
        }
        else
        {
            NotContact();
        }
    }
    void NotContact()
    {
        if (isContact)
        {
            isContact = false;
            go_InteractiveCrosshair.SetActive(false);
            go_NomalCrosshair.SetActive(true);
        }
    }
    void ClickLeftBtn()
    {
        if (!isInteract)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isContact)
                {
                    Interact();
                }
            }
        }

    }
    void Interact()
    {
        isInteract = true;
       
        ps_QuestionEffect.gameObject.SetActive(true);
        Vector3 t_targetPos = hitInfo.transform.position;
        ps_QuestionEffect.GetComponent<QuestionEffect>().SetTarget(t_targetPos);
        ps_QuestionEffect.transform.position = cam.transform.position;

        StartCoroutine(waitCollision());
    }
    IEnumerator waitCollision()
    {
        yield return new WaitUntil(()=>QuestionEffect.isCollide);
        QuestionEffect.isCollide = false; 

        
        
        theDM.ShowDialogue(hitInfo.transform.GetComponent<InteractionEvent>().GetDialogue());
    }
}
