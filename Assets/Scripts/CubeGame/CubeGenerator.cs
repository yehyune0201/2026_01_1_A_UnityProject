using UnityEngine;

public class CubeGame : MonoBehaviour
{
    public GameObject cubePrefab; 
    public int totalCubes = 10; 
    public float cebeSpacing = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenCube();
    }

    // Update is called once per frame
    public void GenCube()
    {
        Vector3 myPosition = transform.position;

        for (int i = 0; i < totalCubes; i++)
        {
            Vector3 position = new Vector3(myPosition.x, myPosition.y, myPosition.z + (i * cebeSpacing));
            Instantiate(cubePrefab, position, Quaternion.identity);
        }
    }
}
