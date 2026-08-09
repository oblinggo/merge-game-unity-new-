using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;

public class ToggleSwitch : MonoBehaviour
{
    [Header("UI 참조")]
    public RectTransform handle;
    public RectTransform labelRect;
    public Image background;
    public TextMeshProUGUI labelText;

    [Header("스프라이트")]
    public Sprite onSprite;
    public Sprite offSprite;

    [Header("텍스트")]
    public string onText = "On";
    public string offText = "Off";
    public Color onTextColor = Color.white;                          // On일 때 텍스트 색
    public Color offTextColor = new Color(0.35f, 0.22f, 0.12f);      // Off일 때 어두운 색

    [Header("위치 설정")]
    public float handleOnX = 23f;
    public float handleOffX = -16.4f;
    public float labelOnX = -10f;
    public float labelOffX = 16f;

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
        Color targetTextColor = isOn ? onTextColor : offTextColor;

        // 배경 이미지 변경
        if (background != null && targetSprite != null)
            background.sprite = targetSprite;

        // 텍스트 내용 + 색상 + 위치 바로 변경
        if (labelText != null)
        {
            labelText.text = targetText;
            labelText.color = targetTextColor;
        }

        if (labelRect != null)
            labelRect.anchoredPosition = new Vector2(targetLabelX, labelRect.anchoredPosition.y);

        if (instant)
        {
            handle.anchoredPosition = new Vector2(targetHandleX, handle.anchoredPosition.y);
            isAnimating = false;
        }
        else
        {
            handle.DOKill();
            handle.DOAnchorPosX(targetHandleX, duration)
                .SetEase(easeType)
                .OnComplete(() => isAnimating = false);
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