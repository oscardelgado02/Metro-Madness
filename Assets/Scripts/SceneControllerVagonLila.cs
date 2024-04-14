using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneControllerVagonLila : MonoBehaviour
{
    private bool closeEnoughDoor;
    private float detectionRange;

    private bool closeEnoughNPC11;

    private bool finishNPC11;
    private int optionNPC11; //Option Q: Value 1 - Option E: Value 2

    public GameObject player;
    public GameObject puerta;
    public GameObject contorno;
    public GameObject pressF;

    public NPC_Obligated NPC11;

    public PlayerController pController;

    public Text clock;
    public Image clock_color;

    public Renderer parpadeo1;
    public Renderer parpadeo2;
    public Renderer parpadeo3;
    public Renderer parpadeo4;

    public GameObject NPC_Mother;
    public GameObject happy_text_NPC11;

    public AudioSource sonido_ambiente;
    public AudioSource sonido_final;

    public SpriteRenderer fade;

    public float timer;
    public int parpadeo_timer;

    // Start is called before the first frame update
    void Start()
    {
        detectionRange = 4.0f;

        pController.remordimiento = GameController.remordimiento;
        pController.tiempo = GameController.tiempo;

        timer = 0.0f;
        parpadeo_timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        parpadeo_timer = (int)timer;

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
        closeEnoughNPC11 = false;


        if ((Vector2.Distance(player.transform.position, puerta.transform.position) <= detectionRange) && EndOfMetro())
        {
            closeEnoughDoor = true;
        }

        closeEnoughNPC11 = NPC11.GetCloseEnough();

        ChangeColor();
        ShowDoor();
        ShowF();
        FinishNPC();
        ExitMetro();
        ChangeNPC();
        ChangeParpadeo();
        Add3Minutes();
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
        finishNPC11 = NPC11.GetFinishInteraccion();

        if (optionNPC11 != 3)
        {
            optionNPC11 = NPC11.GetOptionSelected();
        }
    }

    void ExitMetro()
    {
        if (closeEnoughDoor && Input.GetKey("f") && EndOfMetro())
        {
            SceneManager.LoadScene("Final");
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
        if (optionNPC11 == 2)
        {
            StartCoroutine(ScreenFade());
            optionNPC11 = 3;
        }

        if (timer > 20.0f && timer < 22.0f)
        {
            NPC11.MoveLeftNPC();
        }

        if (timer > 22.0f && finishNPC11)
        {
            Time.timeScale = 0;
        }

        if (!finishNPC11)
        {
            Time.timeScale = 1;
        }

        if (timer > 40.0f && optionNPC11 < 2)
        {
            NPC11.MoveNPC();
        }

        if (timer > 40.0f)
        {
            happy_text_NPC11.active = false;
        }
    }

    void ChangeParpadeo()
    {
        if (parpadeo_timer % 2 == 0)
        {
            parpadeo1.enabled = true;

            if (timer > 20.0f)
            {
                parpadeo2.enabled = true;
            }

            if (timer > 40.0f)
            {
                parpadeo3.enabled = true;
            }

            if (timer > 60.0f)
            {
                parpadeo4.enabled = true;
                sonido_ambiente.mute = true;
            }
        }
        else
        {
            parpadeo1.enabled = false;

            if (timer > 20.0f)
            {
                parpadeo2.enabled = false;
            }

            if (timer > 40.0f)
            {
                parpadeo3.enabled = false;
            }

            if (timer > 60.0f)
            {
                parpadeo4.enabled = false;
                sonido_ambiente.mute = true;
                sonido_final.Play();
            }
        }

        if (timer > 20.0f)
        {
            parpadeo1.enabled = true;
        }

        if (timer > 40.0f)
        {
            parpadeo2.enabled = true;
        }

        if (timer > 60.0f)
        {
            parpadeo3.enabled = true;
        }
    }

    private bool EndOfMetro()
    {
        return (timer > 60.0f);
    }

    void Add3Minutes()
    {
        int check = (int)timer;
        if (check == 20 || check == 40 || check == 60 || check == 80)
        {
            timer += 1.0f;

            if ((GameController.min + 3) >= 60)
            {
                int horas = (GameController.min + 3) / 60;
                GameController.hora += horas;
                GameController.min = (GameController.min + 3) - (60 * horas);
            }
            else
            {
                GameController.min = GameController.min + 3;
            }
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
            yield return new WaitForSeconds(0.05f);
        }

        for (float f = 1.0f; f > 0.0f; f -= 0.05f)
        {
            happy_text_NPC11.active = true;
            NPC_Mother.active = true;

            Color color = new Color(0, 0, 0, 0);
            color.a = f;
            fade.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
