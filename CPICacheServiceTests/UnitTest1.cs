using CPICacheService.Utilities;
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

            string json = MockApi.Response;
            JObject jsonObject = JObject.Parse(json);

            //JToken series = (JToken)jsonObject["Results"]["series"][0]["data"][0];

            foreach (JToken series in jsonObject["Results"]["series"])
            {

                foreach (JToken data in series["data"])
                {
                    Trace.WriteLine((string)series["seriesID"]);
                }
            }

            Assert.IsNotNull(json);
        }
    }
}

//(JToken)jsonObject["Results"]["series"][0]["data"] is JArray
//true
//(JToken)jsonObject["Results"]["series"] is JArray
//true
