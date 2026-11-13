using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    public void Facil()      { Jugar(6, 90f, 5f); }
    public void Intermedio() { Jugar(8, 70f, 3f); }
    public void Experto()    { Jugar(8, 45f, 1f); }

    void Jugar(int p, float t, float b)
    {
        Dificultad.parejas = p;
        Dificultad.tiempo = t;
        Dificultad.bonus = b;
        SceneManager.LoadScene("Juego");
    }
}
