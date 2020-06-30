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
        private int currentAnimal; //-1 : pas d'animal sélectionné
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
            if (ConfirmEditCnotsaved()&&ConfirmEditAnotsaved())
            {
                currentClient = new Client(listeRecherche[comboBox_searchC_res.SelectedIndex], DBAnimaux, ChampNom1, ChampNom2, ChampPrenom1, ChampPrenom2, ChampN, ChampRue, ChampCpl, ChampCodePost, ChampVille, ChampTelPrinc, ChampMobile1, ChampMobile2);
                currentClient.FillClientChamps();
                groupBoxInfoClient.Enabled = true;
                buttonEditC.Text = "Enregistrer modifications";
                buttonEditC.Enabled = true;
                buttonDeleteC.Enabled = true;
                List<string> listeA = new List<string>();
                listeA.Add("Sélectionner un animal");
                foreach (FacturierDatabaseDataSet.AnimauxRow animal in listeRecherche[comboBox_searchC_res.SelectedIndex].GetAnimauxRows())
                {
                    listeA.Add(animal.Nom + ", " + animal.Espece);
                    currentClient.Animals.Add(new Animal(animal, ChampANom, radioButtonChien, radioButtonChat, radioButtonAutre, ChampAAutre, ChampARace, ChampANaiss, checkBoxFriand, checkBoxParfum, ChampAParfum, richTextBox1));
                }
                comboBox_listA.Items.Clear();
                comboBox_listA.Items.AddRange(listeA.ToArray());
                comboBox_listA.SelectedIndex = 0;
                currentAnimal = -1;
                comboBox_listA.Enabled = true;
                button_newA.Enabled = true;
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
                comboBox_listA.Items.Clear();
                groupBoxInfoAnimal.Enabled = false;
                buttonDeleteA.Enabled = false;
                button_newA.Enabled = false;
                button_editA.Enabled = false;
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
                    clientsTableAdapter.Update(DBClients);
                    currentClient.IsNew = false;
                    buttonEditC.Text = "Enregistrer modifications";
                    buttonDeleteC.Enabled = true;
                    button_newA.Enabled = true;
                    MessageBox.Show("Nouveau client créé.", "Tâche complétée", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    currentClient.EditClientInDB();
                    clientsTableAdapter.Update(DBClients);
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
                foreach(Animal animal in currentClient.Animals)
                {
                    animal.DeleteAFromDB();
                }
                animauxTableAdapter.Update(DBAnimaux);
                comboBox_listA.Items.Clear();
                comboBox_listA.SelectedIndex = 0;
                groupBoxInfoAnimal.Enabled = false;
                button_newA.Enabled = false;
                button_editA.Enabled = false;
                buttonDeleteA.Enabled = false;
                currentClient.DeleteCFromDB();
                clientsTableAdapter.Update(DBClients);
                currentClient.ClearClientChamps();
                groupBoxInfoClient.Enabled = false;
                buttonEditC.Enabled = false;
                buttonDeleteC.Enabled = false;
            }
        }

        private void LoadAnimal(object sender, EventArgs e)
        {
            if (comboBox_listA.SelectedIndex != 0 && comboBox_listA.SelectedIndex != currentAnimal + 1 && ConfirmEditAnotsaved())
            {
                currentAnimal = comboBox_listA.SelectedIndex - 1;
                groupBoxInfoAnimal.Enabled = true;
                currentClient.Animals[currentAnimal].FillAnimalChamps();
                button_editA.Text = "Enregistrer modifications";
                button_editA.Enabled = true;
                buttonDeleteA.Enabled = true;
            }
        }

        private bool ConfirmEditAnotsaved()
        {
            if (currentClient.Animals[currentAnimal].WereChangesToAMade() == true)
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

        private void CreerAnimal(object sender, EventArgs e)
        {
            if(ConfirmEditAnotsaved())
            {
                currentAnimal = currentClient.Animals.Count;
                currentClient.Animals.Add(new Animal(ChampANom, radioButtonChien, radioButtonChat, radioButtonAutre, ChampAAutre, ChampARace, ChampANaiss, checkBoxFriand, checkBoxParfum, ChampAParfum, richTextBox1));
                currentClient.Animals[currentAnimal].ClearAnimalChamps();
                button_editA.Text = "Créer l'animal";
                button_editA.Enabled = true;
                buttonDeleteA.Enabled = false;

            }
        }

        private void DeleteAnimal(object sender, EventArgs e)
        {
            if(MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet animal ?", "Demande de confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                groupBoxInfoAnimal.Enabled = false;
                button_newA.Enabled = true;
                button_editA.Enabled = false;
                buttonDeleteA.Enabled = false;
                currentClient.Animals[currentAnimal].ClearAnimalChamps();
                currentClient.Animals[currentAnimal].DeleteAFromDB();
                animauxTableAdapter.Update(DBAnimaux);                
                comboBox_listA.Items.RemoveAt(currentAnimal + 1);
                comboBox_listA.SelectedIndex = 0;
                currentAnimal = -1;
            }
        }

        private void EditAnimal(object sender, EventArgs e)
        {
            groupBoxInfoAnimal.Enabled = false;
            Animal animal = currentClient.Animals[currentAnimal];
            if (animal.CheckIfValid())
            {
                if (animal.IsNew == true)
                {
                    animal.AddAnimalToDB(DBAnimaux);
                    animauxTableAdapter.Update(DBAnimaux);
                    animal.IsNew = false;
                    button_editA.Text = "Enregistrer modifications";
                    buttonDeleteA.Enabled = true;
                    currentAnimal = comboBox_listA.Items.Count - 1;
                    comboBox_listA.Items.Add(ChampANom.Text);
                    comboBox_listA.SelectedIndex = currentAnimal + 1;
                    MessageBox.Show("Nouvel animal créé.", "Tâche complétée", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    animal.EditAnimalInDB();
                    animauxTableAdapter.Update(DBAnimaux);
                    MessageBox.Show("Informations animal modifiées.", "Tâche complétée", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                }
                clientsTableAdapter.Update(DBClients);
            }
            else
            { MessageBox.Show("Erreur : Informations obligatoires manquantes.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            groupBoxInfoAnimal.Enabled = true;
        }

        private void ToggleChampP(object sender, EventArgs e)
        {
            ChampAParfum.Enabled = checkBoxParfum.Checked;
            if (!ChampAParfum.Enabled) { ChampAParfum.Text = ""; }
        }

        private void ToggleChampE(object sender, EventArgs e)
        {
            ChampAAutre.Enabled = radioButtonAutre.Checked;
            if (!ChampAAutre.Enabled) { ChampAAutre.Text = ""; }
        }
    }
}
