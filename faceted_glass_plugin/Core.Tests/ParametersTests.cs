using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Core.Tests
{
    [TestFixture]
    public sealed class ParametersTests
    {
        [Test]
        [Description("Конструктор должен корректно инициализировать" +
            " все числовые параметры с предопределенным набором типов")]
        public void Constructor_ShouldInitializeAllParameters()
        {
            var parameters = new Parameters();

            parameters.NumericalParameters.Should().NotBeNull();
            parameters.NumericalParameters.Count.Should().Be(7);

            parameters.NumericalParameters.Should().ContainKey
                (ParameterType.HeightTotal);
            parameters.NumericalParameters.Should().ContainKey
                (ParameterType.Radius);
            parameters.NumericalParameters.Should().ContainKey
                (ParameterType.HeightBottom);
            parameters.NumericalParameters.Should().ContainKey
                (ParameterType.ThicknessLowerEdge);
            parameters.NumericalParameters.Should().ContainKey
                (ParameterType.ThicknessUpperEdge);
            parameters.NumericalParameters.Should().ContainKey
                (ParameterType.HeightUpperEdge);
            parameters.NumericalParameters.Should().ContainKey
                (ParameterType.NumberOfEdge);
        }

        [Test]
        [Description("Все инициализированные параметры должны иметь" +
            " корректные диапазоны значений, соответствующие требованиям " +
                "предметной области")]
        public void Constructor_ParametersShouldHaveCorrectRanges()
        {
            var parameters = new Parameters();

            parameters.GetParameter(ParameterType.HeightTotal)
                .MinValue.Should().Be(100);
            parameters.GetParameter(ParameterType.HeightTotal)
                .MaxValue.Should().Be(150);

            parameters.GetParameter(ParameterType.Radius)
                .MinValue.Should().Be(45);
            parameters.GetParameter(ParameterType.Radius)
                .MaxValue.Should().Be(60);

            parameters.GetParameter(ParameterType.HeightBottom)
                .MinValue.Should().Be(10);
            parameters.GetParameter(ParameterType.HeightBottom)
                .MaxValue.Should().Be(25);

            parameters.GetParameter(ParameterType.ThicknessLowerEdge)
                .MinValue.Should().Be(2);
            parameters.GetParameter(ParameterType.ThicknessLowerEdge)
                .MaxValue.Should().Be(5);

            parameters.GetParameter(ParameterType.ThicknessUpperEdge)
                .MinValue.Should().Be(4);
            parameters.GetParameter(ParameterType.ThicknessUpperEdge)
                .MaxValue.Should().Be(7);

            parameters.GetParameter(ParameterType.HeightUpperEdge)
                .MinValue.Should().Be(20);
            parameters.GetParameter(ParameterType.HeightUpperEdge)
                .MaxValue.Should().Be(40);

            parameters.GetParameter(ParameterType.NumberOfEdge)
                .MinValue.Should().Be(8);
            parameters.GetParameter(ParameterType.NumberOfEdge)
                .MaxValue.Should().Be(11);
        }

        [Test]
        [Description("Метод GetParameter должен возвращать " +
            "существующий параметр по его типу")]
        public void GetParameter_ExistingParameter_ShouldReturnParameter()
        {
            var parameters = new Parameters();

            var param = parameters.GetParameter(ParameterType.HeightTotal);

            param.Should().NotBeNull();
            param.MinValue.Should().Be(100);
            param.MaxValue.Should().Be(150);
        }

        [Test]
        [Description("Метод TryGetParameter должен возвращать true и" +
            " параметр для существующего типа параметра")]
        public void TryGetParameter_ExistingParameter_ShouldReturn()
        {
            var parameters = new Parameters();

            var success = parameters.TryGetParameter
                (ParameterType.Radius, out var param);

            success.Should().BeTrue();
            param.Should().NotBeNull();
            param.MinValue.Should().Be(45);
            param.MaxValue.Should().Be(60);
        }

        [Test]
        [Description("Метод TryGetParameter должен возвращать false и " +
            "null для несуществующего типа параметра")]
        public void TryGetParameter_NonExistentParameter_ShouldReturnFalse()
        {
            var parameters = new Parameters();

            var dict = parameters.NumericalParameters;
            dict.Remove(ParameterType.Radius);

            var success = parameters.TryGetParameter(ParameterType.Radius,
                out var param);

            success.Should().BeFalse();
            param.Should().BeNull();
        }

        [Test]
        [Description("Метод SetDependencies должен устанавливать зависимые" +
            " диапазоны для параметра на основе независимого параметра и" +
                " заданных коэффициентов")]
        public void SetDependencies_ValidParameters_ShouldSetCorrectRange()
        {
            var parameters = new Parameters();
            var independent = new NumericalParameter(100, 200);
            independent.Value = 150;
            var dependent = new NumericalParameter(0, 100);

            parameters.SetDependencies(independent, dependent, 0.5, 0.2);

            dependent.MinValue.Should().Be(30);
            dependent.MaxValue.Should().Be(75);
        }

        [Test]
        [Description("При установке зависимостей без указания" +
            " минимального коэффициента должен использоваться коэффициент" +
                " по умолчанию (10% от максимального коэффициента)")]
        public void SetDependencies_WithMinRatio_ShouldUseDefaultMinRatio()
        {
            var parameters = new Parameters();
            var independent = new NumericalParameter(100, 200);
            independent.Value = 200;
            var dependent = new NumericalParameter(0, 100);

            parameters.SetDependencies(independent, dependent, 0.4);

            dependent.MinValue.Should().Be(8);
            dependent.MaxValue.Should().Be(80);
        }

        [Test]
        [Description("Метод SetDependencies должен генерировать исключение" +
            " при передаче null в качестве независимого параметра")]
        public void SetDependencies_IndependentNull_ShouldThrowArgumentNull()
        {
            var parameters = new Parameters();
            var dependent = new NumericalParameter(0, 100);

            Action action = () => parameters.SetDependencies(null,
                dependent, 0.5);

            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("*independentParameter*");
        }

        [Test]
        [Description("Метод SetDependencies должен генерировать исключение" +
            " при передаче null в качестве зависимого параметра")]
        public void SetDependencies_DependentNull_ShouldThrowArgumentNulln()
        {
            var parameters = new Parameters();
            var independent = new NumericalParameter(0, 100);

            Action action = () => parameters.SetDependencies(independent,
                null, 0.5);

            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("*dependentParameter*");
        }

        [Test]
        [Description("Метод SetDependencies должен генерировать исключение" +
            " при нулевом максимальном коэффициенте")]
        public void SetDependencies_MaxRatio_ShouldThrowArgumentException()
        {
            var parameters = new Parameters();
            var independent = new NumericalParameter(0, 100);
            var dependent = new NumericalParameter(0, 100);

            Action action = () => parameters.SetDependencies(independent,
                dependent, 0);

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage("Коэффициент maxRatio должен быть больше 0*");
        }

        [Test]
        [Description("Метод SetDependencies должен генерировать исключение "+
            "при отрицательном максимальном коэффициенте")]
        public void SetDependencies_MaxRatioNegative_ShouldThrowArgument()
        {
            var parameters = new Parameters();
            var independent = new NumericalParameter(0, 100);
            var dependent = new NumericalParameter(0, 100);

            Action action = () => parameters.SetDependencies(independent,
                dependent, -0.5);

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage("Коэффициент maxRatio должен быть больше 0*");
        }

        [Test]
        [Description("Метод SetDependencies должен генерировать исключение" +
            " при отрицательном минимальном коэффициенте")]
        public void SetDependencies_MinRatioNegative_ShouldThrowArgument()
        {
            var parameters = new Parameters();
            var independent = new NumericalParameter(0, 100);
            var dependent = new NumericalParameter(0, 100);

            Action action = () => parameters.SetDependencies(independent,
                dependent, 0.5, -0.1);

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage("Коэффициент minRatio не может " +
                    "быть отрицательным*");
        }

        [Test]
        [Description("Метод SetDependencies должен генерировать " +
            "исключение, когда минимальный коэффициент " +
                "превышает максимальный коэффициент")]
        public void SetDependencies_MinRatioGreaterThanMaxRatio()
        {
            var parameters = new Parameters();
            var independent = new NumericalParameter(0, 100);
            var dependent = new NumericalParameter(0, 100);

            Action action = () => parameters.SetDependencies(independent,
                dependent, 0.5, 0.6);

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage("minRatio не может быть больше maxRatio");
        }

        [Test]
        [Description("Метод SetDependencies должен корректно работать," +
            " когда минимальный коэффициент равен максимальному " +
                "коэффициенту (диапазон вырождается в точку)")]
        public void SetDependencies_MinRatioEqualToMaxRatio_ShouldWork()
        {
            var parameters = new Parameters();
            var independent = new NumericalParameter(100, 200);
            independent.Value = 150;
            var dependent = new NumericalParameter(0, 100);

            parameters.SetDependencies(independent, dependent, 0.5, 0.5);

            dependent.MinValue.Should().Be(75);
            dependent.MaxValue.Should().Be(75);
        }

        [Test]
        [Description("Метод SetDependencies должен корректно вычислять " +
            "диапазоны для различных значений независимого параметра")]
        public void SetDependencies_ShouldWorkWithDifferentIndependentValue()
        {
            var parameters = new Parameters();
            var independent = new NumericalParameter(0, 1000);
            var dependent = new NumericalParameter(0, 100);

            independent.Value = 500;
            parameters.SetDependencies(independent, dependent, 0.2, 0.1);
            dependent.MinValue.Should().Be(50);
            dependent.MaxValue.Should().Be(100);

            independent.Value = 100;
            parameters.SetDependencies(independent, dependent, 0.5, 0.3);
            dependent.MinValue.Should().Be(30);
            dependent.MaxValue.Should().Be(50);
        }

        [Test]
        [Description("Свойство NumericalParameters должно позволять" +
            " установку нового словаря параметров")]
        public void NumericalParameters_Property_ShouldBeSettable()
        {
            var parameters = new Parameters();
            var newDictionary = new Dictionary<ParameterType,
                NumericalParameter>
            {
                [ParameterType.HeightTotal] = new NumericalParameter(1, 2)
            };

            parameters.NumericalParameters = newDictionary;

            parameters.NumericalParameters.Should().BeSameAs(newDictionary);
            parameters.NumericalParameters[ParameterType.HeightTotal]
                .MinValue.Should().Be(1);
        }

        [Test]
        [Description("Свойство NumericalParameters должно быть доступно" +
            " для чтения и возвращать корректный словарь параметров")]
        public void NumericalParameters_Property_ShouldBeGettable()
        {
            var parameters = new Parameters();

            var dict = parameters.NumericalParameters;

            dict.Should().NotBeNull();
            dict.Should().HaveCount(7);
        }
    }
}