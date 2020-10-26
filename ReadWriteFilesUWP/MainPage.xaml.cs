using Newtonsoft.Json;
using ReadWriteFilesUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ReadWriteFilesUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StorageFolder storageFolder;
        private ObservableCollection<string> CsvRows = new ObservableCollection<string>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnCreatejsonfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnCreatecsvfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnCreatetxtfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnCreatexmlfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnLoadjsonfile_Click(object sender, RoutedEventArgs e)
        {
            var json = new FileOpenPicker();
            json.ViewMode = PickerViewMode.List;
            json.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            json.FileTypeFilter.Add(".json");


            StorageFile file = await json.PickSingleFileAsync();
            string text = await FileIO.ReadTextAsync(file);
            List<Person> person = JsonConvert.DeserializeObject<List<Person>>(text);
            lvReaddata.ItemsSource = person;
        }

        private async void btnLoadcsvfile_Click(object sender, RoutedEventArgs e)
        {
            var csv = new FileOpenPicker();
            csv.ViewMode = PickerViewMode.List;
            csv.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            csv.FileTypeFilter.Add(".csv");

            StorageFile file = await csv.PickSingleFileAsync();

            CsvRows.Clear();

            using (CsvParse.CsvFileReader csvReader = new CsvParse.CsvFileReader(await file.OpenStreamForReadAsync()))
            {
                CsvParse.CsvRow row = new CsvParse.CsvRow();
                while (csvReader.ReadRow(row))
                {
                    string newRow = "";
                    for (int i = 0; i < row.Count; i++)
                    {
                        newRow += row[i] + ",";
                    }
                    CsvRows.Add(newRow);
                }
                lvReadcsv.ItemsSource = CsvRows;
            }
        }

        private async void btnLoadtxtfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnLoadxmlfile_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker Xml = new FileOpenPicker();
            Xml.ViewMode = PickerViewMode.Thumbnail;
            Xml.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            Xml.FileTypeFilter.Add(".xml");
            StorageFile file = await Xml.PickSingleFileAsync();

            using (var stream = await file.OpenStreamForReadAsync())
            {
                XDocument xmldata = XDocument.Load(stream);
                var data = from query in xmldata.Descendants("person")
                           select new Person
                           {
                               FirstName = (string)query.Element("FirstName"),
                               LastName = (string)query.Element("LastName"),
                               Age = (int)query.Element("Age"),
                               City = (string)query.Element("City"),
                           };

                lvReadXml.ItemsSource = data;
            }
        }
    }
}
