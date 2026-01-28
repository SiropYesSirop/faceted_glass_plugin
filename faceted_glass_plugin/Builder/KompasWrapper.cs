using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Builder
{
    /// <summary>
    /// Обертка для работы с API КОМПАС-3D
    /// </summary>
    /// <remarks>
    /// Класс предоставляет методы для создания и
    /// редактирования 3D-деталей в КОМПАС-3D
    /// </remarks>
    public class KompasWrapper
    {
        /// <summary>
        /// Основной объект КОМПАС-3D
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// 3D-документ для работы
        /// </summary>
        private ksDocument3D _document3D;

        /// <summary>
        /// Деталь в документе
        /// </summary>
        private ksPart _part;

        /// <summary>
        /// Флаг подключения к КОМПАС-3D
        /// </summary>
        private bool _isCadAttached = false;

        /// <summary>
        /// Подключается к КОМПАС-3D
        /// </summary>
        /// <returns>Успешность подключения</returns>
        public bool ConnectCAD()
        {
            try
            {
                if (_kompas != null)
                {
                    _kompas.Visible = true;
                    _kompas.ActivateControllerAPI();
                    _isCadAttached = true;
                    return true;
                }
                try
                {
                    _kompas = (KompasObject)Marshal.GetActiveObject
                        ("KOMPAS.Application.5");
                }
                catch (COMException)
                {
                    var kompasType = Type.GetTypeFromProgID
                        ("KOMPAS.Application.5");
                    if (kompasType == null)
                        return false;

                    _kompas = (KompasObject)Activator.
                        CreateInstance(kompasType);
                }

                if (_kompas == null)
                    return false;

                _kompas.Visible = true;
                _kompas.ActivateControllerAPI();
                _isCadAttached = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Создает новый 3D-документ
        /// </summary>
        /// <returns>Успешность создания документа</returns>
        public bool CreateDocument()
        {
            try
            {
                if (!_isCadAttached && !ConnectCAD())
                    return false;

                _document3D = (ksDocument3D)_kompas.Document3D();
                _document3D.Create();
                _document3D = (ksDocument3D)_kompas.ActiveDocument3D();
                _part = (ksPart)_document3D.GetPart
                    ((short)Part_Type.pTop_Part);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Тестовое подключение к КОМПАС
        /// </summary>
        /// <returns>Успешность тестового подключения</returns>
        public bool TestConnection()
        {
            try
            {
                return ConnectCAD();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Создает новый эскиз на указанной плоскости
        /// </summary>
        /// <param name="planeType">Тип плоскости для создания эскиза</param>
        /// <returns>Созданный эскиз или null в случае ошибки</returns>
        public ksEntity CreateSketch(short planeType)
        {
            try
            {
                var plane = (ksEntity)_part.GetDefaultEntity(planeType);
                var sketch = (ksEntity)_part.NewEntity
                    ((short)Obj3dType.o3d_sketch);
                var sketchDef = (ksSketchDefinition)sketch.GetDefinition();
                sketchDef.SetPlane(plane);
                sketch.Create();
                return sketch;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Создает новый эскиз на смещенной плоскости
        /// </summary>
        /// <param name="basePlaneType">Базовый тип плоскости</param>
        /// <param name="offset">Смещение плоскости от базовой</param>
        /// <returns>Созданный эскиз или null в случае ошибки</returns>
        public ksEntity CreateSketchOnOffsetPlane(short basePlaneType,
            double offset)
        {
            try
            {
                var basePlane = (ksEntity)_part.GetDefaultEntity
                    (basePlaneType);

                var offsetPlane = (ksEntity)_part.NewEntity
                    ((short)Obj3dType.o3d_planeOffset);
                var offsetDef = (ksPlaneOffsetDefinition)offsetPlane.
                    GetDefinition();
                offsetDef.SetPlane(basePlane);
                offsetDef.direction = true;
                offsetDef.offset = offset;
                offsetPlane.Create();

                var sketch = (ksEntity)_part.NewEntity
                    ((short)Obj3dType.o3d_sketch);
                var sketchDef = (ksSketchDefinition)sketch.GetDefinition();
                sketchDef.SetPlane(offsetPlane);
                sketch.Create();
                return sketch;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Начинает редактирование эскиза
        /// </summary>
        /// <param name="sketch">Эскиз для редактирования</param>
        /// <returns>2D-документ для редактирования эскиза</returns>
        public ksDocument2D BeginSketchEdit(ksEntity sketch)
        {
            try
            {
                var sketchDef = (ksSketchDefinition)sketch.GetDefinition();
                return sketchDef.BeginEdit();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Завершает редактирование эскиза
        /// </summary>
        /// <param name="sketch">Эскиз, редактирование 
        /// которого завершается</param>
        public void EndSketchEdit(ksEntity sketch)
        {
            try
            {
                var sketchDef = (ksSketchDefinition)sketch.GetDefinition();
                sketchDef.EndEdit();
            }
            catch { }
        }

        /// <summary>
        /// Рисует окружность в эскизе
        /// </summary>
        /// <param name="doc2D">2D-документ для рисования</param>
        /// <param name="centerX">Координата X центра окружности</param>
        /// <param name="centerY">Координата Y центра окружности</param>
        /// <param name="radius">Радиус окружности</param>
        /// <param name="style">Стиль линии</param>
        public void DrawCircle(ksDocument2D doc2D, double centerX,
            double centerY, double radius, int style = 1)
        {
            try
            {
                doc2D.ksCircle(centerX, centerY, radius, style);
            }
            catch { }
        }

        /// <summary>
        /// Создает операцию выдавливания
        /// </summary>
        /// <param name="sketch">Эскиз для выдавливания</param>
        /// <param name="direction">Направление выдавливания 
        /// (true - прямое)</param>
        /// <param name="depth">Глубина выдавливания</param>
        /// <param name="name">Имя операции</param>
        /// <returns>Созданная операция выдавливания или 
        /// null в случае ошибки</returns>
        public ksEntity CreateExtrusion(ksEntity sketch, bool direction,
            double depth, string name = "")
        {
            try
            {
                var extrude = (ksEntity)_part.NewEntity
                    ((short)Obj3dType.o3d_baseExtrusion);
                var extrudeDef = (ksBaseExtrusionDefinition)extrude.
                    GetDefinition();
                extrudeDef.directionType = (short)Direction_Type.dtNormal;
                extrudeDef.SetSketch(sketch);
                extrudeDef.SetSideParam(direction,
                    (short)End_Type.etBlind, depth, 0, false);
                extrudeDef.SetThinParam(false, 0, 0, 0);
                extrude.Create();

                if (!string.IsNullOrEmpty(name))
                {
                    try
                    {
                        dynamic extrudeDynamic = extrude;
                        extrudeDynamic.Name = name;
                    }
                    catch { }
                }

                return extrude;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Создает операцию вырезания выдавливанием
        /// </summary>
        /// <param name="sketch">Эскиз для вырезания</param>
        /// <param name="direction">Направление вырезания 
        /// (true - прямое)</param>
        /// <param name="depth">Глубина вырезания</param>
        /// <param name="name">Имя операции</param>
        /// <returns>Созданная операция вырезания или 
        /// null в случае ошибки</returns>
        public ksEntity CreateCutExtrusion(ksEntity sketch, bool direction,
            double depth, string name = "")
        {
            try
            {
                var cut = (ksEntity)_part.NewEntity
                    ((short)Obj3dType.o3d_cutExtrusion);
                var cutDef = (ksCutExtrusionDefinition)cut.GetDefinition();

                cutDef.directionType = (short)Direction_Type.dtNormal;
                cutDef.SetSketch(sketch);
                cutDef.SetSideParam(direction,
                    (short)End_Type.etBlind, depth, 0, false);
                cutDef.SetThinParam(false, 0, 0, 0);
                cut.Create();

                if (!string.IsNullOrEmpty(name))
                {
                    try
                    {
                        dynamic cutDynamic = cut;
                        cutDynamic.Name = name;
                    }
                    catch { }
                }

                return cut;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Создает простой цилиндр
        /// </summary>
        /// <param name="name">Имя цилиндра</param>
        /// <param name="radius">Радиус цилиндра</param>
        /// <param name="height">Высота цилиндра</param>
        /// <param name="offsetZ">Смещение по оси Z</param>
        /// <returns>Успешность создания цилиндра</returns>
        public bool CreateCylinder(string name, double radius,
            double height, double offsetZ = 0)
        {
            try
            {
                ksEntity sketch;
                if (offsetZ == 0)
                {
                    sketch = CreateSketch((short)Obj3dType.o3d_planeXOY);
                }
                else
                {
                    sketch = CreateSketchOnOffsetPlane((short)
                        Obj3dType.o3d_planeXOY, offsetZ);
                }

                if (sketch == null)
                {
                    return false;
                }

                var doc2D = BeginSketchEdit(sketch);
                if (doc2D == null)
                {
                    return false;
                }

                DrawCircle(doc2D, 0, 0, radius);
                EndSketchEdit(sketch);

                var extrude = CreateExtrusion(sketch, true, height, name);
                if (extrude == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Вырезает внутреннюю полость в цилиндре (начиная с высоты offsetZ)
        /// </summary>
        /// <param name="name">Имя операции вырезания</param>
        /// <param name="internalRadius">Внутренний радиус полости</param>
        /// <param name="cutHeight">Высота вырезаемой части</param>
        /// <param name="startOffsetZ">Начальная высота вырезания</param>
        /// <returns>Успешность создания полости</returns>
        public bool CutInternalCavity(string name, double internalRadius,
            double cutHeight, double startOffsetZ)
        {
            try
            {
                if (cutHeight <= 0)
                {

                    return false;
                }

                if (internalRadius <= 0)
                {
                    return false;
                }

                var sketch = CreateSketchOnOffsetPlane
                    ((short)Obj3dType.o3d_planeXOY, startOffsetZ);
                if (sketch == null)
                {
                    return false;
                }

                var doc2D = BeginSketchEdit(sketch);
                if (doc2D == null)
                {
                    return false;
                }

                DrawCircle(doc2D, 0, 0, internalRadius);
                EndSketchEdit(sketch);

                var cut = CreateCutExtrusion(sketch, true, cutHeight, name);
                if (cut == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Создает полый стакан (внешняя оболочка + внутренняя полость)
        /// </summary>
        /// <param name="externalRadiusLower">Внешний радиус
        /// нижней части</param>
        /// <param name="externalRadiusUpper">Внешний радиус
        /// верхней части</param>
        /// <param name="internalRadiusLower">Внутренний радиус
        /// нижней части</param>
        /// <param name="internalRadiusUpper">Внутренний радиус
        /// верхней части</param>
        /// <param name="heightBottom">Высота дна</param>
        /// <param name="heightMiddle">Высота средней части</param>
        /// <param name="heightUpper">Высота верхней части</param>
        /// <returns>Успешность создания стакана</returns>
        public bool CreateHollowGlass(double externalRadiusLower,
            double externalRadiusUpper, double internalRadiusLower,
                double internalRadiusUpper, double heightBottom,
                double heightMiddle, double heightUpper)
        {
            try
            {
                bool lowerSuccess = CreateCylinder("ВнешнийНижний",
                    externalRadiusLower, heightBottom, 0);
                if (!lowerSuccess) return false;

                bool middleSuccess = CreateCylinder("ВнешнийСредний",
                    externalRadiusLower, heightMiddle, heightBottom);
                if (!middleSuccess) return false;

                double upperOffset = heightBottom + heightMiddle;
                bool upperSuccess = CreateCylinder("ВнешнийВерхний",
                    externalRadiusUpper, heightUpper, upperOffset);
                if (!upperSuccess) return false;

                double upperCutOffset = heightBottom + heightMiddle
                    + heightUpper;
                bool upperCutSuccess = CutInternalCavity("ПолостьОбщая",
                    internalRadiusUpper, heightMiddle + heightUpper,
                        upperCutOffset);
                if (!upperCutSuccess) return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Рисует отрезок линии
        /// </summary>
        /// <param name="doc2D">2D-документ для рисования</param>
        /// <param name="x1">Координата X начальной точки</param>
        /// <param name="y1">Координата Y начальной точки</param>
        /// <param name="x2">Координата X конечной точки</param>
        /// <param name="y2">Координата Y конечной точки</param>
        /// <param name="style">Стиль линии</param>
        private void DrawLineSeg(ksDocument2D doc2D, double x1, double y1,
            double x2, double y2, int style = 1)
        {
            try
            {
                doc2D.ksLineSeg(x1, y1, x2, y2, style);
            }
            catch { }
        }

        /// <summary>
        /// Создает граненый стакан
        /// </summary>
        /// <param name="heightTotal">Общая высота стакана</param>
        /// <param name="externalRadius">Внешний радиус верхней части</param>
        /// <param name="heightBottom">Высота дна</param>
        /// <param name="thicknessLowerEdge">Толщина нижней грани</param>
        /// <param name="thicknessUpperEdge">Толщина верхней грани</param>
        /// <param name="heightUpperEdge">Высота верхней грани</param>
        /// <param name="numberOfEdges">Количество граней</param>
        public void CreateFacetedGlass(double heightTotal,
            double externalRadius, double heightBottom,
                double thicknessLowerEdge, double thicknessUpperEdge,
                    double heightUpperEdge, int numberOfEdges)
        {
            try
            {
                double internalRadiusUpper = externalRadius -
                    thicknessUpperEdge - thicknessLowerEdge;
                if (internalRadiusUpper <= 0)
                {
                    throw new ArgumentException($"Внутренний радиус верхнего" +
                        $" цилиндра отрицательный: {internalRadiusUpper} мм");
                }

                double externalRadiusLower = externalRadius -
                    thicknessUpperEdge;
                double internalRadiusLower = externalRadiusLower -
                    thicknessLowerEdge;
                if (internalRadiusLower <= 0)
                {
                    throw new ArgumentException($"Внутренний радиус нижнего " +
                        $"цилиндра отрицательный: {internalRadiusLower} мм");
                }

                double heightMiddle = heightTotal - heightBottom -
                    heightUpperEdge;
                if (heightMiddle <= 0)
                {
                    throw new ArgumentException($"Средняя часть стакана " +
                        $"имеет отрицательную высоту: {heightMiddle} мм");
                }

                bool glassSuccess = CreateHollowGlass(externalRadiusLower,
                    externalRadius, internalRadiusLower, internalRadiusUpper,
                    heightBottom, heightMiddle, heightUpperEdge);

                if (!glassSuccess)
                {
                    throw new Exception("Не удалось создать полый стакан");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Создает касательную плоскость к цилиндрической поверхности
        /// </summary>
        /// <param name="cylinderRadius">Радиус цилиндра</param>
        /// <param name="angleDegrees">Угол положения плоскости 
        /// вокруг оси Z (0° = по оси X)</param>
        /// <param name="heightOffset">Высота плоскости от 
        /// плоскости XOY</param>
        /// <returns>Касательная плоскость или null в случае ошибки</returns>
        public ksEntity CreateTangentPlaneToCylinder(double cylinderRadius,
            double angleDegrees = 0, double heightOffset = 0)
        {
            try
            {
                var basePlane = (ksEntity)_part.GetDefaultEntity
                    ((short)Obj3dType.o3d_planeYOZ);
                var anglePlane = (ksEntity)_part.NewEntity
                    ((short)Obj3dType.o3d_planeAngle);
                var angleDef = (ksPlaneAngleDefinition)
                    anglePlane.GetDefinition();

                angleDef.SetPlane(basePlane);
                var axisZ = (ksEntity)_part.GetDefaultEntity
                    ((short)Obj3dType.o3d_axisOZ);
                angleDef.SetAxis(axisZ);

                double angleRad = angleDegrees * Math.PI / 180.0;
                angleDef.angle = angleRad;

                anglePlane.Create();

                var offsetPlane = (ksEntity)_part.NewEntity
                    ((short)Obj3dType.o3d_planeOffset);
                var offsetDef = (ksPlaneOffsetDefinition)
                    offsetPlane.GetDefinition();

                offsetDef.SetPlane(anglePlane);
                offsetDef.direction = true;
                offsetDef.offset = cylinderRadius;

                offsetPlane.Create();

                return offsetPlane;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Ошибка: {ex.Message}");
            }
        }


        /// <summary>
        /// Создает прямоугольный эскиз на касательной плоскости
        /// </summary>
        /// <param name="tangentPlane">Касательная плоскость
        /// к цилиндру</param>
        /// <param name="rectangleHeight">Высота прямоугольника
        /// (вдоль цилиндра)</param>
        /// <param name="heightBottom">Высота дна стакана</param>
        /// <param name="baseOffset">Смещение от основания плоскости
        /// по высоте</param>
        /// <returns>Эскиз прямоугольника или null</returns>
        public ksEntity CreateRectangleOnTangentPlane(ksEntity tangentPlane,
            double rectangleHeight, double heightBottom,
                double baseOffset = 0)
        {
            try
            {
                var sketch = (ksEntity)_part.NewEntity
                    ((short)Obj3dType.o3d_sketch);
                var sketchDef = (ksSketchDefinition)sketch.GetDefinition();

                sketchDef.SetPlane(tangentPlane);
                sketch.Create();

                var doc2D = BeginSketchEdit(sketch);
                if (doc2D == null)
                {
                    throw new ArgumentException
                        ($"Не удалось начать редактирование!");
                }

                double halfWidth = 10;

                double x1 = -halfWidth;
                double y1 = heightBottom;

                double x2 = halfWidth;
                double y2 = heightBottom;

                double x3 = halfWidth;
                double y3 = baseOffset + rectangleHeight + heightBottom;

                double x4 = -halfWidth;
                double y4 = baseOffset + rectangleHeight + heightBottom;

                DrawLineSeg(doc2D, x1, y1, x2, y2, 1);
                DrawLineSeg(doc2D, x2, y2, x3, y3, 1);
                DrawLineSeg(doc2D, x3, y3, x4, y4, 1);
                DrawLineSeg(doc2D, x4, y4, x1, y1, 1);

                EndSketchEdit(sketch);
                return sketch;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Создает круговой массив элементов вокруг оси Z
        /// </summary>
        /// <param name="entityToCopy">Элемент для копирования</param>
        /// <param name="numberOfCopies">Количество копий в массиве</param>
        /// <returns>Успешность создания массива</returns>
        public bool CreateCircularArrayForEdge(ksEntity entityToCopy,
            int numberOfCopies)
        {
            try
            {
                var circularCopy = (ksEntity)_part.NewEntity
                    ((short)Obj3dType.o3d_circularCopy);
                if (circularCopy == null)
                {
                    return false;
                }

                var copyDef = (ksCircularCopyDefinition)
                    circularCopy.GetDefinition();
                if (copyDef == null)
                {
                    return false;
                }

                var axisZ = (ksEntity)_part.GetDefaultEntity
                    ((short)Obj3dType.o3d_axisOZ);
                if (axisZ == null)
                {
                    return false;
                }

                copyDef.SetAxis(axisZ);

                var operations = (ksEntityCollection)
                    copyDef.GetOperationArray();
                if (operations == null)
                {
                    return false;
                }

                operations.Clear();
                operations.Add(entityToCopy);
                bool radialResult = copyDef.SetCopyParamAlongDir
                    (1, 0, false, true);
                dynamic dynamicCopyDef = copyDef;
                dynamicCopyDef.count1 = 1;
                dynamicCopyDef.count2 = numberOfCopies;
                dynamicCopyDef.step2 = 360.0 / numberOfCopies;
                dynamicCopyDef.factor2 = false;
                dynamicCopyDef.inverce = false;
                dynamicCopyDef.geomArray = false;
                bool createResult = circularCopy.Create();

                if (!createResult)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Перечисление для формата экспорта
        /// </summary>
        public enum AdditionFormatType
        {
            /// <summary>
            /// STL формат
            /// </summary>
            FormatSTL = 6
        }

        /// <summary>
        /// Метод экспорта в STL
        /// </summary>
        public bool ExportToStlSimple(string filePath)
        {
            try
            {
                if (_document3D == null)
                    return false;

                filePath = Path.ChangeExtension(filePath, ".stl");

                dynamic doc3D = _document3D;

                try
                {
                    return doc3D.SaveAs(filePath, 6);
                }
                catch
                {
                    try
                    {
                        object paramObj = new { format = 6 };
                        return doc3D.SaveAsToAdditionFormat(filePath,
                            paramObj);
                    }
                    catch
                    {
                        return doc3D.SaveAs(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ExportToStlSimple error: {ex.Message}");
                return false;
            }
        }
    }
}