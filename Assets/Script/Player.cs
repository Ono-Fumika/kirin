using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    float moveSpeed = 3.0f;
    Vector2 direction = Vector2.up;
    Vector2 previousDirection; // 前回の方向を保存

    // 生成するオブジェクト
    [SerializeField] GameObject neck;

    [SerializeField] CountdownManager countdownManager;

    GameObject currentNeck; // 現在のneckオブジェクトを保持
    List<GameObject> allNecks = new List<GameObject>(); // 生成された全てのneckを追跡
    List<Vector3> positionHistory = new List<Vector3>(); // プレイヤーの位置履歴
    List<float> rotationHistory = new List<float>();    // プレイヤーの回転履歴

    bool isRewinding = false; // 逆再生中かどうかのフラグ

    void Start()
    {
        previousDirection = direction; // 初期方向を保存
        positionHistory.Add(transform.position); // 初期位置を記録
        rotationHistory.Add(transform.rotation.eulerAngles.z); // 初期回転を記録

        // 初期位置にneckを生成
        SpawnNeck();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(RewindMovement()); // スペースキーを押すと逆再生を開始
        //}

        if (!isRewinding) // 逆再生中でなければ通常の移動
        {
            Move();
        }
    }

    void Move()
    {
        // 矢印キーで方向を指定
        float horizontal = Input.GetAxis("Horizontal"); // 左右キー
        float vertical = Input.GetAxis("Vertical");     // 上下キー

        // 入力を1方向に制限（優先順位：水平 > 垂直）
        if (horizontal != 0)
        {
            vertical = 0; // 水平方向の入力がある場合、垂直方向を無視
        }

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

        // プレイヤーの位置と回転を記録
        if (positionHistory.Count == 0 || transform.position != positionHistory[positionHistory.Count - 1])
        {
            positionHistory.Add(transform.position);
            rotationHistory.Add(transform.rotation.eulerAngles.z); // 回転を記録
        }

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
        // neckオブジェクトをプレイヤーの下辺に生成
        Vector3 neckPosition = transform.position + new Vector3(0, 0, 0); // プレイヤーの下辺を基準に配置
        currentNeck = Instantiate(neck, neckPosition, Quaternion.identity);

        // 最新のneckオブジェクトをリストに追加
        allNecks.Add(currentNeck);

        // neckの角度を固定化
        float neckAngle = 0;
        if (direction == Vector2.up) neckAngle = 0f;
        else if (direction == Vector2.right) neckAngle = -90f;
        else if (direction == Vector2.down) neckAngle = 180f;
        else if (direction == Vector2.left) neckAngle = 90f;

        currentNeck.transform.rotation = Quaternion.Euler(0, 0, neckAngle); // 回転を固定
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

    public void StartRewind()
    {
        if (!isRewinding)
        {
            StartCoroutine(RewindMovement());
        }
    }

    IEnumerator RewindMovement()
    {
        isRewinding = true; // 逆再生を開始

        float rewindSpeed = 3.0f; // neckとプレイヤーの戻る速度
        int rewindIndex = positionHistory.Count - 1; // 位置履歴の最後のインデックス

        // neckとプレイヤーを順に戻す
        for (int i = allNecks.Count - 1; i >= 0; i--)
        {
            GameObject neck = allNecks[i];
            Vector3 initialScale = neck.transform.localScale;
            Vector3 targetScale = new Vector3(initialScale.x, 0, initialScale.z); // neckの最終状態

            float elapsed = 0;
            float duration = initialScale.y / rewindSpeed; // neckの長さに基づいて戻る時間を計算

            while (elapsed < duration)
            {
                // neckのスケールを一定速度で更新
                neck.transform.localScale = new Vector3(
                    initialScale.x,
                    Mathf.Lerp(initialScale.y, targetScale.y, elapsed / duration),
                    initialScale.z
                );

                // neckの進行に合わせてプレイヤーの位置と回転を更新
                if (rewindIndex >= 0)
                {
                    transform.position = positionHistory[rewindIndex];
                    transform.rotation = Quaternion.Euler(0, 0, rotationHistory[rewindIndex]); // 回転を適用
                    rewindIndex--; // インデックスを1つ戻す
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(neck); // neckを削除
        }

        allNecks.Clear(); // 全てのneckデータをクリア

        // 残りの位置履歴を正しい速度で処理
        while (rewindIndex >= 0)
        {
            transform.position = positionHistory[rewindIndex];
            transform.rotation = Quaternion.Euler(0, 0, rotationHistory[rewindIndex]); // 回転を適用
            rewindIndex--;
            yield return new WaitForSeconds(1.0f / rewindSpeed);
        }

        positionHistory.Clear(); // すべての履歴をクリア
        rotationHistory.Clear(); // 回転履歴をクリア
        positionHistory.Add(transform.position); // 現在の位置を履歴に記録
        rotationHistory.Add(transform.rotation.eulerAngles.z); // 現在の回転を記録

        isRewinding = false; // 逆再生終了

        countdownManager.ResetCountdown(); // 逆再生完了後にカウントをリセット
        // 動き直した際に新しいneckを生成
        SpawnNeck();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Grass")) // Grassタグを持つオブジェクトに衝突
        {
            Destroy(collision.gameObject); // Grassオブジェクトを消す
            countdownManager.AddSeconds(3); // カウントを3秒追加
        }
    }

}

