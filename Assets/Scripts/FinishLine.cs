using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Horse"))
        {
            Debug.Log(other.gameObject.name + " has finished the race");
        }
    }
}
