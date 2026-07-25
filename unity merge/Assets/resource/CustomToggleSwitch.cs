using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro
using DG.Tweening;

public class CustomToggleSwitch : MonoBehaviour
{
    [Header("UI References (Hierarchy에서 연결)")]
    [SerializeField] private Button switchButton;          // 스위치 전체 버튼
    [SerializeField] private RectTransform knobTransform;   // 움직이는 노브 (손잡이)
    [SerializeField] private GameObject fillObject;        // 만들어두신 Fill 오브젝트 (또는 Image)

    [Header("Text References (Hierarchy에서 연결)")]
    [SerializeField] private GameObject onTextObject;  // Hierarchy의 On 텍스트 오브젝트
    [SerializeField] private GameObject offTextObject; // Hierarchy의 Off 텍스트 오브젝트

    [Header("Positions (X-Axis)")]
    [SerializeField] private float offXPosition = -40f;    // OFF일 때 노브 X 좌표
    [SerializeField] private float onXPosition = 40f;      // ON일 때 노브 X 좌표

    [Header("Animation Settings")]
    [SerializeField] private float duration = 0.25f;       // 노브 이동 속도
    [SerializeField] private Ease easeType = Ease.OutBack; // 노브 튕김 효과

    [Header("State")]
    [SerializeField] private bool isOn = false; // 시작 시 상태 (ON/OFF)

    private Sequence toggleSequence;

    public bool IsOn => isOn;

    private void Awake()
    {
        if (switchButton == null)
            switchButton = GetComponent<Button>();

        if (switchButton != null)
        {
            switchButton.onClick.AddListener(OnClickSwitch);
        }
    }

    private void Start()
    {
        SetStateImmediate(isOn);
    }

    private void OnClickSwitch()
    {
        Toggle(!isOn);
    }

    public void Toggle(bool targetState)
    {
        isOn = targetState;

        toggleSequence?.Kill();

        float targetX = isOn ? onXPosition : offXPosition;

        toggleSequence = DOTween.Sequence();

        // 1. 노브 이동 (노브만 부드럽게 움직임)
        toggleSequence.Join(knobTransform.DOAnchorPosX(targetX, duration).SetEase(easeType));

        // 2. Fill, On Text, Off Text는 잔상 없이 즉시 온/오프 (SetActive)
        UpdateVisualsInstant(isOn);
    }

    private void SetStateImmediate(bool targetState)
    {
        isOn = targetState;

        // 노브 위치 즉시 설정
        float targetX = isOn ? onXPosition : offXPosition;
        knobTransform.anchoredPosition = new Vector2(targetX, knobTransform.anchoredPosition.y);

        // 시각 요소 즉시 설정
        UpdateVisualsInstant(isOn);
    }

    // 잔상 없이 즉시 켜고 끄는 함수
    private void UpdateVisualsInstant(bool state)
    {
        if (fillObject != null)
            fillObject.SetActive(state);

        if (onTextObject != null)
            onTextObject.SetActive(state);

        if (offTextObject != null)
            offTextObject.SetActive(!state);
    }
}