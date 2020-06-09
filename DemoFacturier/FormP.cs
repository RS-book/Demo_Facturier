using DemoFacturier.FacturierDatabaseDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoFacturier
{
    public partial class FormP : Form
    {
        public FormP()
        {
            InitializeComponent();
            DBClients = new FacturierDatabaseDataSet.ClientsDataTable();
            DBAnimaux = new FacturierDatabaseDataSet.AnimauxDataTable();
            clientsTableAdapter = new ClientsTableAdapter();
            animauxTableAdapter = new AnimauxTableAdapter();
            clientsTableAdapter.Fill(DBClients);
            animauxTableAdapter.Fill(DBAnimaux);
            listeRecherche = new List<FacturierDatabaseDataSet.ClientsRow>();
        }

        private readonly FacturierDatabaseDataSet.ClientsDataTable DBClients;
        private readonly FacturierDatabaseDataSet.AnimauxDataTable DBAnimaux;
        private readonly ClientsTableAdapter clientsTableAdapter;
        private readonly AnimauxTableAdapter animauxTableAdapter;

        private Client currentClient;
        private Animal currentAnimal;
        private readonly List<FacturierDatabaseDataSet.ClientsRow> listeRecherche;

        private void RechercheC(object sender, EventArgs e)
        {
            Regex rx = new Regex(input_rechercheClient.Text, RegexOptions.IgnoreCase);
            List<string> resC = new List<string>();
            listeRecherche.Clear();
            foreach (FacturierDatabaseDataSet.ClientsRow row in DBClients.Rows)
            {
                if (rx.IsMatch(row.Nom1) || rx.IsMatch(row.TelPrinc))
                {
                    string result;
                    if (row.IsPrenom1Null()) { result = row.Nom1; }
                    else { result = row.Prenom1 + " " + row.Nom1; }
                    resC.Add(result);
                    listeRecherche.Add(row);
                }
                //idem avec nom 2, autres critères ?
            }
            foreach(FacturierDatabaseDataSet.AnimauxRow row in DBAnimaux.Rows)
            {
                if(rx.IsMatch(row.Nom))
                {
                    foreach (FacturierDatabaseDataSet.ClientsRow proprio in DBClients.Rows)
                    {
                        if (proprio.IdClient == row.Proprietaire)
                        {
                            string result;
                            if (row.IsRaceNull()) { result = proprio.Nom1 + " ; " + row.Nom + ", " + row.Espece; }
                            else { result = proprio.Nom1 + " ; " + row.Nom + ", " + row.Espece + " (" + row.Race + ")"; }
                            resC.Add(result);
                            listeRecherche.Add(proprio); 
                        }
                    }
                }
            }
            if (resC.Count >= 1)
            {
                comboBox_searchC_res.Items.Clear();
                comboBox_searchC_res.Items.AddRange(resC.ToArray());
                comboBox_searchC_res.SelectedIndex = 0;
                comboBox_searchC_res.Enabled = true;
                buttonLoadC.Enabled = true;
            }
            else
            { 
                comboBox_searchC_res.Items.Clear();
                comboBox_searchC_res.Items.Add("Pas de résultat.");
                comboBox_searchC_res.SelectedIndex = 0;
                comboBox_searchC_res.Enabled = false;
                buttonLoadC.Enabled = false;
            }
        }

        private void SearchWatermarkOff(object sender, EventArgs e)
        {
            if (input_rechercheClient.Text == "Nom, téléphone, animal...")
            {
                input_rechercheClient.Text = "";
                input_rechercheClient.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        private void SearchWatermarkOn(object sender, EventArgs e)
        {
            if (input_rechercheClient.Text == "")
            {
                input_rechercheClient.Text = "Nom, téléphone, animal...";
                input_rechercheClient.ForeColor = System.Drawing.SystemColors.GrayText;
            }
        }

        private void ChargerClient(object sender, EventArgs e)
        {
            if (ConfirmEditCnotsaved())
            {
                currentClient = new Client(listeRecherche[comboBox_searchC_res.SelectedIndex], DBAnimaux, ChampNom1, ChampNom2, ChampPrenom1, ChampPrenom2, ChampN, ChampRue, ChampCpl, ChampCodePost, ChampVille, ChampTelPrinc, ChampMobile1, ChampMobile2);
                currentClient.FillClientChamps();
                groupBoxInfoClient.Enabled = true;
                buttonEditC.Text = "Enregistrer modifications";
                buttonEditC.Enabled = true;
                buttonDeleteC.Enabled = true;
                //populate animaux
                //enable liste animaux
            }
        }

        private void CreerClient(object sender, EventArgs e)
        {
            if (ConfirmEditCnotsaved())
            {
                currentClient = new Client(ChampNom1, ChampNom2, ChampPrenom1, ChampPrenom2, ChampN, ChampRue, ChampCpl, ChampCodePost, ChampVille, ChampTelPrinc, ChampMobile1, ChampMobile2);
                currentClient.ClearClientChamps();
                groupBoxInfoClient.Enabled = true;
                buttonEditC.Text = "Créer le client";
                buttonEditC.Enabled = true;
                buttonDeleteC.Enabled = false;
            }
        }

        private void EditClient(object sender, EventArgs e)
        {
            groupBoxInfoClient.Enabled = false;
            if (currentClient.CheckIfValid())
            {
                if (currentClient.IsNew == true)
                {
                    currentClient.AddClientToDB(DBClients);
                    currentClient.IsNew = false;
                    buttonEditC.Text = "Enregistrer modifications";
                    buttonDeleteC.Enabled = true;
                    //enable créer animal
                    MessageBox.Show("Nouveau client créé.", "Tâche complétée", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    currentClient.EditClientInDB(DBClients);
                    MessageBox.Show("Informations client modifiées.", "Tâche complétée", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                }
                clientsTableAdapter.Update(DBClients);
            }
            else
            { MessageBox.Show("Erreur : Informations obligatoires manquantes.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            groupBoxInfoClient.Enabled = true;
        }

        private bool ConfirmEditCnotsaved()
        {
            if (currentClient.WereChangesToCMade() == true)
            {
                if (MessageBox.Show("Les modifications non enregistrées seront perdues, continuer ?", "Demande de confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private void SupprimerC(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce client ? Cela supprimera également les animaux associés.", "Demande de confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                //supprimer animaux associés, update
                //supprimer client de BDD, update
                //disable champs + bouton edit
            }
        }
    }
}
