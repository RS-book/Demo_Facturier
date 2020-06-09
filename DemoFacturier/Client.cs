using System;
using System.Collections.Generic;
using System.Data;
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

        private TextBox Nom1;
        private TextBox Prenom1;
        private TextBox Nom2;
        private TextBox Prenom2;
        private TextBox NumAdr;
        private TextBox Rue;
        private TextBox Cplmt;
        private TextBox Ville;
        private TextBox CodePost;
        private TextBox TelPrinc;
        private TextBox Mobile1;
        private TextBox Mobile2;
        private FacturierDatabaseDataSet.ClientsRow InfosClient;
        public List<Animal> Animals { get; set; }

        public bool CheckIfValid()
        {
            /*if (this.Nom1 == "" || this.TelPrinc == "")
            { return false; }
            else
            { return true; }*/
            return true;
        }

        public void AddClientToDB(FacturierDatabaseDataSet.ClientsDataTable DBClients)
        {
            InfosClient = DBClients.NewClientsRow();
            InfosClient.IdClient = DBClients.Rows.Count + 1;
            //remplir info depuis textboxes
            /*ClientRow.Nom1 = this.Nom1;
            if (this.Nom2 == "") { ClientRow.SetNom2Null(); } else { ClientRow.Nom2 = this.Nom2; }
            if (this.Prenom1 == "") { ClientRow.SetPrenom1Null(); } else { ClientRow.Prenom1 = this.Prenom1; }
            if (this.Prenom2 == "") { ClientRow.SetPrenom2Null(); } else { ClientRow.Prenom2 = this.Prenom2; }
            if (this.NumAdr == "") { ClientRow.SetNumAdrNull(); } else { ClientRow.NumAdr = this.NumAdr; }
            if (this.Rue == "") { ClientRow.SetRueNull(); } else { ClientRow.Rue = this.Rue; }
            if (this.Cplmt == "") { ClientRow.SetCplmtNull(); } else { ClientRow.Cplmt = this.Cplmt; }
            if (this.Ville == "") { ClientRow.SetVilleNull(); } else { ClientRow.Ville = this.Ville; }
            if (this.CodePost == "") { ClientRow.SetCodePostNull(); } else { ClientRow.CodePost = this.CodePost; }
            ClientRow.TelPrinc = this.TelPrinc;
            if (this.Mobile1 == "") { ClientRow.SetMobile1Null(); } else { ClientRow.Mobile1 = this.Mobile1; }
            if (this.Mobile2 == "") { ClientRow.SetMobile2Null(); } else { ClientRow.Mobile2 = this.Mobile2; }*/
            DBClients.AddClientsRow(InfosClient);
            DBClients.AcceptChanges();
        }

        public void EditClientInDB(FacturierDatabaseDataSet.ClientsDataTable DBClients)
        {
            InfosClient.BeginEdit();
            //éditer informations depuis textboxes.
            /*row.Nom1 = this.Nom1;
            if (this.Prenom1 == "") { row.SetPrenom1Null(); } else { row.Prenom1 = this.Prenom1; }
            if (this.Nom2 == "") { row.SetNom2Null(); } else { row.Nom2 = this.Nom2; }
            if (this.Prenom2 == "") { row.SetPrenom2Null(); } else { row.Prenom2 = this.Prenom2; }
            if (this.NumAdr == "") { row.SetNumAdrNull(); } else { row.NumAdr = this.NumAdr; }
            if (this.Rue == "") { row.SetRueNull(); } else { row.Rue = this.Rue; }
            if (this.Cplmt == "") { row.SetCplmtNull(); } else { row.Cplmt = this.Cplmt; }
            if (this.Ville == "") { row.SetVilleNull(); } else { row.Ville = this.Ville; }
            if (this.CodePost == "") { row.SetCodePostNull(); } else { row.CodePost = this.CodePost; }
            row.TelPrinc = this.TelPrinc;
            if (this.Mobile1 == "") { row.SetMobile1Null(); } else { row.Mobile1 = this.Mobile1; }
            if (this.Mobile2 == "") { row.SetMobile2Null(); } else { row.Mobile2 = this.Mobile2; }*/
            DBClients.AcceptChanges();
        }

        public void FillClientChamps()
        {
            //remplir textboxes à partir de ClientRow
            /*Nom1 = row.Nom1;
            if (row.IsNom2Null()) { Nom2 = ""; } else { Nom2 = row.Nom2; }
            if (row.IsPrenom1Null()) { Prenom1 = ""; } else { Prenom1 = row.Prenom1; }
            if (row.IsPrenom2Null()) { Prenom2 = ""; } else { Prenom2 = row.Prenom2; }
            if (row.IsNumAdrNull()) { NumAdr = ""; } else { NumAdr = row.NumAdr; }
            if (row.IsRueNull()) { Rue = ""; } else { Rue = row.Rue; }
            if (row.IsCplmtNull()) { Cplmt = ""; } else { Cplmt = row.Cplmt; }
            if (row.IsVilleNull()) { Ville = ""; } else { Ville = row.Ville; }
            if (row.IsCodePostNull()) { CodePost = ""; } else { CodePost = row.CodePost; }
            TelPrinc = row.TelPrinc;
            if (row.IsMobile1Null()) { Mobile1 = ""; } else { Mobile1 = row.Mobile1; }
            if (row.IsMobile2Null()) { Mobile2 = ""; } else { Mobile2 = row.Mobile2; }*/
        }

        public void ClearClientChamps()
        {
            //vider textboxes
        }

        public bool WereChangesToCMade()
        {
            //vérifier textboxes vs info client, si différence > true, sinon false (comportement différent si IsNew true, moindre donnée = changement vu que nouveau)
            return true;
        }
    }
}
