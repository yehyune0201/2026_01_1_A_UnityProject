using UnityEngine;

public class DraggableRank : MonoBehaviour
{
    public int rankLevel = 1; //랭크의 값
    public float dreagSpeed = 10f; //드래그 속도
    public float snapBackSpeed = 20f; //스냅 임계값


    public bool isDragging = false; //드래그 중인지 여부
    public Vector3 originalPosition; //원래 위치 저장
    public GridCell currentCell; //현재 위치한 셀

    public Camera mainCamera; //메인 카메라 참조
    public Vector3 dragOffset; //드래그 시작 시 마우스와의 오프셋
    public SpriteRenderer spriteRenderer;
    public RankGameManager gameManager;

    private void Awake()
    {
        mainCamera = Camera.main; //메인 카메라 참조
        spriteRenderer = GetComponent<SpriteRenderer>(); //스프라이트 렌더러 참조
        gameManager = FindAnyObjectByType<RankGameManager>(); //게임 매니저 참조
    
    }



    void Start()
    {
        originalPosition = transform.position; //원래 위치 저장

    }


    void Update()
    {
        if(isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + dragOffset; //목적지 계산
            transform.position = Vector3.Lerp(transform.position, targetPosition, dreagSpeed * Time.deltaTime); //부드럽게 이동
        }
        else if(transform.position != originalPosition && currentCell != null)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, snapBackSpeed * Time.deltaTime); //원래 위치로 부드럽게 이동
        }
    }
    void StartDragging()
    {
        isDragging = true; //드래그 시작
        dragOffset = transform.position - GetMouseWorldPosition(); //드래그 시작 시 마우스와의 오프셋 계산
        spriteRenderer.sortingOrder = 10; //드래그 중인 계급장을 다른 오브젝트보다 위에 렌더링
    }

    public void MoveToCell(GridCell targetCell)
    {
        if (currentCell != null)
        {
            currentCell.currentRank = null; //현재 셀에서 계급장 제거
        }

        currentCell = targetCell; //현재 셀 업데이트
        targetCell.currentRank = this; //타겟 셀에 계급장 설정

        originalPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0f); //원래 위치 업데이트
        transform.position = originalPosition; //계급장 위치 업데이트
    }

    public void ReturnToOriginalPosition()
    {
       transform.position = originalPosition; //계급장 위치를 원래 위치로 이동
    }
    public void MergeWithCell(GridCell targetCell)
    {
        if(targetCell.currentRank == null || targetCell.currentRank.rankLevel != rankLevel)
        {
            ReturnToOriginalPosition();
            return; //원래 위치로 돌아감
        }
        if(currentCell != null)
        {
           currentCell.currentRank = null; //현재 셀에서 계급장 제거
        }
        gameManager.MergeRanks(this, targetCell.currentRank); //게임 매니저에서 병합 처리
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition; //마우스 스크린 좌표
        mousePos.z = -mainCamera.transform.position.z; //카메라에서의 거리 설정
        return mainCamera.ScreenToWorldPoint(mousePos); //마우스 월드 좌표 반환
    }

    public void SetRankLevel(int level)
    {
        rankLevel = level; //랭크 레벨 설정

        if(gameManager != null && gameManager. rankSprites. Length > level - 1)
        {
            spriteRenderer.sprite = gameManager.rankSprites[level - 1]; //랭크 레벨에 맞는 스프라이트 설정
        }

    }

    void StopDragging()
    {
        isDragging = false; //드래그 종료
        spriteRenderer.sortingLayerID = 1; //렌더링 순서 초기화
        GridCell targetCell = gameManager.FindClosestCell(transform.position); //가장 가까운 셀 찾기

        if (targetCell != null)
        {
            if(targetCell.currentRank == null)
            {
                MoveToCell(targetCell); //빈 셀로 이동
            }
            else if (targetCell.currentRank != null && targetCell.currentRank.rankLevel == rankLevel)
            {
                MergeWithCell(targetCell); //병합 시도
            }
            else
            {
                ReturnToOriginalPosition(); //원래 위치로 돌아감
            }
        }
        else
        {
            ReturnToOriginalPosition(); //원래 위치로 돌아감
        }
    }

    private void OnMouseDown()
    {
        StartDragging(); //마우스 클릭 시 드래그 시작
    }

    private void OnMouseUp()
    {
        if(!isDragging) return; //드래그 중이 아니면 무시
        StopDragging(); //마우스 릴리스 시 드래그 종료
    }


}
