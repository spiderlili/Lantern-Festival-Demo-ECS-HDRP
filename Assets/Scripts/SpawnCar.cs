using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    public GameObject carPf;
    public GameObject cameraPf;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = new Vector3(10, 10, 10);
            GameObject carInstance = Instantiate(carPf, pos, Quaternion.identity);

            //get the camera to follow the car
            cameraPf.GetComponent<SmoothFollow>().target = carInstance.transform;
        }
    }
}