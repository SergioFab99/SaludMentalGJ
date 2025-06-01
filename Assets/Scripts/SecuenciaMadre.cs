using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
public class SecuenciaMadre : MonoBehaviour
{
    [Header("UI y Componentes")]
    public GameObject canvasNegro;                    // Canvas del fondo semitransparente con texto
    public TextMeshProUGUI textoDialogo;
    public CanvasGroup canvasGroupNegroFinal;         // Para hacer el fade al final

    [Header("Audios")]
    public AudioSource sonidoPuerta;
    public AudioSource sonidoLatidos;
    public AudioSource sonidoRespiracion;

    [Header("Configuración")]
    public float tiempoAntesDeIniciar = 30f;          // Puedes bajarlo para pruebas
    public float velocidadEscritura = 0.05f;
    public float tiempoEntreLineas = 2f;
    public float duracionFade = 2f;                   // Tiempo que toma hacer el fade a negro

    [TextArea(2, 5)]
    public string[] dialogoMadre = {
        "Hija...",
        "Sé que no quieres hablar, pero tenemos que ir al psicólogo.",
        "No puedes seguir así... iremos al psicólogo."
    };

    [Header("Control del juego")]
    public GameObject[] otrosCanvas;
    public MonoBehaviour[] componentesADesactivar;

    void Start()
    {
        textoDialogo.text = "";
        canvasNegro.SetActive(false);
        if (canvasGroupNegroFinal != null)
        {
            canvasGroupNegroFinal.gameObject.SetActive(false);
            canvasGroupNegroFinal.alpha = 0f;
        }

        StartCoroutine(IniciarCinematica());
    }

    IEnumerator IniciarCinematica()
    {
        yield return new WaitForSeconds(tiempoAntesDeIniciar);

        MovimientoPesado.cinemáticaActiva = true;

        // Desactivar otros Canvas
        foreach (var c in otrosCanvas)
            if (c != null) c.SetActive(false);

        // Desactivar scripts del jugador
        foreach (var comp in componentesADesactivar)
            if (comp != null) comp.enabled = false;

        // Activar Canvas Negro con texto
        canvasNegro.SetActive(true);

        // Iniciar sonidos ambientales
        if (sonidoLatidos != null) sonidoLatidos.Play();
        if (sonidoRespiracion != null) sonidoRespiracion.Play();

        // Mostrar diálogo línea por línea con efecto de escritura
        foreach (string linea in dialogoMadre)
        {
            yield return StartCoroutine(MostrarTextoLetraPorLetra(linea));
            yield return new WaitForSeconds(tiempoEntreLineas);
        }

        // Sonido puerta
        if (sonidoPuerta != null) sonidoPuerta.Play();

        // Fade a negro completo
        if (canvasGroupNegroFinal != null)
        {
            canvasGroupNegroFinal.gameObject.SetActive(true);
            yield return StartCoroutine(FadeCanvas(canvasGroupNegroFinal, 0f, 1f, duracionFade));
        }

        textoDialogo.text = "";

        MovimientoPesado.cinemáticaActiva = false;

        // Reactivar scripts si quieres
        foreach (var comp in componentesADesactivar)
            if (comp != null) comp.enabled = true;
    }
    IEnumerator MostrarTextoLetraPorLetra(string linea)
    {
        textoDialogo.text = "";
        foreach (char letra in linea.ToCharArray())
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }
    }

    IEnumerator FadeCanvas(CanvasGroup canvasGroup, float inicio, float fin, float duracion)
    {
        float tiempo = 0f;
        while (tiempo < duracion)
        {
            canvasGroup.alpha = Mathf.Lerp(inicio, fin, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = fin;
        yield return new WaitForSeconds(2f); // Espera 2 segundos antes de cambiar de escena
        SceneManager.LoadScene("Nivel3");
    }
}
