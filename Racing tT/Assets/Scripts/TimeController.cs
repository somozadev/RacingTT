using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private float timer;
    [SerializeField] private bool isRunning;


    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float countdownCount;
    [SerializeField] private float currentCount;
    [SerializeField] private bool isCounting;

    [ContextMenu("StartTimer")]
    public void StartTimer()
    {
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(UpdateTimer());
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timer = 0f;
        UpdateText();
    }

    [ContextMenu("StartCountdown")]
    public void StartCountdown()
    {
        if (!isCounting)
        {
            isCounting = true;
            currentCount = countdownCount;
            StartCoroutine(UpdateCountdown());
        }
    }
    public void StopCountdown()
    {
        isCounting = false;
    }
    public void ResetCountdown()
    {
        isCounting = false;
    }
    private System.Collections.IEnumerator UpdateCountdown()
    {
        while (isCounting && currentCount > 0f)
        {
            currentCount -= Time.deltaTime;
            UpdateCountdownText();
           yield return null;
        }
        currentCount = 0f;
        UpdateCountdownText();
        isCounting = false;
    }
    private void UpdateCountdownText()
    {
        int seconds = Mathf.FloorToInt(currentCount);
        int milliseconds = Mathf.FloorToInt((currentCount * 1000f) % 1000f);
        string formattedMilliseconds = (milliseconds / 10).ToString("00");
        countdownText.text = string.Format("{0:00}:{1}", seconds, formattedMilliseconds);
    }
    private void UpdateText()
    {
        string minutes = Mathf.FloorToInt(timer / 60f).ToString("00");
        string seconds = Mathf.FloorToInt(timer % 60f).ToString("00");
        int milliseconds = Mathf.FloorToInt((timer * 1000f) % 1000f);
        string formattedMilliseconds = (milliseconds / 10).ToString("00");

        counterText.text = string.Format("{0}:{1}:{2}", minutes, seconds, formattedMilliseconds);
    }

    private System.Collections.IEnumerator UpdateTimer()
    {
        while (isRunning)
        {
            timer += Time.deltaTime;
            UpdateText();
            yield return null;
        }
    }
}