using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    public void Move(InputAction.CallbackContext _context)
    {
        Vector3 dir = _context.ReadValue<Vector2>();
        Vector3 newPos = transform.position + (new Vector3(dir.x, 0, dir.y) * speed * Time.deltaTime);
        transform.position = newPos;
    }

    public void Rotate(InputAction.CallbackContext _context)
    {
        // Vector3 dir = _context.ReadValue<Vector2>();
        // Vector3 lookAtPos = transform.position + cam.forward;
        // lookAtPos.y = transform.position.y;
        // transform.LookAt(lookAtPos);
    }
}
