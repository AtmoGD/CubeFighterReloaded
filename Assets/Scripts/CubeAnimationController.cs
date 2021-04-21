using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CubeAnimationController : MonoBehaviour
{
    [SerializeField] private CubeController controller = null;
    private Animator anim = null;
    void Awake()
    {
        anim = GetComponent<Animator>();

        controller.Jumping += OnJump;
        controller.Dashing += OnDash;
        controller.Idle += OnIdle;
    }

    public void OnJump()
    {
        anim.SetFloat("Jump", 1f);
        anim.SetFloat("Dash", 0f);
        anim.SetFloat("Idle", 0f);
    }

    public void OnDash()
    {
        anim.SetFloat("Jump", 0f);
        anim.SetFloat("Dash", 1f);
        anim.SetFloat("Idle", 0f);
    }

    public void OnIdle()
    {
        anim.SetFloat("Jump", 0f);
        anim.SetFloat("Dash", 0f);
        anim.SetFloat("Idle", 1f);
    }
}
