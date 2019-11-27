using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverStandController : MonoBehaviour
{
    [SerializeField] Attractor attractor;
    [SerializeField] Rotator rotator;

    Rigidbody body;

    void Start()
    {
        body = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            body = rb;
            body.isKinematic = true;

            attractor.ActOn(body.transform);
            rotator.ActOn(body.transform);

            attractor.enabled = true;
            rotator.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb) && rb == body)
        {
            body.isKinematic = false;
            body = null;

            attractor.Stop();
            rotator.Stop();

            attractor.enabled = false;
            rotator.enabled = false;
        }
    }
}
