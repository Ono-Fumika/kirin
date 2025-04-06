using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player; // プレイヤーのTransform参照
     float smoothSpeed = 0.125f; // カメラ移動の滑らかさ
     Vector3 offset = new Vector3(0,2,-10);  // カメラのオフセット

    [SerializeField] float maxHeight = 10f; // カメラの上昇を止める高さ

    void LateUpdate()
    {
        // プレイヤーの位置に基づいてカメラを移動
        Vector3 desiredPosition = new Vector3(transform.position.x, player.position.y + offset.y, transform.position.z);

        // y座標が制限を超えないようにする
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, 1, maxHeight);


        // 滑らかに移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
}
