using SloIALib.ANNs.PMLs.Donnees;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloIALib.ANNs.PMLs.Donnees
{
    public class LstEntrees
    {

        #region Variables
        #endregion

        #region Attributs & Accesseurs

        private ObservableCollection<Entree> _LstEntreesObservable;
        public ObservableCollection<Entree> Observable
        {
            get { return _LstEntreesObservable; }
        }

        public double[] Doubles
        {
            get { return ListerEntreesDoubles(); }
        }

        public int Count
        {
            get { return _LstEntreesObservable.Count; }
        }

        #endregion

        #region Constructeurs

        public LstEntrees()
        {
            _LstEntreesObservable = new ObservableCollection<Entree>();
        }

        #endregion

        #region Interface

        public void Ajouter(Entree pEntree)
        {
            _LstEntreesObservable.Add(pEntree);
        }

        public void Modifier(int pIndex, double pValeur)
        {
            _LstEntreesObservable[pIndex].Valeur = pValeur;
        }

        #endregion

        #region Implémentation

        private double[] ListerEntreesDoubles()
        {
            double[] tabDouble = new double[Count];

            for (int i = 0; i < Count; i++)
            {
                tabDouble[i] = _LstEntreesObservable[i].Valeur;
            }

            return tabDouble;
        }

        #endregion

    }
}
