using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourBooker.Logic
{
	public class AppData
	{
		public List<Country> AllCountries { get; private set; }
        public Dictionary<CountryCode, Country> AllCountriesByKey { get; private set; }

        public LinkedList<Country> ItineraryBuilder { get; } = new LinkedList<Country>();

        public SortedDictionary<string, Tour> AllTours { get; private set; } = new SortedDictionary<string, Tour>();

        public Stack<ItineraryChange> ChangeLog { get; } = new Stack<ItineraryChange>();

        public List<Customer> Customers { get; set; } = new List<Customer>() { new Customer("Sean"), new Customer("Rodney") };
        #region BookingRequests
        // ValueTuple (Customer, Tour) represents booking request  -- A struct is clearer
        public Queue<(Customer TheCustomer, Tour TheTour)> BookingRequests { get; } = new Queue<(Customer TheCustomer, Tour theTour)>();

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
            this.AllCountries = reader.ReadAllCountries().OrderBy(n => n.Name).ToList();
            #region Using a Custom Key
            //var dict  = AllCountries.ToDictionary(x => x.Code,                                        // Creates the Country.Code as the dictionary key
            //                                                                                    StringComparer.OrdinalIgnoreCase);      // Intructs key to ignore case when comparing key values 

            var dict = AllCountries.ToDictionary(x => x.Code);
            #endregion


           this.AllCountriesByKey = dict;

            #endregion
            #region Sorted Dictionary
            //var dict = AllCountries.ToDictionary(x => x.Code,                                                                       // Creates the Country.Code as the dictionary key
            //                                                                                    StringComparer.OrdinalIgnoreCase);           // Intructs key to ignore case when comparing key values               

            //this.AllCountriesByKey = new SortedList<string, Country>(dict);                                       // Initalize sorted dict using the unsorted Dictionary 

            #endregion


        }
    }
}
