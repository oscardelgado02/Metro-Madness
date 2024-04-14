using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneControllerTransbordoVerdeLila : MonoBehaviour
{
    private bool closeEnoughDoor;
    private float detectionRange;

    private bool closeEnoughNPC8;
    private bool closeEnoughNPC9;
    private bool closeEnoughNPC10;

    private bool finishNPC8;
    private bool finishNPC9;
    private bool finishNPC10;
    private int optionNPC8; //Option Q: Value 1 - Option E: Value 2
    private int optionNPC9;
    private int optionNPC10;

    public GameObject player;
    public GameObject puerta;
    public GameObject contorno;
    public GameObject pressF;
    public NPC_Obligated NPC8;
    public NPC_CR NPC9;
    public NPC_Obligated NPC10;

    public PlayerController pController;

    public Animator anim_NPC8;
    public Animator anim_NPC9;
    public Animator anim_NPC10t1;
    public Animator anim_NPC10t2;
    public Animator anim_NPC10t3;

    public GameObject movil_tik;
    public Animator movil_tiktok;

    public GameObject happy_text_NPC8;
    public GameObject happy_text_NPC9;
    public GameObject happy_text_NPC9_v2;
    public GameObject happy_text_NPC10;

    public SpriteRenderer fade;

    public Text clock;
    public Image clock_color;

    public AudioSource golpes;
    public AudioSource movil_roto;

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
        closeEnoughNPC8 = false;
        closeEnoughNPC9 = false;
        closeEnoughNPC10 = false;


        if (Vector2.Distance(player.transform.position, puerta.transform.position) <= detectionRange)
        {
            closeEnoughDoor = true;
        }

        closeEnoughNPC8 = NPC8.GetCloseEnough();
        closeEnoughNPC9 = NPC9.GetCloseEnough();
        closeEnoughNPC10 = NPC10.GetCloseEnough();

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
        finishNPC8 = NPC8.GetFinishInteraccion();
        finishNPC9 = NPC9.GetFinishInteraccion();
        finishNPC10 = NPC10.GetFinishInteraccion();

        if (optionNPC8 != 3)
        {
            optionNPC8 = NPC8.GetOptionSelected();
        }

        if (optionNPC9 != 3 && optionNPC9 != 5)
        {
            optionNPC9 = NPC9.GetOptionSelected();
        }

        if (optionNPC10 != 3)
        {
            optionNPC10 = NPC10.GetOptionSelected();
        }
    }

    void ExitMetro()
    {
        if (closeEnoughDoor && Input.GetKey("f"))
        {
            SceneManager.LoadScene("Vagon_Lila");
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
            happy_text_NPC8.active = false;
            happy_text_NPC9.active = false;
            happy_text_NPC9_v2.active = false;
            happy_text_NPC10.active = false;
        }

        if (optionNPC8 == 1)
        {
            anim_NPC8.SetTrigger("Paliza");
            golpes.Play();
            optionNPC8 = 3;
        }

        if(golpes.time > 2.0f)
        {
            golpes.Stop();
        }

        if (optionNPC8 == 2)
        {
            StartCoroutine(ScreenFade());
            optionNPC8 = 3;
        }

        if (optionNPC9 == 2)
        {
            StartCoroutine(ScreenFade());
            optionNPC9 = 3;
        }
        else if (optionNPC9 == 4)
        {
            StartCoroutine(ScreenFade());
            optionNPC9 = 5;
        }

        if (optionNPC10 == 1)
        {
            anim_NPC10t1.SetTrigger("Movil");
            anim_NPC10t2.SetTrigger("Movil");
            anim_NPC10t3.SetTrigger("Movil");
            movil_tik.active = true;
            movil_tiktok.SetTrigger("Movil");
            movil_roto.time = 1.5f;
            movil_roto.Play();
            optionNPC10 = 3;
        }

        if (optionNPC10 == 2)
        {
            StartCoroutine(ScreenFade());
            optionNPC10 = 3;
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
            if (optionNPC8 == 3)
            {
                anim_NPC8.SetTrigger("Happy");
            }

            if (optionNPC9 == 3 || optionNPC9 == 5)
            {
                anim_NPC9.SetTrigger("Happy");
            }

            if (optionNPC10 == 3)
            {
                anim_NPC10t1.SetTrigger("Happy");
                anim_NPC10t2.SetTrigger("Happy");
                anim_NPC10t3.SetTrigger("Happy");
            }

            yield return new WaitForSeconds(0.05f);
        }

        for (float f = 1.0f; f > 0.0f; f -= 0.05f)
        {
            if (optionNPC10 == 3)
            {
                happy_text_NPC10.active = true;
            }
            else if (optionNPC8 == 3)
            {
                happy_text_NPC8.active = true;
            }
            else if (optionNPC9 == 3)
            {
                happy_text_NPC9.active = true;
            }
            else if (optionNPC9 == 5)
            {
                happy_text_NPC9_v2.active = true;
            }

            Color color = new Color(0, 0, 0, 0);
            color.a = f;
            fade.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
