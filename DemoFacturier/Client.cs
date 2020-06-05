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
        public Client(FacturierDatabaseDataSet.ClientsDataTable DBClients)
        {
            IsNew = true;
            IdClient = 0;
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
        public Client(FacturierDatabaseDataSet.ClientsRow row, FacturierDatabaseDataSet.AnimauxDataTable animaux)
        {
            IsNew = false;
            IdClient = row.IdClient;
            Nom1 = row.Nom1;
            Nom2 = row.Nom2;
            Prenom1 = row.Prenom1;
            Prenom2 = row.Prenom2;
            NumAdr = row.NumAdr;
            Rue = row.Rue;
            Cplmt = row.Cplmt;
            Ville = row.Ville;
            CodePost = row.CodePost;
            TelPrinc = row.TelPrinc;
            Mobile1 = row.Mobile1;
            Mobile2 = row.Mobile2;
            Animals = new List<Animal>();
            foreach (FacturierDatabaseDataSet.AnimauxRow animal in row.GetAnimauxRows())
            {
                Animals.Add(new Animal(animal));
            }
        }

        public bool IsNew { get; set; }
        public int IdClient { get; set; }
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

        public bool CheckIfValid()
        {
            if (this.Nom1 == "" || this.TelPrinc == "")
            { return false; }
            else
            { return true; }
        }

        public void AddClientToDB(FacturierDatabaseDataSet.ClientsDataTable DBClients)
        {
            FacturierDatabaseDataSet.ClientsRow ClientRow = DBClients.NewClientsRow();
            ClientRow.IdClient = DBClients.Rows.Count + 1;
            ClientRow.Nom1 = this.Nom1;
            ClientRow.Nom2 = this.Nom2;
            ClientRow.Prenom1 = this.Prenom1;
            ClientRow.Prenom2 = this.Prenom2;
            ClientRow.NumAdr = this.NumAdr;
            ClientRow.Rue = this.Rue;
            ClientRow.Cplmt = this.Cplmt;
            ClientRow.Ville = this.Ville;
            ClientRow.CodePost = this.CodePost;
            ClientRow.TelPrinc = this.TelPrinc;
            ClientRow.Mobile1 = this.Mobile1;
            ClientRow.Mobile2 = this.Mobile2;
            DBClients.AddClientsRow(ClientRow);
            DBClients.AcceptChanges();
        }

        public void EditClientInDB(FacturierDatabaseDataSet.ClientsDataTable DBClients)
        {
            foreach(FacturierDatabaseDataSet.ClientsRow row in DBClients)
            {
                if(row.IdClient==this.IdClient)
                {
                    row.BeginEdit();
                    row.Nom1 = this.Nom1;
                    row.Prenom1 = this.Prenom1;
                    row.Nom2 = this.Nom2;
                    row.Prenom2 = this.Prenom2;
                    row.NumAdr = this.NumAdr;
                    row.Rue = this.Rue;
                    row.Cplmt = this.Cplmt;
                    row.Ville = this.Ville;
                    row.CodePost = this.CodePost;
                    row.TelPrinc = this.TelPrinc;
                    row.Mobile1 = this.Mobile1;
                    row.Mobile2 = this.Mobile2;
                }
            }
            DBClients.AcceptChanges();
        }
    }
}
