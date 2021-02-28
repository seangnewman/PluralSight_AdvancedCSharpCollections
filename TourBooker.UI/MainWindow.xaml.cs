
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TourBooker.Logic;

namespace Pluralsight.AdvCShColls.TourBooker.UI
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppData AllData { get; } = new AppData();
        private List<Tour> GetRequestedTours() => this.lbxToursToBook.SelectedItems.Cast<Tour>().ToList();


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

            // Log the changes
            var change = new ItineraryChange(ChangeType.Append, AllData.ItineraryBuilder.Count, selectedCountry);
            AllData.ChangeLog.Push(change);

            this.UpdateAllLists();
        }

        private void UpdateAllLists()
        {
            this.lbxItinerary.Items.Refresh();
            this.lbxToursToBook.Items.Refresh();
            this.lbxConfirmedBookedTours.Items.Refresh();
            this.lbxRequests.Items.Refresh();
            // Next statement is a workaround because WPF seems to have problems
            // displaying the contents of a concurrent list.
            // Unsure of the cause - most likely an issue with WPF.
            // Realistically, in normal code you wouldn't normally be hooking a WPF listbox
            // up to a concurrent queue anyway because of issues of concurrency and
            // mixing backend data and UI - it's only done in this demo
            // in order to provide an easy way to see what's in the collections.
            this.lbxRequests.ItemsSource = AllData.BookingRequests.ToList();
            this.lbxRequests.Items.Refresh();
            this.tbxNextBookingRequest.Text = GetLatestBookingRequestText();

        }

        private void btnRemoveFromItinerary_Click(object sender, RoutedEventArgs e)
        {
            int selectedItinIndex = this.lbxItinerary.SelectedIndex;

            if (selectedItinIndex < 0)
            {
                return;
            }

            var nodeToRemove = AllData.ItineraryBuilder.GetNthNode(selectedItinIndex);
            AllData.ItineraryBuilder.Remove(nodeToRemove);
            // log changes
            var change = new ItineraryChange(ChangeType.Remove, selectedItinIndex, nodeToRemove.Value);
            AllData.ChangeLog.Push(change);

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
            // Log changes
            var change = new ItineraryChange(ChangeType.Insert, selectedItinIndex, selectedCountry);
            AllData.ChangeLog.Push(change);

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

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (AllData.ChangeLog.Count == 0)
            {
                // No changes have been logged
                return;
            }

            ItineraryChange lastChange = AllData.ChangeLog.Pop();
            ChangeUndoer.Undo(AllData.ItineraryBuilder, lastChange);
            this.UpdateAllLists();
        }

        private void tbxToursItinerary_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void lbxToursToBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Tour> selectedTours = GetRequestedTours();

            StringBuilder sb = new StringBuilder();


            // loop through every tour
            foreach (var tour in selectedTours)
            {
                sb.AppendLine($"{tour.Name}");
                // loop through every country in tour
                foreach (var country in tour.Itinerary)
                {
                    sb.AppendLine($"     {country.Name}");
                }
                sb.AppendLine();
            }
            this.tbxToursItinerary.Text = sb.ToString();
            this.lbxCountriesInSelection.ItemsSource = GetCountriesInSelection();
        }

   
        private async void btnBookTour_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = this.lbxCustomer.SelectedItem as Customer;

            if (customer == null)
            {
                MessageBox.Show("You must select which customer you are!");
                return;
            }

            List<Tour> requestedTours = GetRequestedTours();
            if (requestedTours.Count == 0)
            {
                MessageBox.Show("You must select a tour to book!", "No Tour Selected");
                return;
            }

            List<Task> tasks = new List<Task>();
            foreach (var tour in requestedTours)
            {

                // select ValueTuple for each cutomer and tour
                Task task = Task.Run( () => this.AllData.BookingRequests.Enqueue((customer, tour)) );
                tasks.Add(task);
                 
            }
            await Task.WhenAll(tasks);

            MessageBox.Show($"{requestedTours.Count} tours requested", "Tours Requested");

            this.UpdateAllLists();
        }

        private void btnApproveRequest_Click(object sender, RoutedEventArgs e)
        {
            //if (AllData.BookingRequests.Count == 0)
            //{
            //    return;
            //}

            //var request = AllData.BookingRequests.Dequeue();
            bool success = AllData.BookingRequests.TryDequeue(out var request);

            if (success)
            {
                request.TheCustomer.BookedTours.Add(request.TheTour);
                this.UpdateAllLists();
            }
            
        }

        private string GetLatestBookingRequestText()
        {
            //if (AllData.BookingRequests.Count == 0)
            //{
            //    return null;
            //}
            //else
            //{
            //    return AllData.BookingRequests.Peek().ToString();
            //}

            bool success = AllData.BookingRequests.TryPeek(out var result);

           return success ? result.ToString() : null;
        }

        private void lbxRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //private SortedSet<Country> GetCountriesInSelection()
        //{
        //    var countries = new SortedSet<Country>(CountryNameComparer.Instance);

        //    List<Tour> selectedTours = GetRequestedTours();

        //    foreach (var tour in selectedTours)
        //    {
        //        foreach (var country in tour.Itinerary)
        //        {
        //            countries.Add(country);
        //        }
        //    }
        //    // Returns duplicate values in tours
        //    //return countries;
        //    // Avoid due to inefficiencies
        //    //return countries.Distinct().ToList();

        //    // return as HashSet removes duplicates
        //    // HashSet ignores duplicate values
        //    return countries;
        //}

        private SortedSet<Country> GetCountriesInSelection()
        {
            List<Tour> selectTours = GetRequestedTours();

            if (selectTours.Count == 0)
            {
                return new SortedSet<Country>(CountryNameComparer.Instance);
            }

            var allSets = new List<SortedSet<Country>>();

            foreach (var tour in selectTours)
            {
                SortedSet<Country> tourCountries = new SortedSet<Country>(tour.Itinerary, CountryNameComparer.Instance);
                allSets.Add(tourCountries);
            }

            SortedSet<Country> result = allSets[0];

            for (int i = 0; i < allSets.Count; i++)
            {
                result.UnionWith(allSets[i]);             // Returns unique countries in all selected sets
               // result.IntersectWith(allSets[i]);    // Only returns countries that appear in all selected sets
            }

            return result;
        }

        private void lbxCountriesInSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lbxCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Customer customer = this.lbxCustomer.SelectedItem as Customer;
            this.gbxBookedTours.DataContext = customer;
        }

        private void lbxConfirmedBookedTours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
