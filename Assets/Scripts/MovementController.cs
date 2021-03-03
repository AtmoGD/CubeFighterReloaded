using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float rotationSpeed = 3f;

    public float MoveDir { get; private set; }

    private Vector3 rotationDir { get; set; }
    public Vector3 RotationDir
    {
        get { return rotationDir != null ? rotationDir : new Vector3(); }
        private set { rotationDir = value; }
    }

    public void FixedUpdate()
    {
        Move();
        Rotate();
    }

    public void Move()
    {
        Vector3 newPos = transform.position + (transform.forward * MoveDir * speed * Time.deltaTime);
        transform.position = newPos;
    }

    public void Rotate()
    {
        Quaternion targetDir = Quaternion.LookRotation((transform.position + RotationDir) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetDir, rotationSpeed * Time.deltaTime);

        // float angle = Vector3.Angle(transform.forward, RotationDir);
        // Vector3 rot = transform.rotation.eulerAngles;
        // rot.y += (angle * rotationSpeed * Time.deltaTime);
        // transform.rotation = Quaternion.Euler(rot);
        // Vector3 lookAtPos = transform.position + (RotationDir * rotationSpeed * Time.deltaTime);
        // transform.LookAt(lookAtPos);
    }
    public void OnMove(InputAction.CallbackContext _context)
    {
        MoveDir = _context.ReadValue<Vector2>().y;
    }

    public void OnRotate(InputAction.CallbackContext _context)
    {
        Vector2 dir = _context.ReadValue<Vector2>();
        RotationDir = new Vector3(dir.x, 0, dir.y);
    }
}
