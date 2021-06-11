using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPCamera : MonoBehaviour
{
    public Enemy enemy;
    public Image enemyHPUI;
    Transform cam;
    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
    }

    void LateUpdate()
    {
        enemyHPUI.fillAmount = (float)enemy.curHealth / enemy.maxHealth;
    }
}
