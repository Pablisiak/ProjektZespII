using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    public Text waveTimerText;

    public void UpdateWaveTimer(float timeRemaining)
    {
        if (timeRemaining > 0)
            waveTimerText.text = $"Czas do końca fali: {timeRemaining:F1}s";
        else
            waveTimerText.text = "Fala zakończona!";
    }
}
