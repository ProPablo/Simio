using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    public Material mat;
    public Light sunlight;
    [Range(0, 1)] public float primaryDayH;
    [Range(0, 1)] public float secondaryDayH;
    [Range(0, 1)] public float primaryDayS;
    [Range(0, 1)] public float primaryDayV;
    [Range(0, 1)] public float secondaryDayS;
    [Range(0, 1)] public float secondaryDayV;
    [Range(0, 1)] public float primaryNightH;
    [Range(0, 1)] public float secondaryNightH;
    [Range(0, 1)] public float primaryNightS;
    [Range(0, 1)] public float primaryNightV;
    [Range(0, 1)] public float secondaryNightS;
    [Range(0, 1)] public float secondaryNightV;

    [Range(0, 1)] public float dayCycle; // Lerped using current x rotation of camera around globe
    public float timeForLoop = 60f;
    private void Update()
    {
        dayCycle = 0.5f * Mathf.Cos(2 * Mathf.PI / timeForLoop * Time.time) + 0.5f;

        var primaryColour = Color.HSVToRGB(KongrooUtils.RemapRange(dayCycle, 0, 1, primaryDayH, primaryNightH), KongrooUtils.RemapRange(dayCycle, 0, 1, primaryDayS, primaryNightS), KongrooUtils.RemapRange(dayCycle, 0, 1, primaryDayV, primaryNightV));
        var secondaryColour = Color.HSVToRGB(KongrooUtils.RemapRange(dayCycle, 0, 1, secondaryDayH, secondaryNightH), KongrooUtils.RemapRange(dayCycle, 0, 1, secondaryDayS, secondaryNightS), KongrooUtils.RemapRange(dayCycle, 0, 1, secondaryDayV, secondaryNightV));
        mat.SetColor("_col1", primaryColour);
        mat.SetColor("_col2", secondaryColour);
        Color.RGBToHSV(secondaryColour, out var H, out _, out _);
        sunlight.color = Color.HSVToRGB(H,0.2f,1);
        sunlight.intensity = -0.5f * Mathf.Cos(2 * Mathf.PI / timeForLoop * Time.time) + 0.75f;
    }
}
