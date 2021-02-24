using System;

namespace Pluralsight.AdvCShColls.TourBooker.Logic
{
    public class Tour
    {
        public string Name { get; }
        public Country[] Itinerary { get; }

        public override string ToString() => $"{Name} ({Itinerary.Length} countries)";

        public Tour(string name, Country[] itinerary)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Tour must have a non-whitespace name", nameof(name));

            }
            else if (itinerary == null)
            {
                throw new ArgumentException("Itinerary cannot be null", nameof(itinerary));
            }
            else if(itinerary.Length == 0)
            {
                throw new ArgumentException("Tour must have at least one country");
            }

            this.Name = name;
            this.Itinerary = itinerary;
        }
         

    }
}