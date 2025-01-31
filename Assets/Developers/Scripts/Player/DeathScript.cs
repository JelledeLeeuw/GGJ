using System.Collections;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;

    public GameObject player;
    public Transform respawnPoint;


    public bool respawnCoins;
    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        //player = GetComponent<GameObject>();
    }

    // Coroutine to handle the fade-out effect
    IEnumerator FadeOut()
    {
        float fadeDuration = 0.5f; // How long the fade lasts
        float timeElapsed = 0f;

        Color initialColor = spriteRenderer.color;

        //fade the player sprite to transparent
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            yield return null;
        }

        spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        Respawn();
    }

    private void Respawn()
    {
        Color initialColor = spriteRenderer.color;
        spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
        player.transform.position = respawnPoint.position;
        isDead = false;
        respawnCoins = true;
    }

    public void Die()
    {
        // Check for player's death
        if (isDead)
        {
            return; // Do nothing if already fading out
        }
        isDead = true;
        StartCoroutine(FadeOut());
    }
}
