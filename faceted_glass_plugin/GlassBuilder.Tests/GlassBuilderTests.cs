using Core;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Reflection;

namespace GlassPlugin.Tests
{
    [TestFixture]
    public class GlassBuilderTests
    {
        private GlassBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new GlassBuilder();
        }

        [Test]
        [Description("Проверяет, что ValidateParameters" +
            " возвращает false для null параметров")]
        public void ValidateParameters_WithNullParameters_ShouldReturnFalse()
        {
            bool result = _builder.ValidateParameters(null);
            result.Should().BeFalse();
        }

        [Test]
        [Description("Проверяет, что ValidateParameters" +
            " возвращает true для корректных параметров")]
        public void ValidateParameters_WithValidParameters_ShouldReturnTrue()
        {
            var parameters = CreateValidParameters();
            bool result = _builder.ValidateParameters(parameters);
            result.Should().BeTrue("Все параметры находятся" +
                " в допустимых диапазонах и удовлетворяют бизнес-правилам");
        }

        [Test]
        [Description("Проверяет приватный метод " +
            "валидации с корректными значениями")]
        public void ValidateParametersFields_WithValidValues_ShouldNotThrow()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                50.0,
                15.0,
                3.0,
                5.0,
                30.0,
                10
            });

            act.Should().NotThrow();
        }

        [Test]
        [Description("Проверяет валидацию на некорректную" +
            " высоту средней части стакана")]
        public void ValidateParametersFields_TotalLowerHeightBottom()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                100.0,
                50.0,
                25,
                3.0,
                5.0,
                75.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage("*Высота средней части стакана" +
                " не может быть меньше либо равна нулю!*");
        }

        [Test]
        [Description("Проверяет валидацию отрицательной общей высоты")]
        public void ValidateParametersFields_NegativeHeightTotal_Throw()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                -10.0,
                50.0,
                15.0,
                3.0,
                5.0,
                30.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<FacetedGlassException>()
                .Which.ExceptionType.Should().Be
                (FacetedGlassExceptionType.HeightTotalInvalid);
        }

        [Test]
        [Description("Проверяет валидацию нулевой общей высоты")]
        public void ValidateParametersFields_ZeroHeightTotal_ShouldThrow()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                0.0,
                50.0,
                15.0,
                3.0,
                5.0,
                30.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<FacetedGlassException>()
                .Which.ExceptionType.Should().Be
                (FacetedGlassExceptionType.HeightTotalInvalid);
        }

        [Test]
        [Description("Проверяет валидацию отрицательного радиуса")]
        public void ValidateParametersFields_NegativeRadius_ShouldThrow()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                -5.0,
                15.0,
                3.0,
                5.0,
                30.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<FacetedGlassException>()
                .Which.ExceptionType.Should().Be
                (FacetedGlassExceptionType.RadiusInvalid);
        }

        [Test]
        [Description("Проверяет валидацию нулевого радиуса")]
        public void ValidateParametersFields_ZeroRadius_ShouldThrow()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                0.0,
                15.0,
                3.0,
                5.0,
                30.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<FacetedGlassException>()
                .Which.ExceptionType.Should().Be
                (FacetedGlassExceptionType.RadiusInvalid);
        }

        [Test]
        [Description("Проверяет, что высота дна не может быть нулевой")]
        public void ValidateParametersFields_ZeroHeightBottom_NotThrow()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                50.0,
                0.0,
                3.0,
                5.0,
                30.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<FacetedGlassException>()
                .Which.ExceptionType.Should().Be
                (FacetedGlassExceptionType.HeightBottomInvalid);
        }

        [Test]
        [Description("Проверяет валидацию нулевой толщины нижней стенки")]
        public void ValidateParametersFields_ZeroThicknessLowerEdge()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                50.0,
                15.0,
                0.0,
                5.0,
                30.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<FacetedGlassException>()
                .Which.ExceptionType.Should().Be
                (FacetedGlassExceptionType.ThicknessLowerEdgeInvalid);
        }

        [Test]
        [Description("Проверяет валидацию отрицательной" +
            " толщины нижней стенки")]
        public void ValidateParametersFields_NegativeThicknessLowerEdge()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                50.0,
                15.0,
                -2.0,
                5.0,
                30.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<FacetedGlassException>()
                .Which.ExceptionType.Should().Be
                (FacetedGlassExceptionType.ThicknessLowerEdgeInvalid);
        }

        [Test]
        [Description("Проверяет валидацию отрицательной" +
            " толщины верхней стенки")]
        public void ValidateParametersFields_NegativeThicknessUpperEdge()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                50.0,
                15.0,
                3.0,
                -2.0,
                30.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<FacetedGlassException>()
                .Which.ExceptionType.Should().Be
                (FacetedGlassExceptionType.ThicknessUpperEdgeInvalid);
        }

        [Test]
        [Description("Проверяет валидацию отрицательной " +
            "высоты верхней стенки")]
        public void ValidateParametersFields_NegativeHeightUpperEdge()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                50.0,
                15.0,
                3.0,
                4.0,
                -2.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<FacetedGlassException>()
                .Which.ExceptionType.Should().Be
                (FacetedGlassExceptionType.HeightUpperEdgeInvalid);
        }

        [Test]
        [Description("Проверяет валидацию слишком малого количества граней")]
        public void ValidateParametersFields_LessThan3Edges_ShouldThrow()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                50.0,
                15.0,
                3.0,
                5.0,
                30.0,
                7
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<FacetedGlassException>()
                .Which.ExceptionType.Should().Be
                (FacetedGlassExceptionType.NumberOfEdgesInvalid);
        }

        [Test]
        [Description("Проверяет валидацию слишком " +
            "большого количества граней")]
        public void ValidateParametersFields_MoreThan20Edges_ShouldThrow()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                50.0,
                15.0,
                3.0,
                5.0,
                30.0,
                12
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<FacetedGlassException>()
                .Which.ExceptionType.Should().Be
                (FacetedGlassExceptionType.NumberOfEdgesInvalid);
        }

        [Test]
        [Description("Проверяет граничный случай: ровно 8 граней")]
        public void ValidateParametersFields_Exactly3Edges_ShouldNotThrow()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                50.0,
                15.0,
                3.0,
                5.0,
                30.0,
                8
            });

            act.Should().NotThrow();
        }

        [Test]
        [Description("Проверяет граничный случай: ровно 11 граней")]
        public void ValidateParametersFields_Exactly20Edges_ShouldNotThrow()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                50.0,
                15.0,
                3.0,
                5.0,
                30.0,
                11
            });

            act.Should().NotThrow();
        }

        [Test]
        [Description("Проверяет бизнес-правило: " +
            "толщина нижней стенки должна быть меньше радиуса")]
        public void ValidateParametersField_ThicknessLowerEdgeGreaterRadius()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                10.0,
                15.0,
                12.0,
                5.0,
                30.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage("*Толщина нижней стенки*");
        }

        [Test]
        [Description("Проверяет бизнес-правило:" +
            " толщина верхней стенки должна быть меньше радиуса")]
        public void ValidateParameters_ThicknessUpperEdgeGreaterThanRadius()
        {
            var privateMethod = GetPrivateValidateMethod();

            Action act = () => privateMethod.Invoke(_builder, new object[]
            {
                120.0,
                10.0,
                15.0,
                3.0,
                12.0,
                30.0,
                10
            });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage("*Толщина верхней стенки*");
        }

        /// <summary>
        /// Получает приватный метод ValidateParameters через рефлексию
        /// </summary>
        private MethodInfo GetPrivateValidateMethod()
        {
            return typeof(GlassBuilder)
                .GetMethod("ValidateParametersFields",
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new Type[] {
                        typeof(double), typeof(double), typeof(double),
                        typeof(double), typeof(double),
                        typeof(double), typeof(int)
                    },
                    null);
        }

        /// <summary>
        /// Создает параметры с корректными значениями
        /// </summary>
        private Parameters CreateValidParameters()
        {
            var parameters = new Parameters();
            parameters.NumericalParameters
                [ParameterType.HeightTotal].Value = 120.0;
            parameters.NumericalParameters
                [ParameterType.Radius].Value = 50.0;
            parameters.NumericalParameters
                [ParameterType.HeightBottom].Value = 15.0;
            parameters.NumericalParameters
                [ParameterType.ThicknessLowerEdge].Value = 3.0;
            parameters.NumericalParameters
                [ParameterType.ThicknessUpperEdge].Value = 5.0;
            parameters.NumericalParameters
                [ParameterType.HeightUpperEdge].Value = 30.0;
            parameters.NumericalParameters
                [ParameterType.NumberOfEdge].Value = 10;

            return parameters;
        }

        [Test]
        [Description("Проверяет, что BuildFacetedGlass" +
            " выбрасывает исключение при null параметрах")]
        public void BuildFacetedGlass_WithNullParameters_ShouldThrow()
        {
            Action act = () => _builder.BuildFacetedGlass(null);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Ошибка построения стакана*");
        }

        [Test]
        [Description("Проверяет, что BuildFacetedGlass" +
            " вызывает валидацию параметров")]
        public void BuildFacetedGlass_ShouldValidateParameters()
        {
            var parameters = CreateValidParameters();

            Action act = () => _builder.BuildFacetedGlass(parameters);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Не удалось подключиться к КОМПАС*");
        }

        [Test]
        [Description("Проверяет дефолтное сообщение " +
            "при неизвестном типе исключения")]
        public void GetDefaultMessage_InvalidEnumValue_ReturnDefaultMessage()
        {
            var methodInfo = typeof(FacetedGlassException)
                .GetMethod("GetDefaultMessage",
                    BindingFlags.NonPublic | BindingFlags.Static);

            methodInfo.Should().NotBeNull("Метод GetDefaultMessage" +
                " должен существовать");

            var invalidEnumValue = (FacetedGlassExceptionType)999;

            string result = (string)methodInfo.Invoke(null, new object[]
            {
                invalidEnumValue,
                123.45
            });

            result.Should().Be("Произошла ошибка при построении" +
                " гранёного стакана");
        }

        [Test]
        [Description("Проверяет, что свойство ParameterValue " +
            "возвращает переданное значение")]
        public void FacetedGlassException_ParameterValue_ReturnCorrectValue()
        {
            var testValue = new { Test = "Value", Number = 123 };
            var exceptionType = FacetedGlassExceptionType.HeightTotalInvalid;

            var exception = new FacetedGlassException(
                exceptionType,
                "Test message",
                "TestParameter",
                testValue);

            exception.ParameterValue.Should().Be(testValue);
            exception.ParameterValue.Should().BeOfType(testValue.GetType());
        }
    }
}