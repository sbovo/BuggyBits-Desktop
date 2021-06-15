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
using System.Threading.Tasks;

namespace UWP_BuggyBits
{
    public static class HugeWork
    {
        private static object syncobj = new object();


        public static async Task<string> DoIt(DateTime startingPoint)
        {
            lock (syncobj)
            {
                // ⚠ This is a demo code to show performance issues
                // ⚠ Never use this code in production as it freezes the UI
                System.Threading.Tasks.Task.Delay(5000).Wait();
                var endingPoint = DateTime.Now;
                return $"It took: {endingPoint.Subtract(startingPoint).TotalSeconds}s";
            }
        }
    }
}
