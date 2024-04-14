using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maquina_Exprendedora : MonoBehaviour
{
    private bool closeEnough;
    private float detectionRange;
    private bool finishInteraction;

    public GameObject player;
    public Inventario inv;
    public GameObject contorno;

    public AudioSource sonido_maquina;

    // Start is called before the first frame update
    void Start()
    {
        detectionRange = 2.0f;
        finishInteraction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (finishInteraction)
        {
            closeEnough = false;

            if (Vector2.Distance(player.transform.position, this.gameObject.transform.position) <= detectionRange)
            {
                closeEnough = true;
                if (Input.GetKey("f") && GameController.showBilletes)
                {
                    GetItem();
                }
            }

            ShowContorno();
        }
    }

    void ShowContorno()
    {
        contorno.SetActive(closeEnough);
    }

    void GetItem()
    {
        inv.SetBilletes(false);
        sonido_maquina.Play();
        GameController.showCola = true;
    }
}
