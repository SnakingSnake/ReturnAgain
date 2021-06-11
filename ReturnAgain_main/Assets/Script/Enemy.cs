using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;
using System.Net.NetworkInformation;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    public enum Type { Goblin, Golem, Mage, Ghost, Boomer, InstantMelee, InstantRage, Skeleton, BossGolin, BossMage };
    public Type enemyType;
    public int maxHealth; //최대체력
    public int curHealth; //현재체력
    public BoxCollider meleeArea;
    public GameObject magic;
    public GameObject itemMoney;
    public GameObject itemHeart;
    public GameObject itemShild;
    public GameObject itemWeapon;
    public GameObject target;
    public bool isChase;
    public bool isAttack;
    public bool isDead;
    public bool isGetHit;
    public const int unarmDamage = 5;
    public GameObject HPbar;

    public AudioClip audioAttack;
    public AudioClip audioHit;
    public AudioClip audioDeath;

    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs;
    public NavMeshAgent nav;
    public Animator anim;
    public Vector3 orgPos;
    public Player_knights pUser;
    public AudioSource audio;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();

        if(enemyType != Type.BossGolin)
        Invoke("ChaseStart", 1f);
    }

    void ChaseStart()
    {
        isChase = true; //추적 시작시
        anim.SetBool("Moving", true);
    }

    void MoveStop() // 움직이는거 멈춰!!
    {
        nav.enabled = true;
    }

    void GetHitOut() // 폭력 멈춰!!
    {
        isGetHit = false;
    }
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        orgPos = this.gameObject.transform.position;
    }
    void Update()
    {

        if (nav.enabled && enemyType != Type.BossGolin)
        {
            if (Vector3.Distance(target.transform.position, transform.position) <= 20f) //일정거리 이내에서만 추적
            {
                anim.SetBool("Moving", true);
                nav.SetDestination(target.transform.position);
            }
            else if ((Vector3.Distance(target.transform.position, transform.position) > 20f)) //거리를 벗어나면 멈춤
            {
                nav.SetDestination(orgPos);
                if (Vector3.Distance(orgPos, transform.position) < 1f)
                {
                    anim.SetBool("Moving", false);
                    nav.isStopped = false;
                    nav.velocity = Vector3.zero;
                } //원래 위치로 돌아가는 코드
            }
        }

        if (isChase == false)
        {
            nav.velocity = Vector3.zero;
        }

        if(curHealth < maxHealth)
        {
            HPbar.SetActive(true);
        }

    }
    void FreezeVelocity()
    {
        if (isChase) //목표 추적이 물리엔진 오류해결
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void PlaySound(String action)
    {
        switch (action)
        {
            case "Attack":
                audio.clip = audioAttack;
                audio.volume = 0.7f;
                audio.Play();
                break;
            case "Hit":
                audio.clip = audioHit;
                audio.volume = 0.7f;
                audio.Play();
                break;
            case "Death":
                audio.clip = audioDeath;
                audio.volume = 0.7f;
                audio.Play();
                break;
        }
    }

    void Targeting()
    {
        if (!isDead && enemyType != Type.BossGolin)
        {
            float targetRadius = 0; //공격 정확도
            float targetRange = 0; //공격사거리

            switch (enemyType)
            {
                case Type.Goblin:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.Golem:
                    targetRadius = 1f;
                    targetRange = 3f;
                    break;
                case Type.Mage:
                    targetRadius = 0.5f;
                    targetRange = 15f;
                    break;
                case Type.Ghost:
                    targetRadius = 1f;
                    targetRange = 3f;
                    break;
                case Type.InstantMelee:
                    targetRadius = 1f;
                    targetRange = 3f;
                    break;
                case Type.BossGolin:
                    targetRadius = 1f;
                    targetRange = 5f;
                    break;
            }

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position,
                targetRadius,
                transform.forward,
                targetRange,
                LayerMask.GetMask("Player")); //플레이어가 공격범위에 들어왔을때
            if (rayHits.Length > 0 && !isAttack) //범위 밖에있을땐 공격하지않음
            {
                StartCoroutine(Attack()); //아래 코루틴 작동
            }
        }
    }

    IEnumerator Attack() //공격구현로직
    {
        {
            isChase = false;
            isAttack = true;
            anim.SetBool("isAttack", true);
            PlaySound("Attack");

            switch (enemyType)
            {
                case Type.Goblin:
                    yield return new WaitForSeconds(0.7f); //공격시간
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(0.3f); //공격후 딜레이 시간
                    meleeArea.enabled = false;
                    break;
                case Type.Golem: //돌격형 몬스터
                    yield return new WaitForSeconds(0.1f); //공격시간
                    rigid.AddForce(transform.forward * 20, ForceMode.Impulse); //돌격 구현
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(1.4f); //공격후 딜레이 시간
                    rigid.velocity = Vector3.zero; //공격 후 멈춤
                    meleeArea.enabled = false;
                    anim.SetBool("isAttack", false);

                    yield return new WaitForSeconds(2f); //공격후 재추적까지의 시간
                    break;
                case Type.Mage:
                    yield return new WaitForSeconds(1.5f); //공격시간
                    GameObject instantMagic = Instantiate(magic, transform.position, transform.rotation); //마법 생성
                    Rigidbody rigidMagic = instantMagic.GetComponent<Rigidbody>();
                    rigidMagic.velocity = transform.forward * 7;
                    nav.velocity = Vector3.zero;
                    yield return new WaitForSeconds(1.3f); //공격후 재공격까지의 시간
                    anim.SetBool("isAttack", false);
                    break;
                case Type.Ghost:
                    yield return new WaitForSeconds(0.2f); //공격시간
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(1f); //공격후 딜레이 시간
                    meleeArea.enabled = false;
                    break;
                case Type.InstantMelee:
                    yield return new WaitForSeconds(0.7f); //공격시간
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(0.3f); //공격후 딜레이 시간
                    meleeArea.enabled = false;
                    break;
                case Type.InstantRage:
                    yield return new WaitForSeconds(0.7f); //공격시간
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(0.3f); //공격후 딜레이 시간
                    meleeArea.enabled = false;
                    break;
                case Type.Skeleton:
                    yield return new WaitForSeconds(0.7f); //공격시간
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(0.3f); //공격후 딜레이 시간
                    meleeArea.enabled = false;
                    break;
            }

            isChase = true;
            isAttack = false;
            anim.SetBool("isAttack", false);

            yield return null;
        }
    }

    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unarm" && !isGetHit)
        {

            isGetHit = true; // 트리거가 2번연속 동작하지 않게 
            Invoke("GetHitOut", 0.3f);
            curHealth -= unarmDamage; //데미지 입음
            anim.SetTrigger("doGetHit");
            PlaySound("Hit");
            nav.enabled = false; // 데미지 입을 시 멈추게
            Invoke("MoveStop", 0.7f); // 다시 움직일 수 있게
            Vector3 reactVec = transform.position - other.transform.position; //넉백

            StartCoroutine(OnDamage(reactVec, false));

        }
        if (other.tag == "Melee" && !isGetHit) //웨펀 공격
        {

            Weapon weapon = other.GetComponent<Weapon>(); //근접무기 공격시
            isGetHit = true; // 트리거가 2번연속 동작하지 않게 
            Invoke("GetHitOut", 0.5f);
            curHealth -= weapon.damage; //데미지 입음
            anim.SetTrigger("doGetHit");
            PlaySound("Hit");
            nav.enabled = false; // 데미지 입을 시 멈추게
            Invoke("MoveStop", 0.7f); // 다시 움직일 수 있게
            Vector3 reactVec = transform.position - other.transform.position; //넉백

            StartCoroutine(OnDamage(reactVec, false));
        }

        if (other.tag == "Giant" && !isGetHit)
        {

            isGetHit = true; // 트리거가 2번연속 동작하지 않게 
            Invoke("GetHitOut", 0.5f);
            curHealth -= 50; //데미지 입음
            anim.SetTrigger("doGetHit");
            PlaySound("Hit");
            nav.enabled = false; // 데미지 입을 시 멈추게
            Invoke("MoveStop", 0.7f); // 다시 움직일 수 있게
            Vector3 reactVec = transform.position - other.transform.position; //넉백

            StartCoroutine(OnDamage(reactVec, true));
        }

        if (other.tag == "Berserker" && !isGetHit)
        {
            isGetHit = true; // 트리거가 2번연속 동작하지 않게 
            Invoke("GetHitOut", 0.5f);
            curHealth -= 20; //데미지 입음
            pUser.heart += 20;
            anim.SetTrigger("doGetHit");
            PlaySound("Hit");
            nav.enabled = false; // 데미지 입을 시 멈추게
            Invoke("MoveStop", 0.7f); // 다시 움직일 수 있게
            Vector3 reactVec = transform.position - other.transform.position; //넉백

            StartCoroutine(OnDamage(reactVec, true));
        }
    }

    public void HitByMagic(Vector3 explosionPos, string skillName, int damage = 0)
    {
        if (skillName == "Bomb")
        {
            isGetHit = true; // 트리거가 2번연속 동작하지 않게 
            Invoke("GetHitOut", 0.5f);
            curHealth -= 100; //데미지 입음
            anim.SetTrigger("doGetHit");
            PlaySound("Hit");
            nav.enabled = false; // 데미지 입을 시 멈추게
            Invoke("MoveStop", 0.7f); // 다시 움직일 수 있게
            Vector3 reatVec = transform.position - explosionPos;

            StartCoroutine(OnDamage(reatVec, true));
        }

        if (skillName == "Void")
        {
            isGetHit = true; // 트리거가 2번연속 동작하지 않게 
            Invoke("GetHitOut", 0.5f);
            curHealth -= 10; //데미지 입음
            anim.SetTrigger("doGetHit");
            PlaySound("Hit");
            nav.enabled = false; // 데미지 입을 시 멈추게
            Invoke("MoveStop", 0.7f); // 다시 움직일 수 있게
            Vector3 reatVec = transform.position - explosionPos;
            rigid.AddForce(reatVec / reatVec.magnitude * -300, ForceMode.Impulse);

            StartCoroutine(OnDamage(reatVec, true));
        }

        if (skillName == "Reflect")
        {
            isGetHit = true; // 트리거가 2번연속 동작하지 않게 
            Invoke("GetHitOut", 0.5f);
            curHealth -= damage; //데미지 입음
            anim.SetTrigger("doGetHit");
            PlaySound("Hit");
            nav.enabled = false; // 데미지 입을 시 멈추게
            Invoke("MoveStop", 0.7f); // 다시 움직일 수 있게
            Vector3 reatVec = transform.position - explosionPos;

            StartCoroutine(OnDamage(reatVec, true));
        }

        if (skillName == "AuraSword")
        {
            isGetHit = true; // 트리거가 2번연속 동작하지 않게 
            Invoke("GetHitOut", 0.5f);
            curHealth -= 10; //데미지 입음
            anim.SetTrigger("doGetHit");
            PlaySound("Hit");
            nav.enabled = false; // 데미지 입을 시 멈추게
            Invoke("MoveStop", 0.7f); // 다시 움직일 수 있게
            Vector3 reatVec = transform.position - explosionPos;

            StartCoroutine(OnDamage(reatVec, true));
        }

        if (skillName == "WaterPool")
        {
            isGetHit = true; // 트리거가 2번연속 동작하지 않게 
            Invoke("GetHitOut", 0.5f);
            curHealth -= 15; //데미지 입음
            anim.SetTrigger("doGetHit");
            PlaySound("Hit");
            nav.enabled = false; // 데미지 입을 시 멈추게
            Invoke("MoveStop", 0.7f); // 다시 움직일 수 있게
            Vector3 reatVec = transform.position - explosionPos;

            StartCoroutine(OnDamage(reatVec, true));
        }
    }
    IEnumerator OnDamage(Vector3 reactVec, bool isMagic)
    {

        if (curHealth > 0 )
        {
            Debug.Log("쳐맞고있어요");
        }
        else
        {
            isDead = true;
            gameObject.layer = 12;
            isChase = false; //사망시 네비게이션 꺼짐
            nav.enabled = false;
            anim.SetTrigger("doDie"); //사망시 사망모션 출력
            PlaySound("Death");

            if (isMagic)
            {

                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                rigid.freezeRotation = false;

                rigid.AddForce(reactVec * +15, ForceMode.Impulse); //죽을시 넉백
                rigid.AddTorque(reactVec * +15, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 15, ForceMode.Impulse); //죽을시 넉백
            }


            Destroy(gameObject, 3);
            Debug.Log("죽었어요");

            int ran = Random.Range(0, 10);
            int rand = Random.Range(0, 10);
            if (ran < 5) //Money 60퍼
            {
                Instantiate(itemMoney, transform.position, itemMoney.transform.rotation);
            }
            else if (ran < 7.5f) //Heart 25퍼
            {
                Instantiate(itemHeart, transform.position, itemHeart.transform.rotation);
            }
            else if (ran < 9) //Shild 10퍼
            {
                Instantiate(itemShild, transform.position, itemShild.transform.rotation);
            }
            else if (ran < 10) //Weapon 10퍼
            {
                Instantiate(itemWeapon, transform.position, itemWeapon.transform.rotation);
            }

            if (enemyType != Type.BossGolin)
                Destroy(gameObject, 1.2f);
        }
        yield return new WaitForSeconds(0.1f);
    }
    void FootR()
    {

    }
    void FootL()
    {

    }
    void Hit()
    {

    }
}
