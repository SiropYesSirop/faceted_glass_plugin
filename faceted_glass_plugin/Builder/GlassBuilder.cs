using Builder;
using Core;
using System;

namespace GlassPlugin
{
    /// <summary>
    /// Класс для построения граненого стакана в КОМПАС-3D
    /// </summary>
    public class GlassBuilder
    {
        /// <summary>
        /// Обертка для работы с API КОМПАС-3D
        /// </summary>
        private KompasWrapper _wrapper;

        /// <summary>
        /// Функция, запускающая и проводящая процесс постройки
        /// граненого стакана
        /// </summary>
        /// <param name="parameters">Параметры для постройки стакана</param>
        public void BuildFacetedGlass(Parameters parameters)
        {
            try
            {
                _wrapper = new KompasWrapper();

                if (!_wrapper.TestConnection())
                {
                    throw new ArgumentException("Не удалось подключиться " +
                        "к КОМПАС. "+"Убедитесь, что КОМПАС 3D установлен!");
                }

                _wrapper.ConnectCAD();
                _wrapper.CreateDocument();

                double heightTotal = parameters.NumericalParameters
                    [ParameterType.HeightTotal].Value;

                double externalRadius = parameters.NumericalParameters
                    [ParameterType.Radius].Value;

                double heightBottom = parameters.NumericalParameters
                    [ParameterType.HeightBottom].Value;

                double thicknessLowerEdge = parameters.NumericalParameters
                    [ParameterType.ThicknessLowerEdge].Value;

                double thicknessUpperEdge = parameters.NumericalParameters
                    [ParameterType.ThicknessUpperEdge].Value;

                double heightUpperEdge = parameters.NumericalParameters
                    [ParameterType.HeightUpperEdge].Value;

                int numberOfEdges = (int)parameters.NumericalParameters
                    [ParameterType.NumberOfEdge].Value;

                ValidateParametersFields(heightTotal, externalRadius, 
                    heightBottom, thicknessLowerEdge,thicknessUpperEdge, 
                        heightUpperEdge, numberOfEdges);

                _wrapper.CreateFacetedGlass(
                    heightTotal: heightTotal,
                    externalRadius: externalRadius,
                    heightBottom: heightBottom,
                    thicknessLowerEdge: thicknessLowerEdge,
                    thicknessUpperEdge: thicknessUpperEdge,
                    heightUpperEdge: heightUpperEdge,
                    numberOfEdges: numberOfEdges
                );

                double cylinderRadius = externalRadius - thicknessUpperEdge;
                double planeAngle = 90;
                double planeHeight = heightBottom;

                var tangentPlane = _wrapper.CreateTangentPlaneToCylinder
                    (cylinderRadius, planeAngle, planeHeight);
                var rectangleSketch = _wrapper.CreateRectangleOnTangentPlane
                    (tangentPlane,heightTotal - heightBottom - heightUpperEdge, 
                        heightBottom, 0);
                var singleEdge = _wrapper.CreateCutExtrusion
                    (rectangleSketch, true, 2, "ПерваяГрань");
                bool arraySuccess = _wrapper.CreateCircularArrayForEdge
                    (singleEdge, numberOfEdges);

                if (tangentPlane != null)
                {
                }
                else
                {
                    throw new Exception
                        ("Не удалось создать касательную плоскость!");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException
                    ($"Ошибка построения стакана:\n{ex.Message}");
            }
        }

        /// <summary>
        /// Проверка параметров на валидность
        /// </summary>
        /// <param name="heightTotal">Общая высота стакана</param>
        /// <param name="externalRadius">Внешний радиус стакана</param>
        /// <param name="heightBottom">Высота дна стакана</param>
        /// <param name="thicknessLowerEdge">Толщина нижней стенки стакана</param>
        /// <param name="thicknessUpperEdge">Толщина верхней стенки стакана</param>
        /// <param name="heightUpperEdge">Высота верхней кромки стакана</param>
        /// <param name="numberOfEdges">Количество граней стакана</param>
        private void ValidateParametersFields(double heightTotal, 
            double externalRadius, double heightBottom,
            double thicknessLowerEdge, double thicknessUpperEdge,
                       double heightUpperEdge, int numberOfEdges)
        {
            if (heightTotal <= 0) throw new ArgumentException
                ("Общая высота должна быть положительной");

            if (externalRadius <= 0) throw new ArgumentException
                ("Внешний радиус должен быть положительным");

            if (heightBottom <= 0) throw new ArgumentException
                ("Высота дна не может быть меньше либо равна нулю!");

            if (thicknessLowerEdge <= 0) throw new ArgumentException
                ("Толщина нижней стенки должна быть положительной");

            if (thicknessUpperEdge <= 0) throw new ArgumentException
                ("Высота верхней стенки не может быть меньше либо равна нулю!");

            if (heightUpperEdge <= 0) throw new ArgumentException
                ("Высота верхней стенки должна быть положительной");

            if (numberOfEdges < 8) throw new ArgumentException
                ("Количество граней должно быть не менее 8");

            if (numberOfEdges > 11) throw new ArgumentException
                ("Количество граней не должно превышать 11");

            if (heightTotal - heightUpperEdge - heightBottom <= 0) 
                throw new ArgumentException
                    ("Высота средней части стакана " +
                        "не может быть меньше либо равна нулю!");

            if (thicknessLowerEdge >= externalRadius)
                throw new ArgumentException ($"Толщина нижней " +
                    $"стенки ({thicknessLowerEdge}) " + $"должна быть меньше" +
                        $" внешнего радиуса ({externalRadius})");

            if (thicknessUpperEdge >= externalRadius)
                throw new ArgumentException
                    ($"Толщина верхней стенки ({thicknessUpperEdge}) " +
                        $"должна быть меньше внешнего радиуса " +
                            $"({externalRadius})");
        }

        /// <summary>
        /// Проверка параметров объекта Parameters
        /// </summary>
        /// <param name="parameters">Объект параметров для проверки</param>
        /// <returns>Результат проверки параметров (true - параметры валидны)</returns>
        public bool ValidateParameters(Parameters parameters)
        {
            try
            {
                double heightTotal = parameters.NumericalParameters
                    [ParameterType.HeightTotal].Value;

                double externalRadius = parameters.NumericalParameters
                    [ParameterType.Radius].Value;

                double heightBottom = parameters.NumericalParameters
                    [ParameterType.HeightBottom].Value;

                double thicknessLowerEdge = parameters.NumericalParameters
                    [ParameterType.ThicknessLowerEdge].Value;

                double thicknessUpperEdge = parameters.NumericalParameters
                    [ParameterType.ThicknessUpperEdge].Value;

                double heightUpperEdge = parameters.NumericalParameters
                    [ParameterType.HeightUpperEdge].Value;

                int numberOfEdges = (int)parameters.NumericalParameters
                    [ParameterType.NumberOfEdge].Value;

                ValidateParametersFields(heightTotal, externalRadius, 
                    heightBottom, thicknessLowerEdge,
                    thicknessUpperEdge, heightUpperEdge, numberOfEdges);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}