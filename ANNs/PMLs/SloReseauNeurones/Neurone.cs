using System;
using System.Collections.Generic;
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


        private double[] _LstPoids;
        public double[] LstPoids
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
            _LstPoids = new double[_NbreEntrees + BIAIS];
            initPoids();
            _FonctionTransfert = pFonctionTransfert;
            _Sortie = double.NaN;
            _DeltaErr = double.NaN;

            _iBiais = _NbreEntrees + BIAIS - 1;
        }

        #endregion

        #region Interface

        public void calculerSortie(double[] pLstValEntrees)
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

        public void mabSortie()
        {
            _Sortie = double.NaN;
        }

        public void majPoids(int pIndex, double pValeur)
        {
            _LstPoids[pIndex] = pValeur;
            DeltaErr = double.NaN;
        }

        #endregion

        #region Implémentation

        /// <summary>
        ///  Initialise la liste des poids avec des valeurs comprises entre -1 et 1
        /// </summary>
        private void initPoids()
        {
            //ThreadLocal.Sleep(100);
            Random hasard = new Random();

            for (int i = 0; i < _LstPoids.Length; i++)
            {
                _LstPoids[i] = hasard.NextDouble() * 2 - 1;
            }
        }

        #endregion

    }
}
