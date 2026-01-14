using System;
using Core;
using FluentAssertions;
using NUnit.Framework;

namespace Core.Tests
{
    [TestFixture]
    public sealed class NumericalParameterTests
    {
        private const double Tolerance = 1e-10;

        [Test]
        [Description("Проверяет, что конструктор по умолчанию инициализирует все значения нулями")]
        public void Constructor_DefaultValues_ShouldBeZero()
        {
            var parameter = new NumericalParameter();
            parameter.MinValue.Should().Be(0.0);
            parameter.MaxValue.Should().Be(0.0);
            parameter.Value.Should().Be(0.0);
        }

        [Test]
        [Description("Проверяет, что свойства MinValue, MaxValue и Value могут быть установлены и получены корректно")]
        public void Properties_CanBeSetAndRetrieved()
        {
            var parameter = new NumericalParameter();

            parameter.MinValue = 10.5;
            parameter.MaxValue = 99.9;
            parameter.Value = 50.2;

            parameter.MinValue.Should().BeApproximately(10.5, Tolerance);
            parameter.MaxValue.Should().BeApproximately(99.9, Tolerance);
            parameter.Value.Should().BeApproximately(50.2, Tolerance);
        }

        [Test]
        [Description("Проверяет установку допустимого значения в пределах диапазона MinValue-MaxValue")]
        public void Value_SetValidValue_ShouldSetValue()
        {
            var parameter = new NumericalParameter
            {
                MinValue = 10,
                MaxValue = 100
            };

            parameter.Value = 50;
            parameter.Value.Should().Be(50);
        }

        [Test]
        [Description("Проверяет, что можно установить значение, равное минимальному допустимому значению")]
        public void Value_SetEqualToMinValue_ShouldBeValid()
        {
            var parameter = new NumericalParameter
            {
                MinValue = 10,
                MaxValue = 100
            };

            parameter.Value = 10;
            parameter.Value.Should().Be(10);
        }

        [Test]
        [Description("Проверяет, что можно установить значение, равное максимальному допустимому значению")]
        public void Value_SetEqualToMaxValue_ShouldBeValid()
        {
            var parameter = new NumericalParameter
            {
                MinValue = 10,
                MaxValue = 100
            };

            parameter.Value = 100;
            parameter.Value.Should().Be(100);
        }

        [Test]
        [Description("Проверяет, что попытка установить значение меньше MinValue вызывает исключение ArgumentException")]
        public void Value_SetBelowMinValue_ShouldThrowArgumentException()
        {
            var parameter = new NumericalParameter
            {
                MinValue = 10,
                MaxValue = 100
            };

            Action action = () => parameter.Value = 9.999;

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage("Введённое значение слишком маленькое!");
        }

        [Test]
        [Description("Проверяет, что попытка установить значение больше MaxValue вызывает исключение ArgumentException")]
        public void Value_SetAboveMaxValue_ShouldThrowArgumentException()
        {
            var parameter = new NumericalParameter
            {
                MinValue = 10,
                MaxValue = 100
            };

            Action action = () => parameter.Value = 100.001;

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage("Введённое значение слишком большое!");
        }

        [Test]
        [Description("Проверяет, что при неудачной попытке установки недопустимого значения предыдущее корректное значение сохраняется")]
        public void Value_AfterInvalidSet_ShouldKeepPreviousValidValue()
        {
            var parameter = new NumericalParameter
            {
                MinValue = 10,
                MaxValue = 100
            };

            parameter.Value = 75;
            double originalValue = parameter.Value;

            Action invalidAction = () => parameter.Value = 150;
            invalidAction.Should().Throw<ArgumentException>();

            parameter.Value.Should().Be(originalValue);
        }

        [Test]
        [Description("Параметризованный тест для проверки различных значений в диапазоне 10-100")]
        [TestCase(10.0, true, TestName = "MinValue_ShouldBeValid")]
        [TestCase(100.0, true, TestName = "MaxValue_ShouldBeValid")]
        [TestCase(55.5, true, TestName = "InsideRange_ShouldBeValid")]
        [TestCase(9.999, false, TestName = "JustBelowMin_ShouldThrow")]
        [TestCase(100.001, false, TestName = "JustAboveMax_ShouldThrow")]
        [TestCase(0.0, false, TestName = "FarBelowMin_ShouldThrow")]
        [TestCase(1000.0, false, TestName = "FarAboveMax_ShouldThrow")]
        public void Value_VariousValues_ShouldValidateCorrectly(
            double testValue, bool shouldBeValid)
        {
            var parameter = new NumericalParameter
            {
                MinValue = 10,
                MaxValue = 100
            };

            if (shouldBeValid)
            {
                parameter.Value = testValue;
                parameter.Value.Should().Be(testValue);
            }
            else
            {
                Action act = () => parameter.Value = testValue;
                act.Should().Throw<ArgumentException>();
            }
        }

        [Test]
        [Description("Проверяет работу с отрицательным диапазоном значений (от -100 до -10)")]
        public void Value_NegativeRange_ShouldWorkCorrectly()
        {
            var parameter = new NumericalParameter
            {
                MinValue = -100,
                MaxValue = -10
            };

            parameter.Value = -50;
            parameter.Value.Should().Be(-50);
        }

        [Test]
        [Description("Проверяет, что в отрицательном диапазоне значение ниже MinValue вызывает исключение")]
        public void Value_NegativeRange_InvalidValue_ShouldThrow()
        {
            var parameter = new NumericalParameter
            {
                MinValue = -100,
                MaxValue = -10
            };

            Action act = () => parameter.Value = -101;
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        [Description("Проверяет работу с нулевым диапазоном (MinValue = MaxValue = 0)")]
        public void Value_ZeroRange_ShouldAcceptOnlyZero()
        {
            var parameter = new NumericalParameter
            {
                MinValue = 0,
                MaxValue = 0
            };

            parameter.Value = 0;
            parameter.Value.Should().Be(0);

            Action act = () => parameter.Value = 0.001;
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        [Description("Проверяет работу с очень узким диапазоном значений (1.0001 - 1.0002)")]
        public void Value_VerySmallRange_ShouldValidatePrecisely()
        {
            var parameter = new NumericalParameter
            {
                MinValue = 1.0001,
                MaxValue = 1.0002
            };

            parameter.Value = 1.00015;
            parameter.Value.Should().BeApproximately(1.00015, Tolerance);

            Action act = () => parameter.Value = 1.0003;
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        [Description("Проверяет, что можно установить MinValue больше MaxValue (хотя это логически некорректно)")]
        public void Properties_MinGreaterThanMax_ShouldBeAllowed()
        {
            var parameter = new NumericalParameter();

            parameter.MinValue = 100;
            parameter.MaxValue = 10;

            parameter.MinValue.Should().Be(100);
            parameter.MaxValue.Should().Be(10);
        }

        [Test]
        [Description("Проверяет, что при MinValue > MaxValue любая попытка установить Value вызывает исключение")]
        public void Value_WhenMinGreaterThanMax_AnyValueShouldThrow()
        {
            var parameter = new NumericalParameter
            {
                MinValue = 100,
                MaxValue = 10
            };

            Action act1 = () => parameter.Value = 5;
            act1.Should().Throw<ArgumentException>();

            Action act2 = () => parameter.Value = 50;
            act2.Should().Throw<ArgumentException>();

            Action act3 = () => parameter.Value = 150;
            act3.Should().Throw<ArgumentException>();
        }

        [Test]
        [Description("Проверяет, что несколько неудачных попыток установки значения не меняют текущее корректное значение")]
        public void Value_MultipleInvalidAttempts_ShouldNotChangeValue()
        {
            var parameter = new NumericalParameter
            {
                MinValue = 0,
                MaxValue = 100
            };

            parameter.Value = 50;
            double initialValue = parameter.Value;

            Assert.Throws<ArgumentException>(() => parameter.Value = 150);

            Assert.That(parameter.Value, Is.EqualTo(initialValue));
        }
    }
}