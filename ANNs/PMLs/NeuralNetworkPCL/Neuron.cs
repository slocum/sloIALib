using SloIALib.Functions;
using System;

namespace SloIALib.ANNs.PMLs.NeuralNetworkPCL
{
    public class Neuron
    {
        #region Variables

        private ActivationFunction _FonctionActivation;

        private double[] _Weights;
        private int _NbreEntrees;

        #endregion

        #region Accesseurs

        private double _Sortie;
        public double Sortie
        {
            get { return _Sortie; }
        }

        #endregion

        #region Constructeur

        public Neuron(ActivationFunction pFonction, int pNbreEntrees)
        {
            _FonctionActivation = pFonction;
            _NbreEntrees = pNbreEntrees;
            _Sortie = Double.NaN;

            Randomize();
        }

        #endregion

        #region Interface

        /// <summary>
        /// Initialise aléatoirement les entrées d'un neurone
        /// </summary>
        /// <remarks>Créée le 05/06/2016 par : JF Enond</remarks>
        public void Randomize()
        {
            Random generator = new Random();

            // Initialisation des poids des entrées du neurone
            _Weights = new double[(_NbreEntrees + 1)];             // +1 pour le biais
            for (int i = 0; i < _Weights.Length; i++)
            {
                _Weights[i] = generator.NextDouble() * 2.0 - 1.0;
            }
        }

        public void Clear()
        {
            _Sortie = Double.NaN;
        }

        public double Weight(int pIndex)
        {
            return _Weights[pIndex];
        }

        //public double Evaluate(Echantillon pEchantillon)
        //{
        //    double[] tbEntrees = pEchantillon.Entrees;
        //    return Evaluate(tbEntrees);
        //}

        /// <summary>
        /// Calcul de la sortie du neurone
        /// </summary>
        /// <param name="pTbEntrees"></param>
        /// <returns></returns>
        public double Evaluate(double[] pTbEntrees)
        {
            if (_Sortie.Equals(Double.NaN))
            {
                // Somme pondérée des entrées
                double x = 0.0;

                for (int i = 0; i < _NbreEntrees; i++)
                {
                    x += pTbEntrees[i] * _Weights[i];
                }
                x += _Weights[_NbreEntrees];                    // Somme pondérée + Biais

                // Sigmoïde - Fonction d'activation
                _Sortie = _FonctionActivation.Fonction(x);
            }

            return _Sortie;
        }

        public void AdjustWeight(int pIndex, double pValue)
        {
            _Weights[pIndex] = pValue;
        }

        #endregion
    }
}
