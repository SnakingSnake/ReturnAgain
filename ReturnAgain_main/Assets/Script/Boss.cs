﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Boss : Enemy
{
    public Transform magicPos;
    public BoxCollider patternArea;
    public int PatternStack = 0;

    public AudioClip audioAttack1;
    public AudioClip audioAttack2;
    public AudioClip audioAttack3;
    public AudioClip audioSearch;
    public AudioClip audioPattern1;
    public AudioClip audioPattern2;

    Vector3 lookVec; //이동경로 예측;
    Vector3 searchVec; //점프
    public bool isLook;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        nav.isStopped = true;
        StartCoroutine(Think());
    }
    void PlaySound(String action)
    {
        switch (action)
        {
            case "Attack1":
                audio.clip = audioAttack1;
                audio.volume = 0.7f;
                audio.Play();
                break;
            case "Attack2":
                audio.clip = audioAttack2;
                audio.volume = 0.7f;
                audio.Play();
                break;
            case "Attack3":
                audio.clip = audioAttack3;
                audio.volume = 0.7f;
                audio.Play();
                break;
            case "Search":
                audio.clip = audioSearch;
                audio.volume = 0.7f;
                audio.Play();
                break;
            case "Pattern1":
                audio.clip = audioPattern1;
                audio.volume = 0.7f;
                audio.Play();
                break;
            case "Pattern2":
                audio.clip = audioPattern2;
                audio.volume = 0.7f;
                audio.Play();
                break;
        }
    }
    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 0f;
            transform.LookAt(target.transform.position + lookVec);
        }
        else if (!isLook)
            nav.SetDestination(searchVec);
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.5f); //난이도 조정 공격 시간

        if (Vector3.Distance(target.transform.position, transform.position) <= 100f && Vector3.Distance(target.transform.position, transform.position) > 15f)
        {
            StartCoroutine(Search());
        }
        else if (Vector3.Distance(target.transform.position, transform.position) <= 15f)
        {
            if (PatternStack < 4)
            {
                int ranAction = Random.Range(0, 10);
                if (ranAction < 5)
                {
                    StartCoroutine(Primary1());
                    PatternStack++;
                }
                else if (ranAction < 10)
                {
                    StartCoroutine(Primary2());
                    PatternStack++;
                }
            }
            else if(PatternStack >= 4)
            {
                int ranPattern = Random.Range(0, 10);
                if (ranPattern < 5)
                {
                    StartCoroutine(Pattern1());
                    PatternStack = 0;
                }
                else if (ranPattern < 10)
                {
                    StartCoroutine(Pattern2());
                    PatternStack = 0;
                }
            }
        }
        else
        {
            StartCoroutine(Stop());
        }
    }

    IEnumerator Primary1()
    {
        anim.SetTrigger("doAttack1");
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.4f);
        PlaySound("Attack2");
        yield return new WaitForSeconds(0.4f);
        anim.SetTrigger("doAttack2");
        yield return new WaitForSeconds(0.4f);
        PlaySound("Attack3");
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.4f);

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;

        StartCoroutine(Think());
    }
    IEnumerator Primary2()
    {
        anim.SetTrigger("doPrimary1");
        meleeArea.enabled = true;
        yield return new WaitForSeconds(1.12f);
        meleeArea.enabled = false;
        PlaySound("Attack1");
        yield return new WaitForSeconds(1f);

        StartCoroutine(Think());
    }
    IEnumerator Pattern1()
    {
        nav.isStopped = false;
        anim.SetTrigger("doPattern1");
        nav.speed = 6;
        nav.SetDestination(target.transform.position);
        meleeArea.enabled = true;
        PlaySound("Attack2");
        yield return new WaitForSeconds(0.38f);
        PlaySound("Attack2");
        yield return new WaitForSeconds(0.38f);
        PlaySound("Attack2");
        yield return new WaitForSeconds(0.38f);
        PlaySound("Pattern1");
        meleeArea.enabled = false;
        nav.isStopped = true;
        yield return new WaitForSeconds(1f);

        StartCoroutine(Think());
    }
    IEnumerator Pattern2()
    {
        isLook = false;
        anim.SetTrigger("doPattern2");
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.82f);
        PlaySound("Attack1");
        yield return new WaitForSeconds(0.82f);
        PlaySound("Attack1");
        yield return new WaitForSeconds(1f);
        PlaySound("Pattern2");
        yield return new WaitForSeconds(1f);
        isLook = true;
        meleeArea.enabled = false;

        StartCoroutine(Think());
    }
    IEnumerator Search()
    {
        searchVec = target.transform.position + lookVec;

        isLook = false;
        nav.isStopped = false;

        anim.SetTrigger("doSearch");
        nav.speed = 80;
        PlaySound("Search");
        patternArea.enabled = false;
        yield return new WaitForSeconds(1f);
        PlaySound("Attack1");
        patternArea.enabled = true;
        yield return new WaitForSeconds(1.11f);
        patternArea.enabled = false;
        isLook = true;
        nav.isStopped = true;
        yield return new WaitForSeconds(1f);

        StartCoroutine(Think());
    }
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("doIdle");
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(Think());
    }
}