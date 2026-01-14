using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Перечисление параметров гранёного стакана
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// Общая высота стакана
        /// </summary>
        HeightTotal,

        /// <summary>
        /// Радиус стакана
        /// </summary>
        Radius,

        /// <summary>
        /// Высота дна стакана
        /// </summary>
        HeightBottom,

        /// <summary>
        /// Толщина нижней стенки стакана
        /// </summary>
        ThicknessLowerEdge,

        /// <summary>
        /// Толщина верхней стенки стакана
        /// </summary>
        ThicknessUpperEdge,

        /// <summary>
        /// Высота верхней стенки стакана
        /// </summary>
        HeightUpperEdge,

        /// <summary>
        /// Количество граней стакана
        /// </summary>
        NumberOfEdge
    }
}
