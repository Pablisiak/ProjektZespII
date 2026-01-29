using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadCursor : MonoBehaviour
{
    public RectTransform cursorRect;
    public float speed = 1000f;
    public Vector2 screenBounds;

    private Vector2 position;

    public InputActionReference moveAction;
    public InputActionReference clickAction;

    void OnEnable()
    {
        moveAction.action.Enable();
        clickAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        clickAction.action.Disable();
    }

    void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        if(input.sqrMagnitude > 0.001f){
        position += input * speed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, 0, Screen.width);
        position.y = Mathf.Clamp(position.y, 0, Screen.height);
        cursorRect.position = position;
        }
        if (clickAction.action.triggered)
        {
            UnityEngine.EventSystems.PointerEventData pointerData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
            {
                position = position
            };
            var results = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(pointerData, results);

            foreach (var r in results)
            {
                var button = r.gameObject.GetComponent<UnityEngine.UI.Button>();
                if (button != null)
                    button.onClick.Invoke();
            }
        }
    }
}
