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
        private Parameters _parameters;

        /// <summary>
        /// Метод инициализации, выполняемый перед каждым тестом
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _parameters = new Parameters();
        }

        [Test]
        [Description("Свойство NumericalParameters должно быть " +
            "доступно для чтения и возвращать корректный" +
                " словарь параметров")]
        public void Constructor_ShouldInitializeNumericalParametersDefault()
        {
            _parameters.NumericalParameters.Should().NotBeNull();
            _parameters.NumericalParameters.Keys.Should().HaveCount(7);
            _parameters.NumericalParameters.Should().ContainKeys(
                ParameterType.HeightTotal,
                ParameterType.Radius,
                ParameterType.HeightBottom,
                ParameterType.ThicknessLowerEdge,
                ParameterType.ThicknessUpperEdge,
                ParameterType.HeightUpperEdge,
                ParameterType.NumberOfEdge
            );

            _parameters.EdgeType.Should().Be(EdgeType.Rectangular);

            _parameters.GetParameter(ParameterType.HeightTotal)
                .MinValue.Should().Be(100);
            _parameters.GetParameter(ParameterType.HeightTotal)
                .MaxValue.Should().Be(150);
            _parameters.GetParameter(ParameterType.Radius)
                .MinValue.Should().Be(45);
            _parameters.GetParameter(ParameterType.Radius)
                .MaxValue.Should().Be(60);
            _parameters.GetParameter(ParameterType.NumberOfEdge)
                .MinValue.Should().Be(8);
            _parameters.GetParameter(ParameterType.NumberOfEdge)
                .MaxValue.Should().Be(11);
        }

        [Test]
        [Description("EdgeType свойство должно корректно " +
            "устанавливаться и получаться")]
        public void EdgeType_ShouldBeSettableAndGettable()
        {
            _parameters.EdgeType = EdgeType.Oval;
            _parameters.EdgeType.Should().Be(EdgeType.Oval);
            _parameters.EdgeType = EdgeType.Trapezoidal;
            _parameters.EdgeType.Should().Be(EdgeType.Trapezoidal);
        }

        [Test]
        [Description("SetDependencies должен корректно устанавливать" +
            " зависимые границы с указанным minRatio")]
        public void SetDependencies_WithMinRatio_ShouldSetCorrectRange()
        {
            var independentParam = _parameters.GetParameter
                (ParameterType.HeightTotal);
            var dependentParam = _parameters.GetParameter
                (ParameterType.HeightBottom);
            independentParam.Value = 120;
            double maxRatio = 0.3;
            double minRatio = 0.1;

            _parameters.SetDependencies(independentParam, dependentParam,
                maxRatio, minRatio);
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
            var independentParam = _parameters.GetParameter
                (ParameterType.HeightTotal);
            var dependentParam = _parameters.GetParameter
                (ParameterType.HeightBottom);
            independentParam.Value = 120;
            double maxRatio = 0.3;

            _parameters.SetDependencies(independentParam, dependentParam,
                maxRatio, 0);

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
            var dependentParam = _parameters.GetParameter
                (ParameterType.HeightBottom);

            Action act = () => _parameters.SetDependencies
            (null, dependentParam, 0.5, 0.1);

            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("independentParameter");
        }

        [Test]
        [Description("SetDependencies должен выбрасывать" +
            " ArgumentNullException при передаче null в dependentParameter")]
        public void SetDependencies_WithNullDependentParameter()
        {
            var independentParam = _parameters.GetParameter
                (ParameterType.HeightTotal);

            Action act = () => _parameters.SetDependencies
            (independentParam, null, 0.5, 0.1);

            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("dependentParameter");
        }

        [Test]
        [Description("SetDependencies должен выбрасывать" +
            " ArgumentException при maxRatio <= 0")]
        public void SetDependencies_WithInvalidMaxRatio()
        {
            var independentParam = _parameters.GetParameter
                (ParameterType.HeightTotal);
            var dependentParam = _parameters.GetParameter
                (ParameterType.HeightBottom);

            Action act = () => _parameters.SetDependencies
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
            var independentParam = _parameters.GetParameter
                (ParameterType.HeightTotal);
            var dependentParam = _parameters.GetParameter
                (ParameterType.HeightBottom);

            Action act = () => _parameters.SetDependencies
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
            var independentParam = _parameters.GetParameter
                (ParameterType.HeightTotal);
            var dependentParam = _parameters.GetParameter
                (ParameterType.HeightBottom);
            Action act = () => _parameters.SetDependencies
            (independentParam, dependentParam, 0.3, 0.5);

            act.Should().Throw<ArgumentException>()
                .WithMessage("minRatio не может быть больше maxRatio");
        }

        [Test]
        [Description("GetParameter должен возвращать" +
            " корректный параметр по типу")]
        public void GetParameter_ByType_ShouldReturnCorrectParameter()
        {
            var heightTotalParam = _parameters.GetParameter
                (ParameterType.HeightTotal);
            var radiusParam = _parameters.GetParameter
                (ParameterType.Radius);

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
            Action act = () => _parameters.GetParameter((ParameterType)999);
            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        [Description("TryGetParameter должен возвращать true" +
            " и корректный параметр при успешном поиске")]
        public void TryGetParameter_WithExistingType()
        {
            bool result = _parameters.TryGetParameter
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
            bool result = _parameters.TryGetParameter
                ((ParameterType)999, out var parameter);
            result.Should().BeFalse();
            parameter.Should().BeNull();
        }

        [Test]
        [Description("GetRangeString должен возвращать строку" +
            " с диапазоном в правильном формате для мм")]
        public void GetRangeString_ForLengthParameter()
        {
            string rangeString = _parameters.GetRangeString
                (ParameterType.Radius);
            rangeString.Should().Be("от 45,0 до 60,0 мм");
        }

        [Test]
        [Description("GetRangeString должен возвращать строку" +
            " с диапазоном в правильном формате для шт")]
        public void GetRangeString_ForNumberOfEdgeParameter()
        {
            string rangeString = _parameters.GetRangeString
                (ParameterType.NumberOfEdge);
            rangeString.Should().Be("от 8,0 до 11,0 шт.");
        }

        [Test]
        [Description("Validate должен возвращать true" +
            " для валидных параметров")]
        public void Validate_WithValidParameters_ShouldReturnTrue()
        {
            SetValidParameters();
            bool isValid = _parameters.Validate();
            isValid.Should().BeTrue();
        }

        [Test]
        [Description("Validate должен возвращать false" +
            " при возникновении исключения в ValidateFields")]
        public void Validate_WhenExceptionOccurs_ShouldReturnFalse()
        { 
            var parameters = new Parameters();
            parameters.NumericalParameters.Remove(ParameterType.Radius);
            bool isValid = parameters.Validate();
            isValid.Should().BeFalse();
        }

        [Test]
        [Description("ValidateAndThrow не должен выбрасывать" +
            " исключение для валидных параметров")]
        public void ValidateAndThrow_WithValidParameters_ShouldNotThrow()
        {
            SetValidParameters();
            Action act = () => _parameters.ValidateAndThrow();
            act.Should().NotThrow();
        }

        [Test]
        [Description("ValidateAndThrow должен выбрасывать" +
            " исключение при возникновении ошибки в ValidateFields")]
        public void ValidateAndThrow_WhenExceptionOccurs_ShouldThrow()
        {
            var parameters = new Parameters();
            parameters.NumericalParameters.Remove(ParameterType.Radius);
            Action act = () => parameters.ValidateAndThrow();
            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        [Description("TryValidate должен возвращать true и null" +
            " errorMessage для валидных параметров")]
        public void TryValidate_WithValidParameters()
        {
            SetValidParameters();
            bool isValid = _parameters.TryValidate(out string errorMessage);
            isValid.Should().BeTrue();
            errorMessage.Should().BeNull();
        }


        [Test]
        [Description("TryValidate должен возвращать false" +
            " и сообщение об ошибке при возникновении KeyNotFoundException")]
        public void TryValidate_WhenKeyNotFoundExceptionOccurs_ShouldThrow()
        {
            var parameters = new Parameters();
            parameters.NumericalParameters.Remove(ParameterType.Radius);
            Action act = () => parameters.TryValidate(out _);
            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        [Description("Проверка граничных значений через TryValidate")]
        public void TryValidate_WithBoundaryValues()
        {
            SetValidParameters();
            _parameters.GetParameter
                (ParameterType.HeightTotal).Value = 100;
            _parameters.GetParameter
                (ParameterType.Radius).Value = 45;
            _parameters.GetParameter
                (ParameterType.HeightBottom).Value = 10;
            _parameters.GetParameter
                (ParameterType.ThicknessLowerEdge).Value = 2;
            _parameters.GetParameter
                (ParameterType.ThicknessUpperEdge).Value = 4;
            _parameters.GetParameter
                (ParameterType.HeightUpperEdge).Value = 20;
            _parameters.GetParameter
                (ParameterType.NumberOfEdge).Value = 8;

            bool isValid = _parameters.TryValidate(out string errorMessage);
            isValid.Should().BeTrue();
            errorMessage.Should().BeNull();

            _parameters.GetParameter(ParameterType.HeightTotal).Value = 150;
            _parameters.GetParameter(ParameterType.Radius).Value = 60;
            _parameters.GetParameter(ParameterType.HeightBottom).Value = 25;
            _parameters.GetParameter(ParameterType.ThicknessLowerEdge)
                .Value = 5;
            _parameters.GetParameter(ParameterType.ThicknessUpperEdge)
                .Value = 7;
            _parameters.GetParameter(ParameterType.HeightUpperEdge)
                .Value = 40;
            _parameters.GetParameter(ParameterType.NumberOfEdge)
                .Value = 11;

            isValid = _parameters.TryValidate(out errorMessage);
            isValid.Should().BeTrue();
            errorMessage.Should().BeNull();
        }

        [Test]
        [Description("Проверка точности вычисления внутренних радиусов")]
        public void Validate_WithPreciseCalculations_ShouldValidate()
        {
            SetValidParameters();
            _parameters.GetParameter(ParameterType.Radius).Value = 50;
            _parameters.GetParameter(ParameterType.ThicknessUpperEdge)
                .Value = 5;
            _parameters.GetParameter(ParameterType.ThicknessLowerEdge)
                .Value = 3;
            bool isValid = _parameters.Validate();
            isValid.Should().BeTrue();
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
        private void SetValidParameters()
        {
            _parameters.GetParameter(ParameterType.HeightTotal).Value = 120;
            _parameters.GetParameter(ParameterType.Radius).Value = 50;
            _parameters.GetParameter(ParameterType.HeightBottom).Value = 15;
            _parameters.GetParameter(ParameterType.ThicknessLowerEdge)
                .Value = 3;
            _parameters.GetParameter(ParameterType.ThicknessUpperEdge)
                .Value = 5;
            _parameters.GetParameter(ParameterType.HeightUpperEdge)
                .Value = 30;
            _parameters.GetParameter(ParameterType.NumberOfEdge).Value = 10;
        }
    }
}