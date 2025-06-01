using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinDelDespertar : MonoBehaviour
{
    public EmotionalBar barraEmocion;          // Referencia al script con emotionValue
    public string escenaVictoria = "Victoria"; // Nombre escena victoria
    public string escenaDerrota = "Derrota";   // Nombre escena derrota

    void Start()
    {
        StartCoroutine(EsperarYRevisarResultado(50f));
    }

    IEnumerator EsperarYRevisarResultado(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        RevisarResultado();
    }

    public void RevisarResultado()
    {
        if (barraEmocion == null)
        {
            Debug.LogError("No asignaste la barra de emoción");
            return;
        }

        if (barraEmocion.emotionValue > 80f)
        {
            SceneManager.LoadScene(escenaVictoria);
        }
        else
        {
            SceneManager.LoadScene(escenaDerrota);
        }
    }
}