using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Horse1 : Horse
{
    public override void Movement()
    {
        base.Movement();
        //if (transform.position.x < stopLine.transform.position.x)
        //{
        //    transform.Translate(Vector3.right * baseRunSpeed *1.1f * Time.deltaTime);
        //    transform.Translate(Vector3.up * baseSwaySpeed * 5 * Time.deltaTime);
        //    transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 2f), 0);
        //}
    }
}
