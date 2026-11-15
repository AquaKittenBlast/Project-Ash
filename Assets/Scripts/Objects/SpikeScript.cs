using UnityEngine;

public class SpikeScript : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponentInParent<PlayerMovement>();
            player.ToSpawn();
        }
    }
}
