using UnityEngine;
using UnityEngine.InputSystem;

public class InputMover : MonoBehaviour
{
    [Tooltip("Speed in meters per second")]
    [SerializeField] float speed = 8f;

    [Tooltip("Action that gives the paddle its move input (Y axis only)")]
    [SerializeField] InputAction moveAction;

    [Tooltip("Padding from camera top/bottom")]
    [SerializeField] float padding = 0.5f;

    [SerializeField] Camera cam;

    private void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    private void OnEnable()
    {
        if (moveAction != null)
            moveAction.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null)
            moveAction.Disable();
    }

    private void Update()
    {
        // Read Y axis only (Up = +1, Down = -1)
        float moveY = moveAction.ReadValue<Vector2>().y;

        // Move paddle
        Vector3 movement = Vector3.up * (moveY * speed * Time.deltaTime);
        transform.position += movement;

        ClampInsideCamera();
    }

    private void ClampInsideCamera()
    {
        if (!cam || !cam.orthographic) return;

        float halfHeight = cam.orthographicSize;
        float minY = cam.transform.position.y - halfHeight + padding;
        float maxY = cam.transform.position.y + halfHeight - padding;

        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
