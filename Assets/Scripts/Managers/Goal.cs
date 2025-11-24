using UnityEngine;

//OnTriggerEnter2D disk-Goal and call score methods in gameManager
public class Goal : MonoBehaviour
{
    [SerializeField] private bool RightGoal;
    [SerializeField] private GameManager gameManager; //methods of score

    private void OnTriggerEnter2D(Collider2D other)
    {
        //check Tag of Disk
        if (!other.CompareTag("Disk"))
            return;

        //update score after trigger
        if (!RightGoal)
            gameManager.ScoreRightPlayer();
        else if (RightGoal)
            gameManager.ScoreLeftPlayer();
    }
}
