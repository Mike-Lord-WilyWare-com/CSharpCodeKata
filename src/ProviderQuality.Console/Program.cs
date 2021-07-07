using System;
using System.Collections.Generic;

namespace ProviderQuality.Console
{
    public class Program
    {
        public IList<Award> Awards
        {
            get;
            set;
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("Updating award metrics...!");

            var app = new Program()
            {
                Awards = new List<Award>
                {
                    new Award {Name = "Gov Quality Plus", SellIn = 10, Quality = 20},
                    new Award {Name = "Blue First", SellIn = 2, Quality = 0},
                    new Award {Name = "ACME Partner Facility", SellIn = 5, Quality = 7},
                    new Award {Name = "Blue Distinction Plus", SellIn = 0, Quality = 80},
                    new Award {Name = "Blue Compare", SellIn = 15, Quality = 20},
                    new Award {Name = "Top Connected Providres", SellIn = 3, Quality = 6},
                    new Award {Name = "Blue Star", SellIn = 10, Quality = 50 }
                }

            };

            app.UpdateQuality();

            System.Console.ReadKey();

        }
        /// <remarks>
        /// Though it is not entirely obvious, SellIn must be the same as ExpiresIn.
        /// The reason is SellIn decrements by one each time this method is invoked.
        /// In other words, once SellIn reaches zero, that is the same as saying the
        /// expiration date has passed. For example: if (Awards[i].SellIn < 0)
        /// 
        /// '# Your Assignment' says nothing about renaming this property or creating
        /// a new ExpiresIn property, so I will leave it alone.  Although in the real-
        /// world, I would at least question whether this should be renamed/refactored.
        /// 
        /// There is a requirement for a "Blue Star" Award 'grant date', but seeing awards
        /// should lose quality value twice as fast as normal awards, it is not really
        /// important what the actual date was. Thus there is no reason to persist it. If
        /// on the other hand calculations where different before and after this date, then
        /// we would have to persist it by adding it to the Award class.  For normal Awards
        /// quality score degrades twice as fast after the expiration date passes, so "Blue
        /// Star" should lose quality value twice as fast as normal awards, which means when
        /// normal awards is doubled, "Blue Star" should be quadrupled.
        /// </remarks>
        public void UpdateQuality()
        {
            for (var i = 0; i < Awards.Count; i++)
            {
                if (Awards[i].Name != "Blue First" && Awards[i].Name != "Blue Compare")
                {
                    //The quality of an award is never negative
                    if (Awards[i].Quality > 0)
                    {
                        if (Awards[i].Name != "Blue Distinction Plus")
                        {
                            //quality score degrades by one
                            Awards[i].Quality = Awards[i].Quality - 1;

                            if (Awards[i].Quality > 0 && Awards[i].Name == "Blue Star")
                            {
                                //"Blue Star" awards should lose quality value twice as fast as normal awards
                                Awards[i].Quality = Awards[i].Quality - 1;
                            }
                        }
                    }
                }
                else
                {
                    if (Awards[i].Quality < 50)
                    {
                        //"Blue First" awards actually increase in quality the older they get
                        Awards[i].Quality = Awards[i].Quality + 1;

                        if (Awards[i].Name == "Blue Compare")
                        {
                            //"Blue Compare", similar to "Blue First", increases in quality as the expiration date approaches;
                            if (Awards[i].SellIn < 11)
                            {
                                if (Awards[i].Quality < 50)
                                {
                                    //Quality increases by 2 when there are 10 days or less left
                                    Awards[i].Quality = Awards[i].Quality + 1;
                                }
                            }

                            if (Awards[i].SellIn < 6)
                            {
                                if (Awards[i].Quality < 50)
                                {
                                    //and by 3 where there are 5 days or less left
                                    Awards[i].Quality = Awards[i].Quality + 1;
                                }
                            }
                        }
                    }
                }

                if (Awards[i].Name != "Blue Distinction Plus")
                {
                    Awards[i].SellIn = Awards[i].SellIn - 1;
                }

                if (Awards[i].SellIn < 0)
                {
                    if (Awards[i].Name != "Blue First")
                    {
                        if (Awards[i].Name != "Blue Compare")
                        {
                            //The quality of an award is never negative
                            if (Awards[i].Quality > 0)
                            {
                                if (Awards[i].Name != "Blue Distinction Plus")
                                {
                                    //Once the expiration date has passed, quality score degrades twice as fast
                                    Awards[i].Quality = Awards[i].Quality - 1;

                                    if (Awards[i].Quality > 0 && Awards[i].Name == "Blue Star")
                                    {
                                        //"Blue Star" awards should lose quality value twice as fast as normal awards
                                        Awards[i].Quality = Awards[i].Quality - 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //quality value drops to 0 after the expiration date
                            Awards[i].Quality = Awards[i].Quality - Awards[i].Quality;
                        }
                    }
                    else
                    {
                        if (Awards[i].Quality < 50)
                        {
                            //"Blue First" awards actually increase in quality the older they get
                            Awards[i].Quality = Awards[i].Quality + 1;
                        }
                    }
                }
            }
        }

    }

}
