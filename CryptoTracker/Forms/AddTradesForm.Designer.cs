﻿namespace CryptoTracker
{
    partial class AddTradeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addTradeCalender = new System.Windows.Forms.MonthCalendar();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.addTradeDate_TB = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.exchangeLabel = new MetroFramework.Controls.MetroLabel();
            this.exchange_CB = new MetroFramework.Controls.MetroComboBox();
            this.tradePairLabel = new MetroFramework.Controls.MetroLabel();
            this.typeLabel = new MetroFramework.Controls.MetroLabel();
            this.type_CB = new MetroFramework.Controls.MetroComboBox();
            this.priceLabel = new MetroFramework.Controls.MetroLabel();
            this.importQuantity_TB = new MetroFramework.Controls.MetroTextBox();
            this.quantityLabel = new MetroFramework.Controls.MetroLabel();
            this.importPrice_TB = new MetroFramework.Controls.MetroTextBox();
            this.addTradeOkayButton = new MetroFramework.Controls.MetroButton();
            this.addTradeCancelButton = new MetroFramework.Controls.MetroButton();
            this.tradeBase_CB = new MetroFramework.Controls.MetroComboBox();
            this.SuspendLayout();
            // 
            // addTradeCalender
            // 
            this.addTradeCalender.Location = new System.Drawing.Point(29, 69);
            this.addTradeCalender.MaxSelectionCount = 1;
            this.addTradeCalender.Name = "addTradeCalender";
            this.addTradeCalender.TabIndex = 1;
            this.addTradeCalender.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.addTradeCalender_DateSelected);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(268, 69);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(39, 19);
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "Date:";
            // 
            // addTradeDate_TB
            // 
            this.addTradeDate_TB.Enabled = false;
            this.addTradeDate_TB.Location = new System.Drawing.Point(341, 69);
            this.addTradeDate_TB.Name = "addTradeDate_TB";
            this.addTradeDate_TB.Size = new System.Drawing.Size(79, 23);
            this.addTradeDate_TB.TabIndex = 3;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(268, 114);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(41, 19);
            this.metroLabel2.TabIndex = 4;
            this.metroLabel2.Text = "Time:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(341, 114);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(79, 20);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // exchangeLabel
            // 
            this.exchangeLabel.AutoSize = true;
            this.exchangeLabel.Location = new System.Drawing.Point(268, 155);
            this.exchangeLabel.Name = "exchangeLabel";
            this.exchangeLabel.Size = new System.Drawing.Size(67, 19);
            this.exchangeLabel.TabIndex = 6;
            this.exchangeLabel.Text = "Exchange:";
            // 
            // exchange_CB
            // 
            this.exchange_CB.FontSize = MetroFramework.MetroLinkSize.Small;
            this.exchange_CB.FormattingEnabled = true;
            this.exchange_CB.ItemHeight = 19;
            this.exchange_CB.Location = new System.Drawing.Point(341, 155);
            this.exchange_CB.Name = "exchange_CB";
            this.exchange_CB.Size = new System.Drawing.Size(79, 25);
            this.exchange_CB.TabIndex = 7;
            // 
            // tradePairLabel
            // 
            this.tradePairLabel.AutoSize = true;
            this.tradePairLabel.Location = new System.Drawing.Point(268, 201);
            this.tradePairLabel.Name = "tradePairLabel";
            this.tradePairLabel.Size = new System.Drawing.Size(73, 19);
            this.tradePairLabel.TabIndex = 8;
            this.tradePairLabel.Text = "Trade Pair:";
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(457, 73);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(40, 19);
            this.typeLabel.TabIndex = 10;
            this.typeLabel.Text = "Type:";
            // 
            // type_CB
            // 
            this.type_CB.FontSize = MetroFramework.MetroLinkSize.Small;
            this.type_CB.FormattingEnabled = true;
            this.type_CB.ItemHeight = 19;
            this.type_CB.Location = new System.Drawing.Point(530, 71);
            this.type_CB.Name = "type_CB";
            this.type_CB.Size = new System.Drawing.Size(79, 25);
            this.type_CB.TabIndex = 11;
            // 
            // priceLabel
            // 
            this.priceLabel.AutoSize = true;
            this.priceLabel.Location = new System.Drawing.Point(456, 159);
            this.priceLabel.Name = "priceLabel";
            this.priceLabel.Size = new System.Drawing.Size(41, 19);
            this.priceLabel.TabIndex = 14;
            this.priceLabel.Text = "Price:";
            // 
            // importQuantity_TB
            // 
            this.importQuantity_TB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.importQuantity_TB.Location = new System.Drawing.Point(529, 114);
            this.importQuantity_TB.Name = "importQuantity_TB";
            this.importQuantity_TB.Size = new System.Drawing.Size(79, 23);
            this.importQuantity_TB.TabIndex = 13;
            // 
            // quantityLabel
            // 
            this.quantityLabel.AutoSize = true;
            this.quantityLabel.Location = new System.Drawing.Point(456, 114);
            this.quantityLabel.Name = "quantityLabel";
            this.quantityLabel.Size = new System.Drawing.Size(61, 19);
            this.quantityLabel.TabIndex = 12;
            this.quantityLabel.Text = "Quantity:";
            // 
            // importPrice_TB
            // 
            this.importPrice_TB.Location = new System.Drawing.Point(529, 157);
            this.importPrice_TB.Name = "importPrice_TB";
            this.importPrice_TB.Size = new System.Drawing.Size(79, 23);
            this.importPrice_TB.TabIndex = 18;
            // 
            // addTradeOkayButton
            // 
            this.addTradeOkayButton.Location = new System.Drawing.Point(539, 256);
            this.addTradeOkayButton.Name = "addTradeOkayButton";
            this.addTradeOkayButton.Size = new System.Drawing.Size(75, 23);
            this.addTradeOkayButton.TabIndex = 21;
            this.addTradeOkayButton.Text = "Okay";
            this.addTradeOkayButton.Click += new System.EventHandler(this.addTradeOkayButton_Click);
            // 
            // addTradeCancelButton
            // 
            this.addTradeCancelButton.Location = new System.Drawing.Point(452, 256);
            this.addTradeCancelButton.Name = "addTradeCancelButton";
            this.addTradeCancelButton.Size = new System.Drawing.Size(75, 23);
            this.addTradeCancelButton.TabIndex = 22;
            this.addTradeCancelButton.Text = "Cancel";
            this.addTradeCancelButton.Click += new System.EventHandler(this.addTradeCancelButton_Click);
            // 
            // tradeBase_CB
            // 
            this.tradeBase_CB.FontSize = MetroFramework.MetroLinkSize.Small;
            this.tradeBase_CB.FormattingEnabled = true;
            this.tradeBase_CB.ItemHeight = 19;
            this.tradeBase_CB.Location = new System.Drawing.Point(341, 199);
            this.tradeBase_CB.Name = "tradeBase_CB";
            this.tradeBase_CB.Size = new System.Drawing.Size(79, 25);
            this.tradeBase_CB.TabIndex = 23;
            // 
            // AddTradeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Drawing.MetroBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(637, 290);
            this.Controls.Add(this.tradeBase_CB);
            this.Controls.Add(this.addTradeCancelButton);
            this.Controls.Add(this.addTradeOkayButton);
            this.Controls.Add(this.importPrice_TB);
            this.Controls.Add(this.priceLabel);
            this.Controls.Add(this.importQuantity_TB);
            this.Controls.Add(this.quantityLabel);
            this.Controls.Add(this.type_CB);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.tradePairLabel);
            this.Controls.Add(this.exchange_CB);
            this.Controls.Add(this.exchangeLabel);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.addTradeDate_TB);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.addTradeCalender);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "AddTradeForm";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Red;
            this.Text = "Add Trade";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar addTradeCalender;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTextBox addTradeDate_TB;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private MetroFramework.Controls.MetroLabel exchangeLabel;
        private MetroFramework.Controls.MetroComboBox exchange_CB;
        private MetroFramework.Controls.MetroLabel tradePairLabel;
        private MetroFramework.Controls.MetroLabel typeLabel;
        private MetroFramework.Controls.MetroComboBox type_CB;
        private MetroFramework.Controls.MetroLabel priceLabel;
        private MetroFramework.Controls.MetroTextBox importQuantity_TB;
        private MetroFramework.Controls.MetroLabel quantityLabel;
        private MetroFramework.Controls.MetroTextBox importPrice_TB;
        private MetroFramework.Controls.MetroButton addTradeOkayButton;
        private MetroFramework.Controls.MetroButton addTradeCancelButton;
        private MetroFramework.Controls.MetroComboBox tradeBase_CB;
    }
}