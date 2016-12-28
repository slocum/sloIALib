using System;
using System.Collections.Generic;
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
            initLstNeurones(pFonctionTransfert);
        }

        #endregion

        #region Interface

        public void MABSorties()
        {
            for (int i = 0; i < _LstNeurones.Length; i++)
            {
                _LstNeurones[i].mabSortie();
            }
        }

        public void CalculerSorties(double[] pLstValEntrees)
        {
            for (int i = 0; i < _LstNeurones.Length; i++)
            {
                _LstNeurones[i].calculerSortie(pLstValEntrees);
            }
        }

        public double[] listerSorties()
        {
            double[] lstSorties = new double[_LstNeurones.Length];

            for (int i = 0; i < _LstNeurones.Length; i++)
            {
                lstSorties[i] = _LstNeurones[i].Sortie;
            }

            return lstSorties;
        }

        #endregion

        #region Implémentation

        private void initLstNeurones(Functions.ActivationFunction pFonctionTransfert)
        {
            _LstNeurones = new Neurone[_NbreNeurones];

            for (int i = 0; i < _LstNeurones.Length; i++)
            {
                _LstNeurones[i] = new Neurone(i, _NbreEntreesParNeurone, pFonctionTransfert);
            }
        }
        #endregion
    }
}
