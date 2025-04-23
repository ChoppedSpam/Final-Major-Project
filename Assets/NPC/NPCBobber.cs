using UnityEngine;
using System;

public static class NPCReactionEvents
{
    public static Action OnPlayerHit;
    public static Action OnParry;
}

public class NPCBobber : MonoBehaviour
{
    public float bobAmount = 0.1f;
    public float bobSpeed = 5f;

    private Vector3 originalPosition;
    private Conductor conductor;
    private int lastBeat = -1;

    public Sprite normalSprite;
    public Sprite hitSprite;
    public Sprite parrySprite;
    public float reactDuration = 0.3f;

    private SpriteRenderer sr;

    void Start()
    {
        originalPosition = transform.localPosition;
        conductor = FindObjectOfType<Conductor>();

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = normalSprite;

        NPCReactionEvents.OnPlayerHit += ShowHitReaction;
        NPCReactionEvents.OnParry += ShowParryReaction;
    }

    void OnDestroy()
    {
        NPCReactionEvents.OnPlayerHit -= ShowHitReaction;
        NPCReactionEvents.OnParry -= ShowParryReaction;
    }

    void ShowHitReaction()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeSpriteTemporary(hitSprite));
    }

    void ShowParryReaction()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeSpriteTemporary(parrySprite));
    }

    System.Collections.IEnumerator ChangeSpriteTemporary(Sprite newSprite)
    {
        sr.sprite = newSprite;
        yield return new WaitForSeconds(reactDuration);
        sr.sprite = normalSprite;
    }

    void Update()
    {
        int currentBeat = Mathf.FloorToInt(conductor.songPositionInBeats);

        if (currentBeat != lastBeat)
        {
            lastBeat = currentBeat;
            StartCoroutine(Bob());
        }
    }

    System.Collections.IEnumerator Bob()
    {
        float elapsed = 0f;
        Vector3 start = transform.localPosition;
        Vector3 target = originalPosition + Vector3.up * bobAmount;

        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime * bobSpeed;
            transform.localPosition = Vector3.Lerp(start, target, elapsed / 0.1f);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime * bobSpeed;
            transform.localPosition = Vector3.Lerp(target, originalPosition, elapsed / 0.1f);
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
