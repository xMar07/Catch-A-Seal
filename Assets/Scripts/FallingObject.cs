using UnityEngine;
using System;

public class FallingObject : MonoBehaviour
{
    // 1. Added this enum and variable to distinguish good vs bad items
    public enum ObjectType { Good, Bad }
    [Header("Item Type")]
    public ObjectType itemType;

    public static event Action<FallingObject> OnItemCaught;

    [Header("Movement Settings")]
    [SerializeField] private float fallSpeed = 5f;

    private void Update()
    {
        // Move the object straight down every frame
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Optional: Destroy/recycle if it falls past the screen bottom
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player's gloves
        if (collision.CompareTag("PlayerGloves"))
        {
            // Fire the event, passing reference to this object
            OnItemCaught?.Invoke(this);

            // Make the object disappear immediately upon contact
            Destroy(gameObject);
        }
    }
}