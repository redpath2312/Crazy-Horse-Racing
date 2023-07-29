using System.Collections;
using UnityEngine;

public class Horse1 : Horse
{
    public override void Movement()
    {
        if (transform.position.x < stopLine.transform.position.x)
        {
            transform.Translate(Vector3.right * baseRunSpeed *0.5f * Time.deltaTime);
            transform.Translate(Vector3.up * baseSwaySpeed * 5 * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 2f), 0);
        }
    }
}
