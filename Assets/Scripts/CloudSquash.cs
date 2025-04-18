using System.Collections;
using UnityEngine;

public class CloudSquash : MonoBehaviour
{
    public float duration = 0.15f;
    public float squashAmountX = 1.8f;
    public float squashAmountY = 0.8f;

    void Start()
    {
        StartCoroutine(SquashStretch());
    }

    IEnumerator SquashStretch()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 squashed = new Vector3(originalScale.x * squashAmountX, originalScale.y * squashAmountY, originalScale.z);

        float t = 0f;

        // Squash quickly
        while (t < 1f)
        {
            t += Time.deltaTime / (duration / 2f);
            transform.localScale = Vector3.Lerp(originalScale, squashed, t);
            yield return null;
        }

        t = 0f;

        // Stretch back
        while (t < 1f)
        {
            t += Time.deltaTime / (duration / 2f);
            transform.localScale = Vector3.Lerp(squashed, originalScale, t);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}