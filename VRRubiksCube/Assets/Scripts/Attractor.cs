using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] Transform origin;
    [SerializeField] float amount = 9.8f;

    void Start()
    {
        if (!body)
            enabled = false;
    }

    void Update()
    {
        if (body)
            body.transform.position = Vector3.Lerp(body.transform.position, origin.position, amount * Time.deltaTime);
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
