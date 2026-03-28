using System;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Предоставляет набор параметров для построения гранёного стакана
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Формат вывода чисел с одним знаком после запятой
        /// </summary>
        private const string NumberFormat = "F1";

        /// <summary>
        /// Инициализирует новый экземпляр класса Parameters
        /// с параметрами по умолчанию
        /// </summary>
        public Parameters()
        {
            var heightTotal = new NumericalParameter(100, 150);
            var radius = new NumericalParameter(45, 60);
            var heightBottom = new NumericalParameter(10, 25);
            var thicknessLowerEdge = new NumericalParameter(2, 5);
            var thicknessUpperEdge = new NumericalParameter(4, 7);
            var heightUpperEdge = new NumericalParameter(20, 40);
            var numberOfEdge = new NumericalParameter(8, 11);
            EdgeType = EdgeType.Rectangular;

            NumericalParameters =
                new Dictionary<ParameterType, NumericalParameter>
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

        /// <summary>
        /// Словарь численных параметров гранёного стакана
        /// </summary>
        public Dictionary<ParameterType, NumericalParameter>
            NumericalParameters { get; set; }

        /// <summary>
        /// Устанавливает зависимые границы для параметра 
        /// на основе другого параметра
        /// </summary>
        /// <param name="independentParameter">Независимый параметр,
        /// от которого зависит диапазон</param>
        /// <param name="dependentParameter">Зависимый параметр,
        /// для которого устанавливается диапазон</param>
        /// <param name="maxRatio">Максимальное отношение зависимого
        /// параметра к независимому</param>
        /// <param name="minRatio">Минимальное отношение зависимого
        /// параметра к независимому (по умолчанию 0)</param>
        /// <exception cref="ArgumentNullException">Если independentParameter
        /// или dependentParameter равен null</exception>
        /// <exception cref="ArgumentException">Если maxRatio <= 0,
        /// minRatio < 0 или minRatio > maxRatio</exception>
        public void SetDependencies(
            NumericalParameter independentParameter,
            NumericalParameter dependentParameter,
            double maxRatio,
            double minRatio = 0)
        {
            if (independentParameter == null)
            {
                throw new ArgumentNullException
                    (nameof(independentParameter));
            }
            if (dependentParameter == null)
            {
                throw new ArgumentNullException
                    (nameof(dependentParameter));
            }
            if (maxRatio <= 0)
            {
                throw new ArgumentException
                    ("Коэффициент maxRatio должен быть больше 0",
                        nameof(maxRatio));
            }
            if (minRatio < 0)
            {
                throw new ArgumentException("Коэффициент minRatio" +
                    " не может быть отрицательным", nameof(minRatio));
            }
            if (minRatio > maxRatio)
            {
                throw new ArgumentException("minRatio не может" +
                    " быть больше maxRatio");
            }

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
        public NumericalParameter GetParameter(ParameterType type)
        {
            return NumericalParameters[type];
        }

        /// <summary>
        /// Пытается получить параметр по его типу
        /// </summary>
        /// <param name="type">Тип параметра</param>
        /// <param name="parameter">Полученный параметр
        /// (null в случае неудачи)</param>
        /// <returns>true, если параметр найден; иначе false</returns>
        public bool TryGetParameter
            (ParameterType type, out NumericalParameter parameter)
        {
            return NumericalParameters.TryGetValue(type, out parameter);
        }

        /// <summary>
        /// Получает строку с диапазоном допустимых значений для параметра
        /// </summary>
        /// <param name="paramType">Тип параметра</param>
        /// <returns>Строка с диапазоном значений в формате
        /// "от X.X до Y.Y мм/шт."</returns>
        public string GetRangeString(ParameterType paramType)
        {
            var param = NumericalParameters[paramType];
            string unit = paramType ==
                ParameterType.NumberOfEdge ? "шт." : "мм";
            return $"от {param.MinValue.ToString(NumberFormat)} " +
                $"до {param.MaxValue.ToString(NumberFormat)} {unit}";
        }

        /// <summary>
        /// Проверяет корректность всех параметров
        /// </summary>
        /// <returns>true, если все параметры валидны; иначе false</returns>
        public bool Validate()
        {
            try
            {
                ValidateFields();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет корректность всех параметров с выбросом исключения
        /// </summary>
        /// <exception cref="ArgumentException">Выбрасывается при
        /// обнаружении невалидных параметров</exception>
        public void ValidateAndThrow()
        {
            ValidateFields();
        }

        /// <summary>
        /// Проверяет корректность параметров и возвращает 
        /// сообщение об ошибке
        /// </summary>
        /// <param name="errorMessage">Сообщение об ошибке при 
        /// невалидных параметрах (null при успехе)</param>
        /// <returns>true, если все параметры валидны; иначе false</returns>
        public bool TryValidate(out string errorMessage)
        {
            ValidateFields();
            errorMessage = null;
            return true;
        }

        /// <summary>
        /// Выполняет внутреннюю валидацию всех параметров
        /// </summary>
        /// <exception cref="ArgumentException">Выбрасывается
        /// при обнаружении невалидных параметров</exception>
        private void ValidateFields()
        {
            double heightTotal = NumericalParameters
                [ParameterType.HeightTotal].Value;
            double externalRadius = NumericalParameters
                [ParameterType.Radius].Value;
            double heightBottom = NumericalParameters
                [ParameterType.HeightBottom].Value;
            double thicknessLowerEdge = NumericalParameters
                [ParameterType.ThicknessLowerEdge].Value;
            double thicknessUpperEdge = NumericalParameters
                [ParameterType.ThicknessUpperEdge].Value;
            double heightUpperEdge = NumericalParameters
                [ParameterType.HeightUpperEdge].Value;
            int numberOfEdges = (int)NumericalParameters
                [ParameterType.NumberOfEdge].Value;
        }
    }
}