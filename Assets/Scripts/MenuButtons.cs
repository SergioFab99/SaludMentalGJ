using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void EmpezarJuego()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void Opciones()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void Creditos()
    {
        SceneManager.LoadScene("Creditos");
    }
    public void VolverMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Salir()
    {
        // Esto funciona solo en compilaciones, no en editor
        Application.Quit();
        Debug.Log("Salir del juego");
    }
}
