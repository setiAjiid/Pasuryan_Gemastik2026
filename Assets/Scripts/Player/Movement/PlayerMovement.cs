using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    public void Move(float dirX, float speed)
    {
        Vector2 tar = rb.position + new Vector2(dirX, 0f) * (speed * Time.fixedDeltaTime);
        rb.MovePosition(tar);
    }

    public void Face(float dirX)
    {
        if (Mathf.Abs(dirX) > 0.01f)
        {
            sprite.flipX = dirX > 0f;
        } 
    }
}
