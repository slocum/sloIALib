using System;
using static SloIALib.ANNs.PMLs.Echantillon;

namespace SloIALib.ANNs.PMLs.NeuralNetworkPCL
{
    public class BackPropagationLearning
    {
        #region Constantes

        #endregion

        #region Variables

        private ActivationNetwork _Network;

        private int _NbreEntrees;
        private int _NbreNeuronesCaches;
        private int _NbreNeuronesSortis;

        private double _OldError;

        #endregion

        #region Accesseurs

        private double _LearningRate;
        public double LearningRate
        {
            get { return _LearningRate; }
            set { _LearningRate = value; }
        }
    
        private double _Momentum;
        public double Momentum
        {
            get { return _Momentum; }
            set { _Momentum = value; }
        } 

        private int _MaxIterations;
        public int MaxIterations
        {
            get { return _MaxIterations; }
            set { _MaxIterations = value; }
        }

        #endregion

        #region Constructeur

        public BackPropagationLearning(ActivationNetwork pNetwork) 
        {
            _LearningRate = 0.3;
            _Momentum = 0.005;
            _MaxIterations = 10001;
            _OldError = Double.PositiveInfinity;

            _Network = pNetwork;

            _NbreEntrees = _Network.NbreEntrees;
            _NbreNeuronesCaches = _Network.NbreNeuronesCaches;
            _NbreNeuronesSortis = _Network.NbreNeuronesSortis;
        }

        #endregion

        #region Interface

        //public double Run(double[] pLstEntrees, double[] pLstSortiesAApprendre)
        //{
        //    double totalError = _OldError;

        //    int i = 0;
        //    while (i < _MaxIterations && totalError > _Momentum)
        //    {
        //        _OldError = totalError;
        //        totalError = 0;
               
        //        // Calcule les sorties de l'échantillon d'entrées
        //        double[] lstSortiesCalculees = _Network.Compute(pLstEntrees);

        //        // Compare les entrées calculées avec les entrées (réelles) à apprendre
        //        for (int j = 0; j < lstSortiesCalculees.Length; j++)
        //        {
        //            // Différence entre les entrées à apprendre et les entrées à apprendre
        //            double error = pLstSortiesAApprendre[j] - lstSortiesCalculees[j];
        //            // Cumul de la différence au carré
        //            totalError += (error * error);
        //        }

        //        // Calcul des nouveaux poids par rétropropagation
        //        AdjustWeights(CEchantillon(pLstEntrees,pLstSortiesAApprendre), _LearningRate);                

        //        // Changer le taux
        //        if (totalError >= _OldError)
        //        {
        //            _LearningRate = _LearningRate / 2.0;
        //        }

        //        // Information et incrément
        //        i++;
        //    }                 

        //    return totalError;
        //}


        //public void Run()
        //{
        //    int i = 0;
        //    double totalError = Double.PositiveInfinity;
        //    double oldError = Double.PositiveInfinity;
        //    double totalGeneralisationError = Double.PositiveInfinity;
        //    double oldGeneralisationError = Double.PositiveInfinity;
        //    Boolean betterGeneralisation = true;

        //    while (i < _MaxIterations && totalError > _Momentum && betterGeneralisation)
        //    {
        //        oldError = totalError;
        //        totalError = 0;
        //        oldGeneralisationError = totalGeneralisationError;
        //        totalGeneralisationError = 0;

        //        // Evaluation
        //        foreach (Echantillon point in _LstEchantillons.TrainingPoints)
        //        {
        //            double[] outputs = _Network.Compute(point);
        //            for (int outNb = 0; outNb < outputs.Length; outNb++)
        //            {
        //                double error = point.Sorties[outNb] - outputs[outNb];
        //                totalError += (error * error);
        //            }

        //            // Calcul des nouveaux poids par rétropropagation
        //            AdjustWeights(point, _LearningRate);
        //        }

        //        // Généralisation
        //        foreach (Echantillon point in _LstEchantillons.GeneralisationPoints)
        //        {
        //            double[] outputs = _Network.Compute(point);
        //            for (int outNb = 0; outNb < outputs.Length; outNb++)
        //            {
        //                double error = point.Sorties[outNb] - outputs[outNb];
        //                totalGeneralisationError += (error * error);
        //            }
        //        }
        //        if (totalGeneralisationError > oldGeneralisationError)
        //        {
        //            betterGeneralisation = false;
        //        }

        //        // Changer le taux
        //        if (totalError >= oldError)
        //        {
        //            _LearningRate = _LearningRate / 2.0;
        //        }

        //        // Information et incrément
        //        i++;
        //    }
        //}

        #endregion

        #region Implémentation

        // Algo d'apprentissage
        private void AdjustWeights(Echantillon pEchantillon, double pLearningRate)
        {
            // Deltas pour les sorties
            double[] outputDeltas = new double[_NbreNeuronesSortis];
            for (int i = 0; i < _NbreNeuronesSortis; i++)
            {
                double output = _Network.NeuronesSortis[i].Sortie;
                double expectedOutput = pEchantillon.Sorties[i];
                outputDeltas[i] = output * (1 - output) * (expectedOutput - output);
            }

            // Deltas pour les neurones cachés
            double[] hiddenDeltas = new double[_NbreNeuronesCaches];
            for (int i = 0; i < _NbreNeuronesCaches; i++)
            {
                double hiddenOutput = _Network.NeuronesCaches[i].Sortie;
                double sum = 0.0;
                for (int j = 0; j < _NbreNeuronesSortis; j++)
                {
                    sum += outputDeltas[j] * _Network.NeuronesSortis[j].Weight(i);
                }
                hiddenDeltas[i] = hiddenOutput * (1 - hiddenOutput) * sum;
            }

            double value;
            // Ajustement des poids des neurones de sortie
            for (int i = 0; i < _NbreNeuronesSortis; i++)
            {
                Neuron outputNeuron = _Network.NeuronesSortis[i];
                for (int j = 0; j < _NbreNeuronesCaches; j++)
                {
                    value = outputNeuron.Weight(j) + pLearningRate * outputDeltas[i] * _Network.NeuronesCaches[j].Sortie;
                    outputNeuron.AdjustWeight(j, value);
                }
                // Et biais
                value = outputNeuron.Weight(_NbreNeuronesCaches) + pLearningRate * outputDeltas[i] * 1.0;
                outputNeuron.AdjustWeight(_NbreNeuronesCaches, value);
            }

            // Ajustement des poids des neurones cachés
            for (int i = 0; i < _NbreNeuronesCaches; i++)
            {
                Neuron hiddenNeuron = _Network.NeuronesCaches[i];
                for (int j = 0; j < _NbreEntrees; j++)
                {
                    value = hiddenNeuron.Weight(j) + pLearningRate * hiddenDeltas[i] * pEchantillon.Entrees[j];
                    hiddenNeuron.AdjustWeight(j, value);
                }
                // Et biais
                value = hiddenNeuron.Weight(_NbreEntrees) + pLearningRate * hiddenDeltas[i] * 1.0;
                hiddenNeuron.AdjustWeight(_NbreEntrees, value);
            }
        }

        #endregion
 
    }
}
