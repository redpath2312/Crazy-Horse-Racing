using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager instance;
    //Variables/Objects - Entrance Point, Race Start Point, Race Finish Line point, Race Horse Stop Point, End Point
    //Bools Parading, Ready, Racing, Finishing, Stopped

    //Horse has been selected
    //Horses are parading  down to the start for the race , set number of distance, configurable speed
    //Horse stops at start line
    //Press Race to Start
    //send events to subscribe to listen for race starting?
    public bool isHorsePicked;
    public bool isRaceStarted;
    public event EventHandler OnRaceStart;
    public event EventHandler OnParadeStart;
    public event EventHandler OnParadeEnd;
    public float paradeSpeed = 10f;
    [SerializeField]
    List<string> horseNames = new List<string>();

    [SerializeField]
    GameObject entrance, startLine, finishLine;



    private Vector2 target;
    public Vector2 position;


    private bool isParading;
    //Property? Horses Get Properties, Game Manager Sets.
    public bool IsParading
    {
        get
        {
            return isParading;
        }
        //set
        //{
        //    if (isHorsePicked && !isRaceStarted)
        //        isParading = true;
        //    else if (isRaceStarted)
        //    {
        //        isParading = false;
        //    }
        //}
    }

    private void Start()
    {
        instance = this;
        //Setting RaceManagers Object position to the entrance and the target being the start line to act as the Parade point as horses come in
        gameObject.transform.position = entrance.transform.position;
        position = gameObject.transform.position;
        target = startLine.transform.position;

        GameManager.instance.OnHorsePicked += StartParading;
    }

    private void Update()
    {
        if (isRaceStarted)
        {
            if (OnRaceStart != null)
            {
                OnRaceStart(this, EventArgs.Empty);
                isParading= false;
            }
        }

        if (isParading)
        {
            float step = paradeSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target, step);

            //if Horses are still in Parading
            //Press S to Skip

            if (Input.GetKeyDown(KeyCode.S))
            {
                UIFade.instance.Fade();
                Invoke("StartParadingSkip", 1.5f);
            }
        }
        if (transform.position.x >= target.x)
        {
            isParading = false;
            if (OnParadeEnd != null)
            {
                OnParadeEnd(this, EventArgs.Empty);
            }
        }
    }
    private void StartParading(object sender, EventArgs e)
    {
        Debug.Log("Started Parading");
        isHorsePicked = true;
        isParading = true;
        GameManager.instance.OnHorsePicked -= StartParading;

        if (OnParadeStart!= null)
        {
            OnParadeStart(this, EventArgs.Empty);
        }
    }
    
    void StartParadingSkip()
    {
        transform.position = target;
        isParading = false;

        if (OnParadeEnd != null)
        {
            OnParadeEnd(this, EventArgs.Empty);
        }
    }
    public void RaceFinishNames(string horseName)
    {
        horseNames.Add(horseName);
    }
}

