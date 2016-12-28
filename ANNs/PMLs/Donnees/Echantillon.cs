using System;
using System.Collections.Generic;

namespace SloIALib.ANNs.PMLs.Donnees
{
    public class Echantillon
    {
        #region Accesseurs

        private double[] _Entrees;
        public double[] Entrees
        {
            get
            {
                return _Entrees;
            }
        }

        private double[] _Sorties;
        public double[] Sorties
        {
            get
            {
                return _Sorties;
            }
        }

        #endregion

        #region Constructeur

        public Echantillon(string pLigneTxt, int pNbSorties, char pSeparateurChamps)
        {
            string[] ligneTab = pLigneTxt.Split(pSeparateurChamps);

            // Récupération des entrées
            int nbreEntrees = ligneTab.Length - pNbSorties;
            _Entrees = new double[nbreEntrees];
            for (int i = 0; i < nbreEntrees; i++)
            {
                _Entrees[i] = Double.Parse(ligneTab[i]);
            }

            // Récupération des sorties
            _Sorties = new double[pNbSorties];
            for (int i = 0; i < pNbSorties; i++)
            {
                _Sorties[i] = Double.Parse(ligneTab[ligneTab.Length - pNbSorties + i]);
            }
        }

        public Echantillon(double[] pLstEntrees, double[] pLstSorties)
        {
            _Entrees = pLstEntrees;
            _Sorties = pLstSorties;
        }

        #endregion

        #region Interface

        //public static Echantillon CEchantillon(List<Entree> pLstEntrees)
        //{
        //    Echantillon echantillon = new Echantillon(pLstEntrees, null);

        //    return echantillon;

        //}

        //public static Echantillon CEchantillon(List<Entree> pLstEntrees, List<Sortie> pLstSorties)
        //{
        //    Echantillon echantillon = new Echantillon(pLstEntrees, pLstSorties);

        //    return echantillon;

        //}

        #endregion

    }
}
