using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public enum ObjectType { Good, Bad }

    [Header("Item Configuration")]
    public ObjectType itemType; // Set to Good for Seal, Bad for Orca in the Inspector

    [Header("Movement Settings")]
    [SerializeField] private float fallSpeed = 5f;

    private void Update()
    {
        // Move straight down
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Clean up if it falls off screen
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if it hit the player's gloves
        if (collision.CompareTag("PlayerGloves"))
        {
            // Notify the central hub instead of talking to score/health directly
            GameEventManager.TriggerItemCaught(this);

            Destroy(gameObject);
        }
    }
}