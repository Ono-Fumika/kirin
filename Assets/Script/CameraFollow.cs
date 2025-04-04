using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player; // �v���C���[��Transform�Q��
     float smoothSpeed = 0.125f; // �J�����ړ��̊��炩��
     Vector3 offset = new Vector3(0,2,-10);  // �J�����̃I�t�Z�b�g

    void LateUpdate()
    {
        // �v���C���[�̈ʒu�Ɋ�Â��ăJ�������ړ�
        Vector3 desiredPosition = new Vector3(transform.position.x, player.position.y + offset.y, transform.position.z);

        // y���W��1�ȉ��ɂȂ�Ȃ��悤����
        desiredPosition.y = Mathf.Max(desiredPosition.y, 1);
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // ���炩�Ɉړ�
        transform.position = smoothedPosition;
    }
}
