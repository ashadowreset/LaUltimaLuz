using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum EvidenceType
{
    Letter,
    Names,
    Flyer
}

public enum EndingType
{
    Main,
    LetterRemains,
    NamesRemain,
    FlyerRemains,
    UnfinishedTestimony,
    NightFalls
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Tiempo")]
    [Min(30f)] public float timeLimitSeconds = 300f;
    public Text timerText;

    [Header("Mensajes")]
    public Text messageText;
    public Text interactionHintText;

    [Header("Final")]
    public GameObject endingPanel;
    public Text endingTitleText;
    public Text endingBodyText;

    public bool IsEnded { get; private set; }
    public bool PuzzleSolved { get; private set; }

    private float timeRemaining;
    private bool hasLetter;
    private bool hasNames;
    private bool hasFlyer;
    private float messageSeconds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        timeRemaining = timeLimitSeconds;
        if (endingPanel != null) endingPanel.SetActive(false);
        SetMessage("Encontrá las pistas y decidí qué huellas conservar.");
        UpdateTimerLabel();
    }

    private void Update()
    {
        if (IsEnded) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            Finish(EndingType.NightFalls);
        }

        UpdateTimerLabel();

        if (messageSeconds > 0f)
        {
            messageSeconds -= Time.deltaTime;
            if (messageSeconds <= 0f && messageText != null)
                messageText.text = "";
        }
    }

    private void UpdateTimerLabel()
    {
        if (timerText == null) return;
        int total = Mathf.CeilToInt(timeRemaining);
        timerText.text = string.Format("{0:00}:{1:00}", total / 60, total % 60);
    }

    public void SetMessage(string message)
    {
        if (messageText == null) return;
        messageText.text = message;
        messageSeconds = 3.5f;
    }

    public void SetHint(string message)
    {
        if (interactionHintText != null)
            interactionHintText.text = message;
    }

    public void CollectEvidence(EvidenceType evidence)
    {
        switch (evidence)
        {
            case EvidenceType.Letter: hasLetter = true; break;
            case EvidenceType.Names: hasNames = true; break;
            case EvidenceType.Flyer: hasFlyer = true; break;
        }
    }

    public void MarkPuzzleSolved()
    {
        PuzzleSolved = true;
        SetMessage("El acertijo quedó resuelto.");
    }

    public void TryFinish()
    {
        if (IsEnded) return;

        // Final principal: se preservaron las tres huellas y se resolvió el acertijo.
        if (hasLetter && hasNames && hasFlyer && PuzzleSolved)
        {
            Finish(EndingType.Main);
            return;
        }

        // Los finales alternativos reflejan qué huella pudo conservarse.
        if (!hasLetter) Finish(EndingType.LetterRemains);
        else if (!hasNames) Finish(EndingType.NamesRemain);
        else if (!hasFlyer) Finish(EndingType.FlyerRemains);
        else Finish(EndingType.UnfinishedTestimony);
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Finish(EndingType ending)
    {
        if (IsEnded) return;
        IsEnded = true;
        SetHint("");

        string title;
        string body;
        switch (ending)
        {
            case EndingType.Main:
                title = "La memoria llega";
                body = "Las pistas quedaron reunidas. Lo ocurrido no se puede deshacer, pero el testimonio y sus huellas pueden permanecer.";
                break;
            case EndingType.LetterRemains:
                title = "La carta quedó atrás";
                body = "No se pudo conservar la carta. Otras huellas quizá alcancen a permanecer.";
                break;
            case EndingType.NamesRemain:
                title = "La lista quedó atrás";
                body = "No se pudo conservar la lista. El resto del testimonio quedó incompleto.";
                break;
            case EndingType.FlyerRemains:
                title = "El volante quedó atrás";
                body = "No se pudo conservar el volante. Otras huellas sobrevivieron en el departamento.";
                break;
            case EndingType.UnfinishedTestimony:
                title = "Un testimonio incompleto";
                body = "Las tres huellas fueron reunidas, pero el último acertijo quedó sin resolver.";
                break;
            default:
                title = "La noche avanza";
                body = "El tiempo se agotó. Quedaron preguntas abiertas y rastros por recuperar.";
                break;
        }

        if (endingTitleText != null) endingTitleText.text = title;
        if (endingBodyText != null) endingBodyText.text = body;
        if (endingPanel != null) endingPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
