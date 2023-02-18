using UnityEngine;

public class CloudShadowMover : MonoBehaviour
{
    public float speed = 0.01f;
    private Vector2 offset;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offset = spriteRenderer.material.mainTextureOffset;
    }

    private void Update()
    {
        offset.x += speed * Time.deltaTime;
        offset.y += speed * Time.deltaTime;
        spriteRenderer.material.mainTextureOffset = offset;
    }
}