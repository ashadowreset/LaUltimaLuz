using UnityEngine;
using UnityEngine.UI;

public class PuzzlePanel : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelRoot;
    public Text feedbackText;

    [Header("Respuesta")]
    [Range(1, 3)] public int correctOption = 2;

    public bool IsSolved { get; private set; }

    private void Awake()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
    }

    public void Open()
    {
        if (panelRoot != null) panelRoot.SetActive(true);
        if (feedbackText != null) feedbackText.text = "¿Qué fecha aparece en común en las pistas?";
    }

    // Conectá estos tres métodos a los botones 1, 2 y 3 desde Button > On Click.
    public void Answer1() { Choose(1); }
    public void Answer2() { Choose(2); }
    public void Answer3() { Choose(3); }

    public void Close()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
    }

    private void Choose(int option)
    {
        if (IsSolved) return;

        if (option == correctOption)
        {
            IsSolved = true;
            GameManager.Instance?.MarkPuzzleSolved();
            if (feedbackText != null) feedbackText.text = "Las pistas encajan.";
            Close();
        }
        else
        {
            GameManager.Instance?.SetMessage("Esa fecha no coincide con las pistas.");
            if (feedbackText != null) feedbackText.text = "No coincide. Revisá los objetos encontrados.";
        }
    }
}
