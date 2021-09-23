using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public enum SkillName { Berserker, Giant, Bomb, Void, Heal, Reflect, AuraSword, WaterPool };
    public SkillName skillName;    
    public GameObject skillEffect;
    Player_knights pUser;


    void Start()
    {
        pUser = GameObject.FindWithTag("Player").GetComponent<Player_knights>();

    }

    public void Use()
    {

        if (skillName == SkillName.Giant)
        {
            StartCoroutine("Giant");
            Invoke("T_CoolDown", 20f);
        }

        if (skillName == SkillName.Berserker)
        {
            StartCoroutine("Berserker");
            Invoke("T_CoolDown", 10f);
        }

        if (skillName == SkillName.Bomb)
        {
            StartCoroutine("Bomb");
            Invoke("R_CoolDown", 7f);
        }


        if (skillName == SkillName.Void)
        {
            StartCoroutine("Void");
            Invoke("R_CoolDown", 13f);
        }

        if (skillName == SkillName.Heal)
        {
            StartCoroutine("Heal");
            Invoke("R_CoolDown", 7f);
        }

        if (skillName == SkillName.Reflect)
        {
            StartCoroutine("Reflect");
            Invoke("T_CoolDown", 15f);
        }

        if (skillName == SkillName.AuraSword)
        {
            StartCoroutine("AuraSword");
            Invoke("T_CoolDown", 13f);
        }

        if (skillName == SkillName.WaterPool)
        {
            StartCoroutine("WaterPool");
            Invoke("R_CollDown", 20f);
        }
    }

    IEnumerator Giant()
    {
        pUser.t_skillCoolDown = true;
        pUser.gameObject.tag = "Giant";
        pUser.isGiant = true;
        pUser.transform.localScale += new Vector3(10, 10, 10);
        pUser.leftGiantArea.enabled = true;
        pUser.RightGiantArea.enabled = true;
        yield return new WaitForSeconds(10f);
        pUser.gameObject.tag = "Untagged";
        pUser.isGiant = false;
        pUser.transform.localScale -= new Vector3(10, 10, 10);
        pUser.leftGiantArea.enabled = false;
        pUser.RightGiantArea.enabled = false;
    }

    IEnumerator Bomb()
    {
        pUser.r_skillCoolDown = true;
        Ray ray = pUser.followCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit, 100))
        {
            Vector3 nextVec = rayHit.point;
            nextVec.y = 0;
            GameObject instantEffect = Instantiate(skillEffect, nextVec, transform.rotation);
            RaycastHit[] rayHits = Physics.SphereCastAll(nextVec, 15, Vector3.up, 0, LayerMask.GetMask("Enemy"));

            foreach (RaycastHit hitObject in rayHits)
            {
                hitObject.transform.GetComponent<Enemy>().HitByMagic(nextVec, "Bomb");
            }
            Destroy(instantEffect, 5f);

        }
        yield return new WaitForSeconds(0f);
    }

    IEnumerator Berserker()
    {
        pUser.t_skillCoolDown = true;
        gameObject.tag = "Berserker";
        pUser.BerserkerEffect.SetActive(true);
        yield return new WaitForSeconds(10f);
        gameObject.tag = "Melee";
        pUser.BerserkerEffect.SetActive(false);
    }

    IEnumerator Void()
    {
        pUser.r_skillCoolDown = true;
        Ray ray = pUser.followCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit, 100))
        {
            Vector3 nextVec = rayHit.point;
            nextVec.y = 0;

            GameObject instantEffect = Instantiate(skillEffect, nextVec, transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0)));
            for (int i = 0; i < 5; i++)
            {
                RaycastHit[] rayHits = Physics.SphereCastAll(nextVec, 30, Vector3.up, 0, LayerMask.GetMask("Enemy"));

                foreach (RaycastHit hitObject in rayHits)
                {
                    hitObject.transform.GetComponent<Enemy>().HitByMagic(nextVec, "Void");
                }
                yield return new WaitForSeconds(1f);
            }
            Destroy(instantEffect);

        }

        yield return new WaitForSeconds(0f);

    }

    IEnumerator Heal()
    {
        pUser.r_skillCoolDown = true;
        pUser.HealEffect.SetActive(true);
        pUser.heart = pUser.maxHeart;
        yield return new WaitForSeconds(3f);
        pUser.HealEffect.SetActive(false);
    }

    IEnumerator Reflect()
    {
        pUser.t_skillCoolDown = true;
        pUser.gameObject.tag = "Reflect";
        pUser.ReflectEffect.SetActive(true);
        yield return new WaitForSeconds(8f);
        pUser.gameObject.tag = "Untagged";
        pUser.ReflectEffect.SetActive(false);
    }

    IEnumerator AuraSword()
    {
        pUser.t_skillCoolDown = true;

        pUser.AuraSwordEffect.SetActive(true);
        SoundManager.instance.SFXPlay("AuraSword", pUser.auraSwordSound, true, 6.5f);

        for (int i = 0; i < 14; i++)
        {
            Vector3 nextVec = gameObject.transform.position;
            RaycastHit[] rayHits = Physics.SphereCastAll(nextVec, 5, Vector3.up, 0, LayerMask.GetMask("Enemy"));
            foreach (RaycastHit hitObject in rayHits)
            {
                hitObject.transform.GetComponent<Enemy>().HitByMagic(nextVec, "AuraSword");
            }


            yield return new WaitForSeconds(0.5f);
        }

        pUser.AuraSwordEffect.SetActive(false);

    }

    IEnumerator WaterPool()
    {
        pUser.r_skillCoolDown = true;
        Ray ray = pUser.followCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit, 100))
        {
            Vector3 nextVec = rayHit.point;
            nextVec.y = 0;

            GameObject instantEffect = Instantiate(skillEffect, nextVec, transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0)));
            for (int i = 0; i < 4; i++)
            {
                RaycastHit[] rayHits = Physics.SphereCastAll(nextVec, 40, Vector3.up, 0, LayerMask.GetMask("Enemy"));

                foreach (RaycastHit hitObject in rayHits)
                {
                    hitObject.transform.GetComponent<Enemy>().HitByMagic(nextVec, "WaterPool");
                }
                yield return new WaitForSeconds(2f);
            }
            Destroy(instantEffect);

        }

        yield return new WaitForSeconds(0f);

    }


    void T_CoolDown()
    {
        pUser.t_skillCoolDown = false;
    }

    void R_CoolDown()
    {
        pUser.r_skillCoolDown = false;
    }




}
