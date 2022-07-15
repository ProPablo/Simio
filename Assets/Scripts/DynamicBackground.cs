using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    public Material mat;
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
    private void Update()
    {
        var primaryColour = Color.HSVToRGB(KongrooUtils.RemapRange(dayCycle, 0, 1, primaryDayH, primaryNightH), KongrooUtils.RemapRange(dayCycle, 0, 1, primaryDayS, primaryNightS), KongrooUtils.RemapRange(dayCycle, 0, 1, primaryDayV, primaryNightV));
        var secondaryColour = Color.HSVToRGB(KongrooUtils.RemapRange(dayCycle, 0, 1, secondaryDayH, secondaryNightH), KongrooUtils.RemapRange(dayCycle, 0, 1, secondaryDayS, secondaryNightS), KongrooUtils.RemapRange(dayCycle, 0, 1, secondaryDayV, secondaryNightV));
        mat.SetColor("_col1", primaryColour);
        mat.SetColor("_col2", secondaryColour);
    }
}
