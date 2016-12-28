using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloIALib.ANNs.PMLs.Donnees
{
    public class LstSorties
    {

        #region Variables
        #endregion

        #region Attributs & Accesseurs

        private ObservableCollection<Sortie> _LstSortieObservable;
        public ObservableCollection<Sortie> Observable
        {
            get { return _LstSortieObservable; }
        }

        //private double[] _LstSortiesDbl;
        //public double[] Doubles
        //{
        //    get { return _LstSortiesDbl; }
        //}

        public int Count
        {
            get { return _LstSortieObservable.Count; }
        }

        #endregion

        #region Constructeurs

        public LstSorties()
        {
            _LstSortieObservable = new ObservableCollection<Sortie>();
        }

        #endregion

        #region Interface

        public void Ajouter(Sortie pSortie)
        {
            _LstSortieObservable.Add(pSortie);
        }

        public void Modifier(int pIndex, double pValeur)
        {
            _LstSortieObservable[pIndex].Valeur = pValeur;
        }

        public void AjouterLstDouble(double[] pLstDouble)
        {
            double valueOld = 0;

            for (int i = 0; i < pLstDouble.Length; i++)
            {
                Modifier(i, pLstDouble[i]);
                if (pLstDouble[i] > valueOld)
                {
                    _LstSortieObservable[i].Correcte = true;
                }
                else
                {
                    _LstSortieObservable[i].Correcte = false;
                }
            }
        }

        #endregion

        #region Implémentation
        #endregion

    }
}
