using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Move(float dirX, float speed)
    {
        Vector2 tar = rb.position + new Vector2(dirX, 0f) * (speed * Time.fixedDeltaTime);
        rb.MovePosition(tar);
    }
}
