//Manage 4 gate: defines OnCollisionEnter2D, closing gate
using UnityEngine;

public class Gates: MonoBehaviour{
    [SerializeField] private HingeJoint2D hinge;  
    private JointMotor2D motor; //struct

    [SerializeField] private bool isTop = true;
    [SerializeField] private float closeSpeed = 50f; 
    [SerializeField] private float motorF = 1000f; //help fix situation that closing failed
    private Rigidbody2D rb;
    private float restAngle; //original angle

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        //jointAngle at closing state 
        restAngle = hinge.jointAngle;
        //update maxMotorTorque
        motor= hinge.motor;
        motor.maxMotorTorque= motorF;
        hinge.motor= motor;

        //hinge work by f of disk
        hinge.useMotor = false; 
    }

    //Disk --> Gate
    private void OnCollisionEnter2D(Collision2D collision){
        if (!collision.collider.CompareTag("Disk"))
            return;
        //get linearVelocity and update direction v according isTop
        Rigidbody2D diskRb = collision.rigidbody;
        Vector2 v= diskRb.linearVelocity;
        if(isTop)
            v.y = -Mathf.Abs(v.y);
        else
            v.y = Mathf.Abs(v.y);
        diskRb.linearVelocity = v;
    }

    //GameManager call this fun
    public void CloseGate(){
        motor = hinge.motor;
        motor.motorSpeed = isTop ? -closeSpeed : closeSpeed;
        hinge.motor = motor;
        hinge.useMotor = true;
        Invoke(nameof(StopMotor), 0.25f);
    }
    private void StopMotor(){
        hinge.useMotor = false;
        rb.rotation = restAngle;//back to original angle
    }
}
