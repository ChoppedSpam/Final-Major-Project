using UnityEngine;

public class GameOverBop : MonoBehaviour
{
    public enum BopType { UpDown, Sway }
    public BopType bopType = BopType.UpDown;

    public float speed = 1f;
    public float intensity = 10f;

    private RectTransform rectTransform;
    private Vector2 originalAnchoredPos;
    private Quaternion originalRotation;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPos = rectTransform.anchoredPosition;
        originalRotation = rectTransform.localRotation;
    }

    void Update()
    {
        switch (bopType)
        {
            case BopType.UpDown:
                float yOffset = Mathf.Sin(Time.unscaledTime * speed) * intensity;
                rectTransform.anchoredPosition = originalAnchoredPos + new Vector2(0, yOffset);
                break;

            case BopType.Sway:
                float angle = Mathf.Sin(Time.unscaledTime * speed) * intensity;
                rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
                break;
        }
    }
}
