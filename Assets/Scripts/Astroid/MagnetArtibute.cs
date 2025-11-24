using UnityEngine;

public class MagnetArtibute : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D DiskRb;

    [Header("Magnet Settings")]
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private float magnetForce = 21f;
    //[SerializeField] private float deadRadius = 1f; //to stop magnet
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void FixedUpdate()
    {
        if (!DiskRb)
        {
            return;
        }
        Vector2 dir = (Vector2)(transform.position - DiskRb.transform.position);
        float distance = dir.magnitude;

        if (distance > magnetRadius)
        {
            return; 
        }
        dir /= distance;
        float strengthFactor = 1f - Mathf.Clamp01(distance / magnetRadius);
        Vector2 force = dir * magnetForce * strengthFactor;
        DiskRb.AddForce(force, ForceMode2D.Force);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}
