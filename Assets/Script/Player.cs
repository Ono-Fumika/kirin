using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    float moveSpeed = 3.0f;
    Vector2 direction = Vector2.up;
    Vector2 previousDirection; // 前回の方向を保存

    //生成するオブジェクト
    [SerializeField]
    GameObject neck;

    GameObject currentNeck; // 現在のneckオブジェクトを保持

    void Start()
    {
        previousDirection = direction; // 初期方向を保存
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // 矢印キーで方向を指定
        float horizontal = Input.GetAxis("Horizontal"); // 左右キー
        float vertical = Input.GetAxis("Vertical");     // 上下キー

        // 入力がある場合のみ方向を更新
        if (horizontal != 0 || vertical != 0)
        {
            direction = new Vector2(horizontal, vertical).normalized;

            // 方向が変わった場合にオブジェクトを生成
            if (direction != previousDirection)
            {
                SpawnNeck(); // 生成処理
                previousDirection = direction; // 新しい方向を保存
            }
        }

        // 現在の方向にオブジェクトを移動
        Vector3 movement = new Vector3(direction.x, direction.y, 0); // Z方向を固定
        transform.position += movement * moveSpeed * Time.deltaTime;

        // 移動方向に90°単位で回転を適用
        if (direction != Vector2.zero)
        {
            float angle = 0;

            // 固定された角度を設定
            if (direction == Vector2.up) angle = 0f;
            else if (direction == Vector2.right) angle = -90f;
            else if (direction == Vector2.down) angle = 180f;
            else if (direction == Vector2.left) angle = 90f;

            transform.rotation = Quaternion.Euler(0, 0, angle); // Z軸回転のみ適用
        }

        // neckが生成されている場合、引き延ばす
        if (currentNeck != null)
        {
            StretchNeck();
        }
    }

    void SpawnNeck()
    {
        // neckオブジェクトをプレイヤーの中心位置基準に生成
        Vector3 neckPosition = transform.position + new Vector3(0, 0, 0); // プレイヤーの下辺基準で配置
        currentNeck = Instantiate(neck, neckPosition, Quaternion.identity);

        // neckの角度を固定化
        float neckAngle = 0;
        if (direction == Vector2.up) neckAngle = 0f;
        else if (direction == Vector2.right) neckAngle = -90f;
        else if (direction == Vector2.down) neckAngle = 180f;
        else if (direction == Vector2.left) neckAngle = 90f;

        currentNeck.transform.rotation = Quaternion.Euler(0, 0, neckAngle); // 回転を固定
        currentNeck.transform.position = neckPosition; //位置を再調整
    }

    void StretchNeck()
    {
        // neckのスケールを動的に調整
        Vector3 neckPosition = currentNeck.transform.position;
        float distance = Vector3.Distance(neckPosition, transform.position); // プレイヤーとの距離を計算

        // neckのスケールをY軸方向に伸ばす
        currentNeck.transform.localScale = new Vector3(
            currentNeck.transform.localScale.x, // Xスケールは固定
            distance,                           // Yスケールを距離に基づいて設定
            currentNeck.transform.localScale.z // Zスケールは固定
        );
    }
}

