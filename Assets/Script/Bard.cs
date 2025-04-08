using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bard : MonoBehaviour
{
    float moveSpeed = 2.0f; // Bardの移動速度
    private Camera mainCamera; // メインカメラの参照

    void Start()
    {
        // メインカメラを取得
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Bardが画面内にいるか判定
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        // ビュー内（x: 0～1, y: 0～1, z > 0）で動く、それ以外で停止
        if (viewportPos.x >= 0 && viewportPos.x <= 1 &&
            viewportPos.y >= 0 && viewportPos.y <= 1 &&
            viewportPos.z > 0)
        {
            MoveRight(); // 画面内にいる場合は右に動く
        }
        // Bardを右に移動させる
        void MoveRight()
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }
}
