using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SistemaMensajes : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject celular;                     // Celular completo
    public GameObject imagenConBotones;            // Segunda imagen (que aparece tras 20s)
    public GameObject canvasTransicion;            // Canvas negro para parpadeo/flashback
    public GameObject personaje;                   // Personaje a mover
    public GameObject textoDialogo;                // Texto que se desactiva durante transición

    public Transform[] puntosFlashback;            // Puntos del "flashback"
    public float duracionParpadeo = 5f;
    public float intervaloParpadeo = 0.2f;

    private bool yaEntroADiscord = false;
    private bool accionEjecutada = false;
    private Quaternion rotacionCamaraOriginal;

    // Vincula este al botón de Discord
    public void OnDiscordClick()
    {
        if (yaEntroADiscord) return;

        yaEntroADiscord = true;

        // Comenzar cuenta regresiva de 20s
        StartCoroutine(EsperarYActivarOpciones(20f));
    }

    IEnumerator EsperarYActivarOpciones(float segundos)
    {
        yield return new WaitForSeconds(segundos);

        if (imagenConBotones != null)
            imagenConBotones.SetActive(true);
    }

    // Vincula esto a los botones de opciones (una sola función para todos)
    public void OpcionSeleccionada()
    {
        if (accionEjecutada) return;

        accionEjecutada = true;

        // Desactivar celular, botones y texto
        if (celular != null) celular.SetActive(false);
        if (imagenConBotones != null) imagenConBotones.SetActive(false);
        if (textoDialogo != null) textoDialogo.SetActive(false);

        // Iniciar transición con parpadeo
        StartCoroutine(FlashbackConParpadeo());
    }

    IEnumerator FlashbackConParpadeo()
    {
        float tiempoTranscurrido = 0f;
        int indicePosicion = 0;

        // Guardar rotación original de la cámara
        Camera camara = Camera.main;
        if (camara != null)
            rotacionCamaraOriginal = camara.transform.rotation;

        // Activar canvas de transición
        if (canvasTransicion != null)
            canvasTransicion.SetActive(true);

        // Parpadeo y movimiento del personaje
        while (tiempoTranscurrido < duracionParpadeo)
        {
            if (camara != null)
                camara.transform.rotation = rotacionCamaraOriginal;

            if (canvasTransicion != null)
                canvasTransicion.SetActive(!canvasTransicion.activeSelf); // on/off

            if (canvasTransicion.activeSelf && puntosFlashback.Length > 0)
            {
                personaje.transform.position = puntosFlashback[indicePosicion].position;
                indicePosicion = (indicePosicion + 1) % puntosFlashback.Length;
            }

            yield return new WaitForSeconds(intervaloParpadeo);
            tiempoTranscurrido += intervaloParpadeo;
        }

        // Dejar canvas negro activo al final (si quieres, puedes ocultarlo aquí también)
        if (canvasTransicion != null)
            canvasTransicion.SetActive(true);

        accionEjecutada = false;

        // Aquí puedes continuar la historia, mostrar diálogo, etc.
        // textoDialogo.SetActive(true);
    }
}
