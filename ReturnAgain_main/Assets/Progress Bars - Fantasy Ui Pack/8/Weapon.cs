using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;

    public GameObject gameObject;
    public GameObject weaponObject;

    public int damage;

    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;

    Player_knights player;

    Animator anim;

    // Update is called once per frame

    void Start()
    {
        player = gameObject.GetComponent<Player_knights>();
        anim = gameObject.GetComponentInChildren<Animator>();
    }


    public void Use()
    {
        if (weaponObject.tag == "Melee" || weaponObject.tag == "Berserker")
        {
            player.hasTwoHandSword = true;
            player.hasRange = true;
        }
        if (weaponObject.tag == "Range")
        {
            player.hasTwoHandSword = false;
            player.hasRange = true;
        }

        if (player.hasTwoHandSword)
            StartCoroutine("TwoHandSwing");
        else if (player.hasRange)
            StartCoroutine("RangeFire");
    }

    IEnumerator TwoHandSwing()
    {
        anim.SetBool("TwoHandFire1", true);
        anim.SetTrigger("doTwoHandFire1");
        if (meleeArea != null && trailEffect != null)
        {
            meleeArea.enabled = true;
            trailEffect.enabled = true;
        }
        player.isFire = true;
        yield return new WaitForSeconds(0.8f);
        if (player.fireStack == 1)
        {
            ResetFireStack();
            if (meleeArea != null && trailEffect != null)
            {
                meleeArea.enabled = false;
                trailEffect.enabled = false;
            }
        }
        if (player.fireStack >= 2)
        {
            anim.SetBool("TwoHandFire2", true);
            anim.SetTrigger("doTwoHandFire2");
        }

        yield return new WaitForSeconds(0.8f);
        ResetFireStack();


    }

    IEnumerator RangeFire()
    {
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;




        yield return new WaitForSeconds(1f);
        Destroy(intantBullet);

    }



    void ResetFireStack()
    {
        player.fireStack = 0;
        if (meleeArea != null && trailEffect != null)
        {
            meleeArea.enabled = false;
            trailEffect.enabled = false;
        }
        if (player.hasTwoHandSword)
            TwoHandFrieF();
    }

    void TwoHandFrieF()
    {
        anim.SetBool("TwoHandFire1", false);
        anim.SetBool("TwoHandFire2", false);

        player.isFire = false;

    }

    //Use = Main 루틴 -> Swing = 서브루틴 -> Use 메인루틴 아래 로직 순차적 실행
    //Use = Main 루틴 -> Swing = 코루틴 호출시 따로가 아닌 같이 실행이 됨

}