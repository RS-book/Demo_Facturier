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
        public Animal() { IsNew = true; }
        public Animal(FacturierDatabaseDataSet.AnimauxRow animal)
        {
            IsNew = false;
            AnimalRow = animal;
        }

        public bool IsNew { get; set; }
        public FacturierDatabaseDataSet.AnimauxRow AnimalRow { get; set; }
    }
}
