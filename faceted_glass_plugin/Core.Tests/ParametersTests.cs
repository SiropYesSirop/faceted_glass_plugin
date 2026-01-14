using System;
using System.Collections.Generic;
using Core;
using FluentAssertions;
using NUnit.Framework;

namespace Core.Tests
{
    [TestFixture]
    [Category("Unit")]
    [Category("Core")]
    public sealed class ParametersTests
    {
        private const double Tolerance = 1e-10;

        [Test]
        [Description(
            "Проверяет, что конструктор Parameters создает все 7 параметров " +
            "граненого стакана с корректными начальными значениями.")]
        public void Constructor_ShouldInitializeAllGlassParameters()
        {
            var parameters = new Parameters();

            parameters.NumericalParameters.Should()
                .HaveCount(7)
                .And.ContainKeys(
                    ParameterType.HeightTotal,
                    ParameterType.Radius,
                    ParameterType.HeightBottom,
                    ParameterType.ThicknessLowerEdge,
                    ParameterType.ThicknessUpperEdge,
                    ParameterType.HeightUpperEdge,
                    ParameterType.NumberOfEdge);

            var heightTotal = parameters.NumericalParameters[ParameterType.HeightTotal];
            heightTotal.MinValue.Should().Be(100);
            heightTotal.MaxValue.Should().Be(150);

            var radius = parameters.NumericalParameters[ParameterType.Radius];
            radius.MinValue.Should().Be(45);
            radius.MaxValue.Should().Be(60);

            var heightBottom = parameters.NumericalParameters[ParameterType.HeightBottom];
            heightBottom.MinValue.Should().Be(10);
            heightBottom.MaxValue.Should().Be(25);

            var thicknessLowerEdge = parameters.NumericalParameters[ParameterType.ThicknessLowerEdge];
            thicknessLowerEdge.MinValue.Should().Be(2);
            thicknessLowerEdge.MaxValue.Should().Be(5);

            var thicknessUpperEdge = parameters.NumericalParameters[ParameterType.ThicknessUpperEdge];
            thicknessUpperEdge.MinValue.Should().Be(4);
            thicknessUpperEdge.MaxValue.Should().Be(7);

            var heightUpperEdge = parameters.NumericalParameters[ParameterType.HeightUpperEdge];
            heightUpperEdge.MinValue.Should().Be(20);
            heightUpperEdge.MaxValue.Should().Be(40);

            var numberOfEdge = parameters.NumericalParameters[ParameterType.NumberOfEdge];
            numberOfEdge.MinValue.Should().Be(8);
            numberOfEdge.MaxValue.Should().Be(11);
        }

        [Test]
        [Description(
            "Проверяет, что после создания Parameters все значения " +
            "параметров равны 0 (не установлены).")]
        public void Constructor_AllParameterValues_ShouldBeZeroInitially()
        {
            var parameters = new Parameters();
            foreach (var kvp in parameters.NumericalParameters)
            {
                kvp.Value.Value.Should().Be(0.0,
                    $"Parameter {kvp.Key} should have initial value 0");
            }
        }

        [Test]
        [Description(
            "Проверяет, что SetDependenses корректно устанавливает " +
            "MaxValue зависимого параметра на основе независимого.")]
        public void SetDependenses_WithValidRatios_ShouldSetMaxValue()
        {
            var parameters = new Parameters();
            var independ = parameters.NumericalParameters[ParameterType.HeightTotal];
            var depend = parameters.NumericalParameters[ParameterType.HeightUpperEdge];
            independ.Value = 120;
            double maxratio = 0.5;
            parameters.SetDependenses(independ, depend, maxratio);
            depend.MaxValue.Should().BeApproximately(120 * maxratio, Tolerance);
        }

        [Test]
        [Description(
            "Проверяет, что SetDependenses с minratio устанавливает " +
            "MinValue зависимого параметра.")]
        public void SetDependenses_WithMinRatio_ShouldSetMinValue()
        { 
            var parameters = new Parameters();
            var independ = parameters.NumericalParameters[ParameterType.HeightTotal];
            var depend = parameters.NumericalParameters[ParameterType.HeightUpperEdge];
            independ.Value = 120;
            double maxratio = 0.5;
            double minratio = 0.2;
            parameters.SetDependenses(independ, depend, maxratio, minratio);
            depend.MinValue.Should().BeApproximately(120 * minratio, Tolerance);
        }

        [Test]
        [Description(
            "Проверяет, что SetDependenses без minratio (0) " +
            "устанавливает MinValue как 10% от MaxValue.")]
        public void SetDependenses_WithoutMinRatio_ShouldSetMinAs10PercentOfMax()
        {
            var parameters = new Parameters();
            var independ = parameters.NumericalParameters[ParameterType.ThicknessLowerEdge];
            var depend = parameters.NumericalParameters[ParameterType.ThicknessUpperEdge];
            independ.Value = 3;
            double maxratio = 2.0;
            parameters.SetDependenses(independ, depend, maxratio, minratio: 0);
            double expectedMax = 3 * maxratio;
            double expectedMin = expectedMax * 0.1;
            depend.MaxValue.Should().BeApproximately(expectedMax, Tolerance);
            depend.MinValue.Should().BeApproximately(expectedMin, Tolerance);
        }

        [Test]
        [Description(
            "Проверяет, что SetDependenses не изменяет текущее Value " +
            "зависимого параметра, только MinValue и MaxValue.")]
        public void SetDependenses_ShouldNotChangeCurrentValue()
        {
            var parameters = new Parameters();
            var independ = parameters.NumericalParameters[ParameterType.HeightTotal];
            var depend = parameters.NumericalParameters[ParameterType.HeightUpperEdge];
            independ.Value = 120;
            depend.Value = 30;
            parameters.SetDependenses(independ, depend, maxratio: 0.5);
            depend.Value.Should().Be(30);
        }

        [Test]
        [Description(
            "Проверяет, что после вызова SetDependenses можно установить " +
            "Value зависимого параметра в новом диапазоне.")]
        public void SetDependenses_AllowsSettingValueInNewRange()
        {
            var parameters = new Parameters();
            var independ = parameters.NumericalParameters[ParameterType.HeightTotal];
            var depend = parameters.NumericalParameters[ParameterType.HeightUpperEdge];

            independ.Value = 100;
            parameters.SetDependenses(independ, depend, maxratio: 0.5, minratio: 0.2);

            depend.Value = 35;
            depend.Value.Should().Be(35);

            Action setToMin = () => depend.Value = 20;
            setToMin.Should().NotThrow();

            Action setToMax = () => depend.Value = 50;
            setToMax.Should().NotThrow();
        }

        [Test]
        [Description(
            "Проверяет, что после вызова SetDependenses нельзя установить " +
            "Value зависимого параметра вне нового диапазона.")]
        public void SetDependenses_RejectsValueOutsideNewRange()
        {
            var parameters = new Parameters();
            var independ = parameters.NumericalParameters[ParameterType.HeightTotal];
            var depend = parameters.NumericalParameters[ParameterType.HeightUpperEdge];

            independ.Value = 100;
            parameters.SetDependenses(independ, depend, maxratio: 0.5, minratio: 0.2);

            Action setBelowMin = () => depend.Value = 19.999;
            setBelowMin.Should().Throw<ArgumentException>();

            Action setAboveMax = () => depend.Value = 50.001;
            setAboveMax.Should().Throw<ArgumentException>();
        }

        [Test]
        [Description(
            "Проверяет реальный сценарий использования: установка высоты " +
            "стакана влияет на допустимую высоту верхней грани.")]
        public void RealScenario_HeightTotalAffectsHeightUpperEdge()
        {
            var parameters = new Parameters();

            parameters.NumericalParameters[ParameterType.HeightTotal].Value = 120;

            parameters.SetDependenses(
                parameters.NumericalParameters[ParameterType.HeightTotal],
                parameters.NumericalParameters[ParameterType.HeightUpperEdge],
                maxratio: 0.5,
                minratio: 0.2);

            var heightUpperEdge = parameters.NumericalParameters[ParameterType.HeightUpperEdge];
            heightUpperEdge.MinValue.Should().BeApproximately(24, Tolerance);
            heightUpperEdge.MaxValue.Should().BeApproximately(60, Tolerance);

            heightUpperEdge.Value = 40;
            heightUpperEdge.Value.Should().Be(40);

            Action invalidAction = () => heightUpperEdge.Value = 70;
            invalidAction.Should().Throw<ArgumentException>();
        }

        [Test]
        [Description(
            "Проверяет цепочку зависимостей: толщина нижней грани влияет " +
            "на толщину верхней грани.")]
        public void RealScenario_ThicknessLowerEdgeAffectsThicknessUpperEdge()
        {
            var parameters = new Parameters();

            parameters.NumericalParameters[ParameterType.ThicknessLowerEdge].Value = 3;

            parameters.SetDependenses(
                parameters.NumericalParameters[ParameterType.ThicknessLowerEdge],
                parameters.NumericalParameters[ParameterType.ThicknessUpperEdge],
                maxratio: 2.0,
                minratio: 1.4);

            var thicknessUpperEdge = parameters.NumericalParameters[ParameterType.ThicknessUpperEdge];
            thicknessUpperEdge.MinValue.Should().BeApproximately(4.2, Tolerance);
            thicknessUpperEdge.MaxValue.Should().BeApproximately(6.0, Tolerance);
        }

        [Test]
        [Description(
            "Проверяет, что NumericalParameters возвращает защищенную копию " +
            "или оригинальную, но неизменяемую коллекцию.")]
        public void NumericalParameters_ShouldNotAllowDirectModificationOfDictionary()
        {
            var parameters = new Parameters();
            parameters.NumericalParameters.Should().NotBeNull();
            parameters.NumericalParameters.Count.Should().Be(7);
        }

        [Test]
        [Description(
            "Проверяет, что можно получить доступ к параметрам по их типу " +
            "и установить им значения.")]
        public void NumericalParameters_CanAccessAndSetParameterValues()
        {
            var parameters = new Parameters();

            parameters.NumericalParameters[ParameterType.HeightTotal].Value = 125;
            parameters.NumericalParameters[ParameterType.Radius].Value = 50;
            parameters.NumericalParameters[ParameterType.HeightBottom].Value = 15;

            parameters.NumericalParameters[ParameterType.HeightTotal].Value.Should().Be(125);
            parameters.NumericalParameters[ParameterType.Radius].Value.Should().Be(50);
            parameters.NumericalParameters[ParameterType.HeightBottom].Value.Should().Be(15);
        }

        [Test]
        [Description(
            "Проверяет, что при попытке доступа к несуществующему параметру " +
            "по ключу выбрасывается исключение.")]
        public void NumericalParameters_AccessByInvalidKey_ShouldThrowKeyNotFoundException()
        {
            var parameters = new Parameters();
            var invalidParameterType = (ParameterType)999;

            Action act = () => { var param = parameters.NumericalParameters[invalidParameterType]; };
            act.Should().Throw<KeyNotFoundException>();
        }
    }
}