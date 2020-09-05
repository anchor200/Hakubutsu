using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookY : MonoBehaviour
{
    [SerializeField]
    private float y_sensitivity = 3f;

    void Update()
    {
        float y_mouse = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(1))
        {
            Vector3 newRotation = transform.localEulerAngles;
            newRotation.x += y_mouse * y_sensitivity;
            transform.localEulerAngles = newRotation;
        }

    }
}