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

        upButton.onClick.AddListener(() => Rebind("up"));
        downButton.onClick.AddListener(() => Rebind("down"));
        leftButton.onClick.AddListener(() => Rebind("left"));
        rightButton.onClick.AddListener(() => Rebind("right"));
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
    void Rebind(string partName)
    {
        var action = inputActions.FindActionMap(actionMapName)?.FindAction(actionName);
        if (action == null) return;
        int bindingIndex = -1;

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (action.bindings[i].isPartOfComposite && action.bindings[i].name == partName)
            {
                bindingIndex = i;
                break;
            }
        }
        if (bindingIndex == -1) return;
        action.Disable();

        action.PerformInteractiveRebinding(bindingIndex)
        .WithCancelingThrough("<Keyboard>/escape")
        .OnComplete(operation =>
        {
            action.Enable();
            operation.Dispose();
            UpdateMoveButtons();
        })
        .Start();
    }
}

