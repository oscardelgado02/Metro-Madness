using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneControllerVagonVerde : MonoBehaviour
{
    private bool closeEnoughDoor;
    private float detectionRange;

    private bool closeEnoughNPC6;
    private bool closeEnoughNPC7;
    private bool closeEnoughNPCNinja;

    private bool finishNPC6;
    private bool finishNPC7;
    private bool finishNPCNinja;
    private int optionNPC6; //Option Q: Value 1 - Option E: Value 2
    private int optionNPC7;

    public GameObject player;
    public GameObject puerta;
    public GameObject contorno;
    public GameObject pressF;

    public NPC_Obligated NPC6;
    public NPC_Obligated NPC7;
    public NPC_Ninja NPCNinja;

    public PlayerController pController;

    public Text clock;
    public Image clock_color;

    public Renderer parpadeo1;
    public Renderer parpadeo2;
    public Renderer parpadeo3;
    public Renderer parpadeo4;
    public Renderer parpadeo5;
    public Renderer parpadeo6;

    public AudioSource sonido_ambiente;
    public AudioSource sonido_final;

    public SpriteRenderer fade;

    public Animator anim_NPC6;
    public SpriteRenderer sprite_NPC6;
    public Sprite sprite_happy_NPC6;
    public GameObject happy_text_NPC6;

    public GameObject happy_text_NPC7;

    public GameObject carta_charizard;
    public Animator anim_charizard;

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

        optionNPC6 = 0;
        optionNPC7 = 0;
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
        closeEnoughNPC6 = false;
        closeEnoughNPC7 = false;
        closeEnoughNPCNinja = false;


        if ((Vector2.Distance(player.transform.position, puerta.transform.position) <= detectionRange) && EndOfMetro())
        {
            closeEnoughDoor = true;
        }

        closeEnoughNPC6 = NPC6.GetCloseEnough();
        closeEnoughNPC7 = NPC7.GetCloseEnough();
        closeEnoughNPCNinja = NPCNinja.GetCloseEnough();

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
        pressF.SetActive(closeEnoughDoor || (closeEnoughNPCNinja&& finishNPCNinja));
    }

    void FinishNPC()
    {
        finishNPC6 = NPC6.GetFinishInteraccion();
        finishNPC7 = NPC7.GetFinishInteraccion();
        finishNPCNinja = NPCNinja.GetFinishInteraccion();

        if (optionNPC6 != 3)
        {
            optionNPC6 = NPC6.GetOptionSelected();
        }

        if (optionNPC7 != 3)
        {
            optionNPC7 = NPC7.GetOptionSelected();
        }
    }

    void ExitMetro()
    {
        if (closeEnoughDoor && Input.GetKey("f") && EndOfMetro())
        {
            SceneManager.LoadScene("Transbordo_Verde_Lila");
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
        if (optionNPC6 == 2)
        {
            anim_NPC6.enabled = true;
            StartCoroutine(ScreenFade());
            optionNPC6 = 3;
        }

        if(optionNPC7 == 1)
        {
            GameController.showCharizard = true;
            optionNPC7 = 3;
        }

        if (optionNPC7 == 2)
        {
            StartCoroutine(ScreenFade());
            optionNPC7 = 3;
        }

        if (optionNPC7 > 0)
        {
            anim_charizard.SetTrigger("Back");
        }

        if (timer > 20.0f && timer < 22.0f)
        {
            NPC6.MoveLeftNPC();
        }

        if (timer > 22.0f && finishNPC6)
        {
            Time.timeScale = 0;
        }

        if(timer > 35.0f)
        {
            happy_text_NPC6.active = false;
        }

        if (timer > 50.0f)
        {
            anim_NPC6.enabled = false;
            sprite_NPC6.sprite = sprite_happy_NPC6;
            NPC6.MoveNPC();
        }

        if(timer > 70.0f)
        {
            NPC7.MoveLeftNPC();
            carta_charizard.active = true;
            anim_charizard.SetTrigger("Caida");
        }

        if (timer > 71.5f && finishNPC7)
        {
            Time.timeScale = 0;
        }

        if(timer > 75.0f)
        {
            anim_charizard.enabled = false;
            carta_charizard.active = false;
        }

        if (timer > 85.0f)
        {
            happy_text_NPC7.active = false;
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
            }

            if (timer > 100.0f)
            {
                parpadeo6.enabled = true;
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
            }

            if (timer > 100.0f)
            {
                parpadeo6.enabled = false;
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

        if (timer > 100.0f)
        {
            parpadeo5.enabled = true;
        }
    }

    private bool EndOfMetro()
    {
        return (timer > 100.0f);
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
            if(optionNPC6 == 3)
            {
                FinishHappyNPC6();
            }

            yield return new WaitForSeconds(0.05f);
        }

        for (float f = 1.0f; f > 0.0f; f -= 0.05f)
        {
            if (optionNPC6 == 3)
            {
                happy_text_NPC6.active = true;
            }

            if (optionNPC7 == 3)
            {
                happy_text_NPC7.active = true;
            }

            Color color = new Color(0, 0, 0, 0);
            color.a = f;
            fade.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void FinishHappyNPC6()
    {
        anim_NPC6.SetTrigger("Happy");
    }

}
