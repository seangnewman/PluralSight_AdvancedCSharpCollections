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
        //public List<Country> AllCountries { get; private set; }
        // public Dictionary<CountryCode, Country> AllCountriesByKey { get; private set; }
       // public ReadOnlyCollection<Country> AllCountries { get; private set; }

        public ImmutableArray<Country> AllCountries { get; private set; }

        //public ReadOnlyDictionary<CountryCode, Country> AllCountriesByKey { get; private set; }
        public ImmutableDictionary<CountryCode, Country> AllCountriesByKey { get; private set; }

        public LinkedList<Country> ItineraryBuilder { get; } = new LinkedList<Country>();

        public SortedDictionary<string, Tour> AllTours { get; private set; } = new SortedDictionary<string, Tour>();

        public Stack<ItineraryChange> ChangeLog { get; } = new Stack<ItineraryChange>();

        public List<Customer> Customers { get; set; } = new List<Customer>() { new Customer("Sean"), new Customer("Rodney") };
        #region BookingRequests
        // ValueTuple (Customer, Tour) represents booking request  -- A struct is clearer
        public ConcurrentQueue<(Customer TheCustomer, Tour TheTour)> BookingRequests { get; } = new ConcurrentQueue<(Customer TheCustomer, Tour theTour)>();

        //public Queue<BookingRequests> BookingRequests { get; } = new Queue<BookingRequests>();
        #endregion
        // SortedDictionary sorts by keys!
        //public SortedDictionary<string, Country> AllCountriesByKey { get; private set; }

        //public SortedList<string, Country> AllCountriesByKey { get; private set; }


        public void Initialize(string csvFilePath)
        {
            CsvReader reader = new CsvReader(csvFilePath);
            #region Sorting List.Sort vs LINQ
            //this.AllCountries = reader.ReadAllCountries();
            var countries = reader.ReadAllCountries().OrderBy(n => n.Name).ToList();
            this.AllCountries = countries.ToImmutableArray();
            #region Using a Custom Key
            //var dict  = AllCountries.ToDictionary(x => x.Code,                                        // Creates the Country.Code as the dictionary key
            //                                                                                    StringComparer.OrdinalIgnoreCase);      // Intructs key to ignore case when comparing key values 

           // var dict = AllCountries.ToDictionary(x => x.Code);
            #endregion


            //this.AllCountriesByKey = dict;

            #endregion
            #region Sorted Dictionary
            //var dict = AllCountries.ToDictionary(x => x.Code,                                                                       // Creates the Country.Code as the dictionary key
            //                                                                                    StringComparer.OrdinalIgnoreCase);           // Intructs key to ignore case when comparing key values               

            //this.AllCountriesByKey = new SortedList<string, Country>(dict);                                       // Initalize sorted dict using the unsorted Dictionary 

            #endregion
            // Create Demo Data for Display

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
