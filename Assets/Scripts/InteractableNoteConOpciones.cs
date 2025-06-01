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
        BloquearCursor(); // ? Asegúrate que al iniciar, el mouse esté bloqueado
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            canvasOpciones.SetActive(true);
            letraE_UI.SetActive(false);
            LiberarCursor(); // ? Mostrar cursor cuando abres el menú
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
            BloquearCursor(); // ? Vuelve a bloquear si se aleja
        }
    }

    public void BotonOpcion1()
    {
        opcionElegida = true;
        barraEmocional.ModifyEmotion(14);
        canvasOpciones.SetActive(false);
        BloquearCursor(); // ? Oculta mouse luego de elegir
    }

    public void BotonOpcion2()
    {
        opcionElegida = true;
        barraEmocional.ModifyEmotion(-20);
        canvasOpciones.SetActive(false);
        BloquearCursor(); // ? Oculta mouse luego de elegir
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
