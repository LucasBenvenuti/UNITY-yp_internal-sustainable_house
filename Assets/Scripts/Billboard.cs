using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer sprite;
    Vector3 zoomScale;
    Vector3 minScale;
    public Vector3 maxScale;
    public bool repeatable;
    public float speed;
    public float duration;

    IEnumerator Start()
    {
        minScale = sprite.transform.localScale;
        while (repeatable)
        {
            yield return RepeatLerp(minScale, maxScale, duration);
            yield return RepeatLerp(maxScale, minScale, duration);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // transform.LookAt(Camera.main.transform.position, Vector3.up);
    }

    IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }

    }


}
