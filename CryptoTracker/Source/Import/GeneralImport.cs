﻿using ExcelDataReader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker
{
    public static class GeneralImport
    {
        public static float GetHistoricalUsdValue(DateTime date, string currency)
        {
            string input = "https://min-api.cryptocompare.com/data/pricehistorical?fsym=" + currency + "&tsyms=USD&ts=" + date.DateTimeToUNIX().ToString();
            float price = 0.0F;

            //Connect to API
            var cli = new System.Net.WebClient();
            string prices = cli.DownloadString(input);
            dynamic results = JsonConvert.DeserializeObject<dynamic>(prices);

            //TODO - Add other trade pair and default cases
            switch (currency)
            {
                case "ETH":
                    price = (float)Convert.ToDouble(results.ETH.USD);
                    break;
            }

            return price;
            //float tradePrice = price * 0.173699F;

            //Console.WriteLine(tradePrice.ToString());
        }

        public static DataSet ExcelToDataSet(string _filePath)
        {
            DataSet result;

            using (var stream = File.Open(_filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    result = reader.AsDataSet();
                }
            }
            return result;
        }

    }
}
