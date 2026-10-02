using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MatchController : MonoBehaviour
{
    [Header("Fósforos")]
    public int startingMatches = 3;
    public float lightDuration = 8f;
    public Text matchesText;
    [Tooltip("Panel negro de pantalla completa. Dejalo opaco al comenzar.")]
    public CanvasGroup darknessOverlay;

    public bool IsLit { get; private set; }
    private int matchesRemaining;
    private Coroutine lightRoutine;

    private void Awake()
    {
        matchesRemaining = startingMatches;
        SetDarkness(true);
        UpdateLabel();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsEnded) return;

        if (Input.GetKeyDown(KeyCode.F))
            LightMatch();
    }

    private void LightMatch()
    {
        if (IsLit)
        {
            GameManager.Instance?.SetMessage("El fósforo ya está encendido.");
            return;
        }
        if (matchesRemaining <= 0)
        {
            GameManager.Instance?.SetMessage("No te quedan fósforos.");
            return;
        }

        matchesRemaining--;
        UpdateLabel();
        IsLit = true;
        SetDarkness(false);
        if (lightRoutine != null) StopCoroutine(lightRoutine);
        lightRoutine = StartCoroutine(LightTimer());
    }

    private IEnumerator LightTimer()
    {
        yield return new WaitForSeconds(lightDuration);
        IsLit = false;
        SetDarkness(true);
        lightRoutine = null;
        GameManager.Instance?.SetMessage("El fósforo se apagó.");
    }

    private void SetDarkness(bool dark)
    {
        if (darknessOverlay != null)
        {
            darknessOverlay.alpha = dark ? 0.72f : 0f;
            darknessOverlay.blocksRaycasts = false;
            darknessOverlay.interactable = false;
        }
    }

    private void UpdateLabel()
    {
        if (matchesText != null)
            matchesText.text = "Fósforos: " + matchesRemaining;
    }
}
