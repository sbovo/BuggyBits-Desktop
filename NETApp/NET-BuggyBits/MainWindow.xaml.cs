using BuggyBits.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace NET_BuggyBits
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> productsTableString = new List<string>();
        private List<Product> productsTable = new List<Product>();

        public MainWindow()
        {
            this.InitializeComponent();
            tbTitle.Text = $"APPLICATION FOR DEBUGGING PURPOSE";

            var processId = Process.GetCurrentProcess().Id;
            textboxProcDumpCommandCrash.Text = $"Procdump.exe {processId.ToString()} -ma -e 1";
            textboxProcDumpCommandHang.Text = $"Procdump.exe {processId.ToString()} -ma";



        }

        private async void btnHugeWork_Click(object sender, RoutedEventArgs e)
        {
            Parallel.For(0, 10, async i =>
            {
                await HugeWork.DoIt(DateTime.Now);
            });
          
        }



        private async void btnRaiseException_Click(object sender, RoutedEventArgs e)
        {
            var buggy = new BuggyClass();
            await buggy.AccessSomeFileAsync();
        }

        private void btnMemoryLeakProducts_Click(object sender, RoutedEventArgs e)
        {
            var dataLayer = new DataLayer();
            var products = dataLayer.GetAllProducts();
            string oneProductTable = "<tr><th>Product Name</th><th>Description</th><th>Price</th></tr>";
            foreach (var product in products)
            {
                productsTable.Add(product);
                oneProductTable += $"<tr><td>{product.ProductName}</td><td>{product.Description}</td><td>{product.Price}</td>";
            }
            productsTableString.Add(oneProductTable);
        }

        private void btnCopyToClipboardCrashCommand_Click(object sender, RoutedEventArgs e)
        {
            CopyTextToClipboard(textboxProcDumpCommandCrash.Text);
        }



        private void btnCopyToClipboardHangCommand_Click(object sender, RoutedEventArgs e)
        {
            CopyTextToClipboard(textboxProcDumpCommandHang.Text);
        }


        private void CopyTextToClipboard(string textToCopy)
        {
           
        }
    }
}


