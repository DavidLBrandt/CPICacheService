using CPICacheService.Utilities;
using CPICacheService.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CPICacheServiceTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            ApiJsonConverter.ConvertFromJson(MockApi.ResponseJson);

            Trace.WriteLine("TBD");
            Assert.IsNotNull(null);
        }
    }
}
