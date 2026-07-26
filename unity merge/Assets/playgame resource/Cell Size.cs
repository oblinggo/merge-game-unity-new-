using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ResponsiveGrid : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public RectTransform boardRect;

    public int columns = 10; // 가로 칸 수
    public int rows = 6;    // 세로 칸 수

    void Update()
    {
        if (gridLayout == null || boardRect == null) return;

        // 보드의 스케일(Scale) 변화까지 감지하여 실제 픽셀 너비 계산
        Vector3 worldScale = boardRect.lossyScale;

        if (worldScale.x == 0 || worldScale.y == 0) return;

        // 스케일 변화에 맞춰 내부 렌더링 영역 크기 구하기
        float width = boardRect.rect.width;
        float height = boardRect.rect.height;

        // Padding과 Spacing을 적용한 Cell Size 계산
        float totalSpacingX = gridLayout.spacing.x * (columns - 1);
        float totalSpacingY = gridLayout.spacing.y * (rows - 1);

        float totalPaddingX = gridLayout.padding.left + gridLayout.padding.right;
        float totalPaddingY = gridLayout.padding.top + gridLayout.padding.bottom;

        float cellWidth = (width - totalPaddingX - totalSpacingX) / columns;
        float cellHeight = (height - totalPaddingY - totalSpacingY) / rows;

        // 최종 계산된 Cell Size 할당
        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }
}