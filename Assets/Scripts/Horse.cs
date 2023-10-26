using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Horse : MonoBehaviour
{
    public bool isPlayer, isSprinting, isRacing, isTired, isExhausted; //isRacing is when race has started, not finished and (health) not dead.
    public float baseRunSpeed, minRunSpeed, maxRunSpeed, accelerationFactor, baseAccelerationFactor, targetSpeed, currentSpeed,sprintModifier, baseSwaySpeed, minSwaySpeed,maxSwaySpeed,staminaDrain,maxStamina, currentStamina;

    [SerializeField]
    WaitForSeconds speedChangeInterval = new WaitForSeconds(1.5f);

    public GameObject stopLine;
    SpriteRenderer spriteRenderer;

    private bool routineStarted;
    private float exhaustedTime =1f;

    private Vector2 raceManagerPosition; //Reference for where the Race Manager object is for purposes of race build up.
    private Vector2 position;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = 0;
        accelerationFactor = baseAccelerationFactor;
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
            SpeedCheck();
            SprintCheck();
            Movement();
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
                if (!isTired)

                {
                    IsSprinting(true);
                }

                if (currentStamina <= 0 && !isExhausted)
                {
                    {
                        isTired = true;
                        IsSprinting(false);
                        StartCoroutine(IsExhausted());
                    }
                }
                else return;
            }
            else
            {
                if (isSprinting)
                {
                    Debug.Log("stopped sprinting");
                    IsSprinting(false);
                }
                else return;
            }

        }
        else if (isSprinting)
        {
            currentStamina = currentStamina - staminaDrain * Time.deltaTime;

            if (currentStamina <= 0 && !isExhausted)
            {
                {
                    isTired = true;
                    IsSprinting(false);
                    StopCoroutine(AISprint());
                    StartCoroutine(IsExhausted());
                }
            }
            else return;
        }
    }
    private bool IsSprinting(bool isSprintingBool)
    { 
        if (isSprintingBool == true)
        {
            targetSpeed = 1.75f * baseRunSpeed;
            accelerationFactor = 3 * baseAccelerationFactor;
            currentStamina = currentStamina - staminaDrain * Time.deltaTime;
            return isSprinting = true;
        }

        else
        {
            targetSpeed = baseRunSpeed;
            accelerationFactor = baseAccelerationFactor;
            Debug.Log("Bool IsSprinting stopped on " + gameObject.name);
            return isSprinting = false;
        }
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
        while (!isExhausted)
        {
            yield return new WaitForSeconds(1f);
            ////is sprinting for random number of times so that total 

            for (int i = 0; i < 4; i++)
            {

                //Debug.Log("AISprint started on " + gameObject.name);
                IsSprinting(true);
                yield return new WaitForSeconds(1f);
                //Debug.Log("AISprint ended on " + gameObject.name);
                IsSprinting(false);
                //Debug.Log(i + " done");
                yield return new WaitForSeconds(1.5f);
            }
        }

    }

    public virtual void Movement()
    {
        //current speed = base run speed *sprint modifier * bonus modifier
        if (transform.position.x < stopLine.transform.position.x)
        {
            transform.Translate(Vector3.right * currentSpeed *sprintModifier* Time.deltaTime);
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


    void SpeedCheck()
    {
        if (targetSpeed < currentSpeed)
        {
            currentSpeed -= accelerationFactor * Time.deltaTime;
            if (currentSpeed <= targetSpeed)
            {
                currentSpeed = targetSpeed;
            }
        }

        else if (targetSpeed > currentSpeed)
        {
            currentSpeed += accelerationFactor * Time.deltaTime;
            if (currentSpeed >= targetSpeed)
            {
                currentSpeed = targetSpeed;
            }
        }

        if (!isSprinting && !isExhausted)
        {
            targetSpeed = baseRunSpeed;
        }

    }

    IEnumerator RunSpeed()
    {
        //Horse starts on Speed = 0
        //Target speed is random range (runspeedrange)
        //speed = current speed + acceleration factor *Time.delta time
        //If current speed >= target speed
        //current speed = target speed
        //when Interval has passed change target speed
        //if current speed < target speed
        //current speed = current speed + acceleration factor
        //else if current speed > target speed
        //current speed = current speed - acceleration factor

        while(true)
        {
            yield return (speedChangeInterval);
            float runSpeedRange = UnityEngine.Random.Range(minRunSpeed, maxRunSpeed);
            baseRunSpeed = runSpeedRange;
        }
    }

    IEnumerator SwaySpeed()
    {
        while (isRacing)
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
            //Debug.Log("GO less than raceMgr pos");
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
