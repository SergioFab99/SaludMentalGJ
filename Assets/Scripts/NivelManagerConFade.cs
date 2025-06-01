using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NivelManagerConFade : MonoBehaviour
{
 
        public InteractableNoteConOpciones decision1;
        public InteractableNoteConOpciones decision2;
        public InteractableNoteConOpciones decision3;
        public InteractableNoteConOpciones decision4;

        void Update()
        {
            if (decision1.opcionElegida &&
                decision2.opcionElegida &&
                decision3.opcionElegida &&
                decision4.opcionElegida)
            {
                CambiarNivel();
            }
        }

        void CambiarNivel()
        {
            SceneManager.LoadScene("Nivel4");
        }
}
    