using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] Vector3 axis = new Vector3(0.75f, 0.75f, 0);
    [SerializeField] float speed = 15.0f;

    void Start()
    {
        if (!body)
            enabled = false;
    }

    void Update()
    {
        body?.Rotate(axis, speed * Time.deltaTime);
    }

    public void ActOn(Transform tform)
    {
        body = tform;
        enabled = true;
    }

    public void Stop()
    {
        body = null;
        enabled = false;
    }
}
