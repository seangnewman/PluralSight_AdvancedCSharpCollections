using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

namespace TourBooker.Logic
{
    public class AppData
    {
        public IReadOnlyList<Country> AllCountries { get; private set; }
        public ImmutableDictionary<CountryCode, Country> AllCountriesByKey { get; private set; }

        public LinkedList<Country> ItineraryBuilder { get; } = new LinkedList<Country>();

        public SortedDictionary<string, Tour> AllTours { get; private set; } = new SortedDictionary<string, Tour>();

        public Stack<ItineraryChange> ChangeLog { get; } = new Stack<ItineraryChange>();

        public List<Customer> Customers { get; set; } = new List<Customer>() { new Customer("Sean"), new Customer("Rodney") };
        #region BookingRequests
        public ConcurrentQueue<(Customer TheCustomer, Tour TheTour)> BookingRequests { get; } = new ConcurrentQueue<(Customer TheCustomer, Tour theTour)>();

        #endregion


        public void Initialize(string csvFilePath)
        {
            CsvReader reader = new CsvReader(csvFilePath);
            #region Sorting List.Sort vs LINQ
            var countries = reader.ReadAllCountries().OrderBy(n => n.Name);
            this.AllCountries = countries.ToArray();
            #endregion
            
            this.AllCountriesByKey = AllCountries.ToImmutableDictionary(x => x.Code);
            this.SetupHardCodedTours();

        }

        private void SetupHardCodedTours()
        {
            Country finland = AllCountriesByKey[new CountryCode("FIN")];
            Country greenland = AllCountriesByKey[new CountryCode("GRL")];
            Country iceland = AllCountriesByKey[new CountryCode("ISL")];

            Country newZealand = AllCountriesByKey[new CountryCode("NZL")];
            Country maldives = AllCountriesByKey[new CountryCode("MDV")];
            Country fiji = AllCountriesByKey[new CountryCode("FJI")];

            Country newCaledonia = AllCountriesByKey[new CountryCode("NCL")];

            Tour xmas = new Tour("Snowy Christmas", new Country[] { finland, greenland, iceland });
            AllTours.Add(xmas.Name, xmas);
            Tour islands = new Tour("Exotic Islands", new Country[] { newZealand, maldives, fiji });
            AllTours.Add(islands.Name, islands);
            Tour newTour = new Tour("New Countries", new Country[] { newZealand, newCaledonia, newZealand });
            AllTours.Add(newTour.Name, newTour);

        }
    }
}
