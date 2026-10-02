using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover2D : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    private Rigidbody2D body;
    private Vector2 input;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        body.freezeRotation = true;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsEnded)
        {
            input = Vector2.zero;
            return;
        }

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + input * moveSpeed * Time.fixedDeltaTime);
    }
}
