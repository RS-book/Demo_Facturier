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
    public partial class FormClient : Form
    {
        public FormClient(Client client)
        {
            InitializeComponent();
            currentClientF = client;
            if (currentClientF.IsNew) 
            { 
                this.Text = "Nouveau client";
                buttonOK.Text = "Créer nouveau client";
            } 
            else 
            { 
                this.Text = "Éditer client";
                buttonOK.Text = "Enregistrer les modifications";
                ChampNom1.Text = currentClientF.Nom1;
                ChampNom2.Text = currentClientF.Nom2;
                ChampPrenom1.Text = currentClientF.Prenom1;
                ChampPrenom2.Text = currentClientF.Prenom2;
                ChampN.Text = currentClientF.NumAdr;
                ChampRue.Text = currentClientF.Rue;
                ChampCpl.Text = currentClientF.Cplmt;
                ChampCodePost.Text = currentClientF.CodePost;
                ChampVille.Text = currentClientF.Ville;
                ChampTelPrinc.Text = currentClientF.TelPrinc;
                ChampMobile1.Text = currentClientF.Mobile1;
                ChampMobile2.Text = currentClientF.Mobile2;
            }
        }

        public Client currentClientF;

        private void Validate(object sender, EventArgs e)
        {
            if (CheckValid())
            {
                currentClientF.Nom1 = ChampNom1.Text;
                currentClientF.Nom2 = ChampNom2.Text;
                currentClientF.Prenom1 = ChampPrenom1.Text;
                currentClientF.Prenom2 = ChampPrenom2.Text;
                currentClientF.NumAdr = ChampN.Text;
                currentClientF.Rue = ChampRue.Text;
                currentClientF.Cplmt = ChampCpl.Text;
                currentClientF.CodePost = ChampCodePost.Text;
                currentClientF.Ville = ChampVille.Text;
                currentClientF.TelPrinc = ChampTelPrinc.Text;
                currentClientF.Mobile1 = ChampMobile1.Text;
                currentClientF.Mobile2 = ChampMobile2.Text;
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
            if (ChampNom1.Text == "" || ChampTelPrinc.Text == "")
            { return false; }
            else
            { return true; }
        }
    }
}
