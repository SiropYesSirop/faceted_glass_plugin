using System;

/// <summary>
/// Представляет числовой параметр с заданным диапазоном допустимых значений
/// </summary>
public class NumericalParameter
{
    private double _minValue;
    private double _maxValue;
    private double _value;

    /// <summary>
    /// Конструктор класса NumericalParameter.
    /// Инициализирует параметр с указанными границами диапазона
    /// </summary>
    /// <param name="minValue">Минимальное допустимое 
    /// значение параметра</param>
    /// <param name="maxValue">Максимальное допустимое
    /// значение параметра</param>
    /// <exception cref="ArgumentException">Если минимальное значение 
    /// больше максимального</exception>
    public NumericalParameter(double minValue, double maxValue)
    {
        if (minValue > maxValue)
            throw new ArgumentException("Минимальное значение не " +
                "может быть больше максимального");

        _minValue = minValue;
        _maxValue = maxValue;
        _value = minValue;
    }

    /// <summary>
    /// Минимальное допустимое значение параметра
    /// </summary>
    public double MinValue => _minValue;

    /// <summary>
    /// Максимальное допустимое значение параметра
    /// </summary>
    public double MaxValue => _maxValue;

    /// <summary>
    /// Текущее значение параметра
    /// </summary>
    /// <exception cref="ArgumentException">Если присваиваемое значение 
    /// выходит за границы допустимого диапазона</exception>
    public double Value
    {
        get => _value;
        set
        {
            if (value < _minValue || value > _maxValue)
            {
                throw new ArgumentException($"Значение {value} выходит" +
                    $" за границы [{_minValue}, {_maxValue}]");
            }
            _value = value;
        }
    }

    /// <summary>
    /// Устанавливает новые границы параметра
    /// </summary>
    /// <param name="minValue">Новое минимальное значение</param>
    /// <param name="maxValue">Новое максимальное значение</param>
    /// <exception cref="ArgumentException">Если минимальное значение 
    /// больше максимального</exception>
    /// <remarks>Если текущее значение выходит за новые границы,
    /// оно автоматически корректируется до ближайшей границы</remarks>
    public void SetRange(double minValue, double maxValue)
    {
        if (minValue > maxValue)
        {
            throw new ArgumentException("Минимальное значение не может " +
                "быть больше максимального");
        }

        _minValue = minValue;
        _maxValue = maxValue;

        if (_value < _minValue)
        {
            _value = _minValue;
        }
        else if (_value > _maxValue)
        {
            _value = _maxValue;
        }
    }

    /// <summary>
    /// Изменяет минимальное значение с сохранением корректности
    /// </summary>
    /// <param name="value">Новое минимальное значение</param>
    /// <exception cref="ArgumentException">Если новое минимальное значение 
    /// больше текущего максимума</exception>
    /// <remarks>Если текущее значение меньше нового минимума,
    /// оно автоматически увеличивается до нового минимума</remarks>
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
    /// <param name="value">Новое максимальное значение</param>
    /// <exception cref="ArgumentException">Если новый максимум 
    /// меньше текущего минимума</exception>
    /// <remarks>Если текущее значение больше нового максимума,
    /// оно автоматически уменьшается до нового максимума</remarks>
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