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
    public Button keyboardButton;
    public Button gamepadButton;
    public string playerName;

    void Start()
    {
        LoadBindings();
        UpdateMoveButtons();

        upButton.onClick.AddListener(() => Rebind("up"));
        downButton.onClick.AddListener(() => Rebind("down"));
        leftButton.onClick.AddListener(() => Rebind("left"));
        rightButton.onClick.AddListener(() => Rebind("right"));
        keyboardButton.onClick.AddListener(() => SetControlScheme("Keyboard&Mouse"));
        gamepadButton.onClick.AddListener(() => SetControlScheme("Gamepad"));
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
        string activeScheme = PlayerPrefs.GetString("ControlScheme_" + playerName, "Keyboard&Mouse");

        foreach (var b in action.bindings)
        {
            if (!b.isPartOfComposite || b.name != partName) continue;
                if (activeScheme == "Gamepad")
                {
                    return partName switch
                    {
                        "up" => "Left Stick ↑",
                        "down" => "Left Stick ↓",
                        "left" => "Left Stick ←",
                        "right" => "Left Stick →",
                        _ => "-"
                    };
                }
                else
                {
                    return b.ToDisplayString();
                }
        }
        return "";
    }
    void Rebind(string partName)
    {
        string activeScheme = PlayerPrefs.GetString("ControlScheme_" + playerName, "Keyboard&Mouse");
        if (activeScheme == "Gamepad")
        {
            Debug.Log("Nie można rebindować używając pada!");
            return;
        }
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
            string bindingPath = action.bindings[bindingIndex].effectivePath;
            PlayerPrefs.SetString(action.name + "_" + partName + "_" + playerName, bindingPath);
            PlayerPrefs.Save();
            UpdateMoveButtons();
        })
        .Start();
    }
    void SetControlScheme(string scheme)
    {
        PlayerPrefs.SetString("ControlScheme_" + playerName, scheme);
        PlayerPrefs.Save();
        //Debug.Log(""+playerName+"  "+scheme);
        UpdateMoveButtons();
    }
    void LoadBindings()
    {
        var action = inputActions.FindActionMap(actionMapName)?.FindAction(actionName);
        if (action == null) return;

        for (int i = 0; i < action.bindings.Count; i++)
        {
            var b = action.bindings[i];
            if (!b.isPartOfComposite) continue;

            string key = action.name + "_" + b.name + "_" + playerName;
            if (PlayerPrefs.HasKey(key))
            {
                string path = PlayerPrefs.GetString(key);
                action.ApplyBindingOverride(i, path);
            }
        }
    }
}

