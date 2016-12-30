using SloIALib.ANNs.PMLs.Outils;
using SloIALib.ANNs.PMLs.Donnees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloIALib.ANNs.PMLs.SloReseauNeurones
{
    public class MachineAApprendre
    {
        #region Variables

        private ReseauNeurones _Reseau;

        private int _NbreIterations;
        private double _TauxApprentissage;

        double _ErreurTotaleEntreSorties;

        #endregion

        #region Attributs et accesseurs

        public double EQM                                              // Erreur Quadratique Moyenne;
        {
            get { return Math.Sqrt(_ErreurTotaleEntreSorties) / 2; }
        }

        #endregion

        #region Constructeur

        public MachineAApprendre(ReseauNeurones pReseauNeurones, double pTauxApprentissage)
        {
            _Reseau = pReseauNeurones;

            _NbreIterations = 1000;
            _TauxApprentissage = pTauxApprentissage;

            _ErreurTotaleEntreSorties = 0;
        }

        #endregion

        #region Interface

        public void Apprendre(Echantillon pEchantillon)
        {
            ApprendreEchantillon(pEchantillon);
        }

        #endregion

        #region Implémentation

        private void ApprendreEchantillon(Echantillon pEchantillon)
        {
            double[] lstSortiesSouhaitees = pEchantillon.Sorties;
            double[] lstSortiesCalculees = _Reseau.CalculerSorties(pEchantillon.Entrees);

            for (int i = 0; i < lstSortiesCalculees.Length; i++)
            {
                _ErreurTotaleEntreSorties += lstSortiesSouhaitees[i] - lstSortiesCalculees[i];
            }

            // Algoritme de rétro-propagation de gradient
            double tauxApprentissage = _TauxApprentissage;

            //Répéter
            int iteration = 0;
            while (iteration <= _NbreIterations)   // TODO : ajouter un critère 
            {
                // Calcul des Deltas des neurones de la couche de sorties

                int iCouche = _Reseau.LstCouches.Length - 1;
                for (int i = 0; i < _Reseau.LstCouches[iCouche].LstNeurones.Length; i++)
                {
                    //   DeltaErr <- si(1 - si)(yi - si) 
                    _Reseau.LstCouches[iCouche].LstNeurones[i].DeltaErr = lstSortiesCalculees[i] * (1 - lstSortiesCalculees[i]) * (lstSortiesSouhaitees[i] - lstSortiesCalculees[i]);
                }

                // Calcul des Deltas des Couches cachées

                // Parcours des couches cachées de l'avant-dernière à la première
                iCouche = _Reseau.LstCouches.Length - 1;
                for (int i = iCouche; i >= 0; i--)
                {
                    // Parcours des neurones de la couche courante
                    foreach (Neurone neurone in _Reseau.LstCouches[i].LstNeurones)
                    {
                        // o = sortie du neurone
                        // Somme [ poids des neuronnes de la couche suivante * delta ]       //Somme[pour k appartenant aux indices des neurones prenant en entrée la sortie du neurone i] de dk * w_ki
                        double sommePoidsK = 0;
                        // Parcours de la liste suivante des neurones
                        for (int j = 0; j < _Reseau.LstCouches[iCouche + 1].LstNeurones.Length; j++)
                        {
                            sommePoidsK += _Reseau.LstCouches[iCouche + 1].LstNeurones[j].LstPoids[neurone.Id] * _Reseau.LstCouches[iCouche + 1].LstNeurones[j].DeltaErr;
                        }

                        // DetaErr = oi(1 - oi) * Somme
                        double Sortie = _Reseau.LstCouches[iCouche].LstNeurones[i].Sortie;
                        _Reseau.LstCouches[iCouche].LstNeurones[i].DeltaErr = Sortie * (1 - Sortie) * sommePoidsK;
                    }
                }

                // MAJ de tous les poids
                MajPoidsReseauAsync(tauxApprentissage);

                tauxApprentissage = tauxApprentissage / 2;    //TODO : voir si meilleur évaluation du taux d'apprentissage

                // Recalcule des nouvelles sorties avec les nouveaux poids
                lstSortiesCalculees = _Reseau.CalculerSorties(pEchantillon.Entrees);

                iteration++;
            }
        }

        private async void MajPoidsReseauAsync(double pTauxApprentissage)
        {
            // Pour tout poids w_ij < -w_ij + epsilon * di * x_ij finPour
            foreach (Couche couche in _Reseau.LstCouches)
            {
                foreach (Neurone neurone in couche.LstNeurones)
                {
                    double newPoids = 0;

                    for (int i = 0; i < neurone.LstPoids.Count - 1; i++)
                    {
                        newPoids = neurone.LstPoids[i] + pTauxApprentissage * neurone.DeltaErr * neurone.Sortie;
                        neurone.MajPoids(i, newPoids);
                    }

                    // maj du biais
                    newPoids = neurone.LstPoids[neurone.iBiais] + pTauxApprentissage * neurone.DeltaErr;
                    neurone.MajPoids(neurone.iBiais, newPoids);
                }
            }
        }

        #endregion

    }
}
