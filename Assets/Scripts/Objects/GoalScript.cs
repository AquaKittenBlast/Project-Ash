using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    [SerializeField] public RoomManager rm;
    [SerializeField] public TransitionScript trans;
    [SerializeField] public TimerScript timer;


    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("GoalHitbox"))
        {
            TimerScript.ResetTimer();
            if (RoomManager.currentRoom < RoomManager.playerSpawns.Count) { RoomManager.currentRoom += 1; rm.AdvanceToNextRoom();  }
            if (RoomManager.currentRoom >= RoomManager.playerSpawns.Count) {
                trans.Transition(true);
                timer.EndScreen();
            }
        }
    }
}
