using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private bool isLightFlicker = true;
    [SerializeField][Range(0f, 1f)] private float lightFlickerIntensity = 0.817f;
    [SerializeField][Range(0f, 0.2f)] private float lightFlickerTimeMin = 0.003f;
    [SerializeField][Range(0f, 0.2f)] private float lightFlickerTimeMax = 0.016f;

    public float lightIntensity;
    private Light2D light2D;
    private float lightFlickerTimer = 0f;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();

        if (light2D == null)
            enabled = false;
    }

    private void Update()
    {
        if (isLightFlicker)
            lightFlickerTimer -= Time.deltaTime;
    }
    private void LateUpdate()
    {
        if (lightFlickerTimer <= 0f && isLightFlicker)
        {
            Flicker();
        }
        else
        {
            light2D.intensity = lightIntensity;
        }
    }
    private void Flicker()
    {
        light2D.intensity = Random.Range(lightIntensity, lightIntensity + (lightIntensity * lightFlickerIntensity));

        lightFlickerTimer = Random.Range(lightFlickerTimeMin, lightFlickerTimeMax);

    }
}
