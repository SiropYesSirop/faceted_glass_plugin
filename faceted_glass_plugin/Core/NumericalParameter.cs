using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Представляет параметр с ограниченным диапазоном значений
    /// </summary>
    public class NumericalParameter
    {
        /// <summary>
        /// Минимально допустимое значение параметра
        /// </summary>
        private double _minValue;

        /// <summary>
        /// Максимально допустимое значение параметра
        /// </summary>
        private double _maxValue;

        /// <summary>
        /// Текущее значение параметра
        /// </summary>
        private double _value;

        /// <summary>
        /// Получает или задаёт минимальное значение параметра
        /// </summary>
        public double MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        /// <summary>
        /// Получает или задаёт максимальное значение параметра
        /// </summary>
        public double MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        /// <summary>
        /// Получает или задает текущее значение параметра
        /// </summary>
        /// <remarks>
        /// При установке значения проверяется его соответствие допустимому диапазону
        /// </remarks>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                Validate(value);
                _value = value;
            }
        }

        /// <summary>
        /// Проверяет введённое значение на соответствие допустимому диапазону
        /// </summary>
        private void Validate(double value)
        {
            if (value < MinValue)
            {
                throw new ArgumentException("Введённое значение слишком маленькое!\n");
            }
            if (value > MaxValue)
            {
                throw new ArgumentException("Введённое значение слишком большое!\n");
            }
        }
    }
}
