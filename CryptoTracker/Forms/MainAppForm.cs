﻿using System;
using System.Windows.Forms;
using System.Timers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;

using LiveCharts; //Core of the library
using LiveCharts.Wpf; //The WPF controls
using LiveCharts.WinForms; //the WinForm wrappers

//Crypto Images
//https://github.com/cjdowner/cryptocurrency-icons

//Good stock computer icons
//https://www.shareicon.net/motherboard-device-hardware-chip-graphic-card-smps-112175

namespace CryptoTracker
{
    public partial class MainAppForm : MetroFramework.Forms.MetroForm
    {
        //Fields********************************************************************************
        ToolTip toolTip = new ToolTip();
        PriceManager priceManager;
        System.Timers.Timer updatePrices;

        List<string> coinNamesList = new List<string>(); //Stores the names of each coin added

        //UI Lists
        List<Label> priceLabelList = new List<Label>(); //List of labels to iterate through when updating prices
        List<MetroFramework.Controls.MetroTextBox[]> textBoxArrayList = new List<MetroFramework.Controls.MetroTextBox[]>(); //Array of textboxes for each coin, stored in a list

        //MainApp fields
        int flowControlRowCount = 0; //Tracks coins added to row in flow control
        bool updatingUiFlag = false; //Tracks if UI is currently being updated, prevents thread interference
        int coinCount = 0; //Count of coins added to project

        //Constructor***************************************************************************
        public MainAppForm()
        {
            InitializeComponent();

            priceManager = new PriceManager();

            //Create handle created event
            HandleCreated += MainAppForm_HandleCreated;

            //Configure the autoupdate timer
            updatePrices = new System.Timers.Timer();
            updatePrices.Interval = 30000; //30 seconds
            updatePrices.Elapsed += new ElapsedEventHandler(UpdatePrices);
            updatePrices.Start();

            //Configure the tooltip
            toolTip.AutoPopDelay = 15000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            //Initialize the new line labels
            AddNewLine();

            //Parse data in documents folder
            ParseSavedData();     
        }

        //Methods*******************************************************************************
        /// <summary>
        /// Update the UI when the form handle is created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainAppForm_HandleCreated(object sender, EventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// Called by timer every 30 seconds to update prices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdatePrices(object sender, ElapsedEventArgs e)
        {
            //Get data from API
            priceManager.UpdatePriceData(); 

            //Update the UI if it is not currently being updated already
            if (!updatingUiFlag)
            {
                UpdateUI();
            }  
        }

        /// <summary>
        /// Button to manually update data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshButton_Click(object sender, MouseEventArgs e)
        {
            priceManager.UpdatePriceData();
            if (!updatingUiFlag)
            {
                UpdateUI();
            }
        }

        /// <summary>
        /// Updates the UI with values from priceManager updated by API
        /// </summary>
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
                        if (priceManager.toolTipValues.Count == coinCount)
                        {
                            string comma = String.Format("{0:#,###0.#}", Convert.ToDouble(priceManager.toolTipValues[i][1]));
                            toolTip.SetToolTip(priceLabelList[i], "Rank: " + priceManager.toolTipValues[i][0] + "\n" + "Market Cap: $" + comma + "\n" + "% Change 1h: " + priceManager.toolTipValues[i][2] + "%\n" + "% Change 24h: " + priceManager.toolTipValues[i][3] + "%\n" + "% Change 7d: " + priceManager.toolTipValues[i][4] + "%");
                        }
                        
                        totalProfitLabel.Text = "$" + priceManager.totalProfit.ToString("0.00"); // runs on UI thread
                        totalInvestedLabel.Text = "$" + priceManager.totalInvestment.ToString("0.00");
                        totalValueLabel.Text = "$" + priceManager.totalValue.ToString("0.00");
                    });
                }
                else
                {
                    //if priceManager is null, print error
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

            MetroFramework.Controls.MetroLabel quantity = new MetroFramework.Controls.MetroLabel();
            quantity.Margin = new Padding(2, 2, 2, 2);
            quantity.Text = "Quantity:";
            quantity.FontSize = MetroFramework.MetroLabelSize.Small;
            MetroFramework.Controls.MetroLabel invested = new MetroFramework.Controls.MetroLabel();
            invested.Margin = new Padding(2, 2, 2, 2);
            invested.Text = "Total Invested:";
            invested.FontSize = MetroFramework.MetroLabelSize.Small;
            MetroFramework.Controls.MetroLabel value = new MetroFramework.Controls.MetroLabel();
            value.Margin = new Padding(2, 2, 2, 2);
            value.Text = "Value:";
            value.FontSize = MetroFramework.MetroLabelSize.Small;
            MetroFramework.Controls.MetroLabel profit = new MetroFramework.Controls.MetroLabel();
            profit.Margin = new Padding(2, 2, 2, 2);
            profit.Text = "Profit:";
            profit.FontSize = MetroFramework.MetroLabelSize.Small;
            MetroFramework.Controls.MetroLabel profitPercent = new MetroFramework.Controls.MetroLabel();
            profitPercent.Margin = new Padding(2, 2, 2, 2);
            profitPercent.Text = "Profit %:";
            profitPercent.FontSize = MetroFramework.MetroLabelSize.Small;

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
            //Track if we need to add new row labels or not
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
            MetroFramework.Controls.MetroLabel coinName = new MetroFramework.Controls.MetroLabel();
            coinName.Text = addCoin.CoinName;

            MetroFramework.Controls.MetroLabel coinPrice = new MetroFramework.Controls.MetroLabel();
            coinPrice.Name = addCoin.CoinName + "Label";
            coinPrice.Text = "$100,000";

            MetroFramework.Controls.MetroTextBox coinQuantity = new MetroFramework.Controls.MetroTextBox();
            coinQuantity.Name = addCoin.CoinName + "Quantity_TB";
            coinQuantity.Text = addCoin.Quantity.ToString();

            MetroFramework.Controls.MetroTextBox coinInvested = new MetroFramework.Controls.MetroTextBox();
            coinInvested.Name = addCoin.CoinName + "Invested_TB";
            coinInvested.Text = addCoin.NetCost.ToString();

            MetroFramework.Controls.MetroTextBox coinValue = new MetroFramework.Controls.MetroTextBox();
            coinValue.Name = addCoin.CoinName + "Value_TB";

            MetroFramework.Controls.MetroTextBox coinProfit = new MetroFramework.Controls.MetroTextBox();
            coinProfit.Name = addCoin.CoinName + "Profit_TB";

            MetroFramework.Controls.MetroTextBox coinProfitPercent = new MetroFramework.Controls.MetroTextBox();
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
            MetroFramework.Controls.MetroTextBox[] newArray = new MetroFramework.Controls.MetroTextBox[5];
            newArray[0] = coinQuantity;
            newArray[1] = coinInvested;
            newArray[2] = coinValue;
            newArray[3] = coinProfit;
            newArray[4] = coinProfitPercent;
            textBoxArrayList.Add(newArray);

            //Update textbox properties
            foreach (var item in newArray)
            {
                item.Size = new Size(85, 20);
                item.Padding = new Padding(0, 4, 0, 4);
                item.CustomBackground = false;
                item.CustomForeColor = true;
            }
        
            //Update tiles with picture, name, and url link
            try
            {
                //Create metro tile
                MetroFramework.Controls.MetroTile tile = new MetroFramework.Controls.MetroTile();

                //Connect to API to get trade name
                var cli = new System.Net.WebClient();
                string prices = cli.DownloadString(addCoin.APILink);
                dynamic results = JsonConvert.DeserializeObject<dynamic>(prices);

                //Update path to correct image
                string result = results[0].symbol;
                result = result.ToLower();
                string path = @"../../Resources\" + result + "@2x.png";

                //If there is an image for the coin, update tile color and add image to tile
                if (File.Exists(path))
                {
                    //Sample pixel color
                    int x4 = 16;
                    int y = 16;

                    Bitmap b = new Bitmap(path);
                    Color x = b.GetPixel(x4, y);

                    //Add image to tile 
                    tile.TileImage = Image.FromFile(path);
                    tile.UseTileImage = true;
                    tile.TileImageAlign = ContentAlignment.MiddleCenter;
                    tile.BackColor = ControlPaint.Light(x);
                }
                else //If there is no image for the coin, select random color and add default image
                {
                    tile.UseTileImage = true;
                    Random rnd = new Random();
                    Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    tile.BackColor = ControlPaint.Light(randomColor);

                    tile.TileImage = Image.FromFile(@"../../Resources\default_tile.png");
                    tile.UseTileImage = true;
                    tile.TileImageAlign = ContentAlignment.MiddleCenter;
                }
                
                //Update tile name and other parameters         
                tile.Size = new Size(120, 120);
                tile.Visible = true;
                tile.Enabled = true;
                tile.CustomBackground = true;              
                tile.Text = addCoin.CoinName;
                tile.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
                tile.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
                tile.Click += Tile_Click;
                tile.Name = addCoin.APILink;

                infoFlowPanel.Controls.Add(tile);
            }
            catch
            {

            }

            //Add new value array to price manager
            priceManager.AddNewCoin(addCoin);        

            //Update counts
            flowControlRowCount++; //Update row count
            coinCount++; //Update coin count
        }

        /// <summary>
        /// Parse and build url to navigate to coin page on coinmarketcap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Click(object sender, EventArgs e)
        {
            //Need to build link to https://coinmarketcap.com/currencies/coin/ and navigate to website

            //The api url was saved as name, parse the coin name from the url since the coin name in url
            //can differ from the official name
            string name = ((MetroFramework.Controls.MetroTile)sender).Name.ToString().Split('/')[5];

            //Check if the parsed name matches the official name, some protection if the parsing fails
            if (name.Contains(((MetroFramework.Controls.MetroTile)sender).Text.ToLower()))
            {
                try
                {
                    //Open the url in web browser for the user
                    ProcessStartInfo sInfo = new ProcessStartInfo("https://coinmarketcap.com/currencies/" + name + "/");
                    Process.Start(sInfo);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Parses data from text file and adds coin to form
        /// </summary>
        /// <param name="path"></param>
        public void ParseSavedData()
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.MyDoc‌​uments), "CrytoTracker");

            string[] lines;

            if (Directory.Exists(path))
            {
                lines = System.IO.File.ReadAllLines(Path.Combine(path, "CoinData.txt"));

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

                priceManager.UpdatePriceData();
            }
        }

        /// <summary>
        /// Call save function to save data to file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Save data to text file
        /// </summary>
        private void Save()
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.MyDoc‌​uments), "CrytoTracker");

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
                if (!Directory.Exists(Path.Combine(path, "CoinData.txt")))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(Path.Combine(path, "CoinData.txt")))
                {
                    for (int i = 0; i < coinCount; i++)
                    {
                        //Print name, quantity, net cost, and api link to text file
                        file.WriteLine(textFileArray[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Adds new coin to the form. User enters relevant data and UI is updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBuyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCoinForm addCoin = new AddCoinForm(); //Instantiate form

            if (addCoin.ShowDialog() == DialogResult.OK) //Show form
            {
                CoinModel coinModel = addCoin.Coin; //Create new coin model and add data from form

                AddNewCoinToFlowControl(coinModel); //Add coin to form
            }
        }

        /// <summary>
        /// Open the edit coin window
        /// Used to change the quantity and net cost if user makes any trades since last save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditCoinForm editCoin = new EditCoinForm(coinNamesList);

            if (editCoin.ShowDialog() == DialogResult.OK) //Show form
            {
                CoinModel coinModel = editCoin.Coin; //Create new coin model and add data from form

                //Find index of coin to edit
                int index;
                for (index = 0; index < coinCount; index++)
                {
                    if (coinNamesList[index] ==  coinModel.CoinName)
                    {
                        break;
                    }
                }

                //Update lists with new values
                priceManager.valueArrayList[index][0] = coinModel.Quantity;
                priceManager.valueArrayList[index][1] = coinModel.TotalInvested;

                textBoxArrayList[index][0].Text = coinModel.Quantity.ToString();
                textBoxArrayList[index][1].Text ="$" + coinModel.TotalInvested.ToString();

                //Save data if enabled
                if (editCoin.SaveEnabled == true)
                {
                    Save();
                }
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Func<ChartPoint, string> labelPoint = chartPoint =>
    string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            pieChart1.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Maria",
                    Values = new ChartValues<double> {3},
                    PushOut = 15,
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Charles",
                    Values = new ChartValues<double> {4},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frida",
                    Values = new ChartValues<double> {6},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frederic",
                    Values = new ChartValues<double> {2},
                    DataLabels = true,
                    LabelPoint = labelPoint
                }
            };

            pieChart1.LegendLocation = LegendLocation.Bottom;
        }
    }
}
