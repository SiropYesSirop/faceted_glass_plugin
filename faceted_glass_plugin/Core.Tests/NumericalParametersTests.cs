using System;
using FluentAssertions;
using NUnit.Framework;
using Core;

namespace Core.Tests
{
    [TestFixture]
    public sealed class NumericalParameterTests
    {
        /// <summary>
        /// Допустимая погрешность при сравнении чисел с плавающей точкой.
        /// Используется для корректного сравнения значений double в тестах
        /// с учетом возможных ошибок округления
        /// </summary>
        private const double Tolerance = 1e-10;

        [Test]
        [Description("При создании объекта с корректным диапазоном, " +
            "свойства должны быть инициализированы правильно: минимальное" +
                " и максимальное значения устанавливаются, а текущее" +
                    " значение становится равным минимальному")]
        public void Constructor_ValidRange_ShouldInitializeCorrectly()
        {
            var parameter = new NumericalParameter(10.5, 99.5);

            parameter.MinValue.Should().BeApproximately(10.5, Tolerance);
            parameter.MaxValue.Should().BeApproximately(99.5, Tolerance);
            parameter.Value.Should().BeApproximately(10.5, Tolerance);
        }

        [Test]
        [Description("При попытке создать объект с некорректным диапазоном" +
            " (минимальное значение больше максимального) должно" +
                " генерироваться исключение с понятным " +
                    "сообщением об ошибке")]
        public void Constructor_InvalidRange_ShouldThrowArgumentException()
        {
            Action action = () => new NumericalParameter(100, 10);

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage("Минимальное значение не может " +
                    "быть больше максимального");
        }

        [Test]
        [Description("Должна быть возможность создания объекта с " +
            "одинаковыми минимальным и максимальным значениями, при" +
                " этом текущее значение также должно равняться" +
                    " этому значению")]
        public void Constructor_MinEqualsMax_ShouldWork()
        {
            var parameter = new NumericalParameter(50, 50);

            parameter.MinValue.Should().Be(50);
            parameter.MaxValue.Should().Be(50);
            parameter.Value.Should().Be(50);
        }

        [Test]
        [Description("При установке корректного значения в пределах " +
            "диапазона, свойство Value должно успешно изменяться")]
        public void Value_SetValidValue_ShouldSetValue()
        {
            var parameter = new NumericalParameter(10, 100);

            parameter.Value = 50;
            parameter.Value.Should().Be(50);
        }

        [Test]
        [Description("Должна быть возможность установки значения," +
            " равного минимальной границе диапазона")]
        public void Value_SetToMinValue_ShouldWork()
        {
            var parameter = new NumericalParameter(10, 100);

            parameter.Value = 10;
            parameter.Value.Should().Be(10);
        }

        [Test]
        [Description("Должна быть возможность установки значения, " +
            "равного максимальной границе диапазона")]
        public void Value_SetToMaxValue_ShouldWork()
        {
            var parameter = new NumericalParameter(10, 100);

            parameter.Value = 100;
            parameter.Value.Should().Be(100);
        }

        [Test]
        [Description("При попытке установить некорректное значение" +
            " (выходящее за пределы диапазона) должно генерироваться " +
                "исключение, а предыдущее корректное значение должно " +
                    "сохраняться")]
        public void Value_AfterInvalidSet_ShouldKeepPreviousValue()
        {
            var parameter = new NumericalParameter(10, 100);
            parameter.Value = 75;
            var originalValue = parameter.Value;

            Action invalidAction = () => parameter.Value = 150;
            invalidAction.Should().Throw<ArgumentException>();

            parameter.Value.Should().Be(originalValue);
        }

        [Test]
        [Description("Должна быть возможность многократного изменения " +
            "значения в пределах допустимого диапазона")]
        public void Value_SetMultipleTimes_ShouldWorkCorrectly()
        {
            var parameter = new NumericalParameter(0, 100);

            parameter.Value = 25;
            parameter.Value.Should().Be(25);

            parameter.Value = 50;
            parameter.Value.Should().Be(50);

            parameter.Value = 75;
            parameter.Value.Should().Be(75);
        }

        [Test]
        [Description("При установке нового корректного диапазона все " +
            "соответствующие свойства должны обновляться, при этом текущее" +
                " значение должно сохраняться, если оно попадает в" +
                    " новый диапазон")]
        public void SetRange_ValidRange_ShouldUpdateAllProperties()
        {
            var parameter = new NumericalParameter(0, 100);
            parameter.Value = 50;

            parameter.SetRange(20, 80);

            parameter.MinValue.Should().Be(20);
            parameter.MaxValue.Should().Be(80);
            parameter.Value.Should().Be(50);
        }

        [Test]
        [Description("При попытке установить некорректный диапазон " +
            "(минимальное значение больше максимального) должно" +
                " генерироваться исключение")]
        public void SetRange_InvalidRange_ShouldThrowArgumentException()
        {
            var parameter = new NumericalParameter(0, 100);

            Action action = () => parameter.SetRange(200, 100);

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage("Минимальное значение не может быть" +
                    " больше максимального");
        }

        [Test]
        [Description("Если текущее значение меньше нового минимального" +
            " значения при изменении диапазона, оно должно автоматически" +
                " корректироваться до нового минимума")]
        public void SetRange_ValueBelowNewMin_ShouldAdjustValueToMin()
        {
            var parameter = new NumericalParameter(0, 100);
            parameter.Value = 10;

            parameter.SetRange(20, 80);

            parameter.Value.Should().Be(20);
        }

        [Test]
        [Description("Если текущее значение больше нового максимального " +
            "значения при изменении диапазона, оно должно автоматически " +
                "корректироваться до нового максимума")]
        public void SetRange_ValueAboveNewMax_ShouldAdjustValueToMax()
        {
            var parameter = new NumericalParameter(0, 100);
            parameter.Value = 90;

            parameter.SetRange(20, 80);

            parameter.Value.Should().Be(80);
        }

        [Test]
        [Description("Если текущее значение находится в пределах нового " +
            "диапазона, оно должно оставаться неизменным " +
                "при обновлении диапазона")]
        public void SetRange_ValueWithinNewRange_ShouldKeepValue()
        {
            var parameter = new NumericalParameter(0, 100);
            parameter.Value = 50;

            parameter.SetRange(20, 80);

            parameter.Value.Should().Be(50);
        }

        [Test]
        [Description("Должна быть возможность корректного обновления" +
            " только минимального значения, при этом максимальное значение" +
                " и текущее значение (если оно в пределах" +
                    " нового диапазона) сохраняются")]
        public void SetMinValue_ValidValue_ShouldUpdateMinValue()
        {
            var parameter = new NumericalParameter(0, 100);
            parameter.Value = 50;

            parameter.SetMinValue(20);

            parameter.MinValue.Should().Be(20);
            parameter.MaxValue.Should().Be(100);
            parameter.Value.Should().Be(50);
        }

        [Test]
        [Description("При попытке установить минимальное значение," +
            " превышающее текущее максимальное значение, должно" +
                " генерироваться исключение с информативным сообщением")]
        public void SetMinValue_GreaterThanMax_ShouldThrowArgumentException()
        {
            var parameter = new NumericalParameter(0, 100);

            Action action = () => parameter.SetMinValue(150);

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage("Новое минимальное значение 150 не может" +
                    " быть больше текущего максимума 100");
        }

        [Test]
        [Description("Если текущее значение меньше нового минимального" +
            " значения, оно должно автоматически корректироваться" +
                " до нового минимума")]
        public void SetMinValue_ValueBelowNewMin_ShouldAdjustValue()
        {
            var parameter = new NumericalParameter(0, 100);
            parameter.Value = 10;

            parameter.SetMinValue(20);

            parameter.Value.Should().Be(20);
        }

        [Test]
        [Description("Должна быть возможность установки минимального " +
                "значения, равного текущему максимальному значению, " +
                    "при этом текущее значение должно корректироваться" +
                        " до этой границы")]
        public void SetMinValue_EqualToMaxValue_ShouldWork()
        {
            var parameter = new NumericalParameter(0, 100);
            parameter.Value = 50;

            parameter.SetMinValue(100);

            parameter.MinValue.Should().Be(100);
            parameter.Value.Should().Be(100);
        }

        [Test]
        [Description("Должна быть возможность корректного обновления" +
            " только максимального значения, при этом минимальное значение " +
                "и текущее значение (если оно в пределах " +
                    "нового диапазона) сохраняются")]
        public void SetMaxValue_ValidValue_ShouldUpdateMaxValue()
        {
            var parameter = new NumericalParameter(0, 100);
            parameter.Value = 50;

            parameter.SetMaxValue(80);

            parameter.MinValue.Should().Be(0);
            parameter.MaxValue.Should().Be(80);
            parameter.Value.Should().Be(50);
        }

        [Test]
        [Description("При попытке установить максимальное значение, меньшее" +
            " текущего минимального значения, должно генерироваться" +
                " исключение с информативным сообщением")]
        public void SetMaxValue_LessThanMin_ShouldThrowArgumentException()
        {
            var parameter = new NumericalParameter(0, 100);

            Action action = () => parameter.SetMaxValue(-10);

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage("Новый максимум -10 не может быть " +
                    "меньше текущего минимума 0");
        }

        [Test]
        [Description("Если текущее значение больше нового максимального" +
            " значения, оно должно автоматически корректироваться" +
                " до нового максимума")]
        public void SetMaxValue_ValueAboveNewMax_ShouldAdjustValue()
        {
            var parameter = new NumericalParameter(0, 100);
            parameter.Value = 90;

            parameter.SetMaxValue(80);

            parameter.Value.Should().Be(80);
        }

        [Test]
        [Description("Должна быть возможность установки максимального" +
            " значения, равного текущему минимальному значению, при этом " +
                "текущее значение должно корректироваться до этой границы")]
        public void SetMaxValue_EqualToMinValue_ShouldWork()
        {
            var parameter = new NumericalParameter(0, 100);
            parameter.Value = 50;

            parameter.SetMaxValue(0);

            parameter.MaxValue.Should().Be(0);
            parameter.Value.Should().Be(0);
        }

        [Test]
        [Description("Все основные свойства (MinValue, MaxValue, Value)" +
            " должны быть доступны для чтения")]
        public void Properties_ShouldBeReadable()
        {
            var parameter = new NumericalParameter(10, 20);

            var min = parameter.MinValue;
            var max = parameter.MaxValue;
            var val = parameter.Value;

            min.Should().Be(10);
            max.Should().Be(20);
            val.Should().Be(10);
        }

        [Test]
        [Description("Класс должен корректно работать с отрицательными" +
            " значениями диапазона, позволяя устанавливать и " +
                "проверять значения в отрицательном диапазоне")]
        public void Value_WithNegativeRange_ShouldWork()
        {
            var parameter = new NumericalParameter(-100, -50);

            parameter.Value = -75;
            parameter.Value.Should().Be(-75);

            parameter.Value = -100;
            parameter.Value.Should().Be(-100);

            parameter.Value = -50;
            parameter.Value.Should().Be(-50);
        }

        [Test]
        [Description("Класс должен корректно работать с дробными" +
            " значениями, включая проверку границ диапазона " +
                "для дробных чисел")]
        public void Value_WithDecimalRange_ShouldWork()
        {
            var parameter = new NumericalParameter(1.5, 2.5);

            parameter.Value = 2.0;
            parameter.Value.Should().Be(2.0);

            Action action = () => parameter.Value = 3.0;
            action.Should().Throw<ArgumentException>();
        }
    }
}