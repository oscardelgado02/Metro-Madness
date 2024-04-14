using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_CR : MonoBehaviour
{
    private bool closeEnough;
    private bool activeInteraccion;
    private bool finishInteraccion;
    private bool finishAnimation;

    private float detectionRange;
    private float detectionRangeAnim;

    public int RemordimientoAdded;
    public int RemordimientoMinus;
    public int TimeAdded;

    private int option_selected;

    public GameObject player;
    public PlayerController player_functions;
    public Inventario inv;
    public GameObject contorno;
    public GameObject interaccion;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        activeInteraccion = false;
        finishInteraccion = true;
        finishAnimation = true;

        detectionRange = 2.0f;
        detectionRangeAnim = 7.0f;

        option_selected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (finishInteraccion)
        {
            closeEnough = false;


            if (Vector2.Distance(player.transform.position, transform.position) <= detectionRange)
            {
                closeEnough = true;
            }

            if (Vector2.Distance(player.transform.position, transform.position) <= detectionRangeAnim)
            {
                if (finishAnimation)
                {
                    anim.Play("Desmayo");
                    finishAnimation = false;
                }
            }

            ShowContorno();
            ShowInteraccion();
        }

    }

    public bool GetCloseEnough()
    {
        return closeEnough;
    }

    public bool GetFinishInteraccion()
    {
        return finishInteraccion;
    }

    public int GetOptionSelected()
    {
        return option_selected;
    }

    void ShowContorno()
    {
        contorno.SetActive(closeEnough);
    }

    void ShowInteraccion()
    {
        activeInteraccion = true;


        if (!closeEnough)
        {
            activeInteraccion = false;
        }

        interaccion.SetActive(activeInteraccion);
        ChooseElection();
    }

    void ChooseElection()
    {
        if (activeInteraccion)
        {
            player_functions.SetControl(false);
            // Elección poco moral
            if (Input.GetKey("q"))
            {
                option_selected = 1;
                player_functions.AddRemordimiento(RemordimientoAdded);
                player_functions.SetControl(true);
                finishInteraccion = false;
                contorno.SetActive(false);
                interaccion.SetActive(false);

            }

            // Elección moral

            if (Input.GetKey("e"))
            {
                if (GameController.showCola)
                {
                    TimeAdded = 2;
                    option_selected = 4;
                }
                else
                {
                    option_selected = 2;
                }

                player_functions.MinusRemordimiento(RemordimientoMinus);
                player_functions.AddTiempo(TimeAdded);

                player_functions.SetControl(true);
                finishInteraccion = false;
                contorno.SetActive(false);
                interaccion.SetActive(false);
                if ((GameController.min + TimeAdded) >= 60)
                {
                    int horas = (GameController.min + TimeAdded) / 60;
                    GameController.hora += horas;
                    GameController.min = (GameController.min + TimeAdded) - (60 * horas);
                }
                else
                {
                    GameController.min = GameController.min + TimeAdded;
                }
            }
        }
    }

    public void MoveNPC()
    {
        gameObject.transform.Translate(5f * Time.deltaTime, 0, 0);
    }

    public void MoveLeftNPC()
    {
        gameObject.transform.Translate(-5f * Time.deltaTime, 0, 0);
    }
}
