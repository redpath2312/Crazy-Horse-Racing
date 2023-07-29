using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
    public bool isPlayer;
    public float baseRunSpeed, minRunSpeed, maxRunSpeed, currentSpeed, baseSwaySpeed, minSwaySpeed,maxSwaySpeed;

    [SerializeField]
    WaitForSeconds speedChangeInterval = new WaitForSeconds(0.5f);


    public GameObject stopLine;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunSpeed());
        StartCoroutine(SwaySpeed());

        stopLine = GameObject.Find("Stop Line");

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 2; 
        //Could use this to to put horses near bottom of screen as rendered first

    }

    // Update is called once per frame
    void Update()
    {
        Movement();        
    }

    public virtual void Movement()
    {
        if (transform.position.x < stopLine.transform.position.x)
        {
            transform.Translate(Vector3.right * baseRunSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * baseSwaySpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 2f), 0);
        }

    }

    IEnumerator RunSpeed()
    {
        while(true)
        {
            yield return (speedChangeInterval);
            float runSpeedRange = Random.Range(minRunSpeed, maxRunSpeed);
            baseRunSpeed = runSpeedRange;
        }
    }

    IEnumerator SwaySpeed()
    {
        while (true)
        {
            yield return (speedChangeInterval);
            float speedRange = Random.Range(minSwaySpeed, maxSwaySpeed);
            baseSwaySpeed = speedRange;
        }
    }
}
