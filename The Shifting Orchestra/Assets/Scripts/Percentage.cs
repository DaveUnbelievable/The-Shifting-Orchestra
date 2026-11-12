using System;

public class Percentage
{
    public int RealValue {get; private set;}
    public int PercentageValue {get; private set;}

    private int minValue = 0;
    private int maxValue = 0;

    public Percentage(int minValue = 0, int maxValue = 100)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public void AddValue(int value)
    {
        RealValue += value;
        PercentageValue = Math.Clamp(RealValue, minValue, maxValue);
    }

    public void SetMinValue(int newMinValue)
    {
        minValue = newMinValue;
    }

    public void SetMaxValue(int newMaxValue)
    {
        maxValue = newMaxValue;
    }
}
