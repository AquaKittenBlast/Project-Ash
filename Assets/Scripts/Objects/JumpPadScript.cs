using UnityEngine;

public class JumpPadScript : MonoBehaviour
{

    [SerializeField] float PadStrength;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GoalHitbox"))
        {
            PlayerMovement player = collision.GetComponentInParent<PlayerMovement>();
            player.JumpPad(PadStrength);
            Debug.Log("entered");
        }
    }
}
