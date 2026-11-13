using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;
    public Card cartaPrefab;
    public Sprite reverso;
    public Sprite[] ingredientes;   // i1..i8
    public Sprite[] platos;         // p1..p8
    public Text txtIntentos, txtMensaje;
    public Slider barraArroz;
    public Image rellenoBarra;      // Slider > Fill Area > Fill
    public Transform panelTablero;
    public GameObject panelVictoria, panelDerrota;
    public Text txtResultado;

    List<Card> cartas = new List<Card>();
    Card primera, segunda;
    int intentos, parejas;
    float tiempoMax, restante;
    bool jugando = true, bloqueado, quemando;
    Coroutine rutinaMensaje;

    void Awake() { Instancia = this; }

    void Start()
    {
        tiempoMax = restante = Dificultad.tiempo;
        int n = Dificultad.parejas;
        if (txtMensaje != null) txtMensaje.text = "";
        if (rellenoBarra != null) rellenoBarra.color = new Color(0.91f, 0.35f, 0.05f);

        var datos = new List<(int id, Sprite s)>();
        for (int i = 0; i < n; i++)
        {
            datos.Add((i, ingredientes[i]));
            datos.Add((i, platos[i]));
        }
        for (int i = 0; i < datos.Count; i++)   // barajar
        {
            int r = Random.Range(i, datos.Count);
            (datos[i], datos[r]) = (datos[r], datos[i]);
        }

        int filas = Mathf.CeilToInt(datos.Count / 4f);
        if (panelTablero != null)
            panelTablero.localScale = new Vector3(9.5f, Mathf.Min(filas * 2.6f + 0.4f, 10.4f), 1);

        for (int i = 0; i < datos.Count; i++)
        {
            Vector3 pos = new Vector3((i % 4) * 2.2f - 3.3f, ((filas - 1) / 2f - i / 4) * 2.6f, 0);
            Card c = Instantiate(cartaPrefab, pos, Quaternion.identity);
            c.Configurar(datos[i].id, datos[i].s, reverso);
            cartas.Add(c);
        }
    }

    void Update()
    {
        if (!jugando) return;
        restante -= Time.deltaTime;
        barraArroz.value = restante / tiempoMax;

        bool peligro = restante <= tiempoMax * 0.33f;   // último tercio del tiempo
        if (peligro && !quemando)
        {
            Mostrar("¡Se está quemando!");
            AudioManager.Alarma();
        }
        quemando = peligro;
        if (rellenoBarra != null)
            rellenoBarra.color = peligro ? Color.red : new Color(0.91f, 0.35f, 0.05f);

        if (restante <= 0)
        {
            jugando = false;
            panelDerrota.SetActive(true);
            AudioManager.Derrota();
        }
    }

    void Mostrar(string texto)
    {
        if (txtMensaje == null) return;
        txtMensaje.text = texto;
        if (rutinaMensaje != null) StopCoroutine(rutinaMensaje);
        rutinaMensaje = StartCoroutine(BorrarMensaje());
    }

    IEnumerator BorrarMensaje()
    {
        yield return new WaitForSeconds(1.5f);
        txtMensaje.text = "";
    }

    public void CartaClick(Card c)
    {
        if (!jugando || bloqueado || c.volteada) return;
        c.Mostrar();
        AudioManager.Voltear();
        if (primera == null) { primera = c; return; }
        segunda = c;
        intentos++;
        txtIntentos.text = "Intentos: " + intentos;
        StartCoroutine(Comparar());
    }

    IEnumerator Comparar()
    {
        bloqueado = true;
        yield return new WaitForSeconds(0.8f);
        if (primera.id == segunda.id)
        {
            parejas++;
            restante = Mathf.Min(restante + Dificultad.bonus, tiempoMax);
            if (parejas == Dificultad.parejas)
            {
                jugando = false;
                if (txtMensaje != null) txtMensaje.text = "";
                panelVictoria.SetActive(true);
                string rango = intentos <= Dificultad.parejas * 1.5f ? "Chef Maestro"
                            : intentos <= Dificultad.parejas * 2.2f ? "Buen cocinero"
                            : "Aprendiz";
                if (txtResultado != null)
                    txtResultado.text = "Intentos: " + intentos + "\n" + rango;
                AudioManager.Victoria();
            }
            else
            {
                Mostrar("¡Sazón perfecta!");
                AudioManager.Acierto();
            }
        }
        else
        {
            primera.Ocultar(); segunda.Ocultar();
            Mostrar("Ay, ese no era…");
            AudioManager.Error();
        }
        primera = segunda = null;
        bloqueado = false;
    }

    public void Reiniciar() { SceneManager.LoadScene("Juego"); }
    public void IrAlMenu() { SceneManager.LoadScene("Menu"); }
}