using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloIALib.Functions
{
    public abstract class ActivationFunction
    {
        #region Constructeur

        public ActivationFunction()
        {

        }

        #endregion

        #region Interface

        public abstract double Fonction(double x);

        public static ActivationFunction ObjetFonction(Type pTypeFonction)
        {
            if (pTypeFonction == typeof(SigmoidFunction))
            {
                return new SigmoidFunction();
            }
            return null;
        }

        #endregion

    }
}
