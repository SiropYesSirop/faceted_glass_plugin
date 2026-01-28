using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Тип грани стакана
    /// </summary>
    public enum EdgeType
    {
        /// <summary>
        /// Прямоугольная грань
        /// </summary>
        Rectangular,

        /// <summary>
        /// Скруглённая грань
        /// </summary>
        Rounded
    }

    public class Parameters
    {
        /// <summary>
        /// Конструктор класса Parameters. 
        /// Содержит инициализацию численных параметров гранёного стакана
        /// </summary>
        public Parameters()
        {
            //TODO: refactor +
            var heightTotal = new NumericalParameter(100, 150);
            var radius = new NumericalParameter(45, 60);
            var heightBottom = new NumericalParameter(10, 25);
            var thicknessLowerEdge = new NumericalParameter(2, 5);
            var thicknessUpperEdge = new NumericalParameter(4, 7);
            var heightUpperEdge = new NumericalParameter(20, 40);
            var numberOfEdge = new NumericalParameter(8, 11);
            EdgeType = EdgeType.Rectangular;

            /// <summary>
            /// Занесения значений параметров в словарь
            /// с соотвествующими ключами
            /// </summary>
            NumericalParameters = new Dictionary<ParameterType,
                NumericalParameter>()
            {
                [ParameterType.HeightTotal] = heightTotal,
                [ParameterType.Radius] = radius,
                [ParameterType.HeightBottom] = heightBottom,
                [ParameterType.ThicknessLowerEdge] = thicknessLowerEdge,
                [ParameterType.ThicknessUpperEdge] = thicknessUpperEdge,
                [ParameterType.HeightUpperEdge] = heightUpperEdge,
                [ParameterType.NumberOfEdge] = numberOfEdge,
            };
        }

        /// <summary>
        /// Тип боковых граней стакана
        /// </summary>
        public EdgeType EdgeType { get; set; }

        //TODO: RSDN +
        /// <summary>
        /// Словарь параметров гранёного стакана
        /// </summary>
        public Dictionary<ParameterType, NumericalParameter>
            NumericalParameters
        { get; set; }

        //TODO: RSDN +
        /// <summary>
        /// Устанавливает зависимые границы для параметра
        /// на основе другого параметра
        /// </summary>
        /// <param name="independentParameter">Независимый параметр, 
        /// на основе которого вычисляются границы</param>
        /// <param name="dependentParameter">Зависимый параметр, 
        /// для которого устанавливаются границы</param>
        /// <param name="maxRatio">Коэффициент для вычисления 
        /// максимального значения</param>
        /// <param name="minRatio">Коэффициент для вычисления 
        /// минимального значения</param>
        /// <exception cref="ArgumentNullException">Если один 
        /// из параметров равен null</exception>
        /// <exception cref="ArgumentException">Если коэффициенты 
        /// некорректны</exception>
        public void SetDependencies(
            NumericalParameter independentParameter,
            NumericalParameter dependentParameter,
            double maxRatio,
            double minRatio = 0)
        {
            if (independentParameter == null)
                throw new ArgumentNullException(nameof(independentParameter));

            if (dependentParameter == null)
                throw new ArgumentNullException(nameof(dependentParameter));

            if (maxRatio <= 0)
                throw new ArgumentException("Коэффициент maxRatio должен " +
                    "быть больше 0", nameof(maxRatio));

            if (minRatio < 0)
                throw new ArgumentException("Коэффициент minRatio не может" +
                    " быть отрицательным", nameof(minRatio));

            if (minRatio > maxRatio)
                throw new ArgumentException("minRatio не может быть больше" +
                    " maxRatio");

            double currentValue = independentParameter.Value;
            double newMaxValue = currentValue * maxRatio;
            double newMinValue;

            if (minRatio > 0)
            {
                newMinValue = currentValue * minRatio;
            }
            else
            {
                const double defaultMinRatio = 0.1;
                newMinValue = newMaxValue * defaultMinRatio;
            }
            dependentParameter.SetRange(newMinValue, newMaxValue);
        }

        /// <summary>
        /// Получает параметр по его типу
        /// </summary>
        /// <param name="type">Тип параметра</param>
        /// <returns>Численный параметр</returns>
        /// <exception cref="KeyNotFoundException">
        /// Если параметр не найден</exception>
        public NumericalParameter GetParameter(ParameterType type)
        {
            return NumericalParameters[type];
        }

        /// <summary>
        /// Пытается получить параметр по его типу
        /// </summary>
        /// <param name="type">Тип параметра</param>
        /// <param name="parameter">Найденный параметр</param>
        /// <returns>True если параметр найден, иначе False</returns>
        public bool TryGetParameter(ParameterType type, out
            NumericalParameter parameter)
        {
            return NumericalParameters.TryGetValue(type, out parameter);
        }
    }
}
