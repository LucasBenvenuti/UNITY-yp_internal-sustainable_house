using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampTransform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -36.5f, 37.5f), Mathf.Clamp(transform.position.y, 10, 60), Mathf.Clamp(transform.position.z, -45.5f, 20.5f));
    }
}
