using UnityEngine;

//OnTriggerEnter2D disk-limit top&bottom 
//update score of PlayerSide+ CloseGatesAndResetDisk
public class OutOfLimits : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MoveDisk disk;

    [Header("Gates")]
    [SerializeField] private Gates topR;
    [SerializeField] private Gates topL;
    [SerializeField] private Gates bottomR;
    [SerializeField] private Gates bottomL;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Disk"))
            return;

        switch (disk.LastPlayerHit)
        {
            case MoveDisk.PlayerSide.Left:
                gameManager.UpdateScoreLeft();
                break;

            case MoveDisk.PlayerSide.Right:
                gameManager.UpdateScoreRight();
                break;

            case MoveDisk.PlayerSide.None:
                break;
        }
        CloseGatesAndResetDisk();
    }

    private void CloseGatesAndResetDisk()
    {
        topR.CloseGate();
        topL.CloseGate();
        bottomR.CloseGate();
        bottomL.CloseGate();
        disk.Reset();
        disk.FirstMove();
    }

}
