﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace CryptoTracker
{
    class PriceManager
    {
        private const string ALL_COIN_LIMIT = "0";

        //Enums*********************************************************************************


        //Fields********************************************************************************
        public List<CoinModel> coinModelList = new List<CoinModel>();
        private List<CoinModel.CoinNameStruct> allCoinNames = new List<CoinModel.CoinNameStruct>();

        //Fields to hold total investement data
        public float totalProfit = 0.0F;
        public float totalValue = 0.0F;
        public float totalInvestment = 0.0F;

        public List<CoinModel.CoinNameStruct> AllCoinNames
        {
            get
            {
                return allCoinNames;
            }
        }

        public List<CoinModel> CoinModelList
        {
            get
            {
                return coinModelList;
            }
        }
            


        //Constructor***************************************************************************
        public PriceManager()
        {
            Thread getCoinNames = new Thread(new ThreadStart(GetAllCoinNames));
            getCoinNames.Start();
        }

        //Methods*******************************************************************************
        public void UpdatePriceData()
        {
            APIUpdate();
            UpdateValues();
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetAllCoinNames()
        {
            string input = "https://api.coinmarketcap.com/v1/ticker/?limit=" + ALL_COIN_LIMIT;

            //Connect to API
            var cli = new System.Net.WebClient();
            string prices = cli.DownloadString(input);
            dynamic results = JsonConvert.DeserializeObject<dynamic>(prices);

            int i = 0;
            foreach (var item in results)
            {
                CoinModel.CoinNameStruct newCoin = new CoinModel.CoinNameStruct();
                newCoin.Id = item.id;
                newCoin.Name = item.name;
                newCoin.Symbol = item.symbol;
                allCoinNames.Add(newCoin);

                i++;
            }

            AllCoinNames.Sort((x, y) => x.Name.CompareTo(y.Name));
        }


        /// <summary>
        /// Connect to coinmarketcap API and retrieve data for each coin added to form
        /// </summary>
        private void APIUpdate()
        {
            //Read data from API
            for (int i = 0; i < coinModelList.Count; i++)
            {
                string input = coinModelList[i].APILink;

                try
                {
                    //Connect to API
                    var cli = new System.Net.WebClient();
                    string prices = cli.DownloadString(input);
                    dynamic results = JsonConvert.DeserializeObject<dynamic>(prices);

                    //Add price to temp list
                    coinModelList[i].Price = (float)(Convert.ToDouble(results[0].price_usd));

                    //Update tool tip array and add array to tool tip list
                    coinModelList[i].Rank = results[0].rank;
                    coinModelList[i].MarketCap = results[0].market_cap_usd;
                    coinModelList[i].Percent_Change_1h = results[0].percent_change_1h;
                    coinModelList[i].Percent_Change_24hr = results[0].percent_change_24h;
                    coinModelList[i].Percent_Change_7d = results[0].percent_change_7d;
                    coinModelList[i].Symbol = results[0].symbol;
                }
                catch (System.Net.WebException e)
                {
                    //If there is an error connecting to the API, fill list with null data to avoid index out of bounds later
                    coinModelList[i].Price = 0.0F;
                }
            }
        }

        /// <summary>
        /// Updates values in valueArrayList for each coin. Also calculates total investement data.
        /// </summary>
        private void UpdateValues()
        {
            totalProfit = 0.0F;
            totalValue = 0.0F;
            totalInvestment = 0.0F;

            for (int i = 0; i < coinModelList.Count; i++)
            {              
                if (coinModelList[i].Price != 0.0)
                {
                    coinModelList[i].Value = coinModelList[i].Quantity * coinModelList[i].Price;
                    coinModelList[i].Profit = coinModelList[i].Value - coinModelList[i].NetCost;
                    coinModelList[i].ProfitPercent = (coinModelList[i].Profit / coinModelList[i].Value) * 100;

                    totalInvestment += coinModelList[i].NetCost;
                    totalValue += coinModelList[i].Value.Value;
                    totalProfit += coinModelList[i].Profit.Value;
                }
                else
                {
                    coinModelList[i].Value = null;
                    coinModelList[i].Profit = null;
                    coinModelList[i].ProfitPercent = null;
                }
            }
        }

        public void UpdatePriceDataFromTrades(DataTable table)
        {
            //Need to update quantity and net cost for each coin currently being tracked


        }

        /// <summary>
        /// Add newly added coin to coin model list
        /// </summary>
        /// <param name="addCoin">CoinModel class holding coin related data</param>
        public void AddNewCoin(CoinModel addCoin)
        {
            //Call price manager add new coin before adding new control coin to correctly bind data


            //coinApiUrlList.Add(addCoin.APILink); //Add api url to apiurllist

            //float?[] coinValues = new float?[5]; //Create array to be added to valueArrayList
            //coinValues[(int)PriceManager.rowNames.Quantity] = (float)Convert.ToDouble(addCoin.Quantity);
            //coinValues[(int)PriceManager.rowNames.TotalInvested] = (float)Convert.ToDouble(addCoin.NetCost);
            //valueArrayList.Add(coinValues);

            //coinCount++;
        }
    }
}
