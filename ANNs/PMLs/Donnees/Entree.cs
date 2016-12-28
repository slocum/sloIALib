using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloIALib.ANNs.PMLs.Donnees
{
    public class Entree : INotifyPropertyChanged
    {
        #region Variables
        #endregion

        #region Accesseurs

        private string _Nom;
        public string Nom
        {
            get
            {
                return _Nom;
            }
        }

        private double _Valeur;
        public double Valeur
        {
            get
            {
                return _Valeur;
            }
            set
            {
                _Valeur = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Valeur"));
            }
        }

        #endregion

        #region Constructeur

        public Entree(string pNom)
        {
            _Nom = pNom;
            _Valeur = 0;
        }

        public Entree(double pValeur)
        {
            _Nom = string.Empty;
            _Valeur = pValeur;
        }

        #endregion

        #region Interface

        #endregion

        #region Implémentation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion

    }
}
