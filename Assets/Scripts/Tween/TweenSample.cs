using UnityEngine;
using DG.Tweening;
using TMPro;


public class TweenSample : MonoBehaviour
{
    [Header("효과를 위한 UI/Object 타겟")]
    public RectTransform UITarget; // UI 타겟
    public GameObject ObjectTarget; //오브젝트 타겟

    [Header("글자 연출 타겟")]
    public TMP_Text countText;
    public int currentValue = 0;
    public int addValue = 100;

    private int targetValue;

    [Header("색 변형 연출 타겟")]
    public Color flashColor = Color.yellow;

    private Color originalColor;
    [Header("페이드 UI 그룹")]
    public CanvasGroup fadeTarget;
   

    void Start()
    {
        countText.text = currentValue.ToString(); // 초기값을 텍스트에 설정

        originalColor= countText.color; // 원래 색상 저장

        fadeTarget.alpha = 0f; // 초기 투명도 설정
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayPunchUIScale(); // 스페이스 키를 누르면 펀치 스케일 효과 재생
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayPunchObjectScale(); // 알파벳 2 키를 누르면 오브젝트 펀치 스케일 효과 재생
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayUIShake(); // 알파벳 3 키를 누르면 UI 흔들림 효과 재생
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayCountUp(); // 알파벳 4 키를 누르면 카운트 업 효과 재생
        }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayColorFlash(); // 알파벳 5 키를 누르면 색 변형 효과 재생
        }

        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlayFade(); // 알파벳 6 키를 누르면 페이드 효과 재생
        }
    }
    public void PlayPunchUIScale()
    {
        if (UITarget == null) return; // UITarget이 할당되지 않은 경우 함수 종료
        {
            UITarget.DOKill(); // UITarget에 적용된 모든 트윈 효과 제거
            UITarget.localScale = Vector3.one; // UI 타겟의 스케일 초기화
            UITarget.DOPunchScale(Vector3.one * 0.3f, 0.25f, 8, 1.0f); // UI 타겟에 펀치 스케일 효과 적용
        }
    }
    public void PlayPunchObjectScale()
    {
        if (ObjectTarget == null) return; // ObjectTarget이 할당되지 않은 경우 함수 종료
        {
            ObjectTarget.transform.DOKill(); // ObjectTarget에 적용된 모든 트윈 효과 제거
            ObjectTarget.transform.localScale = Vector3.one; // ObjectTarget의 스케일 초기화
            ObjectTarget.transform.DOPunchScale(Vector3.one * 0.3f, 0.25f, 8, 1.0f); // ObjectTarget에 펀치 스케일 효과 적용
        }
    }

    public void PlayUIShake()
    {
        if (UITarget == null) return; // UITarget이 할당되지 않은 경우 함수 종료
        {
            UITarget.DOKill(); // UITarget에 적용된 모든 트윈 효과 제거
            UITarget.DOPunchScale(Vector3.one * 0.3f, 0.20f, 20, 90f);
        }
    }
    public void PlayCountUp()
    {
        if (countText == null) return; // countText가 할당되지 않은 경우 함수 종료
        {
            targetValue += addValue; // 목표값 계산
            DOTween.Kill("CountTween", true); // "CountUp" 태그가 붙은 모든 트윈 효과 제거)

            DOTween.To(
                () => currentValue,
                value =>
                {
                    currentValue = value;
                    countText.text = currentValue.ToString(); // 텍스트 업데이트
                },
                targetValue,
                0.5f
            )
            .SetEase(Ease.OutQuad)
            .SetId("CountTween");

        }
    }

    public void PlayColorFlash()
    {
        if (countText == null) return; // countText가 할당되지 않은 경우 함수 종료
        {
            countText.DOKill(); // countText에 적용된 모든 트윈 효과 제거
            countText.color = originalColor; // 텍스트 색상을 플래시 색상으로 변경
            
            countText.DOColor(flashColor, 0.1f)
                .OnComplete(() =>
                {
                    countText.DOColor(originalColor, 0.2f); // 플래시 색상에서 원래 색상으로 돌아오는 트윈 효과
                });
        }
    }

    public void PlayFade()
    {
        if (fadeTarget == null) return; // fadeTarget이 할당되지 않은 경우 함수 종료
        {
            fadeTarget.DOKill(); // fadeTarget에 적용된 모든 트윈 효과 제거
            fadeTarget.alpha = 0f; // 초기 투명도 설정
        }

        Sequence seq = DOTween.Sequence(); // 시퀀스 생성

        seq.Append(fadeTarget.DOFade(1f, 0.2f)); // 투명도 1로 페이드 인
        seq.AppendInterval(0.5f); // 0.5초 대기
        seq.Append(fadeTarget.DOFade(0f, 0.3f)); // 투명도 0으로 페이드 아웃
    }
}
