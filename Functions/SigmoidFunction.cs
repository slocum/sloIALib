using System;

namespace SloIALib.Functions
{
    public class SigmoidFunction : ActivationFunction
    {
        #region Constantes
        #endregion

        #region Enumérations
        #endregion

        #region Variables
        #endregion

        #region Attributs et Accesseurs
        #endregion

        #region Constructeurs
        #endregion

        #region Interface

        public override double Fonction(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-1.0 * x));
        }
        #endregion

        #region Implémentation
        #endregion

    }
}
