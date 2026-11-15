using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class TransitionScript : MonoBehaviour
{
    [SerializeField] public float spotToGo;    
    [SerializeField] public float speed = 2f;
    [SerializeField] public float pauseAtTarget = 0.2f;
    SpriteRenderer sr;

    private bool isTransitioning = false;

    public void Start()
    {
        GameObject child = GameObject.Find("EndScreen");
        sr = child.GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    public void Transition(bool x)
    {
        if (!isTransitioning)
            StartCoroutine(TransitionCoroutine(x));
    }

    private IEnumerator TransitionCoroutine(bool isEndTransition)
    {
        isTransitioning = true;
        if (isEndTransition)
        {
            sr.enabled = true;
        }

        Vector3 startLocal = transform.localPosition;

        Vector3 targetLocal = startLocal;
        targetLocal.x = spotToGo;


        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(startLocal, targetLocal, t);
            yield return null;
        }

        if (!isEndTransition)
        {
            transform.localPosition = startLocal;
        }

        isTransitioning = false;
    }
}

