using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventario : MonoBehaviour
{
    public GameObject inventario;
    public GameObject billetes;
    public GameObject cola;
    public GameObject charizard;

    private bool show;
    private bool showBilletes;
    private bool showCola;
    private bool showCharizard;

    // Start is called before the first frame update
    void Start()
    {
        show = false;
        try
        {
            showBilletes = GameController.showBilletes;
        }
        catch (Exception e)
        {
            showBilletes = true;
        }

        try
        {
            showCola = GameController.showCola;
        }
        catch (Exception e)
        {
            showCola = false;
        }

        try
        {
            showCharizard = GameController.showCharizard;
        }
        catch (Exception e)
        {
            showCharizard = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        show = false;
        if (Input.GetKey("i"))
        {
            show = true;
        }

        showBilletes = GameController.showBilletes;
        showCola = GameController.showCola;
        showCharizard = GameController.showCharizard;

        inventario.SetActive(show);
        billetes.SetActive(showBilletes);
        cola.SetActive(showCola);
        charizard.SetActive(showCharizard);
    }

    public bool GetBilletes()
    {
        return showBilletes;
    }

    public bool GetCola()
    {
        return showCola;
    }

    public bool GetCharizard()
    {
        return showCharizard;
    }

    public void SetBilletes(bool status)
    {
        showBilletes = status;
        GameController.showBilletes = showBilletes;
    }

    public void SetCola(bool status)
    {
        showCola = status;
        GameController.showCola = showCola;
    }

    public void SetCharizard(bool status)
    {
        showCharizard = status;
        GameController.showCharizard = showCharizard;
    }
}
