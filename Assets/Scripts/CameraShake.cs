using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeDuration = 0f;
    private float shakeAmplitude = 2f;
    private float shakeFrequency = 2f;

    void Start()
    {
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float intensity = 1f, float time = 0.2f)
    {
        shakeAmplitude = intensity;
        shakeDuration = time;

        if (noise != null)
        {
            noise.m_AmplitudeGain = shakeAmplitude;
            noise.m_FrequencyGain = shakeFrequency;
        }

        StopAllCoroutines();
        StartCoroutine(ResetShake());
    }

    IEnumerator ResetShake()
    {
        yield return new WaitForSecondsRealtime(shakeDuration);
        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
    }
}
