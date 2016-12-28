
using SloIALib.Functions;
using System;

namespace SloIALib.ANNs.PMLs.NeuralNetworkPCL
{
    public class ActivationNetwork
    {
        #region Variables

        private ActivationFunction _FonctionActivation;

        #endregion

        #region Attributs

        private int _NbreEntrees;
        public int NbreEntrees
        {
            get { return _NbreEntrees; }
        }

        private int _NbreNeuronesCaches;
        public int NbreNeuronesCaches
        {
            get { return _NbreNeuronesCaches; }
        }

        private int _NbreNeuronesSortis;
        public int NbreNeuronesSortis
        {
            get { return _NbreNeuronesSortis; }
        }

        private Neuron[] _NeuronesCaches;
        public Neuron[] NeuronesCaches
        {
            get { return _NeuronesCaches;  }
        }

        private Neuron[] _NeuronesSortis;
        public Neuron[] NeuronesSortis
        {
            get { return _NeuronesSortis; }
        }

        #endregion

        #region Constructeur

        public ActivationNetwork(ActivationFunction pFonction, int pNbreEntrees, int pNbreCaches, int pNbreSorties)
        {
            _FonctionActivation = pFonction;

            _NbreEntrees = pNbreEntrees;
            _NbreNeuronesCaches = pNbreCaches;
            _NbreNeuronesSortis = pNbreSorties;

            createCoucheNeuronesCaches();

            createCoucheNeuronesSorties();

            Randomize();
        }

        #endregion

        #region Interface

        /// <summary>
        /// Initialisation aléatoire des poids de tous les neurones.
        /// </summary>
        /// <remarks>Créée le 05/06/2016 par :  JF Enond</remarks>
        private void Randomize()
        {
            if (_NeuronesCaches == null) throw new Exception("Couche de neurones cachées inexistante.");
            if (_NeuronesSortis == null) throw new Exception("Couche de neurones de sortie inexistante.");

            foreach(Neuron n in _NeuronesCaches)
            {
                n.Randomize();
            }

            foreach(Neuron n in _NeuronesSortis)
            {
                n.Randomize();
            }
        }
    
        public ActivationNetwork Load(String pFileName)
        {
            return null;
        }

        public void Save(String pFileName)
        {

        }

        public double[] Compute(double[] pLstEntrees)
        {
            clearCoucheNeuronesCaches();

            clearCouchesNeuronesSortis();

            // Calcule les sorties des neurones cachés
            double[] tbSortiesCachees = new double[_NbreNeuronesCaches];
            for (int i = 0; i < _NbreNeuronesCaches; i++)
            {
                tbSortiesCachees[i] = _NeuronesCaches[i].Evaluate(pLstEntrees);
            }

            // Calcule les sorties des neurones de sorties
            double[] tbSorties = new double[_NbreNeuronesSortis];
            for (int i = 0; i < _NbreNeuronesSortis; i++)
            {
                tbSorties[i] =  _NeuronesSortis[i].Evaluate(tbSortiesCachees);
            }

            return tbSorties;
        }

        #endregion

        #region Implémentation

        private void createCoucheNeuronesCaches()
        {
            // Création de la couche des neurones cachés
            _NeuronesCaches = new Neuron[_NbreNeuronesCaches];
            for (int i = 0; i < _NbreNeuronesCaches; i++)
            {
                _NeuronesCaches[i] = new Neuron(_FonctionActivation, _NbreEntrees);
            }
        }

        private void createCoucheNeuronesSorties()
        {
            // Création de la couche des neurones de sortie
            _NeuronesSortis = new Neuron[_NbreNeuronesSortis];
            for (int i = 0; i < _NbreNeuronesSortis; i++)
            {
                _NeuronesSortis[i] = new Neuron(_FonctionActivation, _NbreNeuronesCaches);
            }
        }

        private void clearCoucheNeuronesCaches()
        {
            // Réinitialisation des sorties de tous les neurones
            foreach (Neuron n in _NeuronesCaches)
            {
                n.Clear();
            }
        }

        private void clearCouchesNeuronesSortis()
        {
            foreach (Neuron n in _NeuronesSortis)
            {
                n.Clear();
            }
        }

        #endregion
    }
}
