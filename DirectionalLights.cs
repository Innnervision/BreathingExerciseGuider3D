using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    public Light sceneLight; // The light you want to control
    public Slider intensitySlider; // Slider for controlling light intensity
    public Slider shadowStrengthSlider; // Slider for controlling shadow strength

    void Start()
    {
        // Set the default values of the sliders based on the current light settings
        intensitySlider.value = sceneLight.intensity;
        shadowStrengthSlider.value = sceneLight.shadowStrength;

        // Add listeners to call the respective methods when sliders are changed
        intensitySlider.onValueChanged.AddListener(SetLightIntensity);
        shadowStrengthSlider.onValueChanged.AddListener(SetShadowStrength);
    }

    // Method to change the light intensity
    public void SetLightIntensity(float value)
    {
        sceneLight.intensity = value;
    }

    // Method to change the shadow strength
    public void SetShadowStrength(float value)
    {
        sceneLight.shadowStrength = value;
    }
}
