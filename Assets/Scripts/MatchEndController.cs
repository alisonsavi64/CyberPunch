using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchEndController : MonoBehaviour
{
    [SerializeField] MatchManager matchManager;
    [SerializeField] GameObject panel;
    [SerializeField] Text winnerText;

    void Update()
    {
        bool matchOver = matchManager.phase == MatchManager.MatchPhase.MatchOver;
        panel.SetActive(matchOver);
        if (!matchOver) return;

        winnerText.text = $"PLAYER {matchManager.MatchWinner} WINS\n\nR: Rematch      ESC: Menu";

        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.rKey.wasPressedThisFrame) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else if (kb.escapeKey.wasPressedThisFrame) SceneManager.LoadScene("MainMenu");
    }
}
