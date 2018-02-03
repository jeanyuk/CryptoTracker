﻿using ExcelDataReader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTracker
{
    public class GeneralImport
    {
        //Structures*************************************************************************************
        public struct TradePair
        {
            public string trade;
            public string baseTrade;
        };

        //Enumerations***********************************************************************************
        public enum Type
        {
            BUY,
            SELL
        };

        public enum Status
        {
            FILLED,
            CANCELED
        };

        //Fields*****************************************************************************************
        protected DataTable table;

        //Constructor************************************************************************************
        public GeneralImport()
        {
            table = new DataTable();
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Exchange", typeof(string));
            table.Columns.Add("Trade Pair", typeof(string));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Order Quantity", typeof(float));
            table.Columns.Add("Trade Price", typeof(float));
            table.Columns.Add("Order Cost", typeof(float));
            table.Columns.Add("Net Cost (USD)", typeof(float));
        }

        //Methods*****************************************************************************************
        /// <summary>
        /// Calls correct import function based on exchange name, parses data, and returns a datatable
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public DataTable ImportFromExchange(string exchange, string fileName)
        {
            //TODO - TradesTabIntegration - Check that imported file is in correct format before attempting to parse data
            DataTable table = new DataTable();

            if (exchange == "Binance")
            {
                BinanceImport importBinance = new BinanceImport();
                table = importBinance.ImportBinanceTradeData(fileName);
            }
            else if (exchange == "Coinbase")
            {
                CoinbaseImport importCoinbase = new CoinbaseImport();
                table = importCoinbase.ImportCoinbaseTradeData(fileName);
            }

            return table;
        }

        /// <summary>
        /// Gets the historical price of a coin based on timestamp
        /// </summary>
        /// <param name="date"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public float GetHistoricalUsdValue(DateTime date, string currency)
        {
            //TODO - TradesTabIntegration - return null float if trying to access api for non existent coin
            string input = "https://min-api.cryptocompare.com/data/pricehistorical?fsym=" + currency + "&tsyms=USD&ts=" + date.DateTimeToUNIX().ToString();
            float price = 0.0F;

            //Connect to API
            var cli = new System.Net.WebClient();
            string prices = cli.DownloadString(input);
            dynamic results = (JObject)JsonConvert.DeserializeObject<dynamic>(prices);
            price = (float)Convert.ToDouble((JValue)results[currency]["USD"]);

            return price;
        }

        /// <summary>
        /// Converts an excel document worksheet to a DataSet
        /// </summary>
        /// <param name="_filePath"></param>
        /// <returns></returns>
        protected DataSet ExcelToDataSet(string _filePath)
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
