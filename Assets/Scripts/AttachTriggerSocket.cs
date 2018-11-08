using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTriggerSocket : MonoBehaviour
{
    public Part partOverlapped;

    void OnTriggerEnter(Collider other)
    {
        UpdateOverlap(other, true);
    }

    void OnTriggerExit(Collider other)
    {
        UpdateOverlap(other, false);
    }

    void UpdateOverlap(Collider other, bool value)
    {
        if (other.tag == tag)
        {
            Part part = other.GetComponentInParent<Part>();
            if (part == null || !part.separated)
            {
                return;
            }
            
            partOverlapped = value ? part : null;
        }
    }
}
