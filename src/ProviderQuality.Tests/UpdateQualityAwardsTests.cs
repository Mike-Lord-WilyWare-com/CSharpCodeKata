using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProviderQuality.Console;

namespace ProviderQuality.Tests
{
    [TestClass]
    public class UpdateQualityAwardsTests
    {
        public const int GovQualityPlusQuality = 20;
        public const int BlueStarQuality = 50;

        [TestMethod]
        public void TestImmutabilityOfBlueDistinctionPlus()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Blue Distinction Plus", SellIn = 0, Quality = 80}
                }
            };

            Assert.IsTrue(app.Awards.Count == 1);
            Assert.IsTrue(app.Awards[0].Name == "Blue Distinction Plus");
            Assert.IsTrue(app.Awards[0].Quality == 80);

            app.UpdateQuality();

            Assert.IsTrue(app.Awards[0].Quality == 80);
        }

        [TestMethod]
        public void TestImmutabilityOfBlueStar()
        {
            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Gov Quality Plus", SellIn = 10, Quality = GovQualityPlusQuality},
                    new Award {Name = "Blue Star", SellIn = 10, Quality = BlueStarQuality }
                }
            };

            Assert.IsTrue(app.Awards.Count == 2);
            Assert.IsTrue(app.Awards[0].Name == "Gov Quality Plus");
            Assert.IsTrue(app.Awards[1].Name == "Blue Star");

            for (int i = 1; i < BlueStarQuality; i++)
            {
                app.UpdateQuality();

                Assert.IsTrue(app.Awards[0].Quality == (ComputeGovQualityPlusQuality(i, app.Awards[0]) < 0 ? 0 : ComputeGovQualityPlusQuality(i, app.Awards[0])));
                Assert.IsTrue(app.Awards[1].Quality == (ComputeBlueStarQuantity(i, app.Awards[1]) < 0 ? 0 : ComputeBlueStarQuantity(i, app.Awards[1])));
            }
        }

        protected int ComputeGovQualityPlusQuality(int idx, Award award)
        {
            return GovQualityPlusQuality - idx - (award.SellIn < 0 ? Math.Abs(award.SellIn) : 0);
        }

        protected int ComputeBlueStarQuantity(int idx, Award award)
        {
            int qty = BlueStarQuality - (idx * 2);
            if (award.SellIn < 0)
            {
                int expireIn = (Math.Abs(award.SellIn) * 2);
                if (qty > expireIn)
                {
                    qty -= expireIn;
                }
                else if ((qty - expireIn) < 0)
                {
                    qty = 0;
                }
            }
            return qty;
        }

        // +++To Do - 1/10/2013: Discuss with team about adding more tests.  Seems like a lot of work for something
        //                       that probably won't change.  I watched it all in the debugger and know everything works
        //                       plus QA has already signed off and no one has complained.
    }
}
