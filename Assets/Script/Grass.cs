using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] GameObject twigPrefab; // Twigオブジェクトのプレハブ

    GameObject attachedTwig; // Grassに関連付けられたTwig

    void Start()
    {
        EnsureTwigExists(); // Twigを確実に生成する
    }

    void EnsureTwigExists()
    {
        if (attachedTwig == null)
        {
            SpawnAndStretchTwig();
        }
    }

    void SpawnAndStretchTwig()
    {
        // Grassオブジェクトの中心座標
        Vector3 grassPosition = transform.position;

        // TwigをGrassの中心に生成
        attachedTwig = Instantiate(twigPrefab, grassPosition, Quaternion.identity);

        // Twigのスケールを変更して伸ばす
        Vector3 twigScale = attachedTwig.transform.localScale; // 現在のスケールを取得
        if (grassPosition.x < 0)
        {
            // x座標が負の場合、左に伸ばす
            twigScale.x += 10.0f; // Twigを左方向に伸ばす
            attachedTwig.transform.localScale = twigScale;

            // 中心がGrassから見て左にずれるので位置を調整
            attachedTwig.transform.position -= new Vector3(5.0f, 0, 0); // 伸ばした分の半分ずらす
        }
        else
        {
            // x座標が正の場合、右に伸ばす
            twigScale.x += 10.0f; // Twigを右方向に伸ばす
            attachedTwig.transform.localScale = twigScale;

            // 中心がGrassから見て右にずれるので位置を調整
            attachedTwig.transform.position += new Vector3(5.0f, 0, 0); // 伸ばした分の半分ずらす
        }

        // TwigをGrassの子に設定
        attachedTwig.transform.SetParent(transform);
    }
}
