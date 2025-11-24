using UnityEngine;

public class AsteroidMoverDVD : MonoBehaviour
{

[SerializeField] private float speed = 5f;
private Rigidbody2D rb;

 private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //save the ridgit body comp
    }

    private void Start()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f),Random.Range(-1f, 1f)); //random 2d vector
        if (dir == Vector2.zero)
        {
            dir = Vector2.right; //move right if zero
        }
            

        rb.linearVelocity = dir.normalized * speed; //set the rigdit velocity
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed; //keep moving at the same speed
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    
}

