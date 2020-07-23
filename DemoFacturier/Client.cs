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
    public class Client
    {
        public Client()
        {
            IsNew = true;
            Id = -1;
            Nom1 = "";
            Nom2 = "";
            Prenom1 = "";
            Prenom2 = "";
            NumAdr = "";
            Rue = "";
            Cplmt = "";
            Ville = "";
            CodePost = "";
            TelPrinc = "";
            Mobile1 = "";
            Mobile2 = "";
            Animals = new List<Animal>();
        }
        public Client(FacturierDatabaseDataSet.ClientsRow row)
        {
            IsNew = false;
            Id = row.IdClient;
            Nom1 = row.Nom1;
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
            if (row.IsMobile2Null()) { Mobile2 = ""; } else { Mobile2 = row.Mobile2; }
            Animals = new List<Animal>();
            foreach(FacturierDatabaseDataSet.AnimauxRow animal in row.GetAnimauxRows())
            {
                Animals.Add(new Animal(animal));
            }
        }

        public bool IsNew { get; set; }

        public int Id { get; set; }
        public string Nom1 { get; set; }
        public string Prenom1 { get; set; }
        public string Nom2 { get; set; }
        public string Prenom2 { get; set; }
        public string NumAdr { get; set; }
        public string Rue { get; set; }
        public string Cplmt { get; set; }
        public string Ville { get; set; }
        public string CodePost { get; set; }
        public string TelPrinc { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public List<Animal> Animals { get; set; }

        public void AddClientToDB(FacturierDatabaseDataSet.ClientsDataTable DBClients)
        {
            Id = DBClients.Rows[DBClients.Rows.Count - 1].Field<int>("IdClient") + 1;
            FacturierDatabaseDataSet.ClientsRow InfosClient = DBClients.NewClientsRow();
            InfosClient.BeginEdit();
            InfosClient.IdClient = Id;
            InfosClient.Nom1 = Nom1;
            if (Nom2 == "") { InfosClient.SetNom2Null(); } else { InfosClient.Nom2 = Nom2; }
            if (Prenom1 == "") { InfosClient.SetPrenom1Null(); } else { InfosClient.Prenom1 = Prenom1; }
            if (Prenom2 == "") { InfosClient.SetPrenom2Null(); } else { InfosClient.Prenom2 = Prenom2; }
            if (NumAdr == "") { InfosClient.SetNumAdrNull(); } else { InfosClient.NumAdr = NumAdr; }
            if (Rue == "") { InfosClient.SetRueNull(); } else { InfosClient.Rue = Rue; }
            if (Cplmt == "") { InfosClient.SetCplmtNull(); } else { InfosClient.Cplmt = Cplmt; }
            if (Ville == "") { InfosClient.SetVilleNull(); } else { InfosClient.Ville = Ville; }
            if (CodePost == "") { InfosClient.SetCodePostNull(); } else { InfosClient.CodePost = CodePost; }
            InfosClient.TelPrinc = TelPrinc;
            if (Mobile1 == "") { InfosClient.SetMobile1Null(); } else { InfosClient.Mobile1 = Mobile1; }
            if (Mobile2 == "") { InfosClient.SetMobile2Null(); } else { InfosClient.Mobile2 = Mobile2; }
            InfosClient.EndEdit();
            DBClients.AddClientsRow(InfosClient);
        }

        public void EditClientInDB(FacturierDatabaseDataSet.ClientsDataTable DBClients)
        {
            FacturierDatabaseDataSet.ClientsRow InfosClient = DBClients.FindByIdClient(Id);
            InfosClient.BeginEdit();
            InfosClient.Nom1 = Nom1;
            if (Nom2 == "") { InfosClient.SetNom2Null(); } else { InfosClient.Nom2 = Nom2; }
            if (Prenom1 == "") { InfosClient.SetPrenom1Null(); } else { InfosClient.Prenom1 = Prenom1; }
            if (Prenom2 == "") { InfosClient.SetPrenom2Null(); } else { InfosClient.Prenom2 = Prenom2; }
            if (NumAdr == "") { InfosClient.SetNumAdrNull(); } else { InfosClient.NumAdr = NumAdr; }
            if (Rue == "") { InfosClient.SetRueNull(); } else { InfosClient.Rue = Rue; }
            if (Cplmt == "") { InfosClient.SetCplmtNull(); } else { InfosClient.Cplmt = Cplmt; }
            if (Ville == "") { InfosClient.SetVilleNull(); } else { InfosClient.Ville = Ville; }
            if (CodePost == "") { InfosClient.SetCodePostNull(); } else { InfosClient.CodePost = CodePost; }
            InfosClient.TelPrinc = TelPrinc;
            if (Mobile1 == "") { InfosClient.SetMobile1Null(); } else { InfosClient.Mobile1 = Mobile1; }
            if (Mobile2 == "") { InfosClient.SetMobile2Null(); } else { InfosClient.Mobile2 = Mobile2; }
            InfosClient.EndEdit();
        }

        public void DeleteCFromDB(FacturierDatabaseDataSet.ClientsDataTable DBClients)
        {
            FacturierDatabaseDataSet.ClientsRow InfosClient = DBClients.FindByIdClient(Id);
            InfosClient.Delete();            
        }
    }
}
