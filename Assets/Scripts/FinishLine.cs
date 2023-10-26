using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private string horseName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Horse"))
        {
            Debug.Log(other.gameObject.name + " has finished the race");
            horseName = other.gameObject.name;
            RaceManager.instance.RaceFinishNames(horseName);
        }
    }
    //Set up events for race finish, and then have horse target speed change as crosses the line.
}
