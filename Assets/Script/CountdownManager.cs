using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CountdownManager : MonoBehaviour
{
    [SerializeField] TMP_Text countdownText; // カウントダウン表示用のText
    [SerializeField] Player player;     // Playerスクリプトへの参照

    float countdown = 10f; // 初期カウントダウン秒数
    float initialCountdown; // カウントの初期値を保存

    void Start()
    {
        initialCountdown = countdown; // 初期値を保存
        // 初期のカウント値を表示
        UpdateCountdownUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown > 0)
        {
            // 時間を減少
            countdown -= Time.deltaTime;
            UpdateCountdownUI();
        }
        else
        {
            // カウントが0になったら逆再生開始
            countdown = 0; // カウントを固定
            player.StartRewind(); // プレイヤーの逆再生を開始
        }
    }
    public void ResetCountdown()
    {
        countdown = initialCountdown; // カウントを初期値にリセット
        UpdateCountdownUI(); // 表示を更新
    }

    void UpdateCountdownUI()
    {
        countdownText.text = Mathf.CeilToInt(countdown).ToString();
    }

}
