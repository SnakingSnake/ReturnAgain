using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_knights : MonoBehaviour
{
    /*public float jPower;*/
    /*    bool jDown;
        bool doJump;*/
    
    public static int dungeonLevel = 1; //던전 레벨관련 -> 플레이어가 파괴되지않는 오브젝트라 여기서 레벨을 관리.

    public GameObject[] weapons;
    public GameObject[] shild;
    //-----------------------------------인벤토리 관련 -----------------------------
    public GameObject invenPanel;
    public Slot[] slots;
    public Transform slotHolder;
    Inventory inventory;
    Inventory inven;
    //-----------------------------------인벤토리 관련 -----------------------------
    public bool[] hasWeapons;
    float hAxis;
    float vAxis;
    float fireDelay;
    public float speed;
    public Camera followCamera;

    public int heart;
    public int money;
    public int hasShild = 0;

    int shildStack = 0;
    public int maxHeart;
    public int maxMoney;
    public int maxHasShild;
    public int[] hasWeaponValue;


    public BoxCollider leftUnarmArea;
    public BoxCollider rightUnarmArea;

    // -----------------------------------------기술관련 변수들 ---------------------------------------------------------------------
    public bool isGiant = false;
    public bool t_skillCoolDown = false;
    public bool r_skillCoolDown = false;
    public BoxCollider leftGiantArea;
    public BoxCollider RightGiantArea;
    public GameObject BerserkerEffect;
    public GameObject HealEffect;
    public GameObject ReflectEffect;
    public GameObject ReflectShotEffect;
    public GameObject AuraSwordEffect;

    // -----------------------------------------기술관련 변수들 -------------------------------------------------------------------
    // -----------------------------------------사운드관련 변수들 -------------------------------------------------------------------
    public AudioClip footRClip;
    public AudioClip footLClip;
    public AudioClip twoHandSwordAttack1;
    public AudioClip twoHandSwordAttack2;
    public AudioClip attackVoice1;
    public AudioClip attackVoice2;
    public AudioClip unarmSwing;
    public AudioClip auraSwordSound;
    public AudioClip dodgeSound;


    // -----------------------------------------사운드관련 변수들 -------------------------------------------------------------------




    bool iDown;
    bool invenDown;
    public static bool activeInven = false;

    bool sDown0;
    bool sDown1;
    bool sDown2;
    bool noEquip = true;
    bool isSwap;
    bool dDown;
    bool fDown;
    bool skillDown;
    bool isDamage = false;
    public bool isFire;
    bool isFireReady;
    bool isDiveroll;
    bool isBorder;
    public bool hasTwoHandSword;
    public bool hasRange;


    int dieStack = 0;
    int sDownAct = -1;
    public int fireStack = 0;



    GameObject nearObject;
    Weapon equipWeapon;
    Weapon weapon;
    Vector3 moveVec;
    Vector3 diveVec;
    Animator anim;
    Rigidbody rigid;


    void Start()
    {
        hasWeaponValue[0] = -1;
        hasWeaponValue[1] = -1;
    }
    void Awake()
    {

        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        invenPanel.SetActive(activeInven);
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inventory = gameObject.GetComponent<Inventory>();

        inventory.onChangeItem = new Inventory.OnChangeItem(RedrawSlotUI);
        GameObject start = GameObject.FindWithTag("Start");
        if (start != null)
        {
            Vector3 vec = start.gameObject.transform.position;
            this.gameObject.transform.position = vec;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!InteractionController.isInteract)
        {
            GetInput(); //버튼 받아오기
            LookVec(); // 나아가는 방향으로 시선처리
            Move(); // 이동 관련 움직임
            Fire(); // 공격
            Interaction(); // 상호작용 
            Swap(); // 무기교체
            Diveroll(); // 회피 
            SkillUse();
            Inventory(); // 인벤
            Die();
        }
        followCamera = Camera.main;
    }
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }


    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        iDown = Input.GetButtonDown("Interaction");
        sDown0 = Input.GetButtonDown("Swap0");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        fDown = Input.GetButtonDown("Fire1");
        dDown = Input.GetButtonDown("Diveroll");
        skillDown = Input.GetButtonDown("Skill");
        invenDown = Input.GetButtonDown("Inven");
        /*jDown = Input.GetButtonDown("Jump");*/
    }

    void Move() //이동
    {
        if (moveVec != Vector3.zero)
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }



        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDiveroll)
        {
            moveVec = diveVec;
        }
        if (isFire)
        {
            moveVec = Vector3.zero;
        }




        if (!isBorder)
            transform.position += moveVec * speed * Time.deltaTime;
    }

    void SlotChange()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotnum = i;
        }
    }
    void RedrawSlotUI()
    {
        SlotChange();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();

        }

        for (int i = 0; i < inventory.items.Count; i++)
        {
            slots[i].invenItem = inventory.items[i];
            slots[i].UpdateSlotUI();
        }

    }
    void LookVec() //보이는 방향 바라보기
    {
        transform.LookAt(transform.position + moveVec);
        if (fDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }

    void Interaction() //상호작용
    {
        if (nearObject != null)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                Debug.Log(item.value);

                sDownAct = item.value;

                if (inventory.AddItem(item.value))
                {
                    Destroy(nearObject);
                }

            }

            if (nearObject.tag == "Shild")
            {
                Debug.Log("실행됐어요");
                Item item = nearObject.GetComponent<Item>();
                hasShild += item.value;
                if (hasShild >= maxHasShild)
                {
                    hasShild = maxHasShild;
                    if (shildStack != hasShild)
                    {
                        for (int i = shildStack; i < hasShild; i++)
                        {
                            shild[i].SetActive(true);
                            shildStack++;
                        }
                    }
                }
                else
                {
                    for (int i = shildStack; i < hasShild; i++)
                    {
                        shild[i].SetActive(true);
                        shildStack++;
                    }
                }
                Destroy(nearObject);
            }
        }
    }

    void Inventory() // 인벤토리 활성화 비활성화 ESC 누를때 추가, 메뉴UI에서 별도로 키고끌수 있도록 따로 뺌
    {
        if (invenDown)
        {
            Inventory_Btn();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (invenPanel.activeSelf == true)
            {
                Inventory_Btn();
            }
        }
    }
    public void Inventory_Btn()
    {
        activeInven = !activeInven;
        invenPanel.SetActive(activeInven);
    }

    void Swap() //무기교체
    {
        if (sDown0) //맨손
        {
            ResetEquip();
        }
        if (sDown1 && hasWeapons[0] && !weapons[0].active) //지팡이
        {
            ResetEquip();
            Two_hand_sword(false);
            hasRange = true;
            noEquip = false;
            if (weapons[hasWeaponValue[0]] != null)
            {
                weapons[hasWeaponValue[0]].SetActive(true);
            }

            weapon = weapons[hasWeaponValue[0]].GetComponent<Weapon>();


        }
        if (sDown2 && hasWeapons[1] && !weapons[1].active) //두손검
        {
            ResetEquip();
            Two_hand_sword(true);
            hasRange = false;
            noEquip = false;
            if (weapons[hasWeaponValue[1]] != null)
            {
                weapons[hasWeaponValue[1]].SetActive(true);

            }
            weapon = weapons[hasWeaponValue[1]].GetComponent<Weapon>();

        }

    }

    void SkillUse()
    {
        if (skillDown)
        {
            Debug.Log("스킬실행");
            if (hasTwoHandSword && !t_skillCoolDown)
            {
                Skill skill = weapons[hasWeaponValue[1]].GetComponent<Skill>();
                skill.Use();
            }
            else if (hasRange && !r_skillCoolDown)
            {
                Skill skill = weapons[hasWeaponValue[0]].GetComponent<Skill>();
                skill.Use();
            }


        }
    }

    void ResetEquip()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
            Two_hand_sword(false);
            noEquip = true;
            Debug.Log("실행");
        }
    }

    void Two_hand_sword(bool th) //애니메이션에서 두손검 모션바꾸기위한 함수
    {
        anim.SetBool("hasTwoHand", th);
        hasTwoHandSword = th;
    }

    void SwapOut() // 왜있냐 ?
    {
        isSwap = false;
    }



    void Fire() // 코루틴 사용한 공격
    {
        if (fDown && noEquip)
        {
            if (fireStack == 0)
                StartCoroutine("UnarmFire");
            fireStack++;
        }
        if (fDown && !noEquip && hasTwoHandSword)
        {
            if (fireStack == 0)
                weapon.Use();
            fireStack++;

        }
        if (fDown && !noEquip && hasRange && !isFire)
        {
            weapon.Use();
            isFire = true;
            Invoke("FireDelay", weapon.rate);

        }

    }

    void FireDelay()
    {
        isFire = false;
    }

    IEnumerator UnarmFire() // 맨손 공격
    {
        rightUnarmArea.enabled = true;
        anim.SetBool("UnarmFire1", true);
        anim.SetTrigger("doUnarmFire1");
        isFire = true;
        yield return new WaitForSeconds(0.5f);
        if (fireStack == 1)
        {
            ResetFireStack();
        }
        if (fireStack >= 2)
        {
            rightUnarmArea.enabled = false;
            leftUnarmArea.enabled = true;
            anim.SetBool("UnarmFire2", true);
            anim.SetTrigger("doUnarmFire2");
        }
        yield return new WaitForSeconds(0.5f);
        if (fireStack == 1 || fireStack == 2)
        {
            ResetFireStack();
        }
        if (fireStack >= 3)
        {
            leftUnarmArea.enabled = false;
            rightUnarmArea.enabled = true;
            anim.SetBool("UnarmFire3", true);
            anim.SetTrigger("doUnarmFire3");
        }
        yield return new WaitForSeconds(0.5f);
        ResetFireStack();

    }



    void ResetFireStack() // 연계공격 리셋
    {
        fireStack = 0;
        if (noEquip)
        {
            UnarmFireAnimF();
        }

    }

    void UnarmFireAnimF()
    {
        anim.SetBool("UnarmFire1", false);
        anim.SetBool("UnarmFire2", false);
        anim.SetBool("UnarmFire3", false);
        rightUnarmArea.enabled = false;
        leftUnarmArea.enabled = false;
        isFire = false;

    }





    void OnCollisionEnter(Collision collision)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "item") //아이템 태그 습득 로직, 상호작용 키 없이 바로 습득
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Money:
                    money += item.value;
                    if (money > maxMoney)
                        money = maxMoney;
                    break;
                case Item.Type.Heart:
                    heart += item.value;
                    if (heart > maxHeart)
                        heart = maxHeart;
                    break;
                case Item.Type.Shild:
                    hasShild += item.value;
                    if (hasShild > maxHasShild)
                        hasShild = maxHasShild;
                    break;
                case Item.Type.PlusHeart:
                    maxHeart += item.value;
                    heart += item.value;
                    if (maxHeart > 10000)
                        maxHeart = 10000;
                    heart = maxHeart;
                    break;
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "EnemyMagic") //데미지 처리 코딩
        {
            if (gameObject.tag == "Player")
            {
                if (!isDamage && hasShild != 0 && !isGiant)
                {

                    if (!isFire)
                    {
                        anim.SetTrigger("doGetHit");
                    }
                    shild[hasShild - 1].SetActive(false);
                    hasShild -= 1;
                }
                else if (!isDamage && !isGiant)
                {
                    Magic enemyMagic = other.GetComponent<Magic>();
                    heart -= enemyMagic.damage;
                    isDamage = true;
                    if (!isFire)
                    {
                        anim.SetTrigger("doGetHit");
                    }
                    Invoke("DamageOut", 1f);

                    if (other.GetComponent<Rigidbody>() != null)
                        Destroy(other.gameObject);

                    StartCoroutine(OnDamage());
                }
            }
            else if (gameObject.tag == "Reflect")
            {
                if (!isDamage && hasShild != 0 && !isGiant)
                {

                    if (!isFire)
                    {
                        anim.SetTrigger("doGetHit");
                    }
                    Magic enemyMagic = other.GetComponent<Magic>();
                    heart += enemyMagic.damage;
                    ReflectShot(enemyMagic.damage);
                }
                else if (!isDamage && !isGiant)
                {
                    Magic enemyMagic = other.GetComponent<Magic>();
                    heart += enemyMagic.damage;
                    ReflectShot(enemyMagic.damage);
                    isDamage = true;
                    if (!isFire)
                    {
                        anim.SetTrigger("doGetHit");
                    }
                    Invoke("DamageOut", 1f);

                    if (other.GetComponent<Rigidbody>() != null)
                        Destroy(other.gameObject);

                    StartCoroutine(OnDamage());
                }
            }

        }



    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
        else if (other.tag == "Shild")
        {
            nearObject = other.gameObject;
        }
        
    }
    void DamageOut()
    {
        isDamage = false;
    }
    IEnumerator OnDamage()
    {


        yield return new WaitForSeconds(1f); //무적시간이겠지?


    }

    void Diveroll()
    {
        if (dDown && moveVec != Vector3.zero && !isDiveroll)
        {
            diveVec = moveVec;
            speed *= 2f;
            isDiveroll = true;
            anim.SetTrigger("doDiveroll");
            Invoke("DiveOut", 0.5f);
        }
    }
    public void Die()
    {
        if (heart == 0)
        {
            dieStack++; NextScene.LoadScene("VillageScene");
        }

    }



    void DiveOut()
    {
        speed *= 0.5f;
        isDiveroll = false;

    }

    IEnumerator Giant()
    {
        gameObject.tag = "Giant";
        isGiant = true;
        transform.localScale += new Vector3(10, 10, 10);
        leftGiantArea.enabled = true;
        RightGiantArea.enabled = true;
        yield return new WaitForSeconds(10f);
        gameObject.tag = "Untagged";
        isGiant = false;
        transform.localScale -= new Vector3(10, 10, 10);
        leftGiantArea.enabled = false;
        RightGiantArea.enabled = false;
    }

    void ReflectShot(int enemyDamage)
    {
        Vector3 nextVec = gameObject.transform.position;
        nextVec.y = 2;
        ReflectShotEffect.SetActive(true);
        RaycastHit[] rayHits = Physics.SphereCastAll(nextVec, 5, Vector3.up, 0, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hitObject in rayHits)
        {
            hitObject.transform.GetComponent<Enemy>().HitByMagic(nextVec, "Reflect", enemyDamage);
        }
        Invoke("offEffect", 1f);

    }

    void offEffect(GameObject g)
    {
        g.SetActive(false);
    }
    //---------------------------------------------Animation Event 함수-----------------------------
    void FootR()
    {
         SoundManager.instance.SFXPlay("Walk1", footRClip);

    }
    void FootL()
    {
        SoundManager.instance.SFXPlay("Walk2", footLClip);

    }

    void T_Attack1()
    {
        SoundManager.instance.SFXPlay("THS_Attack1", twoHandSwordAttack1);
        SoundManager.instance.SFXPlay("THS_Man_Voice1", attackVoice1);

    }
    void T_Attack2()
    {
        SoundManager.instance.SFXPlay("THS_Attack2", twoHandSwordAttack2);
        SoundManager.instance.SFXPlay("THS_Man_Voice2", attackVoice2);

    }

    void Hit()
    {
        SoundManager.instance.SFXPlay("UnarmSwing", unarmSwing);
    }

    void Dodge()
    {
        SoundManager.instance.SFXPlay("Dodge", dodgeSound);
    }

    //---------------------------------------------Animation Event 함수-----------------------------
}
