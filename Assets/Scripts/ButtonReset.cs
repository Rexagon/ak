using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonReset : MonoBehaviour
{
    public Vector3 pressOffset;
    public Transform button;

    public bool _pressed = false;

    void OnTriggerStay(Collider collider)
    {
        HandController hand = collider.GetComponent<HandController>();
        if (hand == null)
        {
            return;
        }

        if (hand.device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (!_pressed)
            {
                button.position -= pressOffset;
                _pressed = true;
            }
        }

        if (hand.device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        HandController hand = collider.GetComponent<HandController>();
        if (hand == null)
        {
            return;
        }

        if (_pressed)
        {
            button.position += pressOffset;
            _pressed = false;
        }
    }
}
