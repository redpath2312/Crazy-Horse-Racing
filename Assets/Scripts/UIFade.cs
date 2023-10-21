using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIFade : MonoBehaviour
{
    public static UIFade instance;
    public float fadeSpeed;
    public bool shouldFadeToBlack;
    public bool shouldFadeFromBlack;

    [SerializeField]
    Image fadeBackground;

   
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeToBlack)
        {
            fadeBackground.color = new Color(fadeBackground.color.r, fadeBackground.color.g, fadeBackground.color.b, Mathf.MoveTowards(fadeBackground.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeBackground.color.a == 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        if (shouldFadeFromBlack)
        {
            fadeBackground.color = new Color(fadeBackground.color.r, fadeBackground.color.g, fadeBackground.color.b, Mathf.MoveTowards(fadeBackground.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeBackground.color.a == 0)
            {
                shouldFadeFromBlack = false;
            }
        }
    }

    void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }
    void FadeFromBlack()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }

    public void Fade()
    {
        FadeToBlack();
        Invoke("FadeFromBlack", 1.5f);
    }


}
