using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassManager : MonoBehaviour
{
    // Grass�I�u�W�F�N�g���ꊇ�Ǘ����郊�X�g
    public List<GameObject> grassObjects = new List<GameObject>();
    void Start()
    {
        // Scene���̂��ׂĂ�Grass�I�u�W�F�N�g��o�^
        GameObject[] grasses = GameObject.FindGameObjectsWithTag("Grass");
        grassObjects.AddRange(grasses);
    }
    // Grass�����X�g�ɓo�^����
    public void RegisterGrass(GameObject grass)
    {
        if (!grassObjects.Contains(grass)) // �d���o�^��h��
        {
            grassObjects.Add(grass);
        }
    }

    // Grass��BoxCollider�����ׂĖ�����
    public void DeactivateAllGrassColliders()
    {
        foreach (GameObject grass in grassObjects)
        {
            if (grass.TryGetComponent<BoxCollider>(out BoxCollider collider))
            {
                collider.enabled = false;
            }
        }
    }
    // Grass��BoxCollider�����ׂėL����
    public void ActivateAllGrassColliders()
    {
        foreach (GameObject grass in grassObjects)
        {
            if (grass.TryGetComponent<BoxCollider>(out BoxCollider collider))
            {
                collider.enabled = true;
            }
        }
    }
}
