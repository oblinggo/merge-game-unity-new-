using UnityEngine;
using DG.Tweening;

public class ButtonPulse : MonoBehaviour
{
    [Header("대상")]
    public Transform target;

    [Header("스케일 설정")]
    public float scaleUp = 1.1f;
    public float scaleDuration = 0.2f;

    [Header("로테이션 설정")]
    public float rotationAngle = 5f;
    public float rotationDuration = 0.2f;

    [Header("반복 설정")]
    public float interval = 0.5f;
    public int loops = -1;

    private Sequence pulseSequence;

    void Start()
    {
        if (target == null) target = transform;
        PlayPulse();
    }

    public void PlayPulse()
    {
        pulseSequence?.Kill();
        pulseSequence = DOTween.Sequence();

        // 1단계: 커지면서 Z축 -5도 회전 (동시)
        pulseSequence.Append(target.DOScale(scaleUp, scaleDuration).SetEase(Ease.OutQuad));
        pulseSequence.Join(target.DOLocalRotate(new Vector3(0, 0, -rotationAngle), rotationDuration).SetEase(Ease.OutQuad));

        // 2단계: 원래 크기로 돌아가면서 Z축 +5도 회전 (동시) → 둥근 느낌
        pulseSequence.Append(target.DOScale(1f, scaleDuration).SetEase(Ease.InOutQuad));
        pulseSequence.Join(target.DOLocalRotate(new Vector3(0, 0, rotationAngle), rotationDuration).SetEase(Ease.InOutQuad));

        // 3단계: 회전을 0으로 정리 (선택사항, 더 깔끔하게)
        pulseSequence.Append(target.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.OutQuad));

        // 대기 후 반복
        pulseSequence.AppendInterval(interval);
        pulseSequence.SetLoops(loops);
    }

    public void StopPulse()
    {
        pulseSequence?.Kill();
        if (target != null)
        {
            target.localScale = Vector3.one;
            target.localRotation = Quaternion.identity;
        }
    }

    void OnDestroy()
    {
        pulseSequence?.Kill();
    }
}