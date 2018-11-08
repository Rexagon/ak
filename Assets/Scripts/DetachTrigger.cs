using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == tag)
        {
            Part part = other.GetComponentInParent<Part>();
            if (part == null || part.separated)
            {
                return;
            }

            HandController grabbedHand = part.grabbedBy;
            if (grabbedHand == null || grabbedHand.state != HandController.State.Deconstructing)
            {
                return;
            }

            Part newPart = Instantiate(part);
            newPart.transform.position = part.transform.position;
            newPart.transform.rotation = part.transform.rotation;
            newPart.separated = true;
            newPart.originJoint.connectedBody = null;

            Rigidbody rigidbody = newPart.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;

            grabbedHand.Attach(newPart);
            part.gameObject.SetActive(false);
        }
    }
}
