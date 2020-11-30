using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCubes : MonoBehaviour
{
    public GameObject cubePf;
    public int rows = 10;
    public int columns = 10;
    public float perlinMultiplier = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < columns; z++)
            {
                GameObject cubePfInstance = Instantiate(cubePf);
                //generate 2D perlin noise between 0.0 - 1.0
                Vector3 pos = new Vector3(x, Mathf.PerlinNoise(x * perlinMultiplier, z * perlinMultiplier), z);
                cubePfInstance.transform.position = pos;
            }
        }
    }
}