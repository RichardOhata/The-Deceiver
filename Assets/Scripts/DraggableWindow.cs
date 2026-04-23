using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DraggableWindow : MonoBehaviour
{
    [SerializeField] private InputActionReference clickAction;
    [SerializeField] private InputActionReference pointerPosAction;
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private RectTransform canvasTransform;
    [SerializeField] private RectTransform headerTransform;
    [SerializeField] private Camera uiCamera;


    private Vector2 _initialAnchoredPos;
    private Vector2 _offset;
    private bool _isDragging;
    private bool _hasInitialized = false;

    private void Awake()
    {
        _initialAnchoredPos = panelTransform.anchoredPosition;
        _hasInitialized = true;
    }

    private void OnEnable()
    {
        if (_hasInitialized)
        {
            panelTransform.anchoredPosition = _initialAnchoredPos;
        }
        clickAction.action.Enable();
        pointerPosAction.action.Enable();
        clickAction.action.started += OnClickStarted;
        clickAction.action.canceled += OnClickCanceled;
        pointerPosAction.action.performed += OnPointMoved;
    }

    private void OnDisable()
    {
        clickAction.action.started -= OnClickStarted;
        clickAction.action.canceled -= OnClickCanceled;
        pointerPosAction.action.performed -= OnPointMoved;
        _isDragging = false;
    }

    private bool IsPointerOverHeader(Vector2 pointerPos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(headerTransform, pointerPos, uiCamera);
    }

    private void OnClickStarted(InputAction.CallbackContext ctx)
    {
        var pointerPos = pointerPosAction.action.ReadValue<Vector2>();

        if (IsPointerOverHeader(pointerPos))
        {
            _isDragging = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasTransform, pointerPos, uiCamera, out var localPos
                );

            _offset = panelTransform.anchoredPosition - localPos;
        }
    }

    private void OnClickCanceled(InputAction.CallbackContext ctx)
    {
        _isDragging = false;
    }

    private void OnPointMoved(InputAction.CallbackContext ctx)
    {
        if (!_isDragging) return;
        var pointerPos = ctx.ReadValue<Vector2>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasTransform, pointerPos, uiCamera, out var localPos);

        panelTransform.anchoredPosition = localPos + _offset;
    }
    

}
