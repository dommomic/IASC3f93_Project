using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float sSpeed = 0.123f;
    public Vector3 offset;
    public GameObject lookTarget;

    private void FixedUpdate()
    {
        Vector3 dPos = target.position + offset;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, sSpeed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(lookTarget.transform.position);
    }
}
