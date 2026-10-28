using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// STARTER — Player controller for the "Orb Collector" lab goal.
/// Attach to a cube named "Player" (add a Rigidbody, freeze rotation X/Z).
///
/// ALREADY WORKS: WASD movement with the new Input System, frame-rate
/// independent via Time.deltaTime. A Debug.Log heartbeat proves Update runs
/// every frame.
///
/// YOUR LAB TASKS live in the TODO comments. Each TODO uses one function
/// from this week's lecture. Complete them all to finish the game.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Lab wiring (drag your objects here)")]
    [SerializeField] private GameObject orbPrefab;   // TODO 5: used to spawn a bonus orb
    [SerializeField] private Transform cameraTransform; // used by TODO 3 (camera follow)

    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 12f, -8f);

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Debug.Log("[PlayerController] Ready. Collect all orbs to win.");
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Keyboard.current == null)
            return;

        float h = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            h -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            h += 1f;

        float v = 0f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            v -= 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            v += 1f;

        Vector3 movement = new Vector3(h, 0f, v).normalized;

        // Time.deltaTime makes speed identical at any frame rate (10 FPS or 144 FPS).
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    // ---------------------------------------------------------------
    // TODO 1: Log the name of any wall you bump
    // ---------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[OnCollisionEnter] Bumped into: {collision.gameObject.name}");
    }

    // ---------------------------------------------------------------
    // TODO 2: If other is tagged "Orb", destroy the orb and log pickup
    // ---------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            Debug.Log($"[PlayerController] Collected orb: {other.gameObject.name}");
            Destroy(other.gameObject);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.Collect();
            }
        }
    }

    // ---------------------------------------------------------------
    // TODO 3: Move cameraTransform to player position + offset
    // ---------------------------------------------------------------
    private void LateUpdate()
    {
        if (cameraTransform != null)
        {
            cameraTransform.position = transform.position + cameraOffset;
        }
    }

    // ---------------------------------------------------------------
    // TODO 5: When the Player is destroyed, instantiate bonus orb at last position
    // ---------------------------------------------------------------
    private void OnDestroy()
    {
        if (orbPrefab != null)
        {
            Instantiate(orbPrefab, transform.position, Quaternion.identity);
            Debug.Log("[PlayerController] Player destroyed. Spawned bonus orb at last position.");
        }
    }
}
