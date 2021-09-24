using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DBModel
{
    public class ExplosiveType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int GurneyEnergy { get; set; }
        public decimal ImpulseTNTEquiv { get; set; }
        public decimal PeakPressureTNTEquiv { get; set; }
        public string PressureRange { get; set; }

        public virtual ICollection<CalculationEntry> CalculationEntries { get; set; }

    }
}
