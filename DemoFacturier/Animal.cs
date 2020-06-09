using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoFacturier
{
    class Animal
    {
        public Animal()
        { 
            IsNew = true;
            IdAnimal = 0;
            Nom = "";
            Espece = "Chien";
            Race = "";
            Naiss = "";
            Friand = false;
            Parfum = "Non";
            Remarques = "";
        }
        public Animal(FacturierDatabaseDataSet.AnimauxRow animal)
        {
            IsNew = false;
            IdAnimal = animal.IdAnimal;
            Nom = animal.Nom;
            Espece = animal.Espece;
            if (animal.IsRaceNull()) { Race = ""; } else { Race = animal.Race; }
            if (animal.IsAnNaissNull()) { Naiss = ""; } else { Naiss = animal.AnNaiss; }
            Friand = animal.Friand;
            Parfum = animal.Parfum;
            if (animal.IsRemarquesNull()) { Remarques = ""; } else { Remarques = animal.Remarques; }
        }

        public bool IsNew { get; set; }
        public int IdAnimal { get; set; }
        public string Nom { get; set; }
        public string Espece { get; set; }
        public string Race { get; set; }
        public string Naiss { get; set; }
        public bool Friand { get; set; }
        public string Parfum { get; set; }
        public string Remarques { get; set; }

    }
}
