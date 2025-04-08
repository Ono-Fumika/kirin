using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bard : MonoBehaviour
{
    float moveSpeed = 2.0f; // Bard�̈ړ����x
    private Camera mainCamera; // ���C���J�����̎Q��

    void Start()
    {
        // ���C���J�������擾
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Bard����ʓ��ɂ��邩����
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        // �r���[���ix: 0�`1, y: 0�`1, z > 0�j�œ����A����ȊO�Œ�~
        if (viewportPos.x >= 0 && viewportPos.x <= 1 &&
            viewportPos.y >= 0 && viewportPos.y <= 1 &&
            viewportPos.z > 0)
        {
            MoveRight(); // ��ʓ��ɂ���ꍇ�͉E�ɓ���
        }
        // Bard���E�Ɉړ�������
        void MoveRight()
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }
}
