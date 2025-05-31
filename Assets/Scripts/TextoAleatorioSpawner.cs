using UnityEngine;
using TMPro;
using System.Collections;
public class TextoAleatorioSpawner : MonoBehaviour
{
    public RectTransform canvasRect;         // Referencia al Canvas
    public GameObject prefabTexto;           // Prefab del texto a mostrar
    public string[] mensajes;                // Mensajes posibles

    public float duracionTexto = 3f;         // Cuánto dura el texto en pantalla
    public float margenX = 300f;             // Margen horizontal (menor = más centrado)
    public float margenY = 200f;             // Margen vertical

    void Start()
    {
        StartCoroutine(GenerarTextos());
    }

    IEnumerator GenerarTextos()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 20f)); // 5 a 20 segundos

            MostrarTextoAleatorio();
        }
    }

    void MostrarTextoAleatorio()
    {
        if (mensajes.Length == 0) return;

        // Crear el texto
        GameObject nuevoTexto = Instantiate(prefabTexto, canvasRect);
        TextMeshProUGUI tmp = nuevoTexto.GetComponent<TextMeshProUGUI>();
        tmp.text = mensajes[Random.Range(0, mensajes.Length)];

        RectTransform rect = nuevoTexto.GetComponent<RectTransform>();

        // Calcular posición aleatoria en un área centrada
        float x = Random.Range(-margenX, margenX);
        float y = Random.Range(-margenY, margenY);

        rect.anchoredPosition = new Vector2(x, y);

        // Destruir luego de un tiempo
        Destroy(nuevoTexto, duracionTexto);
    }
}