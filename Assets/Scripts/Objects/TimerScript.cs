using TMPro;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    [SerializeField] GameObject timerPrefab;
    public TextMeshProUGUI timer;
    public static float totalTime;
    public static float time;
    public static bool timerOn = true;
    public static List<float> allTimes = new List<float>();
    public static bool levelOver = false;
    Transform parent;
    

    void Start()
    {
        timer = GetComponent<TextMeshProUGUI>();
        parent = transform.Find("RoomTimers");
    }

    void FixedUpdate()
    {
        if (timerOn)
        {
            if (levelOver) return;
            time += Time.fixedDeltaTime;
            TimeSpan ts = TimeSpan.FromSeconds(time);
            timer.text = ts.ToString(@"mm\:ss\:fff");
        }
    }

    public static void ResetTimer()
    {
        totalTime += time;
        allTimes.Add(time);
        time = 0;
    }

    public void EndScreen()
    {
        levelOver = true;
        TimeSpan ts = TimeSpan.FromSeconds(0);
        timer.text = ts.ToString(@"mm\:ss\:fff");
        StartCoroutine(EndScreenCoroutine());
    }

    private IEnumerator EndScreenCoroutine()
    {
        int roomNumber = 1;
        float posY = 105f;
        float startX = 0f;
        float spacing = 28f;
        float cumulativeTime = 0f;

        yield return new WaitForSeconds(1f);
        foreach (float x in allTimes)
        {
            //Instantiation and declaration
            GameObject entry = Instantiate(timerPrefab, parent, false);
            entry.transform.localScale = Vector3.one;
            TextMeshProUGUI text = entry.GetComponent<TextMeshProUGUI>();
            RectTransform rect = entry.GetComponent<RectTransform>();

            //Makes the time be in a correct format and assigns is
            TimeSpan ts = TimeSpan.FromSeconds(x);
            string timeRightNow = ts.ToString(@"mm\:ss\:fff");
            text.text = $"Room {roomNumber}: {timeRightNow}";

            //Puts it in the right position
            rect.anchoredPosition = new Vector2(startX, posY);
            roomNumber++;
            posY -= spacing;

            cumulativeTime += x;
            TimeSpan ts2 = TimeSpan.FromSeconds(cumulativeTime);
            timer.text = ts2.ToString(@"mm\:ss\:fff");
            yield return new WaitForSeconds(0.5f);
        }
        TimeSpan finalTime = TimeSpan.FromSeconds(totalTime);
        timer.text = finalTime.ToString(@"mm\:ss\:fff");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
