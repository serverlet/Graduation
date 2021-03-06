﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xfy.GraduationPhoto.Manager.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task AmapHelper_Test()
        {
            Xfy.GraduationPhoto.Manager.Code.AmapHelper amapHelper = new Code.AmapHelper();
            IEnumerable<Tuple<double, double>> tuples = new List<Tuple<double, double>>()
            {
                new Tuple<double, double>(116.481488, 39.990464),
                new Tuple<double, double>(120.592114, 31.305903),
            };
            foreach (var item in tuples)
            {
                Code.AmapReturn ama = await amapHelper.Geocode_Regeo(item.Item1, item.Item2);
                Console.WriteLine(ama.Regeocode.FormattedAddress);
                Assert.AreEqual(ama.Status, 1);
                Assert.AreEqual(ama.Info, "OK", false);
            }
        }

        [TestMethod]
        public async Task Db_Test()
        {
            using (Code.DbContext.ImageDbContext context = new Code.DbContext.ImageDbContext())
            {
                context.ImageModels.Where(_ => true);
            }
            Assert.IsTrue(true);
        }
    }
}
