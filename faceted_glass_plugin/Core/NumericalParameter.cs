using System;

public class NumericalParameter
{
    private double _minValue;
    private double _maxValue;
    private double _value;

    public NumericalParameter(double minValue, double maxValue)
    {
        if (minValue > maxValue)
            throw new ArgumentException("Минимальное значение не " +
                "может быть больше максимального");

        _minValue = minValue;
        _maxValue = maxValue;
        _value = minValue;
    }

    public double MinValue => _minValue;
    public double MaxValue => _maxValue;

    public double Value
    {
        get => _value;
        set
        {
            if (value < _minValue || value > _maxValue)
                throw new ArgumentException($"Значение {value} выходит" +
                    $" за границы [{_minValue}, {_maxValue}]");
            _value = value;
        }
    }

    /// <summary>
    /// Устанавливает новые границы параметра
    /// </summary>
    public void SetRange(double minValue, double maxValue)
    {
        if (minValue > maxValue)
            throw new ArgumentException("Минимальное значение не может " +
                "быть больше максимального");

        _minValue = minValue;
        _maxValue = maxValue;

        if (_value < _minValue)
            _value = _minValue;
        else if (_value > _maxValue)
            _value = _maxValue;
    }

    /// <summary>
    /// Изменяет минимальное значение с сохранением корректности
    /// </summary>
    public void SetMinValue(double value)
    {
        if (value > _maxValue)
            throw new ArgumentException($"Новое минимальное значение " +
                $"{value} не может быть больше текущего максимума" +
                    $" {_maxValue}");

        _minValue = value;
        if (_value < _minValue)
            _value = _minValue;
    }

    /// <summary>
    /// Изменяет максимальное значение с сохранением корректности
    /// </summary>
    public void SetMaxValue(double value)
    {
        if (value < _minValue)
            throw new ArgumentException($"Новый максимум {value} " +
                $"не может быть меньше текущего минимума {_minValue}");

        _maxValue = value;
        if (_value > _maxValue)
            _value = _maxValue;
    }
}