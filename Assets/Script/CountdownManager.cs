using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CountdownManager : MonoBehaviour
{
    [SerializeField] TMP_Text countdownText; // �J�E���g�_�E���\���p��Text
    [SerializeField] Player player;     // Player�X�N���v�g�ւ̎Q��

    float countdown = 5f; // �����J�E���g�_�E���b��
    float initialCountdown; // �J�E���g�̏����l��ۑ�

    void Start()
    {
        initialCountdown = countdown; // �����l��ۑ�
        // �����̃J�E���g�l��\��
        UpdateCountdownUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown > 0)
        {
            // ���Ԃ�����
            countdown -= Time.deltaTime;
            UpdateCountdownUI();
        }
        else
        {
            // �J�E���g��0�ɂȂ�����t�Đ��J�n
            countdown = 0; // �J�E���g���Œ�
            player.StartRewind(); // �v���C���[�̋t�Đ����J�n
        }
    }
    public void ResetCountdown()
    {
        countdown = initialCountdown; // �J�E���g�������l�Ƀ��Z�b�g
        UpdateCountdownUI(); // �\�����X�V
    }
    public void AddSeconds(float seconds)
    {
        countdown += seconds; // �b�������Z
        countdown = Mathf.Min(countdown, 5); // �ő�l��10�ɐ���
        UpdateCountdownUI();

    }


    void UpdateCountdownUI()
    {
        countdownText.text = Mathf.CeilToInt(countdown).ToString();
    }

}
