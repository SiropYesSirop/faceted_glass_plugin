using System;

namespace GlassPlugin
{
    /// <summary>
    /// Типы исключений для гранёного стакана
    /// </summary>
    public enum FacetedGlassExceptionType
    {
        /// <summary>
        /// Неверное значение общей высоты
        /// </summary>
        HeightTotalInvalid,

        /// <summary>
        /// Неверное значение радиуса
        /// </summary>
        RadiusInvalid,

        /// <summary>
        /// Неверное значение высоты дна
        /// </summary>
        HeightBottomInvalid,

        /// <summary>
        /// Неверное значение толщины нижней стенки
        /// </summary>
        ThicknessLowerEdgeInvalid,

        /// <summary>
        /// Неверное значение толщины верхней стенки
        /// </summary>
        ThicknessUpperEdgeInvalid,

        /// <summary>
        /// Неверное значение высоты верхней стенки
        /// </summary>
        HeightUpperEdgeInvalid,

        /// <summary>
        /// Неверное количество граней
        /// </summary>
        NumberOfEdgesInvalid,

        /// <summary>
        /// Не удалось подключиться к КОМПАС
        /// </summary>
        KompasConnectionFailed,

        /// <summary>
        /// Не удалось создать деталь
        /// </summary>
        PartCreationFailed,

        /// <summary>
        /// Не удалось создать касательную плоскость
        /// </summary>
        TangentPlaneCreationFailed,

        /// <summary>
        /// Неверные параметры стакана
        /// </summary>
        InvalidGlassParameters,

        /// <summary>
        /// Ошибка при построении кругового массива
        /// </summary>
        CircularArrayCreationFailed
    }

    /// <summary>
    /// Пользовательское исключение для гранёного стакана
    /// </summary>
    public class FacetedGlassException : Exception
    {
        /// <summary>
        /// Тип исключения
        /// </summary>
        public FacetedGlassExceptionType ExceptionType { get; }

        /// <summary>
        /// Значение параметра
        /// </summary>
        public object ParameterValue { get; }

        /// <summary>
        /// Создает новое исключение для гранёного стакана
        /// </summary>
        public FacetedGlassException(
            FacetedGlassExceptionType exceptionType,
            string message,
            string parameterName = null,
            object parameterValue = null)
            : base(message)
        {
            ExceptionType = exceptionType;
            ParameterValue = parameterValue;
        }

        /// <summary>
        /// Создает исключение с автоматическим сообщением
        /// </summary>
        public static FacetedGlassException Create(
            FacetedGlassExceptionType exceptionType,
            string parameterName = null,
            object parameterValue = null)
        {
            var message = GetDefaultMessage(exceptionType, parameterValue);
            return new FacetedGlassException(exceptionType, message,
                parameterName, parameterValue);
        }

        /// <summary>
        /// Получает сообщение по умолчанию для типа исключения
        /// </summary>
        private static string GetDefaultMessage(
            FacetedGlassExceptionType exceptionType,
            object parameterValue)
        {
            switch (exceptionType)
            {
                case FacetedGlassExceptionType.HeightTotalInvalid:
                    return $"Общая высота должна быть положительной" +
                        $" (значение: {parameterValue})";

                case FacetedGlassExceptionType.RadiusInvalid:
                    return $"Радиус должен быть положительным" +
                        $" (значение: {parameterValue})";

                case FacetedGlassExceptionType.HeightBottomInvalid:
                    return $"Высота дна должна быть положительной" +
                        $" (значение: {parameterValue})";

                case FacetedGlassExceptionType.ThicknessLowerEdgeInvalid:
                    return $"Толщина нижней стенки должна быть" +
                        $" положительной и меньше внешнего радиуса" +
                            $" (значение: {parameterValue})";

                case FacetedGlassExceptionType.ThicknessUpperEdgeInvalid:
                    return $"Толщина верхней стенки должна быть" +
                        $" положительной и меньше внешнего радиуса" +
                            $" (значение: {parameterValue})";

                case FacetedGlassExceptionType.HeightUpperEdgeInvalid:
                    return $"Высота верхней стенки должна быть" +
                        $" положительной (значение: {parameterValue})";

                case FacetedGlassExceptionType.NumberOfEdgesInvalid:
                    return $"Количество граней должно быть от 8 до" +
                        $" 11 (значение: {parameterValue})";

                case FacetedGlassExceptionType.KompasConnectionFailed:
                    return "Не удалось подключиться к КОМПАС-3D";

                default:
                    return "Произошла ошибка при построении " +
                        "гранёного стакана";
            }
        }
    }
}