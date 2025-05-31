using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Interactuar: MonoBehaviour
// al presionar E ,vas a poder interactuar con el objeto que tengas delante

{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractuarConObjeto();
        }
    }

    void InteractuarConObjeto()
    {
        // Lógica para interactuar con el objeto
    }
}
