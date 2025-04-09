using Network.Core.Helpers;
using System;
using Xunit;

namespace Homes.UnitTest.Core
{
    public class Test_DateTimeHelpers
    {
        [Theory]
        [InlineData("2222", "2", "02")]
        public void Date2Number(string year, string month, string day)
        {
            DateTime date = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            var numDate = DateTimeHelpers.Date2Number(date);
            if(numDate.ToString().Length == 8)            
                Assert.True(true);            
            else            
                Assert.True(false);             
        }
        [Theory]
        [InlineData("2222", "2", "02", "01", "01", "01")]
        public void Datetime2Number(string year, string month, string day, string hour, string minute, string second)
        {
            DateTime dateTime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second));
            var numDate = DateTimeHelpers.Datetime2Number(dateTime);
            if (numDate.ToString().Length == 14)            
                Assert.True(true);            
            else            
                Assert.True(false);            
        }
        [Theory]
        [InlineData("22220101")]
        public void Number2Date(string numDate)
        {            
            DateTime? date = DateTimeHelpers.Number2Date(long.Parse(numDate));
            if(date != null)            
                Assert.True(true);            
            else
                Assert.True(false);
        }
        [Theory]
        [InlineData("19970202020202")]
        public void Number2Datetime(string numDateTime)
        {
            DateTime? datTime = DateTimeHelpers.Number2Datetime(long.Parse(numDateTime));
            if (datTime != null)
                Assert.True(true);
            else
                Assert.True(false);
        }
    }
}
