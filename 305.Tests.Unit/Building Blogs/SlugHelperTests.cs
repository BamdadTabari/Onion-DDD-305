using _305.BuildingBlocks.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Unit.Building_Blogs
{
    public class SlugHelperTests
    {
        [Theory]
        [InlineData("۱۲۳۴۵۶۷۸۹۰", "1234567890")]
        [InlineData("کد ۱۲۳۴", "کد 1234")]
        [InlineData("۰", "0")]
        [InlineData("abc", "abc")]
        internal void ConvertPersianDigitsToEnglish_ShouldConvertCorrectly(
            string input,
            string expected)
                {
                    var result = SlugHelper.ConvertPersianDigitsToEnglish(input);

                    Assert.Equal(expected, result);
                }
    }
}
