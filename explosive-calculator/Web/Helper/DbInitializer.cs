using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DBModel;
using Web.Entity;

namespace Web.Helper
{
    public static class DbInitializer
    {

        public static void Initialize(ECDBContext context)
        {
            context.Database.EnsureCreated();

            if (context.ExplosiveTypes.Any())
            {
                return;
            }

            var explosiveTypes = new ExplosiveType[]
            {
                new ExplosiveType{ Name="Amatol", GurneyEnergy=1886, ImpulseTNTEquiv=0.98, PeakPressureTNTEquiv=0.99,PressureRange=""},
                new ExplosiveType{ Name="Composition B", GurneyEnergy=2774, ImpulseTNTEquiv=0.98, PeakPressureTNTEquiv=1.11,PressureRange="0.035 - 0.350"},
                new ExplosiveType{ Name="Composition C3", GurneyEnergy=2682, ImpulseTNTEquiv=1.01, PeakPressureTNTEquiv=1.08,PressureRange="0.035 - 0.350"},
                new ExplosiveType{ Name="Composition C4", GurneyEnergy=2530, ImpulseTNTEquiv=0.19, PeakPressureTNTEquiv=1.37,PressureRange="0.070 - 0.700"},
                new ExplosiveType{ Name="HMX", GurneyEnergy=2972, ImpulseTNTEquiv=1.03, PeakPressureTNTEquiv=1.02,PressureRange=""},
                new ExplosiveType{ Name="Octol 75/25", GurneyEnergy=2896, ImpulseTNTEquiv=1.06, PeakPressureTNTEquiv=1.06,PressureRange=""},
                new ExplosiveType{ Name="PETN", GurneyEnergy=2926, ImpulseTNTEquiv=1.11, PeakPressureTNTEquiv=1.27,PressureRange="0.035 - 0.700"},
                new ExplosiveType{ Name="RDX", GurneyEnergy=2926, ImpulseTNTEquiv=1.09, PeakPressureTNTEquiv=1.14,PressureRange=""},
                new ExplosiveType{ Name="RDX/TNT 60/40 (Cyclotol)", GurneyEnergy=2402, ImpulseTNTEquiv=1.09, PeakPressureTNTEquiv=1.14,PressureRange="0.035 - 0.350"},
                new ExplosiveType{ Name="Tetryl", GurneyEnergy=2499, ImpulseTNTEquiv=1.05, PeakPressureTNTEquiv=1.07,PressureRange="0.021 - 0.140"},
                new ExplosiveType{ Name="TNT", GurneyEnergy=2438, ImpulseTNTEquiv=1, PeakPressureTNTEquiv=1,PressureRange="0.021 - 0.140"},
                new ExplosiveType{ Name="Tritonal", GurneyEnergy=2316, ImpulseTNTEquiv=0.96, PeakPressureTNTEquiv=1.07,PressureRange="0.035 - 0.700"},

            };

            context.ExplosiveTypes.AddRange(explosiveTypes);

            context.SaveChanges();
        }
    }
}
