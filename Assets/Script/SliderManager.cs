using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderManager : MonoBehaviour
{
    [SerializeField] Slider slider; // スライダーUI
    [SerializeField] GameObject clogPrefab; // clogオブジェクトのプレハブ
    [SerializeField] Transform player;    // プレイヤーのTransform

    bool isDecreasing = false;     // 減少中の状態を示すフラグ

    void Start()
    {
        slider.value = 0; // 初期値を0に設定
        slider.maxValue = 2; // 最大値を2に設定
    }

    public void IncreaseSlider(float amount)
    {
        if (!isDecreasing) // 減少中でない場合のみ増加可能
        {
            slider.value += amount;

            if (slider.value >= slider.maxValue)
            {
                slider.value = slider.maxValue; // 最大値を超えないように制限
                StartCoroutine(DecreaseSlider()); // 自動減少を開始
            }
        }
    }
    private System.Collections.IEnumerator DecreaseSlider()
    {
        isDecreasing = true; // 減少中フラグを有効化

        while (slider.value > 0)
        {
            slider.value -= Time.deltaTime; // 時間に応じて値を減少
            yield return null; // 次のフレームまで待機
        }

        slider.value = 0; // 最後に値を0に設定
        GenerateClog(); // clogオブジェクトを生成
        isDecreasing = false; // 減少完了後フラグを解除
    }
    private void GenerateClog()
    {
        // プレイヤーの下辺にclogオブジェクトを生成
        Vector3 clogPosition = player.position + new Vector3(0, -0.5f, 0); // プレイヤーの下辺を基準に位置を設定
        Instantiate(clogPrefab, clogPosition, Quaternion.identity); // clogオブジェクト生成
    }


}
