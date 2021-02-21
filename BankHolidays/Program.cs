using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluralSight_AdvancedCSharpCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Arrays, Lists and Collection Equality
            //DateTime[] bankHols1 = {
            //    new DateTime(2021, 1, 1),
            //    new DateTime(2021, 4, 2),
            //    new DateTime(2021, 4, 5),
            //    new DateTime(2021, 5, 3),
            //    new DateTime(2021, 5, 31),
            //    new DateTime(2021, 8, 30),
            //    new DateTime(2021, 12, 27),
            //    new DateTime(2021, 12, 28)
            //};

            //DateTime[] bankHols2 = {
            //    new DateTime(2021, 1, 1),
            //    new DateTime(2021, 4, 2),
            //    new DateTime(2021, 4, 5),
            //    new DateTime(2021, 5, 3),
            //    new DateTime(2021, 5, 31),
            //    new DateTime(2021, 8, 30),
            //    new DateTime(2021, 12, 27),
            //    new DateTime(2021, 12, 28)
            //};

            //DateTime[] bankHols3 = bankHols1;
            //Console.WriteLine($"bn1 == bn2  { bankHols1 == bankHols2}");
            //Console.WriteLine($"bn2 == bn3  { bankHols2 == bankHols3}");
            //Console.WriteLine($"bn3 == bn1  { bankHols3 == bankHols1}");

            //Correct method to test for equality -- Very expensive!
            //Console.WriteLine($"equal values? {bankHols1.SequenceEqual(bankHols2)}");
            #endregion

            #region Lists Under the Hood

            List<DateTime> bankHols1 =  new List<DateTime>{
                new DateTime(2021, 1, 1),
                new DateTime(2021, 4, 2),
                new DateTime(2021, 4, 5),
                new DateTime(2021, 5, 3),
                new DateTime(2021, 5, 31),
                new DateTime(2021, 8, 30),
                new DateTime(2021, 12, 27),
                new DateTime(2021, 12, 28)
            };

            bankHols1.Add(new DateTime(2021, 4, 11));
            #endregion

        }
    }
}
