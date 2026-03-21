using Builder;
using Core;
using Kompas6API5;
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
        /// Запускает и выполняет процесс построения граненого стакана
        /// </summary>
        /// <param name="parameters">Параметры для построения стакана</param>
        /// <exception cref="ArgumentException">Выбрасывается
        /// при ошибках подключения к КОМПАС
        /// или построения модели</exception>
        public void BuildFacetedGlass(Parameters parameters)
        {
            try
            {
                _wrapper = new KompasWrapper();

                if (!_wrapper.TestConnection())
                {
                    throw FacetedGlassException.Create(
                        FacetedGlassExceptionType.KompasConnectionFailed);
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
                ksEntity edgeSketch = null;
                string edgeName = "Грань";

                switch (parameters.EdgeType)
                {
                    case EdgeType.Oval:
                        edgeSketch = _wrapper.
                            CreateRoundedRectangleOnTangentPlane(tangentPlane,
                                heightTotal - heightBottom - heightUpperEdge,
                                    heightBottom, 0);
                        edgeName = "ОвальнаяГрань";
                        break;

                    case EdgeType.Trapezoidal:
                        edgeSketch = _wrapper.
                            CreateTrapezoidalOnTangentPlane(tangentPlane,
                                heightTotal - heightBottom - heightUpperEdge,
                                    heightBottom, 0);
                        edgeName = "ТрапециевиднаяГрань";
                        break;

                    case EdgeType.Rectangular:
                    default:
                        edgeSketch = _wrapper.
                            CreateRectangleOnTangentPlane(tangentPlane,
                                heightTotal - heightBottom - heightUpperEdge,
                                    heightBottom, 0);
                        edgeName = "ПрямоугольнаяГрань";
                        break;
                }

                if (edgeSketch == null)
                {
                    throw new Exception
                        ($"Не удалось создать эскиз для грани типа " +
                            $"{parameters.EdgeType}!");
                }

                var singleEdge = _wrapper.CreateCutExtrusion
                    (edgeSketch, true, 2, edgeName);

                if (singleEdge == null)
                {
                    throw new Exception
                        ("Не удалось создать вырезание для грани!");
                }

                bool arraySuccess = _wrapper.
                    CreateCircularArrayForEdge(singleEdge, numberOfEdges);

                if (!arraySuccess)
                {
                    throw new Exception
                        ("Не удалось создать круговой массив граней!");
                }

                if (tangentPlane == null)
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
        /// Проверяет корректность параметров объекта Parameters
        /// </summary>
        /// <param name="parameters">Объект параметров для проверки</param>
        /// <returns>true, если параметры валидны; иначе false</returns>
        public bool ValidateParameters(Parameters parameters)
        {
            return parameters?.Validate() ?? false;
        }
    }
}