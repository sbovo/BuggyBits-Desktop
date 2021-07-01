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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_BuggyBits
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
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



        private void btnRaiseException_Click(object sender, RoutedEventArgs e)
        {
            Analytics.TrackEvent("Raise exception", Utils.AppCenterDictionarySettings);
            var ex = new System.IO.FileNotFoundException("Oulala we cannot find the file NotExist.jpg");
            throw ex;
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
