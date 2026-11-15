using UnityEngine;

public class MusicianScript : MonoBehaviour
{
    Animator animator;
    public bool canPlay = true;
    public float animStart;
    public float animStop;

    public static float audioTime;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (animStart <= audioTime && audioTime <= animStop) { canPlay = true; }
        else { canPlay = false; }
    }

    public void SetPlayingState(bool isPlaying)
    {
        animator.SetBool("playing", isPlaying);
        animator.SetBool("canPlay", canPlay);
    }
}
