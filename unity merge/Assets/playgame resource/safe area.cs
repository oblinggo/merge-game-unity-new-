using UnityEngine;

[ExecuteAlways] // 에디터 편집 모드에서도 스크립트가 동작하도록 설정
[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea = Rect.zero;
    private Vector2Int lastScreenSize = Vector2Int.zero;
    private ScreenOrientation lastOrientation = ScreenOrientation.Unknown;

    void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update()
    {
        // 화면 해상도, 방향, Safe Area 변경 시 실시간 업데이트
        if (lastSafeArea != Screen.safeArea ||
            lastScreenSize.x != Screen.width ||
            lastScreenSize.y != Screen.height ||
            lastOrientation != Screen.orientation)
        {
            ApplySafeArea();
        }
    }

    // 인스펙터 값이 변경되거나 에디터 상태가 바뀔 때 실행
    private void OnValidate()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    public void ApplySafeArea()
    {
        if (rectTransform == null) return;

        Rect safeArea = Screen.safeArea;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (screenWidth <= 0 || screenHeight <= 0) return;

        // 앵커 비율 계산 (0.0 ~ 1.0)
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= screenWidth;
        anchorMin.y /= screenHeight;
        anchorMax.x /= screenWidth;
        anchorMax.y /= screenHeight;

        // Anchor 적용
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        // 상태 기록
        lastSafeArea = safeArea;
        lastScreenSize = new Vector2Int(Screen.width, Screen.height);
        lastOrientation = Screen.orientation;
    }
}