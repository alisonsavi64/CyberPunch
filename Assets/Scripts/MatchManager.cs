using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public enum MatchPhase { RoundInProgress, RoundEnded, MatchOver }

    [Header("Lutadores")]
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    [Header("Regras")]
    [SerializeField] int roundsToWinMatch = 2;
    [SerializeField] float roundTimeLimit = 60f;
    [SerializeField] float roundIntermissionSeconds = 2f;

    [Header("Debug — observe estes campos mudarem no Play mode")]
    public MatchPhase phase = MatchPhase.RoundInProgress;
    public int player1RoundWins;
    public int player2RoundWins;
    public float roundTimeRemaining;

    FighterHealth player1Health;
    FighterHealth player2Health;
    FighterStateMachine player1StateMachine;
    FighterStateMachine player2StateMachine;
    FighterMovement player1Movement;
    FighterMovement player2Movement;

    Vector3 player1StartPosition;
    Vector3 player2StartPosition;
    float intermissionTimeRemaining;

    void Awake()
    {
        player1Health = player1.GetComponent<FighterHealth>();
        player2Health = player2.GetComponent<FighterHealth>();
        player1StateMachine = player1.GetComponent<FighterStateMachine>();
        player2StateMachine = player2.GetComponent<FighterStateMachine>();
        player1Movement = player1.GetComponent<FighterMovement>();
        player2Movement = player2.GetComponent<FighterMovement>();

        player1StartPosition = player1.transform.position;
        player2StartPosition = player2.transform.position;
    }

    void Start()
    {
        StartRound();
    }

    void Update()
    {
        if (phase == MatchPhase.RoundInProgress) TickRound();
        else if (phase == MatchPhase.RoundEnded) TickIntermission();
    }

    void TickRound()
    {
        roundTimeRemaining -= Time.deltaTime;

        if (player1Health.IsKnockedOut) EndRound(2);
        else if (player2Health.IsKnockedOut) EndRound(1);
        else if (roundTimeRemaining <= 0f) EndRoundByTimeout();
    }

    void EndRoundByTimeout()
    {
        if (player1Health.CurrentHealth > player2Health.CurrentHealth) EndRound(1);
        else if (player2Health.CurrentHealth > player1Health.CurrentHealth) EndRound(2);
        else EndRound(0);
    }

    void EndRound(int winner)
    {
        if (winner == 1) player1RoundWins++;
        else if (winner == 2) player2RoundWins++;

        if (player1RoundWins >= roundsToWinMatch || player2RoundWins >= roundsToWinMatch)
        {
            phase = MatchPhase.MatchOver;
            return;
        }

        phase = MatchPhase.RoundEnded;
        intermissionTimeRemaining = roundIntermissionSeconds;
        AudioManager.Instance?.PlayRoundEnd();
    }

    void TickIntermission()
    {
        intermissionTimeRemaining -= Time.deltaTime;
        if (intermissionTimeRemaining <= 0f) StartRound();
    }

    void StartRound()
    {
        player1Health.ResetHealth();
        player2Health.ResetHealth();
        player1StateMachine.ResetState();
        player2StateMachine.ResetState();
        player1Movement.ResetTo(player1StartPosition);
        player2Movement.ResetTo(player2StartPosition);

        roundTimeRemaining = roundTimeLimit;
        phase = MatchPhase.RoundInProgress;
        AudioManager.Instance?.PlayRoundStart();
    }

    public int PlayerRoundWins(int player) => player == 1 ? player1RoundWins : player2RoundWins;

    public int MatchWinner => player1RoundWins >= roundsToWinMatch ? 1
        : player2RoundWins >= roundsToWinMatch ? 2
        : 0;
}
