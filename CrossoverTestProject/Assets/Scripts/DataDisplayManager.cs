using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataDisplayManager : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI _textMeshPro;

    [SerializeField]
    protected float _clearTimer = 10f;

    protected Coroutine _countdownTimer = null;

    public void SetText(string s)
    {
        _textMeshPro.SetText(s);
        if (null != _countdownTimer)
        {
            StopTimer();
        }
        _countdownTimer = StartCoroutine(Countdown());
    }

    protected IEnumerator Countdown()
    {
        float timer = _clearTimer;
        while(timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
        }

        _textMeshPro.SetText("");
        StopTimer();
    }

    private void StopTimer()
    {
        StopCoroutine(_countdownTimer);
        _countdownTimer = null;
    }
}
