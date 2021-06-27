using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Player_knights player2;
    public Boss boss;
    public GameObject playerUI;
    public Text hpTxt;
    public Text moneyTxt;

    public Image playerHPBar;
    public Image playerMPBar;
    public RectTransform bossHPUI;
    public Image bossHPBar;
    void Update()
    {
        player = GameObject.FindWithTag("Player");
        player2 = player.GetComponent<Player_knights>();
    }
    void LateUpdate()
    {
        //플레이어 UI
        moneyTxt.text = "" + player2.money;
        hpTxt.text = player2.heart + " / " + player2.maxHeart;
        playerHPBar.fillAmount = (float)player2.heart / player2.maxHeart;
        playerMPBar.fillAmount = player2.state / 100;

        //보스 체력 UI
        if (boss != null)
        {
            bossHPBar.fillAmount = (float)boss.curHealth / boss.maxHealth;
        }
    }
}
