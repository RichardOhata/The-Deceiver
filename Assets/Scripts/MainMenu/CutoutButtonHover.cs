using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CutoutButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject idleState;
    [SerializeField] private GameObject hoverState;

    [SerializeField] private bool isDisabled;
    [SerializeField] private TextMeshProUGUI idleText;
    public Color disabledColor;
    private void OnEnable()
    {
       SetToDefaultState();
       SetDisabledState(isDisabled);
    }
    private void Start()
    {
      SetToDefaultState();
      SetDisabledState(isDisabled);
    }

    private void SetToDefaultState()
    {
        idleState.SetActive(true);
        hoverState.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDisabled) return;

        idleState.SetActive(false);
        hoverState.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDisabled) return;

        idleState.SetActive(true);
        hoverState.SetActive(false);
    }

    public void SetDisabledState(bool shouldDisable)
    {
        isDisabled = shouldDisable;
        idleState.SetActive(true);
        hoverState.SetActive(false);
        if (TryGetComponent<Button>(out Button btn))
        {
            btn.interactable = !shouldDisable;
        }
        idleText.color = (shouldDisable) ? disabledColor : Color.white;
    }
}
