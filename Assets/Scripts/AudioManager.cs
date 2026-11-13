using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instancia;
    public AudioSource musica, efectos;
    public AudioClip musicaFondo, hover, click, voltear, error, acierto, alarma, victoria, derrota;

    void Awake()
    {
        if (Instancia != null) { Destroy(gameObject); return; }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musica.clip = musicaFondo;
        musica.loop = true;
        musica.volume = 0.3f;
        musica.Play();
    }

        void Sonar(AudioClip c, float vol = 1f) { if (c != null) efectos.PlayOneShot(c, vol); }

    public static void Hover()    { if (Instancia) Instancia.Sonar(Instancia.hover, 0.5f); }
    public static void Click()    { if (Instancia) Instancia.Sonar(Instancia.click, 1f); }
    public static void Voltear()  { if (Instancia) Instancia.Sonar(Instancia.voltear, 0.6f); }
    public static void Error()    { if (Instancia) Instancia.Sonar(Instancia.error, 1f); }
    public static void Acierto()  { if (Instancia) Instancia.Sonar(Instancia.acierto, 0.7f); }
    public static void Alarma()   { if (Instancia) Instancia.Sonar(Instancia.alarma, 0.7f); }
    public static void Victoria() { if (Instancia) Instancia.Sonar(Instancia.victoria, 0.8f); }
    public static void Derrota()  { if (Instancia) Instancia.Sonar(Instancia.derrota, 0.8f); }
}
