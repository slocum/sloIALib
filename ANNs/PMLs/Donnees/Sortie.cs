using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloIALib.ANNs.PMLs.Donnees
{
    public class Sortie : INotifyPropertyChanged
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

        private Boolean _Correcte;
        public Boolean Correcte {
            get { return _Correcte; }
            set
            {
                _Correcte = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Correcte"));
            }
        }

        #endregion

        #region Constructeur

        public Sortie(string pNom, Boolean pCorrecte = false)
        {
            _Nom = pNom;
            _Valeur = 0.0000;
            _Correcte = pCorrecte;
        }

        //public Sortie(double pValeur)
        //{
        //    _Nom = string.Empty;
        //    _Valeur = pValeur;
        //    _Correcte = false;
        //}


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
