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
        public double TNTWeightForPressure { get; set; }
        public double IncidentPressure { get; set; }
        public double ReflectedPressure { get; set; }
        public double TimeOfArrival { get; set; }
        public double ShockFrontVelocity { get; set; }
        public double TNTWeightForImpulse { get; set; }
        public double IncidentImpulse { get; set; }
        public double ReflectedImpulse { get; set; }
        public double PositivePhaseDuration { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
