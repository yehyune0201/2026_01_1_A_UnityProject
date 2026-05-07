using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x, y;
    public DraggableRank currentRank; //현재칸 계급장
    public SpriteRenderer cellRenderers;

    private void Awake()
    {
        cellRenderers = GetComponent<SpriteRenderer>(); //셀의 SpriteRenderer 가져오기
    }


    public void Initialize(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
        name = "Cell_" + x + "_" + y;  //이름 설정
    }

    public bool IsEmpty()
    {
        return currentRank == null; //계급장이 없으면 빈칸
    }

    public bool ContainsPosition(Vector3 position)
    {
       Bounds bounds = cellRenderers.bounds; //셀의 경계 가져오기
        return bounds.Contains(position); //포지션이 셀의 경계 안에 있는지 확인
    }

    public void SetRank(DraggableRank rank)
    {
        currentRank = rank; //현재 계급장 설정

        if (rank != null)
        {
            rank.currentCell = this; //계급장의 현재 셀 설정
        }
        rank.originalPosition = new Vector3(transform.position.x, transform.position.y, 0);
        rank.transform.position = new Vector3(transform.position.x, transform.position.y, 0); //계급장 위치를 셀 위치로 이동
    }








    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
