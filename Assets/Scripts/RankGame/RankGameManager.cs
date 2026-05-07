using System.Collections.Generic;
using UnityEngine;

public class RankGameManager : MonoBehaviour
{
    public int gridWidth = 7; // 가로 크기
    public int gridHeight = 7; //세로 크기
    public float CellSize = 1.3f; // 칸 크기
    public GameObject cellPrefab; //빈칸 프리팹
    public Transform gridController;

    public GameObject rankPrefabs; //계급장 프리팹
    public Sprite[] rankSprites; //계급장 스프라이트 배열
    public int maxRankLevel = 7; //최대 계급장 레벨
    
    public GridCell[,] grid; //그리드 셀 배열

    private void InitializeGrid()
    {
        grid = new GridCell[gridWidth, gridHeight]; //그리드 배열 초기화

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                 Vector3 position = new Vector3
                 (
                    x * CellSize - (gridWidth * CellSize / 2) + CellSize / 2, //X 위치 계산
                    y * CellSize - (gridHeight * CellSize / 2) + CellSize / 2, //Y 위치 계산
                    1f
                 ) ;

                GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity, gridController); //셀 프리팹 인스턴스화
                GridCell cell = cellObj.AddComponent<GridCell>(); //GridCell 컴포넌트 가져오기
                cell.Initialize(x, y); //셀 초기화

                grid[x, y] = cell; //그리드 배열에 셀 저장
            }
           
        }

    }
    public DraggableRank CreateRankInCell(GridCell cell, int level)
    {
        if (cell == null || !cell. IsEmpty()) return null;

        level = Mathf.Clamp(level, 1, maxRankLevel); //레벨 범위 제한

        Vector3 rankPosition = new Vector3(cell.transform.position.x, cell.transform.position.y, 0f); //계급장 위치 설정
        
        GameObject rankObj = Instantiate(rankPrefabs, rankPosition, Quaternion.identity, gridController); //계급장 프리팹 인스턴스화

        rankObj.name = "Rank_Level_" + level; //계급장 이름 설정

        DraggableRank rank = rankObj.AddComponent<DraggableRank>(); //DraggableRank 컴포넌트 가져오기

        rank.SetRankLevel(level); //계급장 레벨 설정
        cell.SetRank(rank); //셀에 계급장 설정

        return rank; 
    }

    private GridCell FineEmptyCell()
    {
        List<GridCell> emptyCells = new List <GridCell>();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].IsEmpty())
                {
                    emptyCells.Add(grid[x, y]); //빈칸 리스트에 추가
                }
            }
        }
        if (emptyCells.Count == 0)
        {
            return null; //빈칸이 없으면 null 반환
        }

        return emptyCells[Random.Range(0, emptyCells.Count)]; //랜덤으로 빈칸 선택

    }

    public bool SpawnNewRank()
    {
        GridCell emptyCell = FineEmptyCell(); //빈칸 찾기

        if(emptyCell == null) return false; //빈칸이 없으면 스폰 실패

        int rankLevel = Random.Range(0, 100) < 80 ? 1 : 2; //랜덤으로 계급장 레벨 선택 (1 또는 2)

        CreateRankInCell(emptyCell, rankLevel); //빈칸에 계급장 생성

        return true;

    }

    public GridCell FindClosestCell(Vector3 position)
    {
        for(int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].ContainsPosition(position))
                {
                    return grid[x, y]; //포지션이 포함된 셀 반환
                }
            }
        }
        GridCell closesCell = null;
        float clossesDistance = float.MaxValue;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                float distance = Vector3.Distance(position, grid[x, y].transform.position); //포지션과 셀 위치 간의 거리 계산
                
                if (distance < clossesDistance)
                {
                    clossesDistance = distance; //가장 가까운 거리 업데이트
                    closesCell = grid[x, y]; //가장 가까운 셀 업데이트
                }
            }
        }
        if(clossesDistance > CellSize * 2)
        {
            return null; //멀리 있으면 null 반환
        }
        return closesCell;
    }

    public void RemoveRank(DraggableRank rank)
    {
        if (rank == null) return;

        if (rank.currentCell != null)
        {
            rank.currentCell.currentRank = null; //계급장이 위치한 셀에서 계급장 제거
        }
        Destroy(rank.gameObject); //계급장 오브젝트 제거
    }

    public void MergeRanks(DraggableRank draggedRank,  DraggableRank targetRank)
    {
       if(draggedRank == null || targetRank == null || draggedRank.rankLevel != targetRank.rankLevel)
       {
            if(draggedRank != null) draggedRank.ReturnToOriginalPosition(); //드래그된 계급장 원래 위치로 이동
            return;
       }

       int newLevel = targetRank.rankLevel + 1; //새로운 계급장 레벨 계산
        if (newLevel > maxRankLevel)
        {
            RemoveRank(draggedRank); //드래그된 계급장 제거
            return;
        }
        targetRank.SetRankLevel(newLevel); //타겟 계급장 레벨 업데이트
        RemoveRank(draggedRank); //드래그된 계급장 제거
    }





    void Start()
    {
        InitializeGrid();

        for (int i = 0; i < 4; i++) // 4개 생성
        {
            SpawnNewRank(); //초기 계급장 스폰
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            SpawnNewRank(); //D키를 눌러 새로운 계급장 스폰
        }
    }
}
