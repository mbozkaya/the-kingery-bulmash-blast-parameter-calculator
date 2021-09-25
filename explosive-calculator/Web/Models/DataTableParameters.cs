using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class DataTableParameters
    {
        public List<DataTableColumn> Columns { get; set; }
        public int Draw { get; set; }
        public int Length { get; set; }
        public List<DataOrder> Order { get; set; }
        public Search Search { get; set; }
        public int Start { get; set; }
    }
    public class Search
    {
        public bool Regex { get; set; }
        public string Value { get; set; }
    }

    public class DataTableColumn
    {
        public int Data { get; set; }
        public string Name { get; set; }
        public bool Orderable { get; set; }
        public bool Searchable { get; set; }
        public Search Search { get; set; }

    }

    public class DataOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }

    }

    public class DataTableDataModel
    {
        public string ExplosiveType { get; set; }
        public int Id { get; set; }
        public double Weight
        {
            get
            {
                return Math.Round(_Weight, 2);
            }
            set
            {
                _Weight = value;
            }
        }
        private double _Weight { get; set; }
        public double Range
        {
            get
            {
                return Math.Round(_Range, 2);
            }
            set
            {
                _Range = value;
            }
        }
        private double _Range { get; set; }
        public double TNTWeightForPressure
        {
            get
            {
                return Math.Round(_TNTWeightForPressure, 2);
            }
            set
            {
                _TNTWeightForPressure = value;
            }
        }
        private double _TNTWeightForPressure { get; set; }
        public double IncidentPressure
        {
            get
            {
                return Math.Round(_IncidentPressure, 2);
            }
            set
            {
                _IncidentPressure = value;
            }
        }
        private double _IncidentPressure { get; set; }
        public double ReflectedPressure
        {
            get
            {
                return Math.Round(_ReflectedPressure, 2);
            }
            set
            {
                _ReflectedPressure = value;
            }
        }
        private double _ReflectedPressure { get; set; }
        public double TimeOfArrival
        {
            get
            {
                return Math.Round(_TimeOfArrival, 2);
            }
            set
            {
                _TimeOfArrival = value;
            }
        }
        private double _TimeOfArrival { get; set; }
        public double ShockFrontVelocity
        {
            get
            {
                return Math.Round(_ShockFrontVelocity, 2);
            }
            set
            {
                _ShockFrontVelocity = value;
            }
        }
        private double _ShockFrontVelocity { get; set; }
        public double TNTWeightForImpulse
        {
            get
            {
                return Math.Round(_TNTWeightForImpulse, 2);
            }
            set
            {
                _TNTWeightForImpulse = value;
            }
        }
        private double _TNTWeightForImpulse { get; set; }
        public double IncidentImpulse
        {
            get
            {
                return Math.Round(_IncidentImpulse, 2);
            }
            set
            {
                _IncidentImpulse = value;
            }
        }
        private double _IncidentImpulse { get; set; }
        public double ReflectedImpulse
        {
            get
            {
                return Math.Round(_ReflectedImpulse, 2);
            }
            set
            {
                _ReflectedImpulse = value;
            }
        }
        private double _ReflectedImpulse { get; set; }
        public double PositivePhaseDuration
        {
            get
            {
                return Math.Round(_PositivePhaseDuration, 2);
            }
            set
            {
                _PositivePhaseDuration = value;
            }
        }
        private double _PositivePhaseDuration { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class DataTableReturnModel
    {
        public List<DataTableDataModel> Data { get; set; } = new List<DataTableDataModel>();

        public int Draw { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
    }
}
