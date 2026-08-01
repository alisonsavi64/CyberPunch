using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] FighterHealth player1Health;
    [SerializeField] FighterHealth player2Health;
    [SerializeField] MatchManager matchManager;

    [SerializeField] Image player1HealthFill;
    [SerializeField] Image player2HealthFill;
    [SerializeField] Text roundTimerText;
    [SerializeField] Text roundWinsText;

    void Update()
    {
        player1HealthFill.fillAmount = (float)player1Health.CurrentHealth / player1Health.MaxHealth;
        player2HealthFill.fillAmount = (float)player2Health.CurrentHealth / player2Health.MaxHealth;
        roundTimerText.text = Mathf.CeilToInt(Mathf.Max(0f, matchManager.roundTimeRemaining)).ToString();
        roundWinsText.text = $"P1: {matchManager.PlayerRoundWins(1)}   P2: {matchManager.PlayerRoundWins(2)}";
    }
}
