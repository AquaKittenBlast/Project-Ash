using UnityEngine;

public class WallColliderHandler : MonoBehaviour
{
    private PlayerMovement ash;
    private Collider2D wallCollider;

    void Awake()
    {
        ash = GetComponentInParent<PlayerMovement>();
        wallCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (wallCollider.offset == new Vector2(0.1709992f, -0.0006413758f))
            {
                ash.SetWalled(true, 1);
            }
            else { ash.SetWalled(true, -1);}
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ash.SetWalled(false, 0);
        }
    }
}
