using UnityEngine;
using UnityEngine.UI;

public class NitroFuelSlider : MonoBehaviour
{
    [SerializeField] private Slider nitroSlider;

    public void SetFuel(float currentNitro)
    {
            nitroSlider.value = currentNitro;
    }

    public void SetMaxFuel(float maxNitro)
    {
            nitroSlider.maxValue = maxNitro;
    }
}
