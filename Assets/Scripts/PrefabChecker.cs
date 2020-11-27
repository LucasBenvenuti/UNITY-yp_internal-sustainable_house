using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabChecker : MonoBehaviour
{
    public int ObjectType;

    void Start()
    {
        int i = DataStorage.instance.sceneObjectsList[ObjectType];

        if (i != -1)
        {
            Destroy(transform.GetChild(0).gameObject);

            GameObject prefab = Instantiate(GameController.instance.prefabsList[ObjectType].prefabsList[i].gameObject);
            prefab.transform.parent = this.transform;
            prefab.transform.position = prefab.transform.parent.position;
        }
    }
}
