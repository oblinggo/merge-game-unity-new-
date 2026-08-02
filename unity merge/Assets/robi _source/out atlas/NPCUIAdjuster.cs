using UnityEngine;

[ExecuteAlways] // ★ 핵심: 에디터 상태(비플레이 모드)에서도 스크립트가 상시 작동합니다.
public class NPCUIAdjuster : MonoBehaviour
{
    [System.Serializable]
    public struct AspectPreset
    {
        public string name;            // 프리셋 이름 (예: Tablet 4:3)
        public float maxAspectRatio;   // 해당 프리셋이 적용될 최대 가로/세로 비율 (예: 4/3 = 1.34f)
        public Vector2 uiAnchoredPos;  // 해당 비율일 때 UI의 Anchored Position (X, Y)
        public Vector2 uiScale;        // 스케일 조절
    }

    [Header("UI Reference")]
    [SerializeField] private RectTransform npcUIRect; // NPC 머리 위 UI RectTransform

    [Header("Presets (비율 작은 순서대로 설정)")]
    [SerializeField] private AspectPreset tabletPreset = new AspectPreset 
    { 
        name = "4:3 Tablet", 
        maxAspectRatio = 1.4f, 
        uiAnchoredPos = new Vector2(0, 150f), 
        uiScale = Vector2.one 
    };
    
    [SerializeField] private AspectPreset defaultPreset = new AspectPreset 
    { 
        name = "16:9 Default", 
        maxAspectRatio = 2.5f, 
        uiAnchoredPos = new Vector2(0, 200f), 
        uiScale = Vector2.one 
    };

    private float lastAspectRatio = -1f;

    private void Start()
    {
        UpdateUIPosition();
    }

    private void Update()
    {
        // 에디터에서 Game 뷰 해상도 변경 시 실시간 감지
        float currentAspect = GetCurrentAspectRatio();
        if (Mathf.Abs(currentAspect - lastAspectRatio) > 0.01f)
        {
            UpdateUIPosition();
        }
    }

    // Inspector에서 수치(Pos X, Pos Y 등)를 바꿀 때 에디터 화면에 즉시 반영
    private void OnValidate()
    {
        UpdateUIPosition();
    }

    public void UpdateUIPosition()
    {
        if (npcUIRect == null) return;

        float currentAspect = GetCurrentAspectRatio();
        lastAspectRatio = currentAspect;

        if (currentAspect <= tabletPreset.maxAspectRatio)
        {
            ApplyPreset(tabletPreset);
        }
        else
        {
            ApplyPreset(defaultPreset);
        }
    }

    private float GetCurrentAspectRatio()
    {
#if UNITY_EDITOR
        // 에디터 상태에서는 Game View 해상도 창의 가로/세로 비율을 가져옵니다.
        Vector2 gameViewSize = UnityEditor.Handles.GetMainGameViewSize();
        if (gameViewSize.y > 0)
            return gameViewSize.x / gameViewSize.y;
#endif
        return (float)Screen.width / Screen.height;
    }

    private void ApplyPreset(AspectPreset preset)
    {
        npcUIRect.anchoredPosition = preset.uiAnchoredPos;
        npcUIRect.localScale = new Vector3(preset.uiScale.x, preset.uiScale.y, 1f);
    }
}