﻿using System;
using System.Windows.Forms;
using System.Timers;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;

namespace CryptoTracker
{
    public partial class MainAppForm : Form
    {
        ToolTip toolTip = new ToolTip();
        PriceManager priceManager;
        System.Timers.Timer updatePrices;

        List<string> coinNamesList = new List<string>(); //Stores the names of each coin added

        //UI Lists
        List<Label> priceLabelList = new List<Label>(); //List of labels to iterate through when updating prices
        List<TextBox[]> textBoxArrayList = new List<TextBox[]>(); //Array of textboxes for each coin, stored in a list

        //Program variables
        int flowControlRowCount = 0; //Tracks coins added to row in flow control
        bool updatingUiFlag = false; //Tracks if UI is currently being updated, prevents thread interference
        int coinCount = 0; //Count of coins added to project

        public MainAppForm()
        {
            InitializeComponent();
            this.Text = "Crypto Tracker";

            priceManager = new PriceManager();

            updatePrices = new System.Timers.Timer();
            updatePrices.Interval = 30000; //30 seconds
            updatePrices.Elapsed += new ElapsedEventHandler(UpdatePrices);
            updatePrices.Start();

            // Set up the delays for the ToolTip.
            toolTip.AutoPopDelay = 15000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip.ShowAlways = true;

            AddNewLine();
        }

        public void UpdatePrices(object sender, ElapsedEventArgs e)
        {
            priceManager.UpdatePriceData();
            if (!updatingUiFlag)
            {
                UpdateUI();
            }  
        }

        //Refresh data
        private void button1_Click(object sender, EventArgs e)
        {
            priceManager.UpdatePriceData();
            if (!updatingUiFlag)
            {
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            updatingUiFlag = true;

            for (int i = 0; i < coinCount; i++)
            {
                if (priceManager.coinPriceList[i].HasValue)
                {
                    this.Invoke((MethodInvoker)delegate {
                        priceLabelList[i].Text = "$" + priceManager.coinPriceList[i].Value.ToString("0.00"); // runs on UI thread

                        //Update textboxes
                        for (int j = 0; j < 5; j++)
                        {
                            if (j == 0)
                            {
                                textBoxArrayList[i][j].Text = priceManager.valueArrayList[i][j].Value.ToString("0.000000"); // runs on UI thread
                            }
                            else if (j >= 1 && j <= 3)
                            {
                                textBoxArrayList[i][j].Text = "$" + priceManager.valueArrayList[i][j].Value.ToString("0.00"); // runs on UI thread
                            }
                            else if (j == 4)
                            {
                                textBoxArrayList[i][j].Text = priceManager.valueArrayList[i][j].Value.ToString("0.00") + "%"; // runs on UI thread
                            }

                            if (j == 3)
                            {
                                if (priceManager.valueArrayList[i][j] <= 0)
                                {
                                    textBoxArrayList[i][j].ForeColor = Color.Red; // runs on UI thread
                                }
                                else
                                {
                                    textBoxArrayList[i][j].ForeColor = Color.Green; // runs on UI thread
                                }
                            }
                        }

                        //Update tooltip
                        string comma = String.Format("{0:#,###0.#}", Convert.ToDouble(priceManager.toolTipValues[i][1]));
                        toolTip.SetToolTip(priceLabelList[i], "Rank: " + priceManager.toolTipValues[i][0] + "\n" + "Market Cap: $" + comma + "\n" + "% Change 1h: " + priceManager.toolTipValues[i][2] + "%\n" + "% Change 24h: " + priceManager.toolTipValues[i][3] + "%\n" + "% Change 7d: " + priceManager.toolTipValues[i][4] + "%");

                        totalProfitLabel.Text = "$" + priceManager.totalProfit.ToString("0.00"); // runs on UI thread
                        totalInvestedLabel.Text = "$" + priceManager.totalInvestment.ToString("0.00");
                        totalValueLabel.Text = "$" + priceManager.totalValue.ToString("0.00");
                    });
                }
                else
                {
                    this.Invoke((MethodInvoker)delegate {
                        foreach (var item in textBoxArrayList[i])
                        {
                            item.Text = "Error";
                        }
                    });
                }
            }

            updatingUiFlag = false;
        }

        //Open saved file
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ParseSavedData(openFileDialog1.FileName);
            }

            priceManager.UpdatePriceData();
            UpdateUI();
        }

        private void addBuyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO - Check if data is null before adding

            AddCoinForm addCoin = new AddCoinForm(); //Instantiate form

            if (addCoin.ShowDialog() == DialogResult.OK) //Show form
            {
                CoinModel coinModel = addCoin.Coin; //Create new coin model and add data from form

                AddNewCoinToFlowControl(coinModel); //Add coin to form
            }
        }

        public void ParseSavedData(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            foreach (string line in lines)
            {
                CoinModel newCoin = new CoinModel();

                string[] data = line.Split(',');
                newCoin.CoinName = data[0];
                newCoin.Quantity = (float)(Convert.ToDouble(data[1]));
                newCoin.NetCost = (float)(Convert.ToDouble(data[2]));
                newCoin.APILink = data[3];

                AddNewCoinToFlowControl(newCoin);
            }
        }

        /// <summary>
        /// Creates labels for a new row in flow panel
        /// </summary>
        private void AddNewLine()
        {
            FlowLayoutPanel newFlowPanel = new FlowLayoutPanel();
            newFlowPanel.WrapContents = true;
            newFlowPanel.FlowDirection = FlowDirection.BottomUp;
            newFlowPanel.Height = 185;
            newFlowPanel.Width = 90;

            Label quantity = new Label();
            quantity.Margin = new Padding(2, 2, 2, 2);
            quantity.Text = "Quantity:";
            Label invested = new Label();
            invested.Margin = new Padding(2, 2, 2, 2);
            invested.Text = "Total Invested:";
            Label value = new Label();
            value.Margin = new Padding(2, 2, 2, 2);
            value.Text = "Value:";
            Label profit = new Label();
            profit.Margin = new Padding(2, 2, 2, 2);
            profit.Text = "Profit:";
            Label profitPercent = new Label();
            profitPercent.Margin = new Padding(2, 2, 2, 2);
            profitPercent.Text = "Profit %:";

            newFlowPanel.Controls.Add(profitPercent);
            newFlowPanel.Controls.Add(profit);
            newFlowPanel.Controls.Add(value);
            newFlowPanel.Controls.Add(invested);
            newFlowPanel.Controls.Add(quantity);

            flowLayoutPanel1.Controls.Add(newFlowPanel);
        }

        /// <summary>
        /// Adds new coin to crypto tracker
        /// </summary>
        /// <param name="addCoin"></param>
        public void AddNewCoinToFlowControl(CoinModel addCoin)
        {
            if (flowControlRowCount == 7)
            {
                AddNewLine();
                flowControlRowCount = 0;
            }

            //Create flow panel to add coin specific controls
            FlowLayoutPanel newFlowPanel = new FlowLayoutPanel();
            newFlowPanel.WrapContents = true;
            newFlowPanel.FlowDirection = FlowDirection.TopDown;
            newFlowPanel.Height = 185;
            newFlowPanel.Width = 90;

            //Create controls
            Label coinName = new Label();
            coinName.Text = addCoin.CoinName;
            
            Label coinPrice = new Label();
            coinPrice.Name = addCoin.CoinName + "Label";
            coinPrice.Text = "$100,000";

            TextBox coinQuantity = new TextBox();
            coinQuantity.Name = addCoin.CoinName + "Quantity_TB";
            coinQuantity.Text = addCoin.Quantity.ToString();

            TextBox coinInvested = new TextBox();
            coinInvested.Name = addCoin.CoinName + "Invested_TB";
            coinInvested.Text = addCoin.NetCost.ToString();

            TextBox coinValue = new TextBox();
            coinValue.Name = addCoin.CoinName + "Value_TB";

            TextBox coinProfit = new TextBox();
            coinProfit.Name = addCoin.CoinName + "Profit_TB";

            TextBox coinProfitPercent = new TextBox();
            coinProfitPercent.Name = addCoin.CoinName + "ProfitPercent_TB";

            //Add controls to coin specifc flow panel
            newFlowPanel.Controls.Add(coinName);
            newFlowPanel.Controls.Add(coinPrice);
            newFlowPanel.Controls.Add(coinQuantity);
            newFlowPanel.Controls.Add(coinInvested);
            newFlowPanel.Controls.Add(coinValue);
            newFlowPanel.Controls.Add(coinProfit);
            newFlowPanel.Controls.Add(coinProfitPercent);

            //Add new flow panel to base flow panel
            flowLayoutPanel1.Controls.Add(newFlowPanel);

            //Update Lists
            priceLabelList.Add(coinPrice);
            coinNamesList.Add(addCoin.CoinName);

            //Create new textbox array and add to textbox array list
            TextBox[] newArray = new TextBox[5];
            newArray[0] = coinQuantity;
            newArray[1] = coinInvested;
            newArray[2] = coinValue;
            newArray[3] = coinProfit;
            newArray[4] = coinProfitPercent;
            textBoxArrayList.Add(newArray);

            //Add new value array to price manager
            priceManager.AddNewCoin(addCoin);        

            //Update counts
            flowControlRowCount++; //Update row count
            coinCount++; //Update coin count
        }

        /// <summary>
        /// Write coin data to text file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO - Option for user to select save location
            //TODO - Fix so if no coins are added, cannot save
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            string[] textFileArray = new string[coinCount];
            bool readError = false;

            //Loop through each coin and put data in string array, if there is an error, do not write
            for (int i = 0; i < coinCount; i++)
            {
                try
                {
                    textFileArray[i] = coinNamesList[i] + ", " + textBoxArrayList[i][0].Text + ", " + textBoxArrayList[i][1].Text.TrimStart('$') + ", " + priceManager.coinApiUrlList[i];
                }
                catch
                {
                    MessageBox.Show("Error reading data");
                    readError = true;
                }
            }

            if (!readError)
            {
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"C:\Users\Willem\Desktop\CoinPrices.txt"))
                {
                    for (int i = 0; i < coinCount; i++)
                    {
                        //Print name, quantity, net cost, and api link to text file
                        file.WriteLine(textFileArray[i]);
                    }
                }
            }
        }

        private void addSellToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
