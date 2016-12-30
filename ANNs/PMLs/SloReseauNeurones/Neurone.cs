using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SloIALib.ANNs.PMLs.SloReseauNeurones
{
    public class Neurone
    {

        #region Constantes

        private int BIAIS = 1;

        #endregion

        #region Variables

        private Random _Hasard = new Random();


        #endregion

        #region Attributs

        private int _Id;
        public int Id
        {
            get { return _Id; }
        }

        private int _NbreEntrees;

        private int _iBiais;
        public int iBiais
        {
            get { return _iBiais; }
        }


        private List<double> _LstPoids;
        public List<double> LstPoids
        {
            get { return _LstPoids; }
        }

        private Functions.ActivationFunction _FonctionTransfert;

        private double _Sortie;
        public double Sortie
        {
            get { return _Sortie; }
        }

        // DeltaErr ne sert que dans l'apprentissage pour la rétro-propagation par gradient 
        private double _DeltaErr;
        public double DeltaErr
        {
            get { return _DeltaErr; }
            set { _DeltaErr = value; }
        }
        
        #endregion

        #region Constructeur

        public Neurone(int pId, int pNbreEntrees, Functions.ActivationFunction pFonctionTransfert)
        {
            _Id = pId;

            _NbreEntrees = pNbreEntrees;
            _LstPoids = new List<double>(_NbreEntrees + BIAIS);
            InitPoids();

            _FonctionTransfert = pFonctionTransfert;

            _Sortie = double.NaN;
            _DeltaErr = double.NaN;

            _iBiais = _NbreEntrees + BIAIS - 1;
        }

        #endregion

        #region Interface

        /// <summary>
        /// Calcul de la sortie d'un neurone
        /// </summary>
        /// <param name="pLstValEntrees"></param>
        /// <remarks>Créée le 29/12/2016 par : JF Enond</remarks>
        public void CalculerSortie(double[] pLstValEntrees)
        {
            double sumEntrees = 0;

            // Somme pondérée des entrées
            for (int i = 0; i < _NbreEntrees; i++)
            {
                sumEntrees += pLstValEntrees[i] * _LstPoids[i];
            }

            // Coefficient de biais
            sumEntrees -= _LstPoids[_iBiais];   // Hack : Valeur d'entrée du biais (seuil) = -1

            //  Fonction de transfert
            _Sortie = _FonctionTransfert.Fonction(sumEntrees);
        }

        public void MabSortie()
        {
            _Sortie = double.NaN;
        }

        public void MajPoids(int pIndex, double pValeur)
        {

            DeltaErr = double.NaN;

            //await System.Threading.Tasks.Task.Delay(100);
            //Debug.WriteLine(pValeur);
            if (_LstPoids.Count != _LstPoids.Capacity)
            {
                _LstPoids.Add(pValeur);
            } else { 
                _LstPoids[pIndex] = pValeur;
            }
        }

        #endregion

        #region Implémentation

        /// <summary>
        ///  Initialise la liste des poids avec des valeurs comprises entre -1 et 1
        /// </summary>
        private void InitPoids()
        {
            for (int i = 0; i < _LstPoids.Capacity; i++)
            {

                double poids = _Hasard.NextDouble() * 2 - 1;
                Task.Delay(5);

                Debug.WriteLine(poids);

                MajPoids(i, poids);
            }
        }

        #endregion

    }
}
