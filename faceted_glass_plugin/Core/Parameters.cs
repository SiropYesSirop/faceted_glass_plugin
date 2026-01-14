using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Parameters
    {
        /// <summary>
        /// Конструктор класса Parameters. Содержит инициализацию численных параметров гранёного стакана
        /// </summary>
        public Parameters()
        {
            /// <summary>
            /// Общая высота стакана
            /// </summary>
            NumericalParameter heighttotal = new NumericalParameter();
            heighttotal.MinValue = 100;
            heighttotal.MaxValue = 150;

            /// <summary>
            /// Радиус стакана
            /// </summary>
            NumericalParameter radius = new NumericalParameter();
            radius.MinValue = 45;
            radius.MaxValue = 60;

            /// <summary>
            /// Высота дна стакана
            /// </summary>
            NumericalParameter heightbottom = new NumericalParameter();
            heightbottom.MinValue = 10;
            heightbottom.MaxValue = 25;

            /// <summary>
            /// Толщина нижней стенки стакана
            /// </summary>
            NumericalParameter thicknessloweredge = new NumericalParameter();
            thicknessloweredge.MinValue = 2;
            thicknessloweredge.MaxValue = 5;

            /// <summary>
            /// Толщина верхней стенки стакана
            /// </summary>
            NumericalParameter thicknessupperedge = new NumericalParameter();
            thicknessupperedge.MinValue = 4;
            thicknessupperedge.MaxValue = 7;

            /// <summary>
            /// Высота верхней стенки стакана
            /// </summary>
            NumericalParameter heightupperedge = new NumericalParameter();
            heightupperedge.MinValue = 20;
            heightupperedge.MaxValue = 40;

            /// <summary>
            /// Количество граней стакана
            /// </summary>
            NumericalParameter numberofedge = new NumericalParameter();
            numberofedge.MinValue = 8;
            numberofedge.MaxValue = 11;

            /// <summary>
            /// Занесения значений параметров в словарь с соотвествующими ключами
            /// </summary>
            NumericalParameters = new Dictionary<ParameterType, NumericalParameter>()
            {
                [ParameterType.HeightTotal] = heighttotal,
                [ParameterType.Radius] = radius,
                [ParameterType.HeightBottom] = heightbottom,
                [ParameterType.ThicknessLowerEdge] = thicknessloweredge,
                [ParameterType.ThicknessUpperEdge] = thicknessupperedge,
                [ParameterType.HeightUpperEdge] = heightupperedge,
                [ParameterType.NumberOfEdge] = numberofedge,
            };

        }

        /// <summary>
        /// Словарь параметров гранёного стакана
        /// </summary>
        public Dictionary<ParameterType, NumericalParameter> NumericalParameters { get; set; }

        /// <summary>
        /// Выставляет максимальное и минимальное значения для параметра
        /// </summary>
        /// <param name="independ">Параметр на основе которого будет вычислятся макс и мин значения</param>
        /// <param name="depend">Параметр к которому будет примернятся максимально и минимальное значение</param>
        /// <param name="maxratio">Соотношение велечин для максимального значения</param>
        /// <param name="minratio">Соотношение велечин для минимального значения</param>
        public void SetDependenses(NumericalParameter independ, NumericalParameter depend, double maxratio, double minratio = 0)
        {
            depend.MaxValue = independ.Value * maxratio;
            if (minratio != 0)
            {
                depend.MinValue = independ.Value * minratio;
            }
            else
            {
                depend.MinValue = depend.MaxValue * 0.1;
            }
        }
    }
}
