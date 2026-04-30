using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitType;  // 과일 index 설정 (0: 사과, 1: 바나나, 2: 오렌지 )
    public bool hasMerged = false; // 병합 여부 확인

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMerged)  // 이미 병합된 과일은 무시
            return;
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        if (otherFruit != null && !otherFruit.hasMerged && otherFruit.fruitType == fruitType) // 병합 조건 충족: 같은 종류의 과일이 충돌하고, 둘 다 병합되지 않은 경우
        {
            hasMerged = true;  //합쳐짐 표시
            otherFruit.hasMerged = true;

            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f; // 병합된 과일의 위치 계산

            FruitGame gameManager = FindAnyObjectByType<FruitGame>();

            if (gameManager != null)
            {
                gameManager.MergeFruits(fruitType, mergePosition); // 병합된 과일 생성 요청
            }

            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }

    }
}
