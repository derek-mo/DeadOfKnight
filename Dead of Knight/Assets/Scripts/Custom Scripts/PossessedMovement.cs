using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PossessedMovement : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionAsset playerInput;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string xInput = "xInput";
    private const string yInput = "yInput";
    private InputAction input;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        input = playerInput.FindActionMap("Player").FindAction("Move");
    }

    private void Update() {
        movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        rb.velocity = movement * moveSpeed;
        animator.SetFloat(horizontal, movement.x);
        animator.SetFloat(vertical, movement.y);

        if (movement != Vector2.zero) {
            animator.SetFloat(xInput, movement.x);
            animator.SetFloat(yInput, movement.y);
        }

        if (PauseMenu.isPaused) {
            input.Disable();
        }
        else {
            input.Enable();
        }
    }
}