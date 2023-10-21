using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Horse : MonoBehaviour
{
    public bool isPlayer, isSprinting, isRacing, isTired, isExhausted; //isRacing is when race has started, not finished and (health) not dead.
    public float baseRunSpeed, minRunSpeed, maxRunSpeed, currentSpeed,sprintModifier, baseSwaySpeed, minSwaySpeed,maxSwaySpeed,staminaDrain,maxStamina, currentStamina;

    [SerializeField]
    WaitForSeconds speedChangeInterval = new WaitForSeconds(0.5f);


    public GameObject stopLine;
    SpriteRenderer spriteRenderer;

    private bool routineStarted;
    private float exhaustedTime =1f;

    private Vector2 raceManagerPosition; //Reference for where the Race Manager object is for purposes of race build up.
    private Vector2 position;

    // Start is called before the first frame update
    void Start()
    {
        stopLine = GameObject.Find("Stop Line");

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 2;
        //Could use this to to put horses near bottom of screen as rendered first

        currentStamina = maxStamina;
        sprintModifier = 1f;
        RaceManager.instance.OnParadeStart += ParadeStart;
        RaceManager.instance.OnParadeEnd += ParadeEnd;
        RaceManager.instance.OnRaceStart += RaceStartedTest;



        raceManagerPosition = RaceManager.instance.position; //This is temporary, it doesnt update
        position = new Vector3(raceManagerPosition.x, gameObject.transform.position.y);
        gameObject.transform.position = position;

    }

    // Update is called once per frame
    void Update()
    {
        if (RaceManager.instance.isRaceStarted)
        {

            Movement();
            SprintCheck();

        }
        if (RaceManager.instance.IsParading)
        {
            ParadeMovement();
        }
        
    }

    private void SprintCheck()
    {
        if (isPlayer)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                currentStamina = currentStamina - staminaDrain * Time.deltaTime;

                if (currentStamina < 0 && !isExhausted)
                {
                    {
                        isTired = true;
                        isSprinting = false;
                        StartCoroutine(IsExhausted());
                    }
                }

                else if (!isSprinting && !isTired)
                {
                    IsSprinting();
                }
                else return;
            }
            else
            {
                if (isSprinting)
                {
                    //Debug.Log("Stopped Sprinting");
                    isSprinting = false;
                    sprintModifier = 1f;
                }
                else return;
            }
        }
        else

        {
            if (isSprinting)
            currentStamina = currentStamina - staminaDrain * Time.deltaTime;
        }



    }
    private bool IsSprinting()
    {

        //Debug.Log("Sprinting is called");
        sprintModifier = 1.75f;
        return isSprinting = true;

    }
    IEnumerator IsExhausted()
    {
        isExhausted = true;
        currentStamina = 0;
        sprintModifier = 0.25f;
        yield return new WaitForSeconds(exhaustedTime);
        sprintModifier = 1f;
        isExhausted= false;
    }

    IEnumerator AISprint()
    {
        yield return new WaitForSeconds(1f);
        ////is sprinting for random number of times so that total 

        for (int i = 0; i < 4; i++)
        {
            //Debug.Log("AISprint started on " + gameObject.name);
            IsSprinting();
            yield return new WaitForSeconds(1f);
            //Debug.Log("AISprint ended on " + gameObject.name);
            isSprinting = false;
            sprintModifier = 1f;
            //Debug.Log(i + " done");
            yield return new WaitForSeconds(0.75f);
        }

    }

    public virtual void Movement()
    {
        //current speed = base run speed *sprint modifier * bonus modifier
        if (transform.position.x < stopLine.transform.position.x)
        {
            transform.Translate(Vector3.right * baseRunSpeed *sprintModifier* Time.deltaTime);
            transform.Translate(Vector3.up * baseSwaySpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 2f), 0);
        }
        else if (transform.position.x >= stopLine.transform.position.x) //or dead and change isRacing to be false to finish line
        {
            isRacing= false;
        }

    }
    void InitialRoutines()
    {
        if (!routineStarted)
        {
            routineStarted = true;
            //Debug.Log("Initial Coroutine Kicked Off");
            StartCoroutine(RunSpeed());
            StartCoroutine(SwaySpeed());

            if (!isPlayer)
            {
                StartCoroutine(AISprint());
            }
        }
        else return;

    }

    IEnumerator RunSpeed()
    {
        while(true)
        {
            yield return (speedChangeInterval);
            float runSpeedRange = UnityEngine.Random.Range(minRunSpeed, maxRunSpeed);
            baseRunSpeed = runSpeedRange;
        }
    }

    IEnumerator SwaySpeed()
    {
        while (true)
        {
            yield return (speedChangeInterval);
            float swaySpeedRange = UnityEngine.Random.Range(minSwaySpeed, maxSwaySpeed);
            baseSwaySpeed = swaySpeedRange;
        }
    }

    void ParadeStart(object sender, EventArgs e)
    {
        Debug.Log("Parade Event Started");
        RaceManager.instance.OnParadeStart -= ParadeStart;
    }
    void ParadeMovement()
    {
        transform.Translate(Vector3.right * RaceManager.instance.paradeSpeed * Time.deltaTime);
    }

    void ParadeEnd(object sender, EventArgs e)
    {
        Debug.Log("Parade Event Ended");
        RaceManager.instance.OnParadeEnd -= ParadeEnd;

       if (gameObject.transform.position.x < RaceManager.instance.transform.position.x)
        {
            Debug.Log("GO less than raceMgr pos");
            gameObject.transform.position = new Vector3(RaceManager.instance.transform.position.x, transform.position.y, 0);
        }
    }

    void RaceStartedTest(object sender, EventArgs e)
    {
        Debug.Log("Race Started Event On " + gameObject.name);
        isRacing = true;
        RaceManager.instance.OnRaceStart -= RaceStartedTest;
        InitialRoutines();
    }
}
