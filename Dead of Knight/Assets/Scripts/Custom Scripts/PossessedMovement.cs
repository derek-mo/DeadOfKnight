using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PossessedMovement : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionAsset playerInput;
	public PlayerAudio playerAudio;


    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string xInput = "xInput";
    private const string yInput = "yInput";
    private InputAction input;
    public Vector2 LastMovement { get; private set; }

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
            LastMovement = movement.normalized;
            animator.SetFloat(xInput, movement.x);
            animator.SetFloat(yInput, movement.y);
        }

        //Debug.Log("Movement: " + (movement != Vector2.zero));
        if (movement != Vector2.zero) {
            // start the loop if not already playing
            if (!playerAudio.WalkSource.isPlaying) {
                playerAudio.WalkSource.Play();
            }
        }
        else {
            // stop when you halt
            if (playerAudio.WalkSource.isPlaying) {
                playerAudio.WalkSource.Stop();
            }
        }

        if (PauseMenu.isPaused) {
            input.Disable();
        }
        else {
            input.Enable();
        }
    }
}