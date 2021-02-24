using Pluralsight.AdvCShColls.TourBooker.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourBooker.Logic;

namespace Pluralsight.AdvCShColls.TourBooker.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private AppData AllData { get; } = new AppData();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			AllData.Initialize(@"PopByLargest.csv");
			// UI Controls will look to AllData for any data
			this.DataContext = AllData;
		}

		private void tbxCountryCode_TextChanged(object sender, TextChangedEventArgs e)
		{
			//string code = tbxCountryCode.Text;
			//this.grdSelectCountry.DataContext = GetCountryWithCode(code);
		}

		private Country GetCountryWithCode(string code)
		{
			if (code.Length != 3)
				return null;
			//return AllData.AllCountries.Find(x => x.Code == code);
			AllData.AllCountriesByKey.TryGetValue(new CountryCode(code), out Country result);
			return result;
		}

        private void btnAddToItinerary_Click(object sender, RoutedEventArgs e)
        {
			int selectedIndex = this.lbxAllCountries.SelectedIndex;

            if (selectedIndex == -1)
            {
				return;
            }

			Country selectedCountry = AllData.AllCountries[selectedIndex];
			AllData.ItineraryBuilder.AddLast(selectedCountry);

			this.UpdateAllLists();
        }

        private void UpdateAllLists()
        {
			this.lbxItinerary.Items.Refresh();
		}

        private void btnRemoveFromItinerary_Click(object sender, RoutedEventArgs e)
        {
			int selectedItinIndex = this.lbxItinerary.SelectedIndex;

            if (selectedItinIndex < 0)
            {
				return;
            }

			var nodeToRemove = AllData.ItineraryBuilder.GetNthNode( selectedItinIndex);
			AllData.ItineraryBuilder.Remove(nodeToRemove);

			this.UpdateAllLists();
        }

        private void btnInsertInItinerary_Click(object sender, RoutedEventArgs e)
        {
			int selectedIndex = this.lbxAllCountries.SelectedIndex;
            if (selectedIndex == -1)
            {
				return;
            }

			int selectedItinIndex = this.lbxItinerary.SelectedIndex;
            if (selectedItinIndex < 0)
            {
				return;
            }

			Country selectedCountry = AllData.AllCountries[selectedIndex];

			var insertBeforeNode = AllData.ItineraryBuilder.GetNthNode(selectedItinIndex);

			AllData.ItineraryBuilder.AddBefore(insertBeforeNode, selectedCountry);

			this.UpdateAllLists();
        }

        private void btnSaveTour_Click(object sender, RoutedEventArgs e)
        {
			string name = this.tbxTourName.Text.Trim();
			Country[] itinerary = AllData.ItineraryBuilder.ToArray();

            try
            {
				Tour newTour = new Tour(name, itinerary);
				AllData.AllTours.Add(newTour.Name, newTour);
            }
            catch (Exception ex)
            {
				MessageBox.Show(ex.Message, "Cannot save tour", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
            }

			AllData.ItineraryBuilder.Clear();
			this.tbxTourName.Text = null;
			this.UpdateAllLists();

			MessageBox.Show("Tour added", "Success");

        }

        private void tbxTourName_TextChanged(object sender, TextChangedEventArgs e)
        {
			

		}

        private void lbxAllCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			int selectedIndex = this.lbxAllCountries.SelectedIndex;

			if (selectedIndex == -1)
			{
				return;
			}

			tbxTourName.Text = $"{AllData.AllCountries[selectedIndex].Code} - {AllData.AllCountries[selectedIndex].Name}"; 

		}
	}
}
