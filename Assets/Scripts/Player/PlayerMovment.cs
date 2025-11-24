using Unity.Burst.CompilerServices;
using UnityEngine;


public class PlayerMovment : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode upKey = KeyCode.W; //initial setting
    [SerializeField] private KeyCode downKey = KeyCode.S;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float padding = 1f;

    [Header("Camera")]
    [SerializeField] private Camera cam;

    private void Awake() //set the main camera as a bound, awake runs before reset vars.
    {
        if (!cam) cam = Camera.main;
    }

    private void Update()
    {
        float dir = 0f;
        if (Input.GetKey(upKey)) dir += 1f;
        if (Input.GetKey(downKey)) dir -= 1f;

        transform.position += Vector3.up * (dir * speed * Time.deltaTime);
        ClampInsideCamera();
    }

    private void ClampInsideCamera()
    {
        if (!cam || !cam.orthographic) return; //2D Game

        float halfHeight = cam.orthographicSize;
        float minY = cam.transform.position.y - halfHeight + padding;
        float maxY = cam.transform.position.y + halfHeight - padding;

        Vector3 p = transform.position;
        p.y = Mathf.Clamp(p.y, minY, maxY); //set bound
        transform.position = p;
    }

    public void SetKeys(KeyCode up, KeyCode down)
    {
        upKey = up;
        downKey = down;
    }
}

