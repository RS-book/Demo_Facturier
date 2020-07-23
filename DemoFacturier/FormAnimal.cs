using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoFacturier
{
    public partial class FormAnimal : Form
    {
        public FormAnimal(Animal animal)
        {
            InitializeComponent();
            currentAnimalF = animal;
            if (currentAnimalF.IsNew)
            { 
                this.Text = "Nouvel animal";
                this.buttonOK.Text = "Créer nouvel animal";
            } 
            else
            { 
                this.Text = "Éditer animal";
                this.buttonOK.Text = "Enregistrer les modifications";
                ChampANom.Text = currentAnimalF.Nom;
                switch(currentAnimalF.Espece)
                {
                    case "Chien":
                        radioButtonChien.Checked = true;
                        radioButtonChat.Checked = false;
                        radioButtonAutre.Checked = false;
                        break;
                    case "Chat":
                        radioButtonChien.Checked = false;
                        radioButtonChat.Checked = true;
                        radioButtonAutre.Checked = false;
                        break;
                    default:
                        radioButtonChien.Checked = false;
                        radioButtonChat.Checked = false;
                        radioButtonAutre.Checked = true;
                        ChampAAutre.Text = currentAnimalF.Espece;
                        break;
                }
                ChampARace.Text = currentAnimalF.Race;
                ChampANaiss.Text = currentAnimalF.AnNaiss;
                checkBoxFriand.Checked = currentAnimalF.Friand;
                switch(currentAnimalF.Parfum)
                {
                    case "oui":
                        checkBoxParfum.Checked = true;
                        break;
                    case "non":
                        checkBoxParfum.Checked = false;
                        break;
                    default:
                        checkBoxParfum.Checked = true;
                        ChampAParfum.Text = currentAnimalF.Parfum;
                        break;
                }
                richTextBox1.Text = currentAnimalF.Remarques;
            }
        }

        public Animal currentAnimalF;

        private void Validate(object sender, EventArgs e)
        {
            if (CheckValid())
            {
                currentAnimalF.Nom = ChampANom.Text;
                if (radioButtonChien.Checked == true)
                {
                    currentAnimalF.Espece = "Chien";
                }
                else
                {
                    if (radioButtonChat.Checked == true)
                    {
                        currentAnimalF.Espece = "Chat";
                    }
                    else
                    {
                        currentAnimalF.Espece = ChampAAutre.Text;
                    }
                }
                currentAnimalF.Race = ChampARace.Text;
                currentAnimalF.AnNaiss = ChampANaiss.Text;
                currentAnimalF.Friand = checkBoxFriand.Checked;
                if (checkBoxParfum.Checked)
                {
                    if (ChampAParfum.Text == "")
                    { currentAnimalF.Parfum = "oui"; }
                    else
                    { currentAnimalF.Parfum = ChampAParfum.Text; }
                }
                else
                { currentAnimalF.Parfum = "non"; }
                currentAnimalF.Remarques = richTextBox1.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            { MessageBox.Show("Erreur : Informations obligatoires manquantes.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Cancel(object sender, EventArgs e)
        {
            if (MessageBox.Show("Les modifications non enregistrées seront perdues, continuer ?", "Demande de confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private bool CheckValid()
        {
            if (ChampANom.Text == "" || (radioButtonAutre.Checked && ChampAAutre.Text == ""))
            { return false; }
            else
            { return true; }
        }
    }
}
