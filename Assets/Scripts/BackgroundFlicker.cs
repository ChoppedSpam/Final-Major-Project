using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFlicker : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer;
    public Sprite baseSprite;  // Your main background sprite
    public List<Sprite> flickerSprites; // Alternate flicker sprites
    public float flickerInterval = 4f;  // How often to flicker
    public float flickerDuration = 0.1f;  // How long the flicker lasts

    void Start()
    {
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(flickerInterval);

            if (flickerSprites.Count == 0) continue;

            // Pick a random flicker sprite
            Sprite flicker = flickerSprites[Random.Range(0, flickerSprites.Count)];

            backgroundRenderer.sprite = flicker;
            yield return new WaitForSeconds(flickerDuration);

            backgroundRenderer.sprite = baseSprite;
        }
    }
}
