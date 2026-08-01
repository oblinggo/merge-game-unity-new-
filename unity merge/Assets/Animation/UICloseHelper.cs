using UnityEngine;

public class UICloseHelper : MonoBehaviour
{
    public void Hide()
    {
        // 부모 오브젝트를 끈다 (검은 배경 + 배너 UI 둘 다 사라짐)
        transform.parent.gameObject.SetActive(false);
    }
}