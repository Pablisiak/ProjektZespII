using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] string gameOverSceneName = "GameOver";
    bool ended = false;

    void Update()
    {
        if (ended) return;

        if (AllPlayersDead())
        {
            ended = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

    bool AllPlayersDead()
    {
        if (Players.players == null || Players.players.PlayersList == null || Players.players.PlayersList.Count == 0)
            return false;

        bool anyActivePlayerFound = false;

        foreach (GameObject go in Players.players.PlayersList)
        {
            if (go == null) continue;
            if (!go.activeInHierarchy) continue;

            var pl = go.GetComponent<Player>();
            if (pl == null) continue;

            anyActivePlayerFound = true;

            if (!pl.IsDead)
                return false; // ktoś żyje
        }

        // jeśli nie znaleźliśmy żadnego aktywnego gracza, nie kończ gry “przez przypadek”
        return anyActivePlayerFound;
    }
}
