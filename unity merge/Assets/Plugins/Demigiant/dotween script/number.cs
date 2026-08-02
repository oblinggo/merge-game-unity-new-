using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Globalization;

public class NumberTypewriterScramble : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public long finalNumber = 100000000;
    public float duration = 2.5f;
    public float scrambleTime = 0.1f;
    public float restartDelay = 1.0f; // 한 번 끝난 후 다시 시작하기 전 대기 시간

    private Sequence mainSequence;

    void Start()
    {
        PlayNumberTypewriter();
    }

    void PlayNumberTypewriter()
    {
        string finalStr = finalNumber.ToString("N0", CultureInfo.InvariantCulture); // "100,000,000"

        mainSequence?.Kill();
        mainSequence = DOTween.Sequence();

        float timePerChar = duration / finalStr.Length;
        string currentText = "";

        for (int i = 0; i < finalStr.Length; i++)
        {
            char targetChar = finalStr[i];
            string nextText = currentText + targetChar;

            if (targetChar == ',')
            {
                string captured = nextText;
                mainSequence.AppendCallback(() => tmpText.text = captured);
                mainSequence.AppendInterval(timePerChar * 0.25f);
            }
            else
            {
                string capturedCurrent = currentText;
                string capturedFinal = nextText;

                mainSequence.AppendCallback(() =>
                {
                    DOTween.To(() => 0f, x =>
                    {
                        char randomDigit = (char)('0' + Random.Range(0, 10));
                        tmpText.text = capturedCurrent + randomDigit;
                    }, 1f, scrambleTime)
                    .OnComplete(() =>
                    {
                        tmpText.text = capturedFinal;
                    });
                });

                mainSequence.AppendInterval(scrambleTime + timePerChar * 0.5f);
            }

            currentText = nextText;
        }

        // 끝난 후 잠시 대기하고 다시 처음부터 반복
        mainSequence.AppendInterval(restartDelay);
        mainSequence.SetLoops(-1); // 무한 반복
    }

    void OnDestroy()
    {
        mainSequence?.Kill();
    }
}