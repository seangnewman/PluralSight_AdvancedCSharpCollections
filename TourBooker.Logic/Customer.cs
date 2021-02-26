using System.Collections.Generic;

namespace TourBooker.Logic
{
    public class Customer
    {
        public string Name { get; }
        public List<Tour> BookedTours { get; } = new List<Tour>();

        public Customer(string name)
        {
            this.Name = name;
        }

        public override string ToString() => Name;
    }
}