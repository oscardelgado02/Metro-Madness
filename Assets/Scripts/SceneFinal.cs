using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFinal : MonoBehaviour
{
    public GameObject final_late;
    public GameObject final_temple;
    public GameObject final_remordimiento;

    public GameObject final_late_text;
    public GameObject final_temple_text;
    public GameObject final_remordimiento_text;

    // Start is called before the first frame update
    void Start()
    {
        if (GameController.hora >= 11)
        {
            final_late.active = true;
            final_late_text.active = true;
        }

        else if (GameController.remordimiento >= 100)
        {
            final_remordimiento.active = true;
            final_remordimiento_text.active = true;
        }

        else
        {
            final_temple.active = true;
            final_temple_text.active = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("Vagon_Azul");
    }
}
