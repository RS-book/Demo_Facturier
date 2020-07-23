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
    public class Animal
    {
        public Animal()
        {
            IsNew = true;
            Id = -1;
            Nom = "";
            Espece = "";
            Race = "";
            AnNaiss = "";
            Friand = false;
            Parfum = "";
            Remarques = "";
        }
        public Animal(FacturierDatabaseDataSet.AnimauxRow animal)
        {
            IsNew = false;
            Id = animal.IdAnimal;
            Nom = animal.Nom;
            Espece = animal.Espece;
            if (animal.IsRaceNull()) { Race = ""; } else { Race = animal.Race; }
            if (animal.IsAnNaissNull()) { AnNaiss = ""; } else { AnNaiss = animal.AnNaiss; }
            Friand = animal.Friand;
            Parfum = animal.Parfum;
            if (animal.IsRemarquesNull()) { Remarques = ""; } else { Remarques = animal.Remarques; }
        }

        public bool IsNew { get; set; }
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Espece { get; set; }
        public string Race { get; set; }
        public string AnNaiss { get; set; }
        public bool Friand { get; set; }
        public string Parfum { get; set; }
        public string Remarques { get; set; }


        public void AddAnimalToDB(FacturierDatabaseDataSet.AnimauxDataTable DBAnimaux)
        {
            Id = DBAnimaux.Rows[DBAnimaux.Rows.Count - 1].Field<int>("IdAnimal") + 1;
            FacturierDatabaseDataSet.AnimauxRow InfosAnimal = DBAnimaux.NewAnimauxRow();
            InfosAnimal.BeginEdit();
            InfosAnimal.IdAnimal = Id;
            InfosAnimal.Nom = Nom;
            InfosAnimal.Espece = Espece;
            if (Race == "") { InfosAnimal.SetRaceNull(); } else { InfosAnimal.Race = Race; }
            if (AnNaiss == "") { InfosAnimal.SetAnNaissNull(); } else { InfosAnimal.AnNaiss = AnNaiss; }
            InfosAnimal.Friand = Friand;
            InfosAnimal.Parfum = Parfum;
            if (Remarques == "") { InfosAnimal.SetRemarquesNull(); } else { InfosAnimal.Remarques = Remarques; }
            InfosAnimal.EndEdit();
            DBAnimaux.AddAnimauxRow(InfosAnimal);
        }

        public void EditAnimalInDB(FacturierDatabaseDataSet.AnimauxDataTable DBAnimaux)
        {
            FacturierDatabaseDataSet.AnimauxRow InfosAnimal = DBAnimaux.FindByIdAnimal(Id);
            InfosAnimal.BeginEdit();
            InfosAnimal.Nom = Nom;
            InfosAnimal.Espece = Espece;
            if (Race == "") { InfosAnimal.SetRaceNull(); } else { InfosAnimal.Race = Race; }
            if (AnNaiss == "") { InfosAnimal.SetAnNaissNull(); } else { InfosAnimal.AnNaiss = AnNaiss; }
            InfosAnimal.Friand = Friand;
            InfosAnimal.Parfum = Parfum;
            if (Remarques == "") { InfosAnimal.SetRemarquesNull(); } else { InfosAnimal.Remarques = Remarques; }
            InfosAnimal.EndEdit();
        }

        public void DeleteAFromDB(FacturierDatabaseDataSet.AnimauxDataTable DBAnimaux)
        {
            FacturierDatabaseDataSet.AnimauxRow InfosAnimal = DBAnimaux.FindByIdAnimal(Id);
            InfosAnimal.Delete();
        }
    }
}
