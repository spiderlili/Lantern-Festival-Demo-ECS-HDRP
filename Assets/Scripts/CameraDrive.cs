using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//from unity documentation: https://docs.unity3d.com/ScriptReference/Input.GetAxis.html - only good if you have an object sitting on the horizontal axis

public class CameraDrive : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float rotationSpeed = 100.0f;

    void Update()
    {
        // get the horizontal & vertical axis. by default: mapped to arrow keys. value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it smoother: move 10 meters per second instead of 10 meters per frame
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis (forward moving axis of the object)
        transform.Translate(0, 0, translation);

        // Rotate around our y-axis (up axis of the object)
        transform.Rotate(0, rotation, 0);
    }
}