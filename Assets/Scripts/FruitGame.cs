using UnityEngine;

public class FruitGame : MonoBehaviour
{
    public GameObject[] fruitPerfabs; // 과일 프리팹 배열
    public float[] fruitSize = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f}; // 과일 크기 배열 (사과, 바나나, 오렌지)
    
    public GameObject currentFruit; // 현재 생성된 과일
    public int currentFruitType; // 현재 과일 종류 (0: 사과, 1: 바나나, 2: 오렌지)

    public float furuitStartHeigt = 6.0f; // 과일이 생성되는 높이
    public float gameWidth = 6.0f; // 과일 낙하 속도
    public bool isGameOver = false; // 게임 오버 여부
    public Camera mainCamera; // 메인 카메라 참조

    public float furitTimer;
  


    void Start()
    {
        mainCamera = Camera.main;
        SpawnnewFruit();
        furitTimer = -3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        if (furitTimer >= 0)
        {
            furitTimer -= Time.deltaTime;
        }
        if(furitTimer < 0 && furitTimer > -2)
        {
            SpawnnewFruit();
            furitTimer = -3.0f;
        }

        if (currentFruit != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 newPosition = currentFruit.transform.position;

            newPosition.x = worldPosition.x;

            float halfFuruitSize = fruitSize[currentFruitType] / 2f;

            if (newPosition.x < -gameWidth / 2 - halfFuruitSize)
            {
                newPosition.x = -gameWidth / 2 - halfFuruitSize;
            }
            else if (newPosition.x > gameWidth / 2 + halfFuruitSize)
            {
                newPosition.x = gameWidth / 2 + halfFuruitSize;
            }
            currentFruit.transform.position = newPosition;
        }

        if (Input.GetMouseButtonDown(0) && furitTimer == -3.0f)        // 마우스 좌클릭시 과일 드롭
        {
            DropFruit();             
        }
    }

    void SpawnnewFruit()
    {
        if(!isGameOver)
        {
            currentFruitType = Random.Range(0, 3);

            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 spawnPosition = new Vector3(worldPosition.x, furuitStartHeigt, 0f);

            float halfFuruitSize = fruitSize[currentFruitType] / 2f;

            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -gameWidth/ 2 + halfFuruitSize, gameWidth/ 2- halfFuruitSize);

            currentFruit = Instantiate(fruitPerfabs[currentFruitType], spawnPosition, Quaternion.identity);
            currentFruit.transform.localScale = new Vector3(fruitSize[currentFruitType], fruitSize[currentFruitType], 1);

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0.0f; // 과일이 낙하하도록 중력 설정
            }
        }
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1.0f;
            currentFruit = null;
            furitTimer = 1.0f;
        }
    }
    public void MergeFruits(int fruitType,Vector3 positone)
    {
        if(fruitType < fruitSize.Length - 1)
        {
            GameObject newFruit = Instantiate(fruitPerfabs[fruitType + 1], positone, Quaternion.identity);
            newFruit.transform.localScale = new Vector3(fruitSize[fruitType + 1], fruitSize[fruitType + 1], 1.0f);

        }
    }
}
