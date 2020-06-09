using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoFacturier
{
    class Client
    {
        public Client(TextBox name1, TextBox name2, TextBox prename1, TextBox prename2, TextBox num, TextBox street, TextBox cpl, TextBox postcode, TextBox city, TextBox maintel, TextBox telmob1, TextBox telmob2)
        {
            IsNew = true;
            Nom1 = name1;
            Nom2 = name2;
            Prenom1 = prename1;
            Prenom2 = prename2;
            NumAdr = num;
            Rue = street;
            Cplmt = cpl;
            Ville = city;
            CodePost = postcode;
            TelPrinc = maintel;
            Mobile1 = telmob1;
            Mobile2 = telmob2;
            Animals = new List<Animal>();
        }
        public Client(FacturierDatabaseDataSet.ClientsRow row, FacturierDatabaseDataSet.AnimauxDataTable animaux, TextBox name1, TextBox name2, TextBox prename1, TextBox prename2, TextBox num, TextBox street, TextBox cpl, TextBox postcode, TextBox city, TextBox maintel, TextBox telmob1, TextBox telmob2)
        {
            IsNew = false;
            InfosClient = row;
            Nom1 = name1;
            Nom2 = name2;
            Prenom1 = prename1;
            Prenom2 = prename2;
            NumAdr = num;
            Rue = street;
            Cplmt = cpl;
            Ville = city;
            CodePost = postcode;
            TelPrinc = maintel;
            Mobile1 = telmob1;
            Mobile2 = telmob2;
            Animals = new List<Animal>();
            foreach (FacturierDatabaseDataSet.AnimauxRow animal in row.GetAnimauxRows())
            {
                Animals.Add(new Animal(animal));
            }
        }

        public bool IsNew { get; set; }

        private readonly TextBox Nom1;
        private readonly TextBox Prenom1;
        private readonly TextBox Nom2;
        private readonly TextBox Prenom2;
        private readonly TextBox NumAdr;
        private readonly TextBox Rue;
        private readonly TextBox Cplmt;
        private readonly TextBox Ville;
        private readonly TextBox CodePost;
        private readonly TextBox TelPrinc;
        private readonly TextBox Mobile1;
        private readonly TextBox Mobile2;
        private FacturierDatabaseDataSet.ClientsRow InfosClient;
        public List<Animal> Animals { get; set; }

        public bool CheckIfValid()
        {
            if (this.Nom1.Text == "" || this.TelPrinc.Text == "")
            { return false; }
            else
            { return true; }
        }

        public void AddClientToDB(FacturierDatabaseDataSet.ClientsDataTable DBClients)
        {
            InfosClient = DBClients.NewClientsRow();
            InfosClient.BeginEdit();
            InfosClient.IdClient = DBClients.Rows.Count + 1;
            InfosClient.Nom1 = this.Nom1.Text;
            if (this.Nom2.Text == "") { InfosClient.SetNom2Null(); } else { InfosClient.Nom2 = this.Nom2.Text; }
            if (this.Prenom1.Text == "") { InfosClient.SetPrenom1Null(); } else { InfosClient.Prenom1 = this.Prenom1.Text; }
            if (this.Prenom2.Text == "") { InfosClient.SetPrenom2Null(); } else { InfosClient.Prenom2 = this.Prenom2.Text; }
            if (this.NumAdr.Text == "") { InfosClient.SetNumAdrNull(); } else { InfosClient.NumAdr = this.NumAdr.Text; }
            if (this.Rue.Text == "") { InfosClient.SetRueNull(); } else { InfosClient.Rue = this.Rue.Text; }
            if (this.Cplmt.Text == "") { InfosClient.SetCplmtNull(); } else { InfosClient.Cplmt = this.Cplmt.Text; }
            if (this.Ville.Text == "") { InfosClient.SetVilleNull(); } else { InfosClient.Ville = this.Ville.Text; }
            if (this.CodePost.Text == "") { InfosClient.SetCodePostNull(); } else { InfosClient.CodePost = this.CodePost.Text; }
            InfosClient.TelPrinc = this.TelPrinc.Text;
            if (this.Mobile1.Text == "") { InfosClient.SetMobile1Null(); } else { InfosClient.Mobile1 = this.Mobile1.Text; }
            if (this.Mobile2.Text == "") { InfosClient.SetMobile2Null(); } else { InfosClient.Mobile2 = this.Mobile2.Text; }
            InfosClient.EndEdit();
            DBClients.AddClientsRow(InfosClient);
        }

        public void EditClientInDB(FacturierDatabaseDataSet.ClientsDataTable DBClients)
        {
            InfosClient.BeginEdit();
            if (this.Nom2.Text == "") { InfosClient.SetNom2Null(); } else { InfosClient.Nom2 = this.Nom2.Text; }
            if (this.Prenom1.Text == "") { InfosClient.SetPrenom1Null(); } else { InfosClient.Prenom1 = this.Prenom1.Text; }
            if (this.Prenom2.Text == "") { InfosClient.SetPrenom2Null(); } else { InfosClient.Prenom2 = this.Prenom2.Text; }
            if (this.NumAdr.Text == "") { InfosClient.SetNumAdrNull(); } else { InfosClient.NumAdr = this.NumAdr.Text; }
            if (this.Rue.Text == "") { InfosClient.SetRueNull(); } else { InfosClient.Rue = this.Rue.Text; }
            if (this.Cplmt.Text == "") { InfosClient.SetCplmtNull(); } else { InfosClient.Cplmt = this.Cplmt.Text; }
            if (this.Ville.Text == "") { InfosClient.SetVilleNull(); } else { InfosClient.Ville = this.Ville.Text; }
            if (this.CodePost.Text == "") { InfosClient.SetCodePostNull(); } else { InfosClient.CodePost = this.CodePost.Text; }
            InfosClient.TelPrinc = this.TelPrinc.Text;
            if (this.Mobile1.Text == "") { InfosClient.SetMobile1Null(); } else { InfosClient.Mobile1 = this.Mobile1.Text; }
            if (this.Mobile2.Text == "") { InfosClient.SetMobile2Null(); } else { InfosClient.Mobile2 = this.Mobile2.Text; }
            InfosClient.EndEdit();
        }

        public void FillClientChamps()
        {
            this.Nom1.Text = InfosClient.Nom1;
            if (InfosClient.IsNom2Null()) { this.Nom2.Text = ""; } else { this.Nom2.Text = InfosClient.Nom2; }
            if (InfosClient.IsPrenom1Null()) { this.Prenom1.Text = ""; } else { this.Prenom1.Text = InfosClient.Prenom1; }
            if (InfosClient.IsPrenom2Null()) { this.Prenom2.Text = ""; } else { this.Prenom2.Text = InfosClient.Prenom2; }
            if (InfosClient.IsNumAdrNull()) { this.NumAdr.Text = ""; } else { this.NumAdr.Text = InfosClient.NumAdr; }
            if (InfosClient.IsRueNull()) { this.Rue.Text = ""; } else { this.Rue.Text = InfosClient.Rue; }
            if (InfosClient.IsCplmtNull()) { this.Cplmt.Text = ""; } else { this.Cplmt.Text = InfosClient.Cplmt; }
            if (InfosClient.IsVilleNull()) { this.Ville.Text = ""; } else { this.Ville.Text = InfosClient.Ville; }
            if (InfosClient.IsCodePostNull()) { this.CodePost.Text = ""; } else { this.CodePost.Text = InfosClient.CodePost; }
            this.TelPrinc.Text = InfosClient.TelPrinc;
            if (InfosClient.IsMobile1Null()) { this.Mobile1.Text = ""; } else { this.Mobile1.Text = InfosClient.Mobile1; }
            if (InfosClient.IsMobile2Null()) { this.Mobile2.Text = ""; } else { this.Mobile2.Text = InfosClient.Mobile2; }
        }

        public void ClearClientChamps()
        {
            this.Nom1.Text = "";
            this.Nom2.Text = "";
            this.Prenom1.Text = "";
            this.Prenom2.Text = "";
            this.NumAdr.Text = "";
            this.Rue.Text = "";
            this.Cplmt.Text = "";
            this.Ville.Text = "";
            this.CodePost.Text = "";
            this.TelPrinc.Text = "";
            this.Mobile1.Text = "";
            this.Mobile2.Text = "";
        }

        public bool WereChangesToCMade()
        {
            bool result = false;
            if ((IsNew && Nom1.Text != "") || (!IsNew && Nom1.Text != InfosClient.Nom1)) { result = true; }
            if (IsNew || InfosClient.IsNom2Null()) { if (Nom2.Text != "") { result = true; } } else { if (Nom2.Text != InfosClient.Nom2) { result = true; } }
            if (IsNew || InfosClient.IsPrenom1Null()) { if (Prenom1.Text != "") { result = true; } } else { if (Prenom1.Text != InfosClient.Prenom1) { result = true; } }
            if (IsNew || InfosClient.IsPrenom2Null()) { if (Prenom2.Text != "") { result = true; } } else { if (Prenom2.Text != InfosClient.Prenom2) { result = true; } }
            if (IsNew || InfosClient.IsNumAdrNull()) { if (NumAdr.Text != "") { result = true; } } else { if (NumAdr.Text != InfosClient.NumAdr) { result = true; } }
            if (IsNew || InfosClient.IsRueNull()) { if (Rue.Text != "") { result = true; } } else { if (Rue.Text != InfosClient.Rue) { result = true; } }
            if (IsNew || InfosClient.IsCplmtNull()) { if (Cplmt.Text != "") { result = true; } } else { if (Cplmt.Text != InfosClient.Cplmt) { result = true; } }
            if (IsNew || InfosClient.IsVilleNull()) { if (Ville.Text != "") { result = true; } } else { if (Ville.Text != InfosClient.Ville) { result = true; } }
            if (IsNew || InfosClient.IsCodePostNull()) { if (CodePost.Text != "") { result = true; } } else { if (CodePost.Text != InfosClient.CodePost) { result = true; } }
            if ((IsNew && TelPrinc.Text != "") || (!IsNew && TelPrinc.Text != InfosClient.TelPrinc)) { result = true; }
            if (IsNew || InfosClient.IsMobile1Null()) { if (Mobile1.Text != "") { result = true; } } else { if (Mobile1.Text != InfosClient.Mobile1) { result = true; } }
            if (IsNew || InfosClient.IsMobile2Null()) { if (Mobile2.Text != "") { result = true; } } else { if (Mobile2.Text != InfosClient.Mobile2) { result = true; } }
            return result;
        }
    }
}
