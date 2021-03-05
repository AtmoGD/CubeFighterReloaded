using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraRotationController : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook cam = null;

    [Range(0f, 10f)]
    private float lookSpeed = 1f;

    [SerializeField]
    private bool invertY = false;

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookMovement = context.ReadValue<Vector2>().normalized;
        lookMovement.y = invertY ? -lookMovement.y : lookMovement.y;
        lookMovement.x = lookMovement.x * 180f;
        cam.m_XAxis.Value += lookMovement.x * lookSpeed * Time.deltaTime;
        cam.m_YAxis.Value += lookMovement.y * lookSpeed * Time.deltaTime;
    }
}
