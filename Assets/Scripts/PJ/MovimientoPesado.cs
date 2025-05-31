using UnityEngine;

public class MovimientoPesado : MonoBehaviour
{
    public float velocidad = 5f;
    public float sensibilidadMouse = 2f;
    public Transform camaraJugador;
    public PensamientosTristes pensamientosTristes; // Asegúrate de arrastrar el componente en el Inspector

    private CharacterController controlador;
    private float rotacionX = 0f;

    private bool primerIntento = true;

    void Start()
    {
        controlador = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Cámara con el mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        camaraJugador.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movimiento con WASD
        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        Vector3 direccion = transform.right * movX + transform.forward * movZ;

        if (direccion.magnitude > 0.1f)
        {
            if (primerIntento)
            {
                primerIntento = false;
                if (pensamientosTristes != null)
                {
                    pensamientosTristes.MostrarPensamiento("¿Realmente tengo que moverme?");
                }
                return; // Bloquea movimiento la primera vez
            }

            controlador.Move(direccion * velocidad * Time.deltaTime);
        }
    }
}