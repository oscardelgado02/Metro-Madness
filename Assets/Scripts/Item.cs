using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private bool closeEnough;
    private float detectionRange;

    public GameObject player;
    public Inventario inv;
    public GameObject contorno;

    // Start is called before the first frame update
    void Start()
    {
        detectionRange = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        closeEnough = false;

        if (Vector2.Distance(player.transform.position, this.gameObject.transform.position) <= detectionRange)
        {
            closeEnough = true;
        }

        ShowContorno();

        GetItem();
    }

    void ShowContorno()
    {
        contorno.SetActive(closeEnough);
    }

    void GetItem()
    {
        if (closeEnough && Input.GetKey("f") && GameController.showBilletes)
        {
            GameController.showCola = true;
            this.gameObject.SetActive(false);
        }
    }
}
