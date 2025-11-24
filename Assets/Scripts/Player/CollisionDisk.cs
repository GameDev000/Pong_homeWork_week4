//Update disk move after OnCollisionEnter2D with player + update LastPlayerHit (MoveDisk())
using UnityEngine;

public class CollisionDisk: MonoBehaviour{
    [SerializeField] private bool isLeftPlayer; //for inspector
    [SerializeField] private float hitSpeed = 7f;       
    [SerializeField] private float yFactor = 0.5f; 
    [SerializeField] private MoveDisk disk;         

    //OnCollisionEnter2D player-disk
    private void OnCollisionEnter2D(Collision2D collision){
        if (!collision.collider.CompareTag("Disk"))
            return;

        Rigidbody2D diskRb = collision.rigidbody;
        
        //change direction of X
        float directionX = isLeftPlayer ? 1f : -1f; 
       
        //direction of Y by hit
        float diskY= collision.transform.position.y;
        float playerY = transform.position.y;
        float difference = diskY- playerY;
        float offset= (diskY- playerY)* yFactor; 
        
        //update move of disk
        Vector2 direction = new Vector2(directionX, offset).normalized;
        diskRb.linearVelocity = direction * hitSpeed;

        //Update LastPlayerHit() (MoveDisk())
        if(disk != null)
            {disk.SetLastPlayerHit(isLeftPlayer ? MoveDisk.PlayerSide.Left: MoveDisk.PlayerSide.Right);
        }    
    }
}
