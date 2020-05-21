using DemoFacturier.FacturierDatabaseDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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
        }

        private void RechercheC(object sender, EventArgs e)
        {
            Regex rx = new Regex(input_rechercheClient.Text, RegexOptions.IgnoreCase);
            List<string> resC = new List<string>();
            FacturierDatabaseDataSet.ClientsDataTable clients = new FacturierDatabaseDataSet.ClientsDataTable();
            ClientsTableAdapter clientsTableAdapter = new ClientsTableAdapter();
            clientsTableAdapter.Fill(clients);
            foreach (DataRow row in clients.Rows)
            {
                if (rx.IsMatch(row.Field<string>("NomClient")) || rx.IsMatch(row.Field<string>("Tel")))
                { resC.Add(row.Field<string>("NomClient")); }
            }
            comboBox_searchC_res.Items.Clear();
            comboBox_searchC_res.Items.AddRange(resC.ToArray());
            comboBox_searchC_res.SelectedIndex = 0;
            comboBox_searchC_res.Enabled = true;
        }

        private void SearchWatermarkOff(object sender, EventArgs e)
        {
            if (input_rechercheClient.Text == "Nom, téléphone...")
            {
                input_rechercheClient.Text = "";
                input_rechercheClient.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        private void SearchWatermarkOn(object sender, EventArgs e)
        {
            if (input_rechercheClient.Text == "")
            {
                input_rechercheClient.Text = "Nom, téléphone...";
                input_rechercheClient.ForeColor = System.Drawing.SystemColors.GrayText;
            }
        }
    }
}
