using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassManager : MonoBehaviour
{
    // Grassオブジェクトを一括管理するリスト
    public List<GameObject> grassObjects = new List<GameObject>();
    void Start()
    {
        // Scene内のすべてのGrassオブジェクトを登録
        GameObject[] grasses = GameObject.FindGameObjectsWithTag("Grass");
        grassObjects.AddRange(grasses);
    }
    // Grassをリストに登録する
    public void RegisterGrass(GameObject grass)
    {
        if (!grassObjects.Contains(grass)) // 重複登録を防ぐ
        {
            grassObjects.Add(grass);
        }
    }

    // GrassのBoxColliderをすべて無効化
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
    // GrassのBoxColliderをすべて有効化
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
