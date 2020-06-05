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

        private FacturierDatabaseDataSet.ClientsDataTable DBClients;
        private FacturierDatabaseDataSet.AnimauxDataTable DBAnimaux;
        private ClientsTableAdapter clientsTableAdapter;
        private AnimauxTableAdapter animauxTableAdapter;

        private Client currentClient;
        private List<FacturierDatabaseDataSet.ClientsRow> listeRecherche;

        private void RechercheC(object sender, EventArgs e)
        {
            Regex rx = new Regex(input_rechercheClient.Text, RegexOptions.IgnoreCase);
            List<string> resC = new List<string>();            
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
            currentClient = new Client(listeRecherche[comboBox_searchC_res.SelectedIndex],DBAnimaux);
            //update contenu boxes from client
            //enable onglet client
            buttonEditC.Text = "Enregistrer modifications";
            buttonEditC.Enabled = true;
            //populate animaux
            //enable animaux
        }

        private void CreerClient(object sender, EventArgs e)
        {
            currentClient = new Client(DBClients);
            //viderboxes (update from client vide)
            //enable onglet client
            buttonEditC.Text = "Créer le client";
            buttonEditC.Enabled = true;
        }

        private void EditClient(object sender, EventArgs e)
        {
            //disableboxes
            if (currentClient.CheckIfValid())
            {
                if (currentClient.IsNew == true)
                {
                    currentClient.AddClientToDB(DBClients);
                    currentClient.IsNew = false;
                    buttonEditC.Text = "Enregistrer modifications";
                    //enable animaux
                }
                else
                {
                    currentClient.EditClientInDB(DBClients);                    
                }
                //popup ok
                clientsTableAdapter.Update(DBClients);
            }
            else
            { MessageBox.Show("Erreur : Informations obligatoires manquantes."); }
            //enableboxes
        }

        private void MajChampC(object sender, EventArgs e)
        {
            currentClient.Nom1 = ChampNom1.Text; 
            currentClient.Prenom1 = ChampPrenom1.Text;
            //répliquer pour reste des champs
        }
    }
}
