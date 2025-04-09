
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.Core.Helpers;
using Xunit;

namespace Homes.UnitTest.Core
{
    public class Test_XmlHelpers
    {
        public readonly string xml = "";
        public Test_XmlHelpers()
        {
            xml = @"
                    <root>
                        <app1 key = ""app10"" />
                        <app1 key = ""app11"" />
                        <app2>app20</app2>
                        <app2>app21</app2>
                    </root>
                   ";
        }
        [Theory]
        [InlineData("//root//app1", "key")]
        public void GetValuesOfAtribute(string xpath, string atribute)
        {
            string[] values = XmlHelpers.GetValuesOfAtribute(xml, xpath, atribute);
            if(values.Length > 0)
                Assert.True(true);
            else
                Assert.True(false);
        }
        [Theory]
        [InlineData("//root//app2")]
        public void GetInnerTextsOfTag(string xpath)
        {
            string[] values = XmlHelpers.GetInnerTextsOfTag(xml, xpath);
            if (values.Length > 0)
                Assert.True(true);
            else
                Assert.True(false);
        }
    }
}
