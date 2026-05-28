using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    public DialogueDataSO myDialogue;
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>();

        if(dialogueManager == null)
        {
            Debug.Log("다이얼 로그 매니저가 없습니다.");
        }
    }

    private void OnMouseDown()
    {
        if(dialogueManager == null) return;              // 매니저 없으면 실행 안함
        if(dialogueManager.IsDialogueActive()) return;   // 대화창이 이미 활성화된 상태라면 실행 하지 않음
        if (myDialogue == null) return;                  // 대화 데이터가 없으면 실행 하지 않음

        dialogueManager.StartDialogue(myDialogue);   //모든 조건 만족 시 대화 시작
    }
}
