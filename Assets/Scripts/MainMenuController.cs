using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string fightSceneName = "Arena";

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.enterKey.wasPressedThisFrame || kb.numpadEnterKey.wasPressedThisFrame)
            SceneManager.LoadScene(fightSceneName);
        else if (kb.escapeKey.wasPressedThisFrame)
            Application.Quit();
    }
}
