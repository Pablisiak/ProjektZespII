using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class RebindLoader : MonoBehaviour
{
void Start()
{
    PlayerInput[] players = FindObjectsOfType<PlayerInput>();
    var usedGamepads = new HashSet<Gamepad>();

    foreach (var player in players)
    {
        string playerName = player.name;
        string schemeKey = "ControlScheme_" + playerName;
        string savedScheme = PlayerPrefs.HasKey(schemeKey) 
            ? PlayerPrefs.GetString(schemeKey) 
            : "Keyboard&Mouse";
            if (savedScheme == "Gamepad")
            {
                Gamepad gamepadToUse = null;
                foreach (var g in Gamepad.all)
                {
                    if (!usedGamepads.Contains(g))
                    {
                        gamepadToUse = g;
                        break;
                    }
                }
                if (gamepadToUse != null && player.user.valid)
                {
                    InputUser.PerformPairingWithDevice(gamepadToUse, player.user);
                    player.SwitchCurrentControlScheme("Gamepad", gamepadToUse);
                    usedGamepads.Add(gamepadToUse);
                    Debug.Log("Gracz "+playerName+ " korzysta z pada!");
                }
                else
                {
                    Debug.Log("Nie wykryto gamepada dla gracza " + playerName);
                    var keyboard = Keyboard.current;
                    var mouse = Mouse.current;
                    player.SwitchCurrentControlScheme("Keyboard&Mouse", keyboard, mouse);
                }
            }
            else
            {
                var keyboard = Keyboard.current;
                var mouse = Mouse.current;
                player.SwitchCurrentControlScheme("Keyboard&Mouse", keyboard, mouse);
                Debug.Log("Gracz "+playerName+ " korzysta z klawiatury!");
            }

        var map = player.actions.FindActionMap("Player");
        if (map != null)
        {
            foreach (var action in map.actions)
            {
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
    }
}
}
