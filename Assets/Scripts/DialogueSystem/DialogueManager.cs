using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소 - 인스펙터 창에서 연결")]

    public GameObject DialoguePanel;   // 대화창 전체 패널
    public Image characterImage;   // 캐릭터 초상화 이미지
    public TextMeshProUGUI characterNameText;   // 캐릭터 이름 텍스트
    public TextMeshProUGUI dialogueText;   // 대화 내용 텍스트
    public Button nextButton;   // 다음 대화  버튼

    [Header("기본 설정")]
    public Sprite defaultCharacterImage;     // 기본 캐릭터 이미지

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f;       // 타이핑 효과 속도
    public bool skipTypingOnClick = true;   // 타이핑 효과 건너뛰기 여부

    //내부 변수들
    private DialogueDataSO currentDialogue;        // 현재 대화 데이터
    private int currentLineIndex = 0;             // 현재 대화 줄 인덱스
    private bool isDialogueActive = false;        // 대화창 활성화 여부
    private bool isTyping = false;               // 타이핑 효과 진행 중 여부]
    private Coroutine typingCoroutine;          // 타이핑 효과 코루틴 참조


    void Start()
    {
        DialoguePanel.SetActive(false);   // 대화창 숨기기
        nextButton.onClick.AddListener(HandleNextInput);   // 다음 버튼 클릭 이벤트 연결
    }

    // Update is called once per frame
    void Update()
    {
        if(isDialogueActive && Input.GetKeyDown(KeyCode.Space))   // 대화창이 활성화된 상태에서 스페이스바 입력 감지
        {
            HandleNextInput();   // 다음 입력 처리 (타이핑 중이면 완료, 아니면 다음줄 보여주기)
        }
    }

    IEnumerator TypeText(string textToTyep)
    {
        isTyping = true;
        dialogueText.text = "";

        for(int i = 0; i < textToTyep.Length; i++)   
        {
            dialogueText.text += textToTyep[i];            // 한글자씩 추가
            yield return new WaitForSeconds(typingSpeed);  //대기시간 설정
        }
        isTyping = false;
    }

    private void CompleteTyping()     // 타이핑 효과를 즉시 완료하는 함수
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // 코루틴 중지
        }

        isTyping = false;  //타이핑 상태 해제

        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];
        }
    }

    void ShowCurrentLine()
    {
        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            if(typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // 이전 타이핑 코루틴 중지
            }
        }


        string currentText =  currentDialogue.dialogueLines[currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));   // 새로운 타이핑 코루틴 시작
    }
    
    public void ShowNextLine()
    {
        currentLineIndex++;

        //마지막 대화였는지 확인
        if(currentLineIndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();   // 대화가 남아 있으면 다음 대화 보여주기
        }
    }

    void EndDialogue()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // 타이핑 코루틴 중지
            typingCoroutine = null;
        }

        isDialogueActive = false;         // 대화창 비활성화
        isTyping = false;                 // 타이핑 상태 해제
        DialoguePanel.SetActive(false);   // 대화창숨기기
        currentLineIndex = 0;             // 인덱스 초기화
    }

    public void HandleNextInput()
    {
        if(isTyping && skipTypingOnClick)
        {
            CompleteTyping();   // 타이핑 중이면 즉시 완료
        }
        else if(!isTyping)
        {
            ShowNextLine();   // 타이핑이 완료 됬으면 다음 대화 보여주기
        }
    }

    public void SkipDialouge()
    {
        EndDialogue();   // 대화 스킵
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;   // 대화창 진행중인지 여부 확인
    }

    public void StartDialogue(DialogueDataSO dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0)return;   // 대화 데이터가 없거나 대화 내용이 없는 경우 시작하지 않음

        currentDialogue = dialogue;                                    //현재  대화 데이터 설정
        currentLineIndex = 0;                                       // 대화 줄 인덱스 초기화
        isDialogueActive = true;                                   // 대화창 활성화

        //UI 업데이트
        DialoguePanel.SetActive(true);   // 대화창 보이기
        characterNameText.text = dialogue.characterName;   // 캐릭터 이름 표시

        if(characterImage != null)
        {
            if(dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;   // 캐릭터 이미지 사용
            }
            else
            {
                characterImage.sprite = defaultCharacterImage;   // 없으면 기본 이미지 사용
            }
            ShowCurrentLine();   // 첫 번째 대화 줄 표시
        }
    }
}
