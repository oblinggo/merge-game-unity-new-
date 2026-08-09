using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;

public class ToggleSwitch : MonoBehaviour
{
    [Header("UI 참조")]
    public RectTransform handle;          // 동그라미
    public RectTransform labelRect;       // 텍스트의 RectTransform
    public Image background;              // 배경
    public TextMeshProUGUI labelText;     // 텍스트

    [Header("스프라이트")]
    public Sprite onSprite;
    public Sprite offSprite;

    [Header("텍스트")]
    public string onText = "On";
    public string offText = "Off";

    [Header("위치 설정")]
    public float handleOnX = 30f;         // On일 때 핸들 위치
    public float handleOffX = -30f;       // Off일 때 핸들 위치

    public float labelOnX = -30f;         // On일 때 텍스트 위치 (핸들 반대쪽)
    public float labelOffX = 30f;         // Off일 때 텍스트 위치 (핸들 반대쪽)

    [Header("애니메이션")]
    public float duration = 0.25f;
    public Ease easeType = Ease.OutBack;

    [Header("상태")]
    public bool isOn = false;

    public UnityEvent<bool> onValueChanged;

    private bool isAnimating = false;

    void Start()
    {
        SetState(isOn, true);
    }

    public void OnClick()
    {
        if (isAnimating) return;

        isOn = !isOn;
        SetState(isOn, false);
        onValueChanged?.Invoke(isOn);
    }

    public void SetState(bool value, bool instant = false)
    {
        isOn = value;
        isAnimating = true;

        float targetHandleX = isOn ? handleOnX : handleOffX;
        float targetLabelX = isOn ? labelOnX : labelOffX;
        Sprite targetSprite = isOn ? onSprite : offSprite;
        string targetText = isOn ? onText : offText;

        // 배경 변경
        if (background != null && targetSprite != null)
            background.sprite = targetSprite;

        // 텍스트 내용 변경
        if (labelText != null)
            labelText.text = targetText;

        if (instant)
        {
            handle.anchoredPosition = new Vector2(targetHandleX, handle.anchoredPosition.y);
            if (labelRect != null)
                labelRect.anchoredPosition = new Vector2(targetLabelX, labelRect.anchoredPosition.y);
            isAnimating = false;
        }
        else
        {
            // 핸들 이동
            handle.DOAnchorPosX(targetHandleX, duration).SetEase(easeType);

            // 텍스트 위치 이동
            if (labelRect != null)
            {
                labelRect.DOAnchorPosX(targetLabelX, duration)
                    .SetEase(easeType)
                    .OnComplete(() => isAnimating = false);
            }
            else
            {
                isAnimating = false;
            }
        }
    }

    public void SetIsOn(bool value)
    {
        if (isOn == value) return;
        isOn = value;
        SetState(isOn, false);
        onValueChanged?.Invoke(isOn);
    }
}