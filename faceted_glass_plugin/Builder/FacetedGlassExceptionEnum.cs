using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassPlugin
{
    /// <summary>
    /// Перечисление ошибок
    /// </summary>
    public enum FacetedGlassExceptionType
    {
        /// <summary>
        /// Неверное значение общей высоты
        /// </summary>
        HeightTotalInvalid,

        /// <summary>
        /// Неверное значение радиуса
        /// </summary>
        RadiusInvalid,

        /// <summary>
        /// Неверное значение высоты дна
        /// </summary>
        HeightBottomInvalid,

        /// <summary>
        /// Неверное значение толщины нижней стенки
        /// </summary>
        ThicknessLowerEdgeInvalid,

        /// <summary>
        /// Неверное значение толщины верхней стенки
        /// </summary>
        ThicknessUpperEdgeInvalid,

        /// <summary>
        /// Неверное значение высоты верхней стенки
        /// </summary>
        HeightUpperEdgeInvalid,

        /// <summary>
        /// Неверное количество граней
        /// </summary>
        NumberOfEdgesInvalid,

        /// <summary>
        /// Не удалось подключиться к КОМПАС
        /// </summary>
        KompasConnectionFailed,

        /// <summary>
        /// Не удалось создать деталь
        /// </summary>
        PartCreationFailed,

        /// <summary>
        /// Не удалось создать касательную плоскость
        /// </summary>
        TangentPlaneCreationFailed,

        /// <summary>
        /// Неверные параметры стакана
        /// </summary>
        InvalidGlassParameters,

        /// <summary>
        /// Ошибка при построении кругового массива
        /// </summary>
        CircularArrayCreationFailed
    }
}
