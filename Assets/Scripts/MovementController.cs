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

    private float moveDir { get; set; }
    public float MoveDir
    {
        get { return moveDir; }
        set { moveDir = value; }
    }
    
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
        Vector3 newPos = transform.position + (transform.forward * moveDir * speed * Time.deltaTime);
        transform.position = newPos;
    }

    public void Rotate()
    {
        Vector3 lookAtPos = transform.position + (RotationDir * rotationSpeed * Time.deltaTime);
        transform.LookAt(lookAtPos);
    }
    public void OnMove(InputAction.CallbackContext _context)
    {
        moveDir = _context.ReadValue<Vector2>().y;
    }

    public void OnRotate(InputAction.CallbackContext _context)
    {
        Vector2 dir = _context.ReadValue<Vector2>();
        RotationDir = new Vector3(dir.x, 0, dir.y);
    }
}
