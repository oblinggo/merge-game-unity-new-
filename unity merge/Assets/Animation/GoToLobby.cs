using UnityEngine;
using DG.Tweening;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup introGroup;
    [SerializeField] private CanvasGroup lobbyGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    public void GoToLobby()
    {
        // Intro 페이드 아웃
        introGroup.DOFade(0f, fadeDuration)
            .OnComplete(() =>
            {
                // 페이드 아웃이 끝난 후 화면 전환
                introGroup.gameObject.SetActive(false);
                lobbyGroup.gameObject.SetActive(true);
                lobbyGroup.alpha = 0f; // 시작을 투명하게

                // Lobby 페이드 인
                lobbyGroup.DOFade(1f, fadeDuration);
            });
    }
}