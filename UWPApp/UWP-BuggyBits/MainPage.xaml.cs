// Sample Code is provided for the purpose of illustration only and is not intended
// to be used in a production environment. THIS SAMPLE CODE AND ANY RELATED INFORMATION
// ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS
// FOR A PARTICULAR PURPOSE. We grant You a nonexclusive, royalty-free right to use and
// modify the Sample Code and to reproduce and distribute the object code form of the
// Sample Code, provided that. You agree: (i) to not use Our name, logo, or trademarks 
// to market Your software product in which the Sample Code is embedded; (ii) to include
// a valid copyright notice on Your software product in which the Sample Code is embedded;
// and (iii) to indemnify, hold harmless, and defend Us and Our suppliers from and against
// any claims or lawsuits, including attorneys’ fees, that arise or result from the use or
// distribution of the Sample Code
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.AppCenter.Analytics;
using BuggyBits.Models;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using System.Net.Http;
using System.Net;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_BuggyBits
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<string> productsTableString = new List<string>();
        private List<Product> productsTable = new List<Product>();

        public MainPage()
        {
            this.InitializeComponent();

            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            string appVersion = string.Format("{0}.{1}.{2}.{3}",
                version.Major, version.Minor, version.Build, version.Revision);
            tbTitle.Text = $"Version {appVersion} - APPLICATION FOR DEBUGGING PURPOSE";

            var processId = ProcessDiagnosticInfo.GetForCurrentProcess().ProcessId;
            textboxProcDumpCommandCrash.Text = $"Procdump.exe {processId.ToString()} -ma -e 1";
            textboxProcDumpCommandHang.Text = $"Procdump.exe {processId.ToString()} -ma";



        }

        private async void btnHugeWork_Click(object sender, RoutedEventArgs e)
        {
            Parallel.For(0, 10, async i =>
                  {
                      await HugeWork.DoIt(DateTime.Now);
                  });
            var f = new Flyout { Content = new TextBlock { Text = await HugeWork.DoIt(DateTime.Now) } };
            f.ShowAt(btnHugeWork);
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
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(textToCopy);
            Clipboard.SetContent(dataPackage);
        }

        private async void btnHttpRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                textboxLog.Text = $"Building a HttpClient request to '{textboxUriForHttpRequest.Text}'\n";
                string result = await UWPPing(textboxUriForHttpRequest.Text);
                textboxLog.Text += $" - Response = '{result.Substring(0, 200)}'\n";
                textboxLog.Text += $"\r\n\r\n\r\n============================================================\r\nBuilding a HttpClient request to '{textboxUriForHttpRequest.Text}'\n";
                bool b = await Ping(textboxUriForHttpRequest.Text);
            }
            catch (Exception ex)
            {
                textboxLog.Text += $" - Exception = '{ex.Message}'\r\n";
            }
           
        }






        private async Task<string> UWPPing(string url)
        {
            try
            {
                Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

                //Add a user-agent header to the GET request. 
                var headers = httpClient.DefaultRequestHeaders;

                //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
                //especially if the header value is coming from user input.
                string header = "ie";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }

                header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }

                Uri requestUri = new Uri(url);


                textboxLog.Text += $" - User agent = '{headers.UserAgent}'\n";
                textboxLog.Text += $" - Sending the request using the HttpClient.GetAsync  method...\n";
                //Send the GET request asynchronously and retrieve the response as a string.
                Windows.Web.Http.HttpResponseMessage httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                textboxLog.Text += $" - Statut code got = '{httpResponse.StatusCode}'\r\n";
                return await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

                textboxLog.Text += $" - Exception = '{ex.Message}'\r\n";
                return "";
            }
      
        }

        public async Task<bool> Ping(string pingUrl)
        {
            try
            {
                HttpRequestMessage pingReq = new HttpRequestMessage(HttpMethod.Get, pingUrl);
                (HttpStatusCode pingHttpCode, string pingRespContent) = await SendRequest<string>(pingReq);
                if (pingHttpCode != HttpStatusCode.OK)
                    return false;

                return !string.IsNullOrEmpty(pingRespContent);
            }
            catch (Exception ex)
            {

                textboxLog.Text += $" - Exception = '{ex.Message}'\r\n";
                return false;
            }
           
        }

        public async Task<(HttpStatusCode, T)> SendRequest<T>(HttpRequestMessage req) where T : class
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    textboxLog.Text += $" - Sending the request using the HttpClient.SendAsync method...\n";
                    using (HttpResponseMessage response = await httpClient.SendAsync(req))
                    {
                        string stringRes = await response.Content.ReadAsStringAsync();
                        HttpStatusCode respCode = response.StatusCode;
                        T responseData;
                        //try
                        //{
                        //    responseData = JsonConvert.DeserializeObject<T>(stringRes);
                        //}
                        //catch (Exception e)
                        //{
                        //    // if the expected result is a string, just return it!
                        //    if (typeof(T) == typeof(string))
                        //        return (response.StatusCode, stringRes as T);
                        //    s_log.ErrorFormat("Cannot parse response for request {0}: {1} due to {2}", req, stringRes, e.Message);
                        //    throw new LTHttpApiException("Cannot parse response for request " + req + ":" + stringRes, HttpStatusCode.PartialContent);
                        //}

                        if (typeof(T) == typeof(string))
                        {
                            textboxLog.Text += $" - Statut code got = '{respCode}'\r\n";
                            textboxLog.Text += $" - Response = '{stringRes.Substring(0, 200)}'\n";
                            return (response.StatusCode, stringRes as T);
                        }
                                
                    }
                }
                catch (HttpRequestException e)
                {
                    textboxLog.Text += $" - Exception = '{e.Message}'\r\n";
                }
            }

            //if (lastExc != null)
            //{
            //    s_log.ErrorFormat("Cannot send request to {0} due to {1}", req.RequestUri, lastExc.Message);
            //    throw lastExc;
            //}

            //throw new LTHttpApiException("Cannot figure out the response from the server", HttpStatusCode.InternalServerError);
            return (HttpStatusCode.OK, "-" as T);
        }



        //private void btnTakeADump_Click(object sender, RoutedEventArgs e)
        //{

        //    Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        //    var processId = ProcessDiagnosticInfo.GetForCurrentProcess().ProcessId;
        //    string dumpFileName = string.Concat(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), "-",
        //        DateTime.Now.Hour.ToString(), "h", DateTime.Now.Minute.ToString(), "min", DateTime.Now.Second.ToString(), "s", DateTime.Now.Millisecond.ToString(), "ms");
        //    string pathToDumps = localFolder.Path;


        //    Process p = new Process();
        //    ProcessStartInfo startInfo = new ProcessStartInfo();
        //    startInfo.FileName = startInfo.FileName = @".\Tools\procdump.exe";
        //    startInfo.Arguments = $@"-ma {processId} {pathToDumps}\BuggyBitsDump{dumpFileName}.dmp -accepteula";
        //    startInfo.CreateNoWindow = true;
        //    p.StartInfo = startInfo;
        //    p.Start();

        //    textBoxPathToDumps.Text = $"Dump created in {pathToDumps}";
        //}

    }


}
