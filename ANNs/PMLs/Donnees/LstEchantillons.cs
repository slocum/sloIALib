using SloOutilsLib.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SloIALib.ANNs.PMLs.Donnees
{
    public class LstEchantillons
    {
        #region Attributs et accesseurs

        private Echantillon[] _TrainingPoints;
        public Echantillon[] TrainingPoints
        {
            get { return _TrainingPoints; }
        }

        private Echantillon[] _GeneralisationPoints;
        public Echantillon[] GeneralisationPoints
        {
            get { return _GeneralisationPoints; }
        }

        #endregion

        #region Constructeur

        public LstEchantillons(String [] pFileContent, int pNbreSorties, double pLearningRate)
        {
            // Lecture de tous les échantillons
            int nbLines = pFileContent.Length;
            List<Echantillon> lstEchantillon = new List<Echantillon>();
            for (int i = 0; i < nbLines; i++)
            {
                lstEchantillon.Add(new Echantillon(pFileContent[i], pNbreSorties, Constantes.POINT_VIRGULE));
            }

            // Récupération des échantillons d'apprentissage
            int nbTrainingEchantillons = (int) (nbLines * pLearningRate);
            _TrainingPoints = new Echantillon[nbTrainingEchantillons];
            Random rand = new Random();
            for (int i = 0; i < nbTrainingEchantillons; i++)
            {
                int index = rand.Next (lstEchantillon.Count);
                _TrainingPoints[i] = lstEchantillon.ElementAt(index);
                lstEchantillon.RemoveAt(index);
            }

            // Récupération des échantillons de généralisation
            _GeneralisationPoints = lstEchantillon.ToArray();
        }

        #endregion

        #region Interface
        #endregion
    }
}
