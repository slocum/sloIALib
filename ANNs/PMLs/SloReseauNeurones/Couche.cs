using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloIALib.ANNs.PMLs.SloReseauNeurones
{
    public class Couche
    {
        #region Variables

        private int _Num;
        private int _NbreNeurones;
        private int _NbreEntreesParNeurone;

        #endregion

        #region Attributs et accesseurs

        private Neurone[] _LstNeurones;
        public Neurone[] LstNeurones
        {
            get { return _LstNeurones; }
        }

        #endregion

        #region Constructeur

        public Couche(int pNum,
                      int pNbreNeurones, 
                      int pNbreSortiesCouchePrecedente, 
                      Functions.ActivationFunction pFonctionTransfert)
        {
            _Num = pNum;
            _NbreNeurones = pNbreNeurones;
            _NbreEntreesParNeurone = pNbreSortiesCouchePrecedente;

            InitLstNeurones(pFonctionTransfert);
        }

        #endregion

        #region Interface

        public void MABSorties()
        {
            for (int i = 0; i < _LstNeurones.Length; i++)
            {
                _LstNeurones[i].MabSortie();
            }
        }

        /// <summary>
        /// Calcul des sorties des neurones de la couche
        /// </summary>
        /// <param name="pLstValEntrees"></param>
        /// <remarks>Créée le 29/12/2016 par : JF Enond</remarks>
        public void CalculerSorties(double[] pLstValEntrees)
        {
            for (int i = 0; i < _LstNeurones.Length; i++)
            {
                _LstNeurones[i].CalculerSortie(pLstValEntrees);
            }
        }

        /// <summary>
        /// Récupère les sorties de tous les neurones de la couche
        /// </summary>
        /// <returns></returns>
        public double[] listerSorties()
        {
            double[] lstSorties = new double[_LstNeurones.Length];

            for (int i = 0; i < _LstNeurones.Length; i++)
            {
                lstSorties[i] = _LstNeurones[i].Sortie;
                //Debug.WriteLine("Couche : " + _Num.ToString() + " Sortie : " + i.ToString() + "  "  + lstSorties[i].ToString());
            }

            return lstSorties;
        }

        #endregion

        #region Implémentation

        /// <summary>
        /// Initialisation des neurones de la couche
        /// </summary>
        /// <param name="pFonctionTransfert"></param>
        /// <remarks>Créée le29/12/2016 par : JF Enond </remarks>
        private void InitLstNeurones(Functions.ActivationFunction pFonctionTransfert)
        {
            // Liste des neurones de la couche
            _LstNeurones = new Neurone[_NbreNeurones];

            for (int i = 0; i < _LstNeurones.Length; i++)
            {
                _LstNeurones[i] = new Neurone(i, _NbreEntreesParNeurone, pFonctionTransfert);
            }
        }
        #endregion
    }
}
