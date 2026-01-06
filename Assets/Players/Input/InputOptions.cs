using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InputOptions : MonoBehaviour
{
    public InputActionAsset inputActions;
    public string actionMapName = "Player";
    public string actionName = "Move";

    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;

    void Start()
    {
        UpdateMoveButtons();
    }

    public void UpdateMoveButtons()
    {
        if (inputActions == null) return;

        var action = inputActions.FindActionMap(actionMapName)?.FindAction(actionName);
        if (action == null) return;

        string upText = GetBindingForComposite(action, "up");
        string downText = GetBindingForComposite(action, "down");
        string leftText = GetBindingForComposite(action, "left");
        string rightText = GetBindingForComposite(action, "right");

        upButton.GetComponentInChildren<TMP_Text>().text = upText;
        downButton.GetComponentInChildren<TMP_Text>().text = downText;
        leftButton.GetComponentInChildren<TMP_Text>().text = leftText;
        rightButton.GetComponentInChildren<TMP_Text>().text = rightText;
    }

    string GetBindingForComposite(InputAction action, string partName)
    {
        foreach (var b in action.bindings)
        {
            if (b.isPartOfComposite && b.name == partName)
            {
                if (b.effectivePath.Contains("stick"))
                {
                    switch (partName)
                    {
                        case "up": return "Left Stick ↑";
                        case "down": return "Left Stick ↓";
                        case "left": return "Left Stick ←";
                        case "right": return "Left Stick →";
                    }
                }
                else
                {
                    return b.ToDisplayString();
                }
            }
        }
        return "";
    }
}
