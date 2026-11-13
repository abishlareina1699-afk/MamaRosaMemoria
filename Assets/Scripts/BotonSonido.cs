using UnityEngine;
using UnityEngine.EventSystems;

public class BotonSonido : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData e) { AudioManager.Hover(); }
    public void OnPointerClick(PointerEventData e) { AudioManager.Click(); }
}
