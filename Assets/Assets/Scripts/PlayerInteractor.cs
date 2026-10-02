using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float interactionRadius = 1.25f;
    public LayerMask interactableLayers = ~0;
    public MatchController matchController;

    private WorldInteractable nearest;

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsEnded) return;

        FindNearestInteractable();

        if (Input.GetKeyDown(KeyCode.E) && nearest != null)
            nearest.Interact(matchController);
    }

    private void FindNearestInteractable()
    {
        nearest = null;
        float bestDistance = interactionRadius;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayers);

        foreach (Collider2D hit in hits)
        {
            WorldInteractable candidate = hit.GetComponent<WorldInteractable>();
            if (candidate == null || candidate.IsUsed) continue;

            float distance = Vector2.Distance(transform.position, candidate.transform.position);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                nearest = candidate;
            }
        }

        if (GameManager.Instance != null)
            GameManager.Instance.SetHint(nearest != null ? "E — " + nearest.prompt : "");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
