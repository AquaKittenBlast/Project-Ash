using UnityEngine;

public class AfterImageScript : MonoBehaviour
{
    public float fadeTime = 0.5f;
    private SpriteRenderer sr;
    private Color originalColor;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(originalColor.a, 0, timer / fadeTime);
        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (timer >= fadeTime)
        {
            Destroy(gameObject);
        }
    }
}
