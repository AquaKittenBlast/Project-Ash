using UnityEngine;

public class GroundColliderHandler : MonoBehaviour
{
    private PlayerMovement ash;
    private Rigidbody2D rb;
    private float groundNormalThreshold = 0.5f;


    void Awake()
    {
        ash = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > groundNormalThreshold)
                {
                    ash.SetGrounded(true);
                    return;
                }
                if (contact.normal.x == 1 || contact.normal.x == -1)
                {
                    ash.ledgePop();
                    return;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ash.SetGrounded(false);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            bool isGrounded = false;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > groundNormalThreshold)
                {
                    isGrounded = true;
                    break;
                }
            }    

            if (rb.linearVelocityY <= 0.1f && isGrounded) { ash.SetGrounded(true);}
            else { ash.SetGrounded(false); }   
        }
    }
}
