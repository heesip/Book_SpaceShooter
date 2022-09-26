#pragma warning disable IDE0051

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrlByEvent : MonoBehaviour
{
    [SerializeField] InputAction moveAction;
    [SerializeField] InputAction attackAction;

    [SerializeField] Animator anim;
    [SerializeField] Vector3 moveDir;

    void Start()
    {
        anim = GetComponent<Animator>();

        moveAction = new InputAction("Move", InputActionType.Value);

        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

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

        moveAction.Enable();

        attackAction = new InputAction("Attack", InputActionType.Button, "<Keyboard>/space");

        attackAction.performed += ctx =>
        {
            anim.SetTrigger("Attack");
        };

        attackAction.Enable();
    }

    void Update()
    {
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);

            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }
}
