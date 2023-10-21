using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAcceleration : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public float minSpeed;
    public float maxSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        speed = speed + acceleration* Time.deltaTime;
        if (speed > maxSpeed )
        {
            speed = maxSpeed;
        }
        transform.Translate(Vector3.right * speed *Time.deltaTime);
    }
}
