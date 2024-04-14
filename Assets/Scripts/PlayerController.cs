using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private bool underControl;
    public bool muted;

    private Animator anim;
    private AudioSource audio;

    public int remordimiento;
    public int tiempo;

    void Start()
    {
        underControl = true;
        muted = true;
        anim = GetComponentInChildren<Animator>();
        audio = GetComponentInChildren<AudioSource>();
        try
        {
            remordimiento = GameController.remordimiento;
        }
        catch (Exception e)
        {
            remordimiento = 0;
        }

        try
        {
            tiempo = GameController.tiempo;
        }
        catch (Exception e)
        {
            tiempo = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        muted = true;

        if (underControl)
        {
            if (Input.GetKey("a"))
            {
                muted = false;
                gameObject.transform.Translate(-5f * Time.deltaTime, 0, 0);
                anim.SetTrigger("Izquierda");
            }

            if (Input.GetKey("d"))
            {
                muted = false;
                gameObject.transform.Translate(5f * Time.deltaTime, 0, 0);
                anim.SetTrigger("Derecha");
            }

            PassTime();
        }

        audio.mute = muted;
        ChangeFace();
    }

    public int getRemordimiento()
    {
        return remordimiento;
    }

    public int getTiempo()
    {
        return tiempo;
    }

    public void AddRemordimiento(int num)
    {
        remordimiento = remordimiento + num;
    }

    public void AddTiempo(int num)
    {
        tiempo = tiempo + num;
    }

    public void MinusRemordimiento(int num)
    {
        remordimiento = remordimiento - num;
        if(remordimiento <= 0)
        {
            remordimiento = 0;
        }
    }

    public void SetControl(bool cond)
    {
        underControl = cond;
    }

    public void ChangeFace()
    {
        if (Input.GetKey("n"))
        {
            anim.SetTrigger("TimePass");
        }
        else
        {
            if (remordimiento >= 0 && remordimiento < 50)
            {
                anim.SetTrigger("Good");
            }

            if (remordimiento >= 50 && remordimiento < 100)
            {
                anim.SetTrigger("Mid");
            }

            if (remordimiento >= 100)
            {
                anim.SetTrigger("Bad");
            }
        }        
    }

    public void PassTime()
    {
        if (Input.GetKey("n"))
        {
            Time.timeScale = 10;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
