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
            currentClient = new Client();
        }

        private readonly FacturierDatabaseDataSet.ClientsDataTable DBClients;
        private readonly FacturierDatabaseDataSet.AnimauxDataTable DBAnimaux;
        private readonly ClientsTableAdapter clientsTableAdapter;
        private readonly AnimauxTableAdapter animauxTableAdapter;

        private Client currentClient;
        private int currentAnimal; //-1 : pas d'animal sélectionné
        private readonly List<FacturierDatabaseDataSet.ClientsRow> listeRecherche;

        private void RechercheC(object sender, EventArgs e) //manque limite de taille liste
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
                //idem avec nom 2, autres critères ? poser limite de résultats pour limiter taille de la liste
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
            currentClient = new Client(listeRecherche[comboBox_searchC_res.SelectedIndex]);
            FillChampsClient(currentClient);
            groupBoxInfoClient.Enabled = true;
            buttonEditC.Enabled = true;
            buttonDeleteC.Enabled = true;
            List<string> listeA = new List<string>();
            listeA.Add("Sélectionner un animal");
            foreach (Animal animal in currentClient.Animals) { listeA.Add(animal.Nom + ", " + animal.Espece); }
            comboBox_listA.Items.Clear();
            comboBox_listA.Items.AddRange(listeA.ToArray());
            comboBox_listA.SelectedIndex = 0;
            currentAnimal = -1;
            comboBox_listA.Enabled = true;
            button_newA.Enabled = true;
        }

        private void CreerClient(object sender, EventArgs e)
        {
            Client tempClient = new Client();
            using (FormClient form = new FormClient(tempClient))
            {
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    currentClient = tempClient;
                    currentClient.AddClientToDB(DBClients);
                    clientsTableAdapter.Update(DBClients);
                    currentClient.IsNew = false;
                    FillChampsClient(currentClient);
                    groupBoxInfoClient.Enabled = true;
                    buttonEditC.Enabled = true;
                    buttonDeleteC.Enabled = true;
                    comboBox_listA.Items.Clear();
                    groupBoxInfoAnimal.Enabled = false;
                    buttonDeleteA.Enabled = false;
                    button_newA.Enabled = true;
                    button_editA.Enabled = false;
                    MessageBox.Show("Nouveau client créé.", "Tâche complétée", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (result == DialogResult.Cancel) { /*rien pour l'instant ?*/}
                }
            }
        }

        private void EditClient(object sender, EventArgs e)
        {
            using (FormClient form = new FormClient(currentClient))
            {
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    currentClient.EditClientInDB(DBClients);
                    clientsTableAdapter.Update(DBClients);
                    MessageBox.Show("Informations client modifiées.", "Tâche complétée", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                }
                else
                {
                    if (result == DialogResult.Cancel) { /*rien pour l'instant ?*/}
                }
            }            
        }

        private void SupprimerC(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce client ? Cela supprimera également les animaux associés.", "Demande de confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                foreach(Animal animal in currentClient.Animals)
                {
                    animal.DeleteAFromDB(DBAnimaux);
                }
                animauxTableAdapter.Update(DBAnimaux);
                ClearChampsAnimal();
                comboBox_listA.Items.Clear();
                comboBox_listA.SelectedIndex = 0;
                groupBoxInfoAnimal.Enabled = false;
                button_newA.Enabled = false;
                button_editA.Enabled = false;
                buttonDeleteA.Enabled = false;
                currentClient.DeleteCFromDB(DBClients);
                clientsTableAdapter.Update(DBClients);
                ClearChampsClient();
                groupBoxInfoClient.Enabled = false;
                buttonEditC.Enabled = false;
                buttonDeleteC.Enabled = false;
            }
        }

        private void LoadAnimal(object sender, EventArgs e)
        {
            currentAnimal = comboBox_listA.SelectedIndex - 1;
            if (currentAnimal != -1)
            {
                groupBoxInfoAnimal.Enabled = true;
                FillChampsAnimal(currentClient.Animals[currentAnimal]);
                button_editA.Enabled = true;
                buttonDeleteA.Enabled = true;
            }
            else
            {
                groupBoxInfoAnimal.Enabled = false;
                ClearChampsAnimal();
                buttonDeleteA.Enabled = false;
                button_editA.Enabled = false;
            }
        }

        private void CreerAnimal(object sender, EventArgs e)
        {
            Animal tempAnimal = new Animal();
            using (FormAnimal form = new FormAnimal(tempAnimal))
            {
                DialogResult result = form.ShowDialog();
                if(result==DialogResult.OK)
                {
                    currentAnimal = currentClient.Animals.Count;
                    tempAnimal.AddAnimalToDB(DBAnimaux);
                    animauxTableAdapter.Update(DBAnimaux);
                    tempAnimal.IsNew = false;
                    currentClient.Animals.Add(tempAnimal);
                    FillChampsAnimal(tempAnimal);
                    groupBoxInfoAnimal.Enabled = true;
                    button_editA.Enabled = true;
                    buttonDeleteA.Enabled = true;
                    comboBox_listA.Items.Add(tempAnimal.Nom + ", " + tempAnimal.Espece);
                    comboBox_listA.SelectedIndex = currentAnimal + 1;
                    MessageBox.Show("Nouvel animal créé.", "Tâche complétée", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (result == DialogResult.Cancel) { /*rien pour l'instant ?*/}
                }
            }
        }

        private void DeleteAnimal(object sender, EventArgs e)
        {
            if(MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet animal ?", "Demande de confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                groupBoxInfoAnimal.Enabled = false;
                currentClient.Animals[currentAnimal].DeleteAFromDB(DBAnimaux);
                animauxTableAdapter.Update(DBAnimaux);
                comboBox_listA.SelectedIndex = 0;
                comboBox_listA.Items.RemoveAt(currentAnimal + 1);
            }
        }

        private void EditAnimal(object sender, EventArgs e)
        {
            Animal animal = currentClient.Animals[currentAnimal];
            using (FormAnimal form = new FormAnimal(animal))
            {
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    animal.EditAnimalInDB(DBAnimaux);
                    animauxTableAdapter.Update(DBAnimaux);
                    MessageBox.Show("Informations de l'animal modifiées.", "Tâche complétée", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (result == DialogResult.Cancel) { /*rien pour l'instant ?*/}
                }
            }
        }

        private void FillChampsClient(Client client)
        {
            ChampNom1.Text = client.Nom1;
            ChampNom2.Text = client.Nom2;
            ChampPrenom1.Text = client.Prenom1;
            ChampPrenom2.Text = client.Prenom2;
            ChampN.Text = client.NumAdr;
            ChampRue.Text = client.Rue;
            ChampCpl.Text = client.Cplmt;
            ChampCodePost.Text = client.CodePost;
            ChampVille.Text = client.Ville;
            ChampTelPrinc.Text = client.TelPrinc;
            ChampMobile1.Text = client.Mobile1;
            ChampMobile2.Text = client.Mobile2;
        }

        private void FillChampsAnimal(Animal animal)
        {
            ChampANom.Text = animal.Nom;
            switch (animal.Espece)
            {
                case "Chien":
                    radioButtonChien.Checked = true;
                    radioButtonChat.Checked = false;
                    radioButtonAutre.Checked = false;
                    ChampAAutre.Text = "";
                    break;
                case "Chat":
                    radioButtonChat.Checked = true;
                    radioButtonChien.Checked = false;
                    radioButtonAutre.Checked = false;
                    ChampAAutre.Text = "";
                    break;
                default:
                    radioButtonChien.Checked = false;
                    radioButtonChat.Checked = false;
                    radioButtonAutre.Checked = true;
                    ChampAAutre.Text = animal.Espece;
                    break;
            }
            ChampARace.Text = animal.Race;
            ChampANaiss.Text = animal.AnNaiss;
            checkBoxFriand.Checked = animal.Friand;
            switch (animal.Parfum)
            {
                case "non":
                    checkBoxParfum.Checked = false;
                    ChampAParfum.Text = "";
                    break;
                case "oui":
                    checkBoxParfum.Checked = true;
                    ChampAParfum.Text = "";
                    break;
                default:
                    checkBoxParfum.Checked = true;
                    ChampAParfum.Text = animal.Parfum;
                    break;
            }
            champARemarques.Text = animal.Remarques; 
        }

        private void ClearChampsClient()
        {
            ChampNom1.Text = "";
            ChampNom2.Text = "";
            ChampPrenom1.Text = "";
            ChampPrenom2.Text = "";
            ChampN.Text = "";
            ChampRue.Text = "";
            ChampCpl.Text = "";
            ChampCodePost.Text = "";
            ChampVille.Text = "";
            ChampTelPrinc.Text = "";
            ChampMobile1.Text = "";
            ChampMobile2.Text = "";
        }

        private void ClearChampsAnimal()
        {
            ChampANom.Text = "";
            radioButtonChien.Checked = true;
            radioButtonChat.Checked = false;
            radioButtonAutre.Checked = false;
            ChampAAutre.Text = "";
            ChampARace.Text = "";
            ChampANaiss.Text = "";
            checkBoxFriand.Checked = false;
            checkBoxParfum.Checked = false;
            ChampAParfum.Text = "";
            champARemarques.Text = "";
        }


    }
}
