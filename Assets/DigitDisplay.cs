using UnityEngine;
using UnityEngine.UI;

public class DigitDisplay : MonoBehaviour
{
    public Image[] digitImages; // Assign your digit UI Images in order
    public Sprite[] digitSprites; // 0–9 sliced sprites

    public void SetNumber(int number)
    {
        string numberStr = number.ToString();
        numberStr = numberStr.PadLeft(digitImages.Length, ' '); // Pad with space instead of zero

        for (int i = 0; i < digitImages.Length; i++)
        {
            if (i >= numberStr.Length) return;

            char c = numberStr[i];

            // Handle leading empty spaces
            if (c == ' ')
            {
                digitImages[i].enabled = false; // Hide unused digits
                continue;
            }

            digitImages[i].enabled = true;

            int digit = c - '0';
            if (digit >= 0 && digit < digitSprites.Length)
            {
                digitImages[i].sprite = digitSprites[digit];
            }
        }
    }
}