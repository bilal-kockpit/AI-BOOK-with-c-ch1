using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ConsoleApp1.Plugins
{
    public class ShowManager
    {
        [KernelFunction, Description("Get a random theme for a knock-knock joke")]
        public string RandomTheme()
        {
            var list = new List<string> { "boo", "dishes", "art", "needle", "tank", "police" };
            return list[new Random().Next(0, list.Count)];
        }
        
        [KernelFunction, Description("Get another random theme")]
        public string RandomTheme2()
        {
            var list = new List<string> { "apple", "banana", "boss", "shoes", "like", "unlike" };
            return list[new Random().Next(0, list.Count)];
        }
    }
}
