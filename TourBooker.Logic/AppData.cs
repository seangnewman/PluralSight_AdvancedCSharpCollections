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
		public void Initialize(string csvFilePath)
		{
			CsvReader reader = new CsvReader(csvFilePath);
            #region Sorting List.Sort vs LINQ
            //this.AllCountries = reader.ReadAllCountries();
            this.AllCountries = reader.ReadAllCountries().OrderBy(n => n.Name).ToList();
            #endregion


        }
    }
}
