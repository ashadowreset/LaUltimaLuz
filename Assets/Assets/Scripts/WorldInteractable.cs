using UnityEngine;

public class WorldInteractable : MonoBehaviour
{
    public enum ActionType { Evidence, Puzzle, Exit }

    [Header("Interacción")]
    public ActionType actionType;
    public string prompt = "Investigar";
    public bool requiresLitMatch = true;
    public EvidenceType evidenceType;
    public PuzzlePanel puzzlePanel;
    public MatchController matchController;

    public bool IsUsed { get; private set; }

    public void Interact(MatchController playerMatches)
    {
        if (GameManager.Instance == null || GameManager.Instance.IsEnded) return;

        MatchController matches = playerMatches != null ? playerMatches : matchController;
        if (requiresLitMatch && (matches == null || !matches.IsLit))
        {
            GameManager.Instance.SetMessage("La oscuridad no te deja distinguirlo. Encendé un fósforo con F.");
            return;
        }

        switch (actionType)
        {
            case ActionType.Evidence:
                GameManager.Instance.CollectEvidence(evidenceType);
                IsUsed = true;
                GameManager.Instance.SetMessage("Guardaste una huella.");
                gameObject.SetActive(false);
                break;

            case ActionType.Puzzle:
                if (puzzlePanel == null)
                {
                    GameManager.Instance.SetMessage("Falta asignar el panel del acertijo en el Inspector.");
                    return;
                }
                if (puzzlePanel.IsSolved)
                    GameManager.Instance.SetMessage("El acertijo ya está resuelto.");
                else
                    puzzlePanel.Open();
                break;

            case ActionType.Exit:
                GameManager.Instance.TryFinish();
                break;
        }
    }
}
