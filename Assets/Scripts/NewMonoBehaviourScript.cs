using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public int Health = 100;   // 체력을 선언한다(변수 정수 표현)
    public float Timer = 1.0f; // 타이머를 설정한다.(변수 실수 표현)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = Health + 100;    //첫 시작 때 100의 체력 추가
    }

    // Update is called once per frame
    void Update()
    {
        Timer = Time.deltaTime;    // 시간 실수를 매 프레임 마다 감소 시킨다.

        if (Timer <= 0)
        {
            Timer = 1.0f;
            Health = Health - 20;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Health = Health + 2;
        }
    }
}
