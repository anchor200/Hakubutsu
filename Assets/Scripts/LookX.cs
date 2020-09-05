using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookX : MonoBehaviour
{
    [SerializeField]
    private float x_sensitivity = 3f;

    void Update()
    {
        float x_mouse = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(1))
        {
            Vector3 newRotation = transform.localEulerAngles;
            newRotation.y += x_mouse * x_sensitivity;
            transform.localEulerAngles = newRotation;
        }
    }
}