using System;
using System.Collections.Generic;
using Core;
using FluentAssertions;
using NUnit.Framework;

namespace Core.Tests
{
    [TestFixture]
    public class ParametersTests
    {
        /// <summary>
        /// Вспомогательный метод для создания новых экземпляров Parameters
        /// </summary>
        private static Parameters CreateDefaultParameters()
        {
            return new Parameters();
        }

        /// <summary>
        /// Устанавливает валидные значения параметров для тестирования
        /// </summary>
        /// <remarks>
        /// Значения выбраны в соответствии с допустимыми диапазонами:
        /// <list type="bullet">
        /// <item>Общая высота: 120 мм (диапазон: 100-150 мм)</item>
        /// <item>Радиус: 50 мм (диапазон: 45-60 мм)</item>
        /// <item>Высота дна: 15 мм (диапазон: 10-25 мм)</item>
        /// <item>Толщина нижней стенки: 3 мм (диапазон: 2-5 мм)</item>
        /// <item>Толщина верхней стенки: 5 мм (диапазон: 4-7 мм)</item>
        /// <item>Высота верхней стенки: 30 мм (диапазон: 20-40 мм)</item>
        /// <item>Количество граней: 10 шт. (диапазон: 8-11 шт.)</item>
        /// </list>
        /// </remarks>
        private static void SetValidParameters(Parameters parameters)
        {
            parameters.GetParameter(ParameterType.HeightTotal).Value = 120;
            parameters.GetParameter(ParameterType.Radius).Value = 50;
            parameters.GetParameter(ParameterType.HeightBottom).Value = 15;
            parameters.GetParameter
                (ParameterType.ThicknessLowerEdge).Value = 3;
            parameters.GetParameter
                (ParameterType.ThicknessUpperEdge).Value = 5;
            parameters.GetParameter(ParameterType.HeightUpperEdge).Value = 30;
            parameters.GetParameter(ParameterType.NumberOfEdge).Value = 10;
        }

        [Test]
        [Description("Свойство NumericalParameters должно быть " +
            "доступно для чтения и возвращать корректный " +
            "словарь параметров")]
        public void Constructor_ShouldInitializeNumericalParametersDefault()
        {
            var parameters = CreateDefaultParameters();

            parameters.NumericalParameters.Should().NotBeNull();
            parameters.NumericalParameters.Keys.Should().HaveCount(7);
            parameters.NumericalParameters.Should().ContainKeys(
                ParameterType.HeightTotal,
                ParameterType.Radius,
                ParameterType.HeightBottom,
                ParameterType.ThicknessLowerEdge,
                ParameterType.ThicknessUpperEdge,
                ParameterType.HeightUpperEdge,
                ParameterType.NumberOfEdge
            );

            parameters.EdgeType.Should().Be(EdgeType.Rectangular);

            parameters.GetParameter(ParameterType.HeightTotal)
                .MinValue.Should().Be(100);
            parameters.GetParameter(ParameterType.HeightTotal)
                .MaxValue.Should().Be(150);
            parameters.GetParameter(ParameterType.Radius)
                .MinValue.Should().Be(45);
            parameters.GetParameter(ParameterType.Radius)
                .MaxValue.Should().Be(60);
            parameters.GetParameter(ParameterType.NumberOfEdge)
                .MinValue.Should().Be(8);
            parameters.GetParameter(ParameterType.NumberOfEdge)
                .MaxValue.Should().Be(11);
        }

        [Test]
        [Description("EdgeType свойство должно корректно " +
            "устанавливаться и получаться")]
        public void EdgeType_ShouldBeSettableAndGettable()
        {
            var parameters = CreateDefaultParameters();
            parameters.EdgeType = EdgeType.Oval;
            parameters.EdgeType.Should().Be(EdgeType.Oval);

            parameters.EdgeType = EdgeType.Trapezoidal;
            parameters.EdgeType.Should().Be(EdgeType.Trapezoidal);
        }

        [Test]
        [Description("SetDependencies должен корректно устанавливать" +
            " зависимые границы с указанным minRatio")]
        public void SetDependencies_WithMinRatio_ShouldSetCorrectRange()
        {
            var parameters = CreateDefaultParameters();
            var independentParam = parameters.GetParameter
                (ParameterType.HeightTotal);
            var dependentParam = parameters.GetParameter
                (ParameterType.HeightBottom);
            independentParam.Value = 120;
            double maxRatio = 0.3;
            double minRatio = 0.1;

            parameters.SetDependencies
                (independentParam, dependentParam, maxRatio, minRatio);

            dependentParam.MinValue.Should().Be
                (independentParam.Value * minRatio);
            dependentParam.MaxValue.Should().Be
                (independentParam.Value * maxRatio);
        }

        [Test]
        [Description("SetDependencies должен использовать" +
            " defaultMinRatio (0.1) от maxValue, когда minRatio = 0")]
        public void SetDependencies_WithMinRatioZero_ShouldUseDefaultMin()
        {
            var parameters = CreateDefaultParameters();
            var independentParam = parameters.GetParameter
                (ParameterType.HeightTotal);
            var dependentParam = parameters.GetParameter
                (ParameterType.HeightBottom);
            independentParam.Value = 120;
            double maxRatio = 0.3;

            parameters.SetDependencies
                (independentParam, dependentParam, maxRatio, 0);

            dependentParam.MaxValue.Should().Be
                (independentParam.Value * maxRatio);
            dependentParam.MinValue.Should().Be
                (dependentParam.MaxValue * 0.1);
        }

        [Test]
        [Description("SetDependencies должен выбрасывать" +
            " ArgumentNullException при передаче null" +
            " в independentParameter")]
        public void SetDependencies_WithNullIndependentParameter()
        {
            var parameters = CreateDefaultParameters();
            var dependentParam = parameters.GetParameter
                (ParameterType.HeightBottom);

            Action act = () => parameters.SetDependencies
                (null, dependentParam, 0.5, 0.1);

            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("independentParameter");
        }

        [Test]
        [Description("SetDependencies должен выбрасывать" +
            " ArgumentNullException при передаче null в dependentParameter")]
        public void SetDependencies_WithNullDependentParameter()
        {
            var parameters = CreateDefaultParameters();
            var independentParam = parameters.GetParameter
                (ParameterType.HeightTotal);

            Action act = () => parameters.SetDependencies
                (independentParam, null, 0.5, 0.1);

            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("dependentParameter");
        }

        [Test]
        [Description("SetDependencies должен выбрасывать" +
            " ArgumentException при maxRatio <= 0")]
        public void SetDependencies_WithInvalidMaxRatio()
        {
            var parameters = CreateDefaultParameters();
            var independentParam = parameters.GetParameter
                (ParameterType.HeightTotal);
            var dependentParam = parameters.GetParameter
                (ParameterType.HeightBottom);

            Action act = () => parameters.SetDependencies
                (independentParam, dependentParam, 0, 0.1);

            act.Should().Throw<ArgumentException>()
                .WithParameterName("maxRatio")
                .WithMessage("Коэффициент maxRatio должен быть больше 0*");
        }

        [Test]
        [Description("SetDependencies должен выбрасывать" +
            " ArgumentException при minRatio < 0")]
        public void SetDependencies_WithNegativeMinRatio()
        {
            var parameters = CreateDefaultParameters();
            var independentParam = parameters.GetParameter
                (ParameterType.HeightTotal);
            var dependentParam = parameters.GetParameter
                (ParameterType.HeightBottom);

            Action act = () => parameters.SetDependencies
                (independentParam, dependentParam, 0.5, -0.1);

            act.Should().Throw<ArgumentException>()
                .WithParameterName("minRatio")
                    .WithMessage("Коэффициент minRatio не может" +
                        " быть отрицательным*");
        }

        [Test]
        [Description("SetDependencies должен выбрасывать" +
            " ArgumentException при minRatio > maxRatio")]
        public void SetDependencies_WithMinRatioGreaterThanMax()
        {
            var parameters = CreateDefaultParameters();
            var independentParam = parameters.GetParameter
                (ParameterType.HeightTotal);
            var dependentParam = parameters.GetParameter
                (ParameterType.HeightBottom);

            Action act = () => parameters.SetDependencies
            (independentParam, dependentParam, 0.3, 0.5);

            act.Should().Throw<ArgumentException>()
                .WithMessage("minRatio не может быть больше maxRatio");
        }

        [Test]
        [Description("GetParameter должен возвращать" +
            " корректный параметр по типу")]
        public void GetParameter_ByType_ShouldReturnCorrectParameter()
        {
            var parameters = CreateDefaultParameters();
            var heightTotalParam = parameters.GetParameter
                (ParameterType.HeightTotal);
            var radiusParam = parameters.GetParameter(ParameterType.Radius);

            heightTotalParam.Should().NotBeNull();
            radiusParam.Should().NotBeNull();
            heightTotalParam.MinValue.Should().Be(100);
            radiusParam.MinValue.Should().Be(45);
        }

        [Test]
        [Description("GetParameter должен выбрасывать" +
            " KeyNotFoundException при запросе несуществующего параметра")]
        public void GetParameter_WithNonExistingType()
        {
            var parameters = CreateDefaultParameters();
            Action act = () => parameters.GetParameter((ParameterType)999);

            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        [Description("TryGetParameter должен возвращать true" +
            " и корректный параметр при успешном поиске")]
        public void TryGetParameter_WithExistingType()
        {
            var parameters = CreateDefaultParameters();
            bool result = parameters.TryGetParameter
                (ParameterType.Radius, out var parameter);

            result.Should().BeTrue();
            parameter.Should().NotBeNull();
            parameter.MinValue.Should().Be(45);
        }

        [Test]
        [Description("TryGetParameter должен возвращать false" +
            " и null при отсутствии параметра")]
        public void TryGetParameter_WithNonExistingType()
        {
            var parameters = CreateDefaultParameters();
            bool result = parameters.TryGetParameter
                ((ParameterType)999, out var parameter);

            result.Should().BeFalse();
            parameter.Should().BeNull();
        }

        [Test]
        [Description("GetRangeString должен возвращать строку" +
            " с диапазоном в правильном формате для мм")]
        public void GetRangeString_ForLengthParameter()
        {
            var parameters = CreateDefaultParameters();
            string rangeString = parameters.GetRangeString
                (ParameterType.Radius);

            rangeString.Should().Be("от 45,0 до 60,0 мм");
        }

        [Test]
        [Description("GetRangeString должен возвращать строку" +
            " с диапазоном в правильном формате для шт")]
        public void GetRangeString_ForNumberOfEdgeParameter()
        {
            var parameters = CreateDefaultParameters();

            string rangeString = parameters.GetRangeString
                (ParameterType.NumberOfEdge);

            rangeString.Should().Be("от 8,0 до 11,0 шт.");
        }

        [Test]
        [Description("Validate должен возвращать true" +
            " для валидных параметров")]
        public void Validate_WithValidParameters_ShouldReturnTrue()
        {
            var parameters = CreateDefaultParameters();
            SetValidParameters(parameters);

            bool isValid = parameters.Validate();

            isValid.Should().BeTrue();
        }

        [Test]
        [Description("Validate должен возвращать false" +
            " при возникновении исключения в ValidateFields")]
        public void Validate_WhenExceptionOccurs_ShouldReturnFalse()
        {
            var parameters = CreateDefaultParameters();
            parameters.NumericalParameters.Remove(ParameterType.Radius);

            bool isValid = parameters.Validate();

            isValid.Should().BeFalse();
        }

        [Test]
        [Description("ValidateAndThrow не должен выбрасывать" +
            " исключение для валидных параметров")]
        public void ValidateAndThrow_WithValidParameters_ShouldNotThrow()
        {
            var parameters = CreateDefaultParameters();
            SetValidParameters(parameters);

            Action act = () => parameters.ValidateAndThrow();

            act.Should().NotThrow();
        }

        [Test]
        [Description("ValidateAndThrow должен выбрасывать" +
            " исключение при возникновении ошибки в ValidateFields")]
        public void ValidateAndThrow_WhenExceptionOccurs_ShouldThrow()
        {
            var parameters = CreateDefaultParameters();
            parameters.NumericalParameters.Remove(ParameterType.Radius);

            Action act = () => parameters.ValidateAndThrow();

            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        [Description("TryValidate должен возвращать true и null" +
            " errorMessage для валидных параметров")]
        public void TryValidate_WithValidParameters()
        {
            var parameters = CreateDefaultParameters();
            SetValidParameters(parameters);

            bool isValid = parameters.TryValidate(out string errorMessage);

            isValid.Should().BeTrue();
            errorMessage.Should().BeNull();
        }

        [Test]
        [Description("TryValidate должен выбрасывать исключение" +
            " при возникновении KeyNotFoundException")]
        public void TryValidate_WhenKeyNotFoundExceptionOccurs_ShouldThrow()
        {
            var parameters = CreateDefaultParameters();
            parameters.NumericalParameters.Remove(ParameterType.Radius);

            Action act = () => parameters.TryValidate(out _);

            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        [Description("Проверка граничных значений через TryValidate")]
        public void TryValidate_WithBoundaryValues()
        {
            var parametersMin = CreateDefaultParameters();
            parametersMin.GetParameter(ParameterType.HeightTotal).Value = 100;
            parametersMin.GetParameter(ParameterType.Radius).Value = 45;
            parametersMin.GetParameter(ParameterType.HeightBottom).Value = 10;
            parametersMin.GetParameter(ParameterType.ThicknessLowerEdge)
                .Value = 2;
            parametersMin.GetParameter(ParameterType.ThicknessUpperEdge)
                .Value = 4;
            parametersMin.GetParameter(ParameterType.HeightUpperEdge)
                .Value = 20;
            parametersMin.GetParameter(ParameterType.NumberOfEdge).Value = 8;

            bool isValidMin = parametersMin.TryValidate
                (out string errorMessageMin);
            isValidMin.Should().BeTrue();
            errorMessageMin.Should().BeNull();

            var parametersMax = CreateDefaultParameters();
            parametersMax.GetParameter(ParameterType.HeightTotal).Value = 150;
            parametersMax.GetParameter(ParameterType.Radius).Value = 60;
            parametersMax.GetParameter(ParameterType.HeightBottom).Value = 25;
            parametersMax.GetParameter(ParameterType.ThicknessLowerEdge)
                .Value = 5;
            parametersMax.GetParameter(ParameterType.ThicknessUpperEdge)
                .Value = 7;
            parametersMax.GetParameter(ParameterType.HeightUpperEdge)
                .Value = 40;
            parametersMax.GetParameter(ParameterType.NumberOfEdge).Value = 11;

            bool isValidMax = parametersMax.TryValidate
                (out string errorMessageMax);
            isValidMax.Should().BeTrue();
            errorMessageMax.Should().BeNull();
        }

        [Test]
        [Description("Проверка точности вычисления внутренних радиусов")]
        public void Validate_WithPreciseCalculations_ShouldValidate()
        {
            var parameters = CreateDefaultParameters();
            SetValidParameters(parameters);
            parameters.GetParameter(ParameterType.Radius).Value = 50;
            parameters.GetParameter(ParameterType.ThicknessUpperEdge)
                .Value = 5;
            parameters.GetParameter(ParameterType.ThicknessLowerEdge)
                .Value = 3;
            bool isValid = parameters.Validate();

            isValid.Should().BeTrue();
        }
    }
}