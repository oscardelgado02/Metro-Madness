using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneControllerTransbordoAzulVerde : MonoBehaviour
{
    private bool closeEnoughDoor;
    private float detectionRange;

    private bool closeEnoughNPC3;
    private bool closeEnoughNPC4;

    private bool finishNPC3;
    private bool finishNPC4;
    private int optionNPC3; //Option Q: Value 1 - Option E: Value 2
    private int optionNPC4;

    public GameObject player;
    public GameObject puerta;
    public GameObject contorno;
    public GameObject pressF;
    public NPC_Obligated NPC3;
    public NPC_Obligated NPC4;

    public PlayerController pController;

    public Animator anim_NPC3;

    public GameObject happy_text_NPC3;
    public GameObject happy_text_NPC4;

    public SpriteRenderer fade;

    public Text clock;
    public Image clock_color;

    // Start is called before the first frame update
    void Start()
    {
        detectionRange = 2.0f;

        pController.remordimiento = GameController.remordimiento;
        pController.tiempo = GameController.tiempo;
    }

    // Update is called once per frame
    void Update()
    {
        GameController.remordimiento = pController.getRemordimiento();
        GameController.tiempo = pController.getTiempo();

        if (GameController.min < 10)
        {
            clock.text = GameController.hora.ToString() + ":0" + GameController.min.ToString();
        }
        else
        {
            clock.text = GameController.hora.ToString() + ":" + GameController.min.ToString();
        }

        closeEnoughDoor = false;
        closeEnoughNPC3 = false;
        closeEnoughNPC4 = false;


        if (Vector2.Distance(player.transform.position, puerta.transform.position) <= detectionRange)
        {
            closeEnoughDoor = true;
        }

        closeEnoughNPC3 = NPC3.GetCloseEnough();
        closeEnoughNPC4 = NPC4.GetCloseEnough();

        ChangeColor();
        ShowDoor();
        ShowF();
        FinishNPC();
        ExitMetro();
        ChangeNPC();
    }

    void ShowDoor()
    {
        contorno.SetActive(closeEnoughDoor);
    }

    void ShowF()
    {
        pressF.SetActive(closeEnoughDoor);
    }

    void FinishNPC()
    {
        finishNPC3 = NPC3.GetFinishInteraccion();
        finishNPC4 = NPC4.GetFinishInteraccion();

        if (optionNPC3 != 3)
        {
            optionNPC3 = NPC3.GetOptionSelected();
        }

        if (optionNPC4 != 3)
        {
            optionNPC4 = NPC4.GetOptionSelected();
        }
    }

    void ExitMetro()
    {
        if (closeEnoughDoor && Input.GetKey("f"))
        {
            SceneManager.LoadScene("Vagon_Verde");
        }
    }

    void ChangeColor()
    {
        if ((GameController.hora == 9) && (GameController.min >= 0))
        {
            clock_color.color = new Color32(0, 255, 20, 100);
        }

        if ((GameController.hora == 9) && (GameController.min >= 30))
        {
            clock_color.color = new Color32(168, 255, 0, 100);
        }

        if ((GameController.hora == 10) && (GameController.min >= 0))
        {
            clock_color.color = new Color32(255, 253, 0, 100);
        }

        if ((GameController.hora == 10) && (GameController.min >= 30))
        {
            clock_color.color = new Color32(255, 169, 0, 100);
        }

        if (GameController.hora == 11)
        {
            clock_color.color = new Color32(255, 34, 0, 100);
        }
    }

    void ChangeNPC()
    {
        if (Input.GetKey("a") || Input.GetKey("d"))
        {
            happy_text_NPC3.active = false;
            happy_text_NPC4.active = false;
        }

        if (optionNPC3 == 2)
        {
            StartCoroutine(ScreenFade());
            optionNPC3 = 3;
        }

        if (optionNPC4 == 1)
        {
            NPC4.MoveLeftNPC();
        }

        if (optionNPC4 == 2)
        {
            StartCoroutine(ScreenFade());
            optionNPC4 = 3;
        }
    }

    public IEnumerator ScreenFade()
    {
        for (float f = 0.0f; f < 1.1f; f += 0.05f)
        {
            Color color = new Color(0, 0, 0, 0);
            color.a = f;
            fade.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        for (float f = 0.0f; f < 1.0f; f += 0.05f)
        {
            if (optionNPC3 == 3)
            {
                anim_NPC3.SetTrigger("Happy");
            }

            yield return new WaitForSeconds(0.05f);
        }

        for (float f = 1.0f; f > 0.0f; f -= 0.05f)
        {
            if (optionNPC3 == 3)
            {
                happy_text_NPC3.active = true;
            }
            else if (optionNPC4 == 3)
            {
                happy_text_NPC4.active = true;
            }

            Color color = new Color(0, 0, 0, 0);
            color.a = f;
            fade.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
