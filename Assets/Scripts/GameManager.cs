using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public EventHandler OnHorsePicked;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            UIFade.instance.Fade();
            Invoke("HorsePicked", 1.5f);

        }
        if (Input.GetKeyDown(KeyCode.R))
        { 
            RaceManager.instance.isRaceStarted= true;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(0);
        }
    }

    void HorsePicked()
    {
        if (OnHorsePicked != null)
        {
            OnHorsePicked(this, EventArgs.Empty);
        }
    }
}
