using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pluralsight.AdvCShColls.TourBooker.Logic
{
	public class AppData
	{
		public List<Country> AllCountries { get; private set; }
        public Dictionary<string, Country> AllCountriesByKey { get; private set; }

        // SortedDictionary sorts by keys!
        //public SortedDictionary<string, Country> AllCountriesByKey { get; private set; }

        //public SortedList<string, Country> AllCountriesByKey { get; private set; }


        public void Initialize(string csvFilePath)
		{
			CsvReader reader = new CsvReader(csvFilePath);
            #region Sorting List.Sort vs LINQ
            //this.AllCountries = reader.ReadAllCountries();
            this.AllCountries = reader.ReadAllCountries().OrderBy(n => n.Name).ToList();
            var dict  = AllCountries.ToDictionary(x => x.Code,                                        // Creates the Country.Code as the dictionary key
                                                                                                StringComparer.OrdinalIgnoreCase);      // Intructs key to ignore case when comparing key values               
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
