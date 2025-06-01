using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture; // Arrastra tu imagen "cursor" aquí en el Inspector
    [SerializeField] private Vector2 hotspot = Vector2.zero; // Punto de "clic" del cursor (ajústalo según necesites)

    private void Start()
    {
        // Cambia el cursor al iniciar el juego
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
}