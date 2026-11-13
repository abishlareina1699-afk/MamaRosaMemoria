using UnityEngine;

public class Card : MonoBehaviour
{
    public int id;
    public Sprite frente, reverso;
    public bool volteada;
    SpriteRenderer sr;

    void Awake() { sr = GetComponent<SpriteRenderer>(); }

    public void Configurar(int id, Sprite frente, Sprite reverso)
    {
        this.id = id; this.frente = frente; this.reverso = reverso;
        Ocultar();
    }

    public void Mostrar() { volteada = true; sr.sprite = frente; }
    public void Ocultar() { volteada = false; sr.sprite = reverso; }

    void OnMouseDown() { GameManager.Instancia.CartaClick(this); }
}