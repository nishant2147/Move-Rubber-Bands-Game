using System.Collections;
using TMPro;
using UnityEngine;

public class TimerCoundDownScript : MonoBehaviour
{
    public float timeRemaining;
    public TMP_Text timerText;
    private bool isTimerRunning = false;
    private float defaultFontSize;
    private Coroutine textAnimationCoroutine;

    private void Start()
    {
        DisplayTime(timeRemaining);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTimerRunning)
        {
            isTimerRunning = true;
        }

        if (isTimerRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isTimerRunning = false;
                Debug.Log("Time's up!");
                Destroy(gameObject);
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeToDisplay <= 10)
        {
            timerText.color = Color.red;
            if (textAnimationCoroutine == null)
            {
                textAnimationCoroutine = StartCoroutine(AnimateFontSize());
            }
        }
    }
    IEnumerator AnimateFontSize()
    {
        while (timeRemaining <= 10)
        {
            float size = Mathf.Lerp(75, 95, Mathf.PingPong(Time.time * 2, 1));
            timerText.fontSize = size;
            yield return null;
        }
    }
}
