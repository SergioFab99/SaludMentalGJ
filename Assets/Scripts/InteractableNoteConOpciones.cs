using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableNoteConOpciones : MonoBehaviour
{
    public PensamientosTristes pensamientos;
    public GameObject indicadorE; // Indicador "Presiona E"
    public GameObject panelOpciones;
    public Button botonPositivo;
    public Button botonNegativo;

    public TextMeshProUGUI textoBotonPositivo;
    public TextMeshProUGUI textoBotonNegativo;

    [TextArea]
    public string textoPositivo;

    [TextArea]
    public string textoNegativo;

    [TextArea]
    public string respuestaPositiva = "Elegiste algo positivo. ¡Mejoraste tu ánimo!";

    [TextArea]
    public string respuestaNegativa = "Elegiste algo negativo. Tu ánimo bajó.";

    public EmotionalBar barraEmocional;

    private bool jugadorCerca = false;
    private bool opcionesAbiertas = false;
    private bool yaInteractuo = false;
    public bool YaInteractuo()
{
    return yaInteractuo;
}
    private void Start()
    {
        if (panelOpciones != null)
            panelOpciones.SetActive(false);

        // Suscribimos solo una vez a los botones
        botonPositivo.onClick.RemoveAllListeners();
        botonNegativo.onClick.RemoveAllListeners();

        botonPositivo.onClick.AddListener(() => OnOpcionElegida(true));
        botonNegativo.onClick.AddListener(() => OnOpcionElegida(false));

        indicadorE.SetActive(false); // Asegurarse que no esté visible al inicio
    }

    private void Update()
    {
        // Bloquear tecla C si el panel está abierto
        if (opcionesAbiertas && Input.GetKeyDown(KeyCode.C))
        {
            // No hacer nada
            return;
        }

        // Mostrar indicador E solo si el jugador está cerca y no está interactuando
        if (jugadorCerca && !CelularUI.celularEstaActivo && !pensamientos.PensamientoMostrandose && !opcionesAbiertas && !yaInteractuo)
        {
            if (!indicadorE.activeSelf)
                indicadorE.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                textoBotonPositivo.text = textoPositivo;
                textoBotonNegativo.text = textoNegativo;

                panelOpciones.SetActive(true);
                indicadorE.SetActive(false);
                opcionesAbiertas = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else
        {
            if (indicadorE.activeSelf)
                indicadorE.SetActive(false);
        }
    }

    private void OnOpcionElegida(bool positiva)
    {
        if (positiva)
        {
            barraEmocional.ModifyEmotion(14);
            pensamientos.MostrarPensamiento(respuestaPositiva);
        }
        else
        {
            barraEmocional.ModifyEmotion(-20);
            pensamientos.MostrarPensamiento(respuestaNegativa);
        }

        panelOpciones.SetActive(false);
        opcionesAbiertas = false;
        yaInteractuo = true;  // Marca que ya interactuó con este objeto

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaInteractuo)
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            indicadorE.SetActive(false);

            if (panelOpciones != null)
                panelOpciones.SetActive(false);

            opcionesAbiertas = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
