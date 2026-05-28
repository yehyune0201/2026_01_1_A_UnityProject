using UnityEngine;
using System.Collections.Generic;   


[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueDataSO : ScriptableObject
{
    [Header("캐릭터 정보")]
    public string characterName = "캐릭터";   // 캐릭터 이름
    public Sprite characterImage;             // 캐릭터 초상화

    [Header("대화 내용")]
    [TextArea(3, 10)]                             // 인스펙터 창에서 대화 내용을 여러 줄 입력 가능하게 설정
    public List<string> dialogueLines = new List<string>();   // 대화 내용들
}
