using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloIALib.Functions
{
    class HeavisideFunction : ActivationFunction
    {
        #region Interface

        public override double Fonction(double x)
        {
            if (x >= 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}
