using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderManager : MonoBehaviour
{
    [SerializeField] Slider slider; // �X���C�_�[UI
    [SerializeField] GameObject clogPrefab; // clog�I�u�W�F�N�g�̃v���n�u
    [SerializeField] Transform player;    // �v���C���[��Transform

    bool isDecreasing = false;     // �������̏�Ԃ������t���O

    void Start()
    {
        slider.value = 0; // �����l��0�ɐݒ�
        slider.maxValue = 2; // �ő�l��2�ɐݒ�
    }

    public void IncreaseSlider(float amount)
    {
        if (!isDecreasing) // �������łȂ��ꍇ�̂ݑ����\
        {
            slider.value += amount;

            if (slider.value >= slider.maxValue)
            {
                slider.value = slider.maxValue; // �ő�l�𒴂��Ȃ��悤�ɐ���
                StartCoroutine(DecreaseSlider()); // �����������J�n
            }
        }
    }
    private System.Collections.IEnumerator DecreaseSlider()
    {
        isDecreasing = true; // �������t���O��L����

        while (slider.value > 0)
        {
            slider.value -= Time.deltaTime; // ���Ԃɉ����Ēl������
            yield return null; // ���̃t���[���܂őҋ@
        }

        slider.value = 0; // �Ō�ɒl��0�ɐݒ�
        GenerateClog(); // clog�I�u�W�F�N�g�𐶐�
        isDecreasing = false; // ����������t���O������
    }
    private void GenerateClog()
    {
        // �v���C���[�̉��ӂ�clog�I�u�W�F�N�g�𐶐�
        Vector3 clogPosition = player.position + new Vector3(0, -0.5f, 0); // �v���C���[�̉��ӂ���Ɉʒu��ݒ�
        Instantiate(clogPrefab, clogPosition, Quaternion.identity); // clog�I�u�W�F�N�g����
    }


}
