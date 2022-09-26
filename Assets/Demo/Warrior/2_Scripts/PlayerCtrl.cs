#pragma warning disable IDE0051

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] new Transform transform;
    [SerializeField] Vector3 moveDir;

    PlayerInput playerInput;
    InputActionMap mainActoinMap;
    InputAction moveAction;
    InputAction attackAction;

    private void Start()
    {
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();

        playerInput = GetComponent<PlayerInput>();
        mainActoinMap = playerInput.actions.FindActionMap("PlayerActions");
        moveAction = mainActoinMap.FindAction("Move");
        attackAction = mainActoinMap.FindAction("Attack");

        moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            anim.SetFloat("Movement", dir.magnitude);
        };
        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;
            anim.SetFloat("Movement", 0.0f);
        };
        attackAction.performed += ctx =>
        {
            Debug.Log("Attack by C# event");
            anim.SetTrigger("Attack");
        };

    }

    private void Update()
    {
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }

    #region SEND_MESSAGW
    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
        anim.SetFloat("Movement", dir.magnitude);

        Debug.Log($"Move = {dir.x}, {dir.y}");
    }

    void OnAttack()
    {
        Debug.Log("Attack");
        anim.SetTrigger("Attack");

    }
    #endregion

    #region UNITY_EVENTS

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
        anim.SetFloat("Movement", dir.magnitude);
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ctx.phase = {ctx.phase}");
        if (ctx.performed)
        {
            Debug.Log("Attack");
            anim.SetTrigger("Attack");

        }
    }
    #endregion
}
