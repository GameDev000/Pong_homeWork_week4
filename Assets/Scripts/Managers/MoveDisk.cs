//Reset disk+ first move + update move in special cases. 
//SetLastPlayerHit(PlayerSide side) method
using UnityEngine;

public class MoveDisk: MonoBehaviour{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float startSpeed = 6f;
    [SerializeField] private float minSpeed = 6f;
    public enum PlayerSide {None, Left, Right}
    public PlayerSide LastPlayerHit = PlayerSide.None;

    private void Start(){
        Reset();
        FirstMove();
    }

    public void Reset(){
        rb.linearVelocity = Vector2.zero;
        rb.position = Vector2.zero;
        LastPlayerHit = PlayerSide.None;
    }

    public void FirstMove(){
        float directionX = Random.value < 0.5f ? -1 : 1;
        float directionY = Random.Range(-3f, 3f);
        Vector2 direction = new Vector2(directionX, directionY).normalized;
        rb.linearVelocity = direction * startSpeed;
    }

    private void FixedUpdate(){
        Vector2 v = rb.linearVelocity;
        //case disk stop
        if (v == Vector2.zero){
            float directionX = Random.value < 0.5f ? -1 : 1;
            float directionY = Random.Range(-1f, 1f);
            rb.linearVelocity = new Vector2(directionX, directionY).normalized * minSpeed;
            return;
        }
        //case slow
        if (v.magnitude < minSpeed)
            rb.linearVelocity = v.normalized * minSpeed;
    }

    public void SetLastPlayerHit(PlayerSide side){
        LastPlayerHit= side;
    }
}
