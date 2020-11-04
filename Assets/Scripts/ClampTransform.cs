using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampTransform : MonoBehaviour
{
    public Transform objectLimit_posX;
    public Transform objectLimit_negX;
    public Transform objectLimit_posZ;
    public Transform objectLimit_negZ;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, objectLimit_negX.position.x, objectLimit_posX.position.x), Mathf.Clamp(transform.position.y, 10, 60), Mathf.Clamp(transform.position.z, objectLimit_negZ.position.z, objectLimit_posZ.position.z));

    }
}
