using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DespertarInteractivo : MonoBehaviour
{
    [Header("Referencias de interfaz")]
    public RectTransform topLid;
    public RectTransform bottomLid;
    public Button wakeButton;
    public GameObject canvasInicio;
    public GameObject canvasDespertar;// ?? Canvas inicial (pantalla negra, texto, etc.)

    [Header("Configuración")]
    public int clicksToWakeUp = 3;
    public float openSpeed = 300f;

    [Header("Referencias externas")]
    public PensamientosTristes pensamientosTristes;

    private int currentClicks = 0;
    private bool isAwake = false;

    private Vector2 topClosedPos;
    private Vector2 bottomClosedPos;
    private Vector2 topOpenPos;
    private Vector2 bottomOpenPos;

    [Header("Texto modificable")]
    [TextArea]
    public string textoPensamientoDespertar = "¿Por qué me levanté? ¿Qué sentido tiene...?";


    void Start()
    {
        // Desactiva el canvas de despertar al inicio
        if (canvasDespertar != null)
            canvasDespertar.SetActive(false);

        if (canvasInicio != null)
            canvasInicio.SetActive(true);

        StartCoroutine(IniciarDespuesDeRetraso(2f));
    }
    IEnumerator IniciarDespuesDeRetraso(float segundos)
    {
        yield return new WaitForSeconds(segundos);

        if (canvasInicio != null)
            canvasInicio.SetActive(false);

        if (canvasDespertar != null)
            canvasDespertar.SetActive(true);

        // Ahora sí: iniciar lo de siempre
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Posiciones de párpados
        topClosedPos = topLid.anchoredPosition;
        bottomClosedPos = bottomLid.anchoredPosition;

        float screenHeight = Screen.height;
        topOpenPos = new Vector2(topClosedPos.x, topClosedPos.y + screenHeight);
        bottomOpenPos = new Vector2(bottomClosedPos.x, bottomClosedPos.y - screenHeight);

        wakeButton.gameObject.SetActive(false);
        wakeButton.onClick.AddListener(OnWakeButtonClick);

        StartCoroutine(ShowWakeButtonAfterDelay(3f));
    }
    public void OnWakeButtonClick()
    {
        Debug.Log("Click detectado en el botón");

        currentClicks++;

        if (currentClicks >= clicksToWakeUp && !isAwake)
        {
            isAwake = true;
            wakeButton.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(AbrirOjosYMostrarPensamiento());
        }
        else
        {
            StartCoroutine(ShakeAndHideButton());
            StartCoroutine(ShowWakeButtonAfterDelay(0.5f));
        }
    }

    IEnumerator ShakeAndHideButton()
    {
        wakeButton.image.color = Color.red;
        Vector3 originalPos = wakeButton.transform.localPosition;

        float duration = 0.3f;
        float strength = 10f;
        float time = 0f;

        while (time < duration)
        {
            Vector3 offset = new Vector3(
                Random.Range(-strength, strength),
                Random.Range(-strength, strength),
                0);
            wakeButton.transform.localPosition = originalPos + offset;
            time += Time.deltaTime;
            yield return null;
        }

        wakeButton.transform.localPosition = originalPos;
        wakeButton.image.color = Color.white;

        wakeButton.gameObject.SetActive(false);
    }

    private IEnumerator AbrirOjosYMostrarPensamiento()
    {
        yield return StartCoroutine(OpenEyes());

        yield return new WaitForSeconds(0.5f);

        if (pensamientosTristes != null)
        {
            pensamientosTristes.MostrarPensamiento(textoPensamientoDespertar);
        }

        // ? Aquí se desactiva para siempre
        if (canvasDespertar != null)
            canvasDespertar.SetActive(false);

    }
    public IEnumerator OpenEyes()
    {
        while (Vector2.Distance(topLid.anchoredPosition, topOpenPos) > 0.5f)
        {
            topLid.anchoredPosition = Vector2.MoveTowards(topLid.anchoredPosition, topOpenPos, openSpeed * Time.deltaTime);
            bottomLid.anchoredPosition = Vector2.MoveTowards(bottomLid.anchoredPosition, bottomOpenPos, openSpeed * Time.deltaTime);
            yield return null;
        }

        topLid.gameObject.SetActive(false);
        bottomLid.gameObject.SetActive(false);
    }

    IEnumerator ShowWakeButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isAwake)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Vector2 randomPos = new Vector2(
                Random.Range(100f, Screen.width - 100f),
                Random.Range(100f, Screen.height - 100f)
            );

            Vector2 localPoint;
            RectTransform canvasRect = wakeButton.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, randomPos, null, out localPoint);

            wakeButton.GetComponent<RectTransform>().anchoredPosition = localPoint;

            wakeButton.gameObject.SetActive(true);
        }
    }
}
