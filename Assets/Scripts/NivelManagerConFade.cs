using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NivelManagerConFade : MonoBehaviour
{
    [Header("Objetos que debe completar el jugador")]
    public InteractableNoteConOpciones[] objetosInteractuables; // Asignar los 4 interactuables

    [Header("Progreso del jugador")]
    public bool[] interaccionesCompletadas = new bool[4]; // Visible desde el Inspector

    [Header("Transición de escena")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1.5f;
    public string nombreSiguienteEscena;

    private bool nivelCompletado = false;

    void Update()
    {
        if (nivelCompletado) return;

        bool todosCompletados = true;

        for (int i = 0; i < objetosInteractuables.Length; i++)
        {
            if (objetosInteractuables[i] != null)
            {
                interaccionesCompletadas[i] = objetosInteractuables[i].YaInteractuo();
                if (!interaccionesCompletadas[i])
                    todosCompletados = false;
            }
        }

        if (todosCompletados)
        {
            nivelCompletado = true;
            StartCoroutine(FadeAndContinue());
        }
    }

    IEnumerator FadeAndContinue()
    {
        fadeCanvas.blocksRaycasts = true;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        if (!string.IsNullOrEmpty(nombreSiguienteEscena))
        {
            SceneManager.LoadScene(nombreSiguienteEscena);
        }
        else
        {
            Debug.Log("¡Nivel completado!");
        }
    }
}
