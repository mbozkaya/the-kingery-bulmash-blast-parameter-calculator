using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DBModel
{
    public class CalculationEntry
    {
        [Key]
        public int Id { get; set; }
        public int ExplosiveTypeId { get; set; }
        public virtual ExplosiveType ExplosiveType { get; set; }
        public decimal Weight { get; set; }
        public decimal Range { get; set; }

        public virtual ICollection<CalculationResult> CalculationResults { get; set; }
    }
}
