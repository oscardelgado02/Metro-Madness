using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneControllerVagonAzul : MonoBehaviour
{
    private bool closeEnoughDoor;
    private float detectionRange;

    private bool closeEnoughNPC1;
    private bool closeEnoughNPC2;

    private bool finishNPC1;
    private bool finishNPC2;
    private int optionNPC1; //Option Q: Value 1 - Option E: Value 2
    private int optionNPC2;

    public GameObject player;
    public GameObject puerta;
    public GameObject contorno;
    public GameObject pressF;

    public NPC NPC1;
    public NPC_Obligated NPC2;

    public PlayerController pController;

    public Text clock;
    public Image clock_color;

    public Renderer parpadeo1;
    public Renderer parpadeo2;
    public Renderer parpadeo3;
    public Renderer parpadeo4;
    public Renderer parpadeo5;

    public AudioSource sonido_ambiente;
    public AudioSource sonido_final;

    public SpriteRenderer fade;

    public GameObject happy_text_NPC1;

    public SpriteRenderer sprite_NPC2;
    public Sprite sprite_happy_NPC2;
    public GameObject happy_text_NPC2;

    public float timer;
    public int parpadeo_timer;

    // Start is called before the first frame update
    void Start()
    {
        GameController.showBilletes = true;
        GameController.showCola = false;
        GameController.showCharizard = false;
        GameController.remordimiento = 0;
        GameController.tiempo = 0;

        detectionRange = 4.0f;

        pController.remordimiento = GameController.remordimiento;
        pController.tiempo = GameController.tiempo;

        GameController.hora = 9;
        GameController.min = 0;

        timer = 0.0f;
        parpadeo_timer = 0;

        optionNPC1 = 0;
        optionNPC2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        parpadeo_timer = (int) timer;

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
        closeEnoughNPC1 = false;
        closeEnoughNPC2 = false;


        if ((Vector2.Distance(player.transform.position, puerta.transform.position) <= detectionRange) && EndOfMetro())
        {
            closeEnoughDoor = true;
        }

        closeEnoughNPC1 = NPC1.GetCloseEnough();
        closeEnoughNPC2 = NPC2.GetCloseEnough();

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
        pressF.SetActive(closeEnoughDoor || (closeEnoughNPC1 && finishNPC1));
    }

    void FinishNPC()
    {
        finishNPC1 = NPC1.GetFinishInteraccion();
        finishNPC2 = NPC2.GetFinishInteraccion();

        if(optionNPC1 != 3)
        {
            optionNPC1 = NPC1.GetOptionSelected();
        }
        
        if(optionNPC2 != 3)
        {
            optionNPC2 = NPC2.GetOptionSelected();
        }
    }

    void ExitMetro()
    {
        if (closeEnoughDoor && Input.GetKey("f") && EndOfMetro())
        {
            SceneManager.LoadScene("Transbordo_Azul_Verde");
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
        if (optionNPC1 == 2)
        {
            GameController.showBilletes = false;
            happy_text_NPC1.active = true;
            optionNPC1 = 3;
        }

        if(timer > 20.0f)
        {
            happy_text_NPC1.active = false;
            NPC1.MoveNPC();
        }

        if(timer > 40.0f && timer < 42.0f)
        {
            NPC2.MoveNPC();
        }

        if(timer > 42.0f && finishNPC2)
        {
            Time.timeScale = 0;
        }

        if (optionNPC2 == 2)
        {
            StartCoroutine(ScreenFade());
            optionNPC2 = 3;
        }
        

        if (timer > 60.0f)
        {
            happy_text_NPC2.active = false;
            NPC2.MoveLeftNPC();
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
            }

            if (timer > 80.0f)
            {
                parpadeo5.enabled = true;
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
            }

            if (timer > 80.0f)
            {
                parpadeo5.enabled = false;
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

        if (timer > 80.0f)
        {
            parpadeo4.enabled = true;
        }
    }

    private bool EndOfMetro()
    {
        return (timer > 80.0f);
    }

    void Add3Minutes()
    {
        int check = (int) timer;
        if(check == 20 || check == 40 || check == 60 || check == 80)
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
            FinishHappyNPC2();
            yield return new WaitForSeconds(0.05f);
        }

        for (float f = 1.0f; f > 0.0f; f -= 0.05f)
        {
            happy_text_NPC2.active = true;
            Color color = new Color(0, 0, 0, 0);
            color.a = f;
            fade.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void FinishHappyNPC2()
    {
        sprite_NPC2.sprite = sprite_happy_NPC2;
    }
}
