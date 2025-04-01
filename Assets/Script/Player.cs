using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    float moveSpeed = 3.0f;
    Vector2 direction = Vector2.up;
    Vector2 previousDirection; // �O��̕�����ۑ�

    //��������I�u�W�F�N�g
    [SerializeField]
    GameObject neck;

    GameObject currentNeck; // ���݂�neck�I�u�W�F�N�g��ێ�

    void Start()
    {
        previousDirection = direction; // ����������ۑ�
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // ���L�[�ŕ������w��
        float horizontal = Input.GetAxis("Horizontal"); // ���E�L�[
        float vertical = Input.GetAxis("Vertical");     // �㉺�L�[

        // ���͂�����ꍇ�̂ݕ������X�V
        if (horizontal != 0 || vertical != 0)
        {
            direction = new Vector2(horizontal, vertical).normalized;

            // �������ς�����ꍇ�ɃI�u�W�F�N�g�𐶐�
            if (direction != previousDirection)
            {
                SpawnNeck(); // ��������
                previousDirection = direction; // �V����������ۑ�
            }
        }

        // ���݂̕����ɃI�u�W�F�N�g���ړ�
        Vector3 movement = new Vector3(direction.x, direction.y, 0); // Z�������Œ�
        transform.position += movement * moveSpeed * Time.deltaTime;

        // �ړ�������90���P�ʂŉ�]��K�p
        if (direction != Vector2.zero)
        {
            float angle = 0;

            // �Œ肳�ꂽ�p�x��ݒ�
            if (direction == Vector2.up) angle = 0f;
            else if (direction == Vector2.right) angle = -90f;
            else if (direction == Vector2.down) angle = 180f;
            else if (direction == Vector2.left) angle = 90f;

            transform.rotation = Quaternion.Euler(0, 0, angle); // Z����]�̂ݓK�p
        }

        // neck����������Ă���ꍇ�A�������΂�
        if (currentNeck != null)
        {
            StretchNeck();
        }
    }

    void SpawnNeck()
    {
        // neck�I�u�W�F�N�g���v���C���[�̒��S�ʒu��ɐ���
        Vector3 neckPosition = transform.position + new Vector3(0, 0, 0); // �v���C���[�̉��ӊ�Ŕz�u
        currentNeck = Instantiate(neck, neckPosition, Quaternion.identity);

        // neck�̊p�x���Œ艻
        float neckAngle = 0;
        if (direction == Vector2.up) neckAngle = 0f;
        else if (direction == Vector2.right) neckAngle = -90f;
        else if (direction == Vector2.down) neckAngle = 180f;
        else if (direction == Vector2.left) neckAngle = 90f;

        currentNeck.transform.rotation = Quaternion.Euler(0, 0, neckAngle); // ��]���Œ�
        currentNeck.transform.position = neckPosition; //�ʒu���Ē���
    }

    void StretchNeck()
    {
        // neck�̃X�P�[���𓮓I�ɒ���
        Vector3 neckPosition = currentNeck.transform.position;
        float distance = Vector3.Distance(neckPosition, transform.position); // �v���C���[�Ƃ̋������v�Z

        // neck�̃X�P�[����Y�������ɐL�΂�
        currentNeck.transform.localScale = new Vector3(
            currentNeck.transform.localScale.x, // X�X�P�[���͌Œ�
            distance,                           // Y�X�P�[���������Ɋ�Â��Đݒ�
            currentNeck.transform.localScale.z // Z�X�P�[���͌Œ�
        );
    }
}

