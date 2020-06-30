using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoFacturier
{
    class Animal
    {
        public Animal(TextBox name, RadioButton chien, RadioButton chat, RadioButton autre, TextBox tautre, TextBox race, TextBox annaiss, CheckBox fri, CheckBox parf, TextBox nparf, RichTextBox rems)
        {
            IsNew = true;
            Nom = name;
            RadioChien = chien;
            RadioChat = chat;
            RadioAutre = autre;
            EspeceAutre = tautre;
            Race = race;
            AnNaiss = annaiss;
            Friand = fri;
            Parfum = parf;
            NomParfum = nparf;
            Remarques = rems;
        }
        public Animal(FacturierDatabaseDataSet.AnimauxRow animal, TextBox name, RadioButton chien, RadioButton chat, RadioButton autre, TextBox tautre, TextBox race, TextBox annaiss, CheckBox fri, CheckBox parf, TextBox nparf, RichTextBox rems)
        {
            IsNew = false;
            Nom = name;
            RadioChien = chien;
            RadioChat = chat;
            RadioAutre = autre;
            EspeceAutre = tautre;
            Race = race;
            AnNaiss = annaiss;
            Friand = fri;
            Parfum = parf;
            NomParfum = nparf;
            Remarques = rems;
            InfosAnimal = animal;
        }

        public bool IsNew { get; set; }

        private readonly TextBox Nom;
        private readonly RadioButton RadioChien;
        private readonly RadioButton RadioChat;
        private readonly RadioButton RadioAutre;
        private readonly TextBox EspeceAutre;
        private readonly TextBox Race;
        private readonly TextBox AnNaiss;
        private readonly CheckBox Friand;
        private readonly CheckBox Parfum;
        private readonly TextBox NomParfum;
        private readonly RichTextBox Remarques;
        private FacturierDatabaseDataSet.AnimauxRow InfosAnimal;

        public bool CheckIfValid()
        {
            if(Nom.Text==""||(RadioAutre.Checked&&EspeceAutre.Text==""))
            { return false; }
            else
            { return true; }
        }

        public void FillAnimalChamps()
        {
            Nom.Text = InfosAnimal.Nom;
            switch(InfosAnimal.Espece)
            {
                case "Chien":
                    RadioChien.Checked = true;
                    RadioChat.Checked = false;
                    RadioAutre.Checked = false;
                    EspeceAutre.Text = "";
                    break;
                case "Chat":
                    RadioChat.Checked = true;
                    RadioChien.Checked = false;
                    RadioAutre.Checked = false;
                    EspeceAutre.Text = "";
                    break;
                default:
                    RadioChien.Checked = false;
                    RadioChat.Checked = false;
                    RadioAutre.Checked = true;
                    EspeceAutre.Text = InfosAnimal.Espece;
                    break;
            }
            if (InfosAnimal.IsRaceNull()) { Race.Text = ""; } else { Race.Text = InfosAnimal.Race; }
            if (InfosAnimal.IsAnNaissNull()) { AnNaiss.Text = ""; } else { AnNaiss.Text = InfosAnimal.AnNaiss; }
            Friand.Checked = InfosAnimal.Friand;
            switch(InfosAnimal.Parfum)
            {
                case "non":
                    Parfum.Checked = false;
                    NomParfum.Text = "";
                    break;
                case "oui":
                    Parfum.Checked = true;
                    NomParfum.Text = "";
                    break;
                default:
                    Parfum.Checked = true;
                    NomParfum.Text = InfosAnimal.Parfum;
                    break;
            }
            if (InfosAnimal.IsRemarquesNull()) { Remarques.Text = ""; } else { Remarques.Text = InfosAnimal.Remarques; }
        }

        public void ClearAnimalChamps()
        {
            Nom.Text = "";
            RadioChien.Checked = true;
            RadioChat.Checked = false;
            RadioAutre.Checked = false;
            EspeceAutre.Text = "";
            Race.Text = "";
            AnNaiss.Text = "";
            Friand.Checked = false;
            Parfum.Checked = false;
            NomParfum.Text = "";
            Remarques.Text = "";
        }

        public void AddAnimalToDB(FacturierDatabaseDataSet.AnimauxDataTable DBAnimaux)
        {
            InfosAnimal = DBAnimaux.NewAnimauxRow();
            InfosAnimal.BeginEdit();
            InfosAnimal.IdAnimal = DBAnimaux.Rows[DBAnimaux.Rows.Count - 1].Field<int>("IdAnimal") + 1;
            if(RadioChien.Checked==true)
            {
                InfosAnimal.Espece = "Chien";
            }
            else
            {
                if(RadioChat.Checked==true)
                {
                    InfosAnimal.Espece = "Chat";
                }
                else
                {
                    InfosAnimal.Espece = EspeceAutre.Text;
                }
            }
            if (Race.Text == "") { InfosAnimal.SetRaceNull(); } else { InfosAnimal.Race = Race.Text; }
            if (AnNaiss.Text == "") { InfosAnimal.SetAnNaissNull(); } else { InfosAnimal.AnNaiss = Race.Text; }
            InfosAnimal.Friand = Friand.Checked;
            if(Parfum.Checked)
            {
                if (NomParfum.Text == "")
                { InfosAnimal.Parfum = "oui"; }
                else
                { InfosAnimal.Parfum = NomParfum.Text; }
            }
            else
            { InfosAnimal.Parfum = "non"; }
            if (Remarques.Text == "") { InfosAnimal.SetRemarquesNull(); } else { InfosAnimal.Remarques = Remarques.Text; }
            InfosAnimal.EndEdit();
            DBAnimaux.AddAnimauxRow(InfosAnimal);
        }

        public void EditAnimalInDB()
        {
            InfosAnimal.BeginEdit();
            if (RadioChien.Checked == true)
            {
                InfosAnimal.Espece = "Chien";
            }
            else
            {
                if (RadioChat.Checked == true)
                {
                    InfosAnimal.Espece = "Chat";
                }
                else
                {
                    InfosAnimal.Espece = EspeceAutre.Text;
                }
            }
            if (Race.Text == "") { InfosAnimal.SetRaceNull(); } else { InfosAnimal.Race = Race.Text; }
            if (AnNaiss.Text == "") { InfosAnimal.SetAnNaissNull(); } else { InfosAnimal.AnNaiss = Race.Text; }
            InfosAnimal.Friand = Friand.Checked;
            if (Parfum.Checked)
            {
                if (NomParfum.Text == "")
                { InfosAnimal.Parfum = "oui"; }
                else
                { InfosAnimal.Parfum = NomParfum.Text; }
            }
            else
            { InfosAnimal.Parfum = "non"; }
            if (Remarques.Text == "") { InfosAnimal.SetRemarquesNull(); } else { InfosAnimal.Remarques = Remarques.Text; }
            InfosAnimal.EndEdit();
        }

        public bool WereChangesToAMade()
        {
            bool result = false;
            if ((IsNew && Nom.Text != "") || (!IsNew && Nom.Text != InfosAnimal.Nom)) { result = true; }
            if (IsNew && !RadioChien.Checked) { result = true; }
            if (!IsNew)
            {
                switch(InfosAnimal.Espece)
                {
                    case "Chien":
                        if (!RadioChien.Checked) { result = true; }
                        break;
                    case "Chat":
                        if (!RadioChat.Checked) { result = true; }
                        break;
                    default:
                        if (!RadioAutre.Checked || EspeceAutre.Text != InfosAnimal.Espece) { result = true; }
                        break;
                }
            }
            if (IsNew || InfosAnimal.IsRaceNull()) { if (Race.Text != "") { result = true; } } else { if (Race.Text != InfosAnimal.Race) { result = true; } }
            if (IsNew || InfosAnimal.IsAnNaissNull()) { if (AnNaiss.Text != "") { result = true; } } else { if (AnNaiss.Text != InfosAnimal.AnNaiss) { result = true; } }
            if ((IsNew && Friand.Checked) || (!IsNew && (Friand.Checked != InfosAnimal.Friand))) { result = true; }
            if (IsNew && Parfum.Checked) { result = true; }
            if (!IsNew)
            {
                switch(InfosAnimal.Parfum)
                {
                    case "non":
                        if (Parfum.Checked) { result = true; }
                        break;
                    case "oui":
                        if (!Parfum.Checked || NomParfum.Text != "") { result = true; }
                        break;
                    default:
                        if(!Parfum.Checked||NomParfum.Text!= InfosAnimal.Parfum) { result = true; }
                        break;
                }
            }
            if (IsNew || InfosAnimal.IsRemarquesNull()) { if (Remarques.Text != "") { result = true; } } else { if (Remarques.Text != InfosAnimal.Remarques) { result = true; } }
            return result;
        }

        public void DeleteAFromDB()
        {
            if(!IsNew)
            { InfosAnimal.Delete(); }
        }
    }
}
