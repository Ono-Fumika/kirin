using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{

    float moveSpeed = 3.0f;
    Vector2 direction = Vector2.up;
    Vector2 previousDirection; // �O��̕�����ۑ�

    // ��������I�u�W�F�N�g
    [SerializeField] GameObject neck;
    [SerializeField] GameObject grass;

    [SerializeField] CountdownManager countdownManager;
    [SerializeField] GrassManager grassManager;
    [SerializeField] SliderManager sliderManager;

    [SerializeField] TMP_Text clearText; // �u�N���A�v�e�L�X�gUI���C���X�y�N�^�[�ŕR�Â�
    bool isClear = false; // �N���A�t���O

    [SerializeField] TMP_Text distanceText; // ������\������e�L�X�gUI
    GameObject checkPoint; // �`�F�b�N�|�C���g�I�u�W�F�N�g
    float distance; // �v���C���[��ӂƃ`�F�b�N�|�C���g�̋���

    GameObject currentNeck; // ���݂�neck�I�u�W�F�N�g��ێ�
    List<GameObject> allNecks = new List<GameObject>(); // �������ꂽ�S�Ă�neck��ǐ�
    List<Vector3> positionHistory = new List<Vector3>(); // �v���C���[�̈ʒu����
    List<float> rotationHistory = new List<float>();    // �v���C���[�̉�]����

    List<GameObject> hiddenGrassObjects = new List<GameObject>(); // ���������̃��X�g��ǐ�


    bool isRewinding = false; // �t�Đ������ǂ����̃t���O
    bool isMove = false;

    void Start()
    {
        previousDirection = direction; // ����������ۑ�
        positionHistory.Add(transform.position); // �����ʒu���L�^
        rotationHistory.Add(transform.rotation.eulerAngles.z); // ������]���L�^

        // �����ʒu��neck�𐶐�
        SpawnNeck();
        // �`�F�b�N�|�C���g���V�[������擾
        checkPoint = GameObject.FindWithTag("CheckPoint");
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(RewindMovement()); // �X�y�[�X�L�[�������Ƌt�Đ����J�n
        //}

        UpdateDistanceText(); // �e�L�X�g�Ƃ̋������v��
        UpdateTextPosition();

        if (isClear && Input.GetKeyDown(KeyCode.Space)) // �X�y�[�X�L�[�ŃV�[�������[�h
        {
            Time.timeScale = 1; // ���Ԃ����ɖ߂�
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // �V�[�����ēǂݍ���
        }
        else if (!isRewinding && !isMove && !isClear) // �N���A���Ă��Ȃ��ꍇ�̂ݒʏ�̈ړ�
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
        grassManager.DeactivateAllGrassColliders(); // Grass��BoxCollider�𖳌���

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

        grassManager.ActivateAllGrassColliders(); // Grass��BoxCollider��L����

        isRewinding = false; // �t�Đ��I��


        RestoreGrass(); // ���𕜊�������
        countdownManager.ResetCountdown(); // �t�Đ�������ɃJ�E���g�����Z�b�g
        SpawnNeck(); // �����������ۂɐV����neck�𐶐�
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Grass")) // Grass�^�O�����I�u�W�F�N�g�ɏՓ�
        {
            collision.gameObject.SetActive(false); // �����A�N�e�B�u�ɂ��Č����ڂƓ����蔻�������
            hiddenGrassObjects.Add(collision.gameObject); // �������X�g�ɒǉ�
            countdownManager.AddSeconds(3); // �J�E���g��3�b�ǉ�
            sliderManager.IncreaseSlider(1); // �X���C�_�[�̒l��1����
        }

        if (collision.collider.CompareTag("Neck"))
        {
            isMove = false;
        }
        if (collision.collider.CompareTag("CheckPoint"))
        {
            HandleClear(); // �N���A�������Ăяo��
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // �t�Đ����̂ݏ��������s
        if (isRewinding && collision.collider.CompareTag("clog"))
        {
            // clog�̈ʒu��ۑ�
            Vector3 clogPosition = collision.transform.position;

            // Grass�I�u�W�F�N�g�𐶐�
            GameObject newGrass = Instantiate(grass, clogPosition, Quaternion.identity);
            
            // BoxCollider���A�N�e�B�u��
            if (newGrass.TryGetComponent<BoxCollider>(out BoxCollider collider))
            {
                collider.enabled = false;
            }

            // GrassManager�ɓo�^
            if (grassManager != null)
            {
                grassManager.RegisterGrass(newGrass);
            }
            // clog�I�u�W�F�N�g���폜
            Destroy(collision.gameObject);
        }
    }

    void RestoreGrass()
    {
        // ��A�N�e�B�u���������𕜊�������
        foreach (GameObject grass in hiddenGrassObjects)
        {
            grass.SetActive(true);
        }

        hiddenGrassObjects.Clear(); // ���X�g���N���A
    }
    void HandleClear()
    {
        // �N���A�t���O�𗧂Ăē�����~
        isClear = true;
        Time.timeScale = 0; // �������~
        clearText.gameObject.SetActive(true); // �u�N���A�v�̕�����\��
    }
    void UpdateDistanceText()
    {
        // �v���C���[�̏�ӈʒu���v�Z
        Vector3 playerTopPosition = new Vector3(transform.position.x, transform.position.y +2.1f, transform.position.z);

        // �`�F�b�N�|�C���g�Ƃ̋������v�Z
        distance = Vector3.Distance(playerTopPosition, checkPoint.transform.position);

        // �������e�L�X�g�ɔ��f�i������1�ʂ܂ŕ\���j
        distanceText.text = $"{Mathf.RoundToInt(distance)} m";
    }
    void UpdateTextPosition()
    {
        // �v���C���[�̓���̈ʒu���v�Z
        Vector3 playerTopPosition = new Vector3(
            transform.position.x + 0.5f,
            transform.position.y + 2.5f, // �v���C���[�̍��� + �I�t�Z�b�g
            transform.position.z
        );

        // �e�L�X�g�̈ʒu���v���C���[�̓���Ɉړ�
        distanceText.transform.position = playerTopPosition;
        distanceText.transform.rotation = Quaternion.identity; // �e�L�X�g����]�����Ȃ�
    }


}