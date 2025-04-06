using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] GameObject twigPrefab; // Twig�I�u�W�F�N�g�̃v���n�u

    GameObject attachedTwig; // Grass�Ɋ֘A�t����ꂽTwig

    void Start()
    {
        EnsureTwigExists(); // Twig���m���ɐ�������
    }

    void EnsureTwigExists()
    {
        if (attachedTwig == null)
        {
            SpawnAndStretchTwig();
        }
    }

    void SpawnAndStretchTwig()
    {
        // Grass�I�u�W�F�N�g�̒��S���W
        Vector3 grassPosition = transform.position;

        // Twig��Grass�̒��S�ɐ���
        attachedTwig = Instantiate(twigPrefab, grassPosition, Quaternion.identity);

        // Twig�̃X�P�[����ύX���ĐL�΂�
        Vector3 twigScale = attachedTwig.transform.localScale; // ���݂̃X�P�[�����擾
        if (grassPosition.x < 0)
        {
            // x���W�����̏ꍇ�A���ɐL�΂�
            twigScale.x += 10.0f; // Twig���������ɐL�΂�
            attachedTwig.transform.localScale = twigScale;

            // ���S��Grass���猩�č��ɂ����̂ňʒu�𒲐�
            attachedTwig.transform.position -= new Vector3(5.0f, 0, 0); // �L�΂������̔������炷
        }
        else
        {
            // x���W�����̏ꍇ�A�E�ɐL�΂�
            twigScale.x += 10.0f; // Twig���E�����ɐL�΂�
            attachedTwig.transform.localScale = twigScale;

            // ���S��Grass���猩�ĉE�ɂ����̂ňʒu�𒲐�
            attachedTwig.transform.position += new Vector3(5.0f, 0, 0); // �L�΂������̔������炷
        }

        // Twig��Grass�̎q�ɐݒ�
        attachedTwig.transform.SetParent(transform);
    }
}
