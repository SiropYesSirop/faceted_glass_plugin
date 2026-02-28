using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Тип боковых граней стакана
    /// </summary>
    public enum EdgeType
    {
        /// <summary>
        /// Прямоугольная грань
        /// </summary>
        Rectangular,

        /// <summary>
        /// Овальная (скруглённая) грань
        /// </summary>
        Oval,

        /// <summary>
        /// Трапециевидная грань (зауженная кверху)
        /// </summary>
        Trapezoidal
    }
}
