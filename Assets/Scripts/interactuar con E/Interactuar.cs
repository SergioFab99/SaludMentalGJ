using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InteractableNote : MonoBehaviour
{
    public GameObject dialogPanel;    // El panel del dialogo (canvas)
    public Text dialogText;           // El texto dentro del panel
    [TextArea(3, 10)]
    public string noteText;           // Texto que se mostrará

    private bool playerInRange = false;
    private bool isDialogOpen = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogOpen)
            {
                OpenDialog();
            }
            else
            {
                CloseDialog();
            }
        }
    }

    private void OpenDialog()
    {
        dialogPanel.SetActive(true);
        dialogText.text = noteText;
        isDialogOpen = true;
    }

    private void CloseDialog()
    {
        dialogPanel.SetActive(false);
        isDialogOpen = false;
    }

    // Detectar si el jugador está cerca usando trigger (colisionador con isTrigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Opcional: mostrar un indicador en pantalla "Presiona E para leer"
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // cerrar diálogo si el jugador se aleja
            CloseDialog();
        }
    }
}
