using SloIALib.ANNs.PMLs.Outils;
using SloIALib.ANNs.PMLs.Donnees;
using SloOutilsLib.Constantes;
using SloOutilsLib.DossiersFichiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Cours : http://alp.developpez.com/tutoriels/intelligence-artificielle/reseaux-de-neurones/
/// </summary>

namespace SloIALib.ANNs.PMLs.SloReseauNeurones
{
    public class ReseauNeurones
    {

        #region Constantes

        private const char SEPARATEUR_CHAMP = Constantes.POINT_VIRGULE;
        private const char SEPARATEUR_ELEMENT = Constantes.DIESE;
         
        #endregion

        #region Variables

        private int _NbreEntrees;
        private int _NbreCouches;
        private string _NomFonctionTransfert;
        private int[] _LstLongueursCouches;

        private Couche[] _LstCouches;
        public Couche[] LstCouches
        {
            get { return _LstCouches; }
        }

        private Dossier _DossierFichiers;
        private Fichier _FicSauvegarde;

        #endregion

        #region Constructeur

        public ReseauNeurones()
        {
            _NbreEntrees = 0;
            _NbreCouches = 0;
            _LstLongueursCouches = null;
            _NomFonctionTransfert = string.Empty;

            _DossierFichiers = null;
        }

        public ReseauNeurones(int pNbreEntrees,
                              int[] pLstLongueursCouches,
                              Functions.ActivationFunction pFonctionTransfert)
        {
            _NbreEntrees = pNbreEntrees;
            _NbreCouches = pLstLongueursCouches.Length;
            _LstLongueursCouches = pLstLongueursCouches;
            _NomFonctionTransfert = pFonctionTransfert.GetType().ToString();

            InitLstCouches(pLstLongueursCouches, pFonctionTransfert);

            _DossierFichiers = new Dossier();

        }

        #endregion

        #region Interface

        /// <summary>
        /// Calcule les sorties du réseau
        /// </summary>
        /// <param name="pLstValeursEntrees">Les valeurs d'entrées du réseau</param>
        /// <remarks>Créée le 20/07/2016 par : JF Enond</remarks>
        public double[] CalculerSorties(double[] pLstValeursEntrees)
        {
            if (pLstValeursEntrees.Length != _NbreEntrees) throw new Exception("Nombre de valeurs en entrée incorrect.");

            // MAB des sorties des couches du réseau
            MabSorties();

            // Calcul des sorties
            double[] lstValeursEntrees = pLstValeursEntrees;

            for (int i = 0; i < _NbreCouches; i++)
            {
                _LstCouches[i].CalculerSorties(lstValeursEntrees);
                lstValeursEntrees = _LstCouches[i].listerSorties();
            }

            // Retourne les valeurs des sorties du réseau
            return ListerSortiesReseau();

        }

        /// <summary>
        /// Sauvegarde des données du réseau : 
        ///     - Nbre d'entrée, 
        ///     - liste des longueurs des couches,
        ///     - la fonction de transfert
        ///     - liste des poids (une ligne par couche)
        /// </summary>
        /// <param name="pFichierNom">Nomp du fichier de sauvegarde</param>
        /// <param name="pEcraserFichier">Autorisation pour écraser ou pas le fichier de sauvegard, true pas défaut</param>
        /// <remarks>Créée le 22/07/2016 par JF Enond</remarks>
        public void Sauvegarder(string pFichierNom, bool pEcraserFichier = true)
        {
            _FicSauvegarde = new Fichier(_DossierFichiers.Documents, pFichierNom);

            // Vérifie si le fichier de sauvegarde existe
            if (_FicSauvegarde.Existe() && !pEcraserFichier) throw new Exception("Fichier déjà existant.");

            _FicSauvegarde.OuvrirAsync(Fichier.EModeOuverture.Ecriture);

            // Ecriture ligne 1 : Nbre Entrées ; Liste longueurs couches ; Fonction transfert
            EcrireLigne1();

            // Ecriture lignes suivantes poids des neurones des couches
            EcrireLignesCouches();

            _FicSauvegarde.FermerAsync();
        }

        public async void ChargerAsync(string pFichierNom)
        {
            _FicSauvegarde = new Fichier(_DossierFichiers.Documents, pFichierNom);

            // Vérifie si le fichier de sauvegarde existe
            if (!_FicSauvegarde.Existe()) throw new Exception("Fichier inexistant.");

            _FicSauvegarde.OuvrirAsync(Fichier.EModeOuverture.Lecture);

            bool estLigne1 = true;
            int coucheId = 0;

            while (!_FicSauvegarde.Fin)
            {
                string ligne = await _FicSauvegarde.LireLigneAsync();

                if (estLigne1)
                {
                    LireLigne1(ligne);
                    estLigne1 = false;
                }
                else
                {
                    LireLignesCouches(ligne, coucheId);
                    coucheId += 1;
                }
            }

            _FicSauvegarde.FermerAsync();
        }

        #endregion

        #region Implémentation

        /// <summary>
        /// Initialisation de la liste des couches du réseau
        /// </summary>
        /// <param name="pLstLongueursCouches">Liste des longueurs des couches</param>
        /// <param name="pFonctionTransfert">Fonction de transfert</param>
        private void InitLstCouches(int[] pLstLongueursCouches, Functions.ActivationFunction pFonctionTransfert)
        {
            _LstCouches = new Couche[_NbreCouches];

            for (int i = 0; i < _NbreCouches; i++)
            {
                if (i==0)       // Première couche
                { 
                    _LstCouches[i] = new Couche(i, pLstLongueursCouches[i], _NbreEntrees, pFonctionTransfert);
                }
                else          // Couches suivantes
                {
                    _LstCouches[i] = new Couche(i, pLstLongueursCouches[i], pLstLongueursCouches[i-1], pFonctionTransfert);
                }
            }
        }

        /// <summary>
        /// Récupère les sorties du réseau
        /// </summary>
        /// <returns>Valeurs des sorties du réseau</returns>
        /// <remarks>Créée le 21/07/2016 par : JF Enond</remarks>
        private double[] ListerSortiesReseau()
        {
            // Sorties de la dernière couche
            return _LstCouches[_NbreCouches - 1].listerSorties();
        }

        /// <summary>
        /// Mise à blance de toutes les sorties des neurones du réseau.
        /// </summary>
        /// <remarks>Créée le 25/07/2016 par : JF Enond</remarks>
        private void MabSorties()
        {
            for (int i = 0; i < _NbreCouches; i++)
            {
                _LstCouches[i].MABSorties();
            }
        }


        #region Fichier de sauvegarde

        private string SupprimerDernierCaractere(string pValue, char pCharASupprimer)
        {
            // Suppression de la dernière virgule
            if (pValue.EndsWith(pCharASupprimer.ToString()))
            {
                pValue = pValue.Remove(pValue.Length - 1, 1);
            }
            return pValue;
        }

        private void EcrireLigne1()
        {
            string lstCouches = string.Empty;
            for (int i = 0; i < _LstCouches.Length; i++)
            {
                lstCouches += _LstLongueursCouches[i].ToString() + SEPARATEUR_ELEMENT;
            }
            lstCouches = SupprimerDernierCaractere(lstCouches, SEPARATEUR_ELEMENT);

            string ligne = string.Empty;

            ligne = _NbreEntrees.ToString();
            ligne += SEPARATEUR_CHAMP + lstCouches;
            ligne += SEPARATEUR_CHAMP + _NomFonctionTransfert;

            _FicSauvegarde.EcrireLigneAsync(ligne);
        }

        private void EcrireLignesCouches()
        {   
            foreach (Couche couche in _LstCouches)
            {
                string lngCouche = string.Empty;

                foreach (Neurone neurone in couche.LstNeurones)
                {
                    string lstPoids = string.Empty;

                    foreach (double poids in neurone.LstPoids)
                    {
                        lstPoids += poids.ToString() + SEPARATEUR_ELEMENT;
                    }
                    lstPoids = SupprimerDernierCaractere(lstPoids, SEPARATEUR_ELEMENT);
                    lngCouche  += lstPoids + SEPARATEUR_CHAMP;
                }

                _FicSauvegarde.EcrireLigneAsync(lngCouche);
            }
        }

        private void LireLigne1(String pLigne)
        {
            string[] lstChamps = pLigne.Split(SEPARATEUR_CHAMP);

            _NbreEntrees = int.Parse(lstChamps[0]);

            string[] lstLongueursCouches = lstChamps[1].Split(SEPARATEUR_ELEMENT);

            _LstLongueursCouches = new int[lstLongueursCouches.Length];

            for (int i = 0; i < lstLongueursCouches.Length; i++)
            {
                _LstLongueursCouches[i] = int.Parse(lstLongueursCouches[i]);
            }

            _NbreCouches = _LstLongueursCouches.Length;
            
            InitLstCouches(_LstLongueursCouches, Functions.ActivationFunction.ObjetFonction(Type.GetType(lstChamps[2]))); 
        }

        private void LireLignesCouches(String pLigne, int pCoucheId)
        {
            string[] lstNeurones = pLigne.Split(SEPARATEUR_CHAMP);

            foreach (Neurone neurone in _LstCouches[pCoucheId].LstNeurones)
            {
                string[] lstPoidsDuNeurone = lstNeurones[neurone.Id].Split(SEPARATEUR_ELEMENT);
                for (int i = 0; i < lstPoidsDuNeurone.Length; i++)
                {
                    neurone.majPoids(i, double.Parse(lstPoidsDuNeurone[i]));
                }
            }
        }

        #endregion

        #endregion
    }
}