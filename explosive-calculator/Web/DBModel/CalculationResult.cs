using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DBModel
{
    public class CalculationResult
    {
        [Key]
        public int Id { get; set; }
        public int CalculationEntryId { get; set; }
        public virtual CalculationEntry CalculationEntry { get; set; }
        public decimal TNTWeightForPressure { get; set; }
        public decimal IncidentPressure { get; set; }
        public decimal ReflectedPressure { get; set; }
        public decimal TimeOfArrival { get; set; }
        public decimal ShockFrontVelocity { get; set; }
        public decimal TNTWeightForImpulse { get; set; }
        public decimal IncidentImpulse { get; set; }
        public decimal ReflectedImpulse { get; set; }
        public decimal PositivePhaseDuration { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
