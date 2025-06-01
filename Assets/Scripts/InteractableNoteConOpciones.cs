using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableNoteConOpciones : MonoBehaviour
{

    public GameObject letraE_UI;
    public GameObject canvasOpciones;
    public EmotionalBar barraEmocional;
    public bool opcionElegida = false;
    private bool jugadorCerca = false;

    void Start()
    {
        letraE_UI.SetActive(false);
        canvasOpciones.SetActive(false);
        BloquearCursor();
    }

    void Update()
    {
        // Si ya se eligió una opción, NO permitir volver a interactuar
        if (jugadorCerca && !opcionElegida && Input.GetKeyDown(KeyCode.E))
        {
            canvasOpciones.SetActive(true);
            letraE_UI.SetActive(false);
            LiberarCursor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !opcionElegida)
        {
            letraE_UI.SetActive(true);
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            letraE_UI.SetActive(false);
            canvasOpciones.SetActive(false);
            jugadorCerca = false;
            BloquearCursor();
        }
    }

    public void BotonOpcion1()
    {
        opcionElegida = true;
        barraEmocional.ModifyEmotion(18);
        canvasOpciones.SetActive(false);
        BloquearCursor();
    }

    public void BotonOpcion2()
    {
        opcionElegida = true;
        barraEmocional.ModifyEmotion(-20);
        canvasOpciones.SetActive(false);
        BloquearCursor();
    }

    void LiberarCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void BloquearCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}