using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    float moveSpeed = 3.0f;
    Vector2 direction = Vector2.up;
    Vector2 previousDirection; // �O��̕�����ۑ�

    // ��������I�u�W�F�N�g
    [SerializeField] GameObject neck;

    [SerializeField] CountdownManager countdownManager;

    GameObject currentNeck; // ���݂�neck�I�u�W�F�N�g��ێ�
    List<GameObject> allNecks = new List<GameObject>(); // �������ꂽ�S�Ă�neck��ǐ�
    List<Vector3> positionHistory = new List<Vector3>(); // �v���C���[�̈ʒu����
    List<float> rotationHistory = new List<float>();    // �v���C���[�̉�]����

    bool isRewinding = false; // �t�Đ������ǂ����̃t���O

    void Start()
    {
        previousDirection = direction; // ����������ۑ�
        positionHistory.Add(transform.position); // �����ʒu���L�^
        rotationHistory.Add(transform.rotation.eulerAngles.z); // ������]���L�^

        // �����ʒu��neck�𐶐�
        SpawnNeck();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(RewindMovement()); // �X�y�[�X�L�[�������Ƌt�Đ����J�n
        //}

        if (!isRewinding) // �t�Đ����łȂ���Βʏ�̈ړ�
        {
            Move();
        }
    }

    void Move()
    {
        // ���L�[�ŕ������w��
        float horizontal = Input.GetAxis("Horizontal"); // ���E�L�[
        float vertical = Input.GetAxis("Vertical");     // �㉺�L�[

        // ���͂�1�����ɐ����i�D�揇�ʁF���� > �����j
        if (horizontal != 0)
        {
            vertical = 0; // ���������̓��͂�����ꍇ�A���������𖳎�
        }

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

        // �v���C���[�̈ʒu�Ɖ�]���L�^
        if (positionHistory.Count == 0 || transform.position != positionHistory[positionHistory.Count - 1])
        {
            positionHistory.Add(transform.position);
            rotationHistory.Add(transform.rotation.eulerAngles.z); // ��]���L�^
        }

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
        // neck�I�u�W�F�N�g���v���C���[�̉��ӂɐ���
        Vector3 neckPosition = transform.position + new Vector3(0, 0, 0); // �v���C���[�̉��ӂ���ɔz�u
        currentNeck = Instantiate(neck, neckPosition, Quaternion.identity);

        // �ŐV��neck�I�u�W�F�N�g�����X�g�ɒǉ�
        allNecks.Add(currentNeck);

        // neck�̊p�x���Œ艻
        float neckAngle = 0;
        if (direction == Vector2.up) neckAngle = 0f;
        else if (direction == Vector2.right) neckAngle = -90f;
        else if (direction == Vector2.down) neckAngle = 180f;
        else if (direction == Vector2.left) neckAngle = 90f;

        currentNeck.transform.rotation = Quaternion.Euler(0, 0, neckAngle); // ��]���Œ�
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

    public void StartRewind()
    {
        if (!isRewinding)
        {
            StartCoroutine(RewindMovement());
        }
    }

    IEnumerator RewindMovement()
    {
        isRewinding = true; // �t�Đ����J�n

        float rewindSpeed = 3.0f; // neck�ƃv���C���[�̖߂鑬�x
        int rewindIndex = positionHistory.Count - 1; // �ʒu�����̍Ō�̃C���f�b�N�X

        // neck�ƃv���C���[�����ɖ߂�
        for (int i = allNecks.Count - 1; i >= 0; i--)
        {
            GameObject neck = allNecks[i];
            Vector3 initialScale = neck.transform.localScale;
            Vector3 targetScale = new Vector3(initialScale.x, 0, initialScale.z); // neck�̍ŏI���

            float elapsed = 0;
            float duration = initialScale.y / rewindSpeed; // neck�̒����Ɋ�Â��Ė߂鎞�Ԃ��v�Z

            while (elapsed < duration)
            {
                // neck�̃X�P�[������葬�x�ōX�V
                neck.transform.localScale = new Vector3(
                    initialScale.x,
                    Mathf.Lerp(initialScale.y, targetScale.y, elapsed / duration),
                    initialScale.z
                );

                // neck�̐i�s�ɍ��킹�ăv���C���[�̈ʒu�Ɖ�]���X�V
                if (rewindIndex >= 0)
                {
                    transform.position = positionHistory[rewindIndex];
                    transform.rotation = Quaternion.Euler(0, 0, rotationHistory[rewindIndex]); // ��]��K�p
                    rewindIndex--; // �C���f�b�N�X��1�߂�
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(neck); // neck���폜
        }

        allNecks.Clear(); // �S�Ă�neck�f�[�^���N���A

        // �c��̈ʒu�����𐳂������x�ŏ���
        while (rewindIndex >= 0)
        {
            transform.position = positionHistory[rewindIndex];
            transform.rotation = Quaternion.Euler(0, 0, rotationHistory[rewindIndex]); // ��]��K�p
            rewindIndex--;
            yield return new WaitForSeconds(1.0f / rewindSpeed);
        }

        positionHistory.Clear(); // ���ׂĂ̗������N���A
        rotationHistory.Clear(); // ��]�������N���A
        positionHistory.Add(transform.position); // ���݂̈ʒu�𗚗��ɋL�^
        rotationHistory.Add(transform.rotation.eulerAngles.z); // ���݂̉�]���L�^

        isRewinding = false; // �t�Đ��I��

        countdownManager.ResetCountdown(); // �t�Đ�������ɃJ�E���g�����Z�b�g
        // �����������ۂɐV����neck�𐶐�
        SpawnNeck();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Grass")) // Grass�^�O�����I�u�W�F�N�g�ɏՓ�
        {
            Destroy(collision.gameObject); // Grass�I�u�W�F�N�g������
            countdownManager.AddSeconds(3); // �J�E���g��3�b�ǉ�
        }
    }

}

