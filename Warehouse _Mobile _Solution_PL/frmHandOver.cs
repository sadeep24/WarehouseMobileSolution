﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Warehouse_Moblie_Solution_Common;
namespace Warehouse__Mobile__Solution_PL
{
    public partial class frmHandOver : Form
    {
        private BarcodeScanner barcodeScanner = null;
        private EventHandler myReadNotifyHandler = null;
        private EventHandler myStatusNotifyHandler = null;

        // The flag to track whether the reader has been initialized or not.
        private bool isReaderInitiated = false;
        private bool isPOScanned = false;
        public Form RefToMenu { get; set; }
        public string userBarcode { get; set; }

        public frmHandOver()
        {
            InitializeComponent();
        }
        private void locadScanner()
        {
            // Initialize the ScanSampleAPI reference.
            this.barcodeScanner = new BarcodeScanner();

            this.isReaderInitiated = this.barcodeScanner.InitReader();

            if (!(this.isReaderInitiated))// If the reader has not been initialized
            {
                // Display a message & exit the application.
                MessageBox.Show("Fail to initialize barcode reader");
                Application.Exit();
            }
            else // If the reader has been initialized
            {
                // Clear the statusbar where subsequent status information would be displayed.
                //statusBar.Text = "";
                // Attach a status natification handler.
                this.myReadNotifyHandler = new EventHandler(myReader_ReadNotify);
                barcodeScanner.AttachReadNotify(myReadNotifyHandler);
                //this.myStatusNotifyHandler = new EventHandler(myReader_StatusNotify);
                //barcodeScanner.AttachStatusNotify(myStatusNotifyHandler);
                this.barcodeScanner.StartRead(false);
            }
        }
        private void myReader_ReadNotify(object Sender, EventArgs e)
        {
            // Checks if the Invoke method is required because the ReadNotify delegate is called by a different thread
            if (this.InvokeRequired)
            {
                // Executes the ReadNotify delegate on the main thread
                this.Invoke(myReadNotifyHandler, new object[] { Sender, e });
            }
            else
            {
                // Get ReaderData
                Symbol.Barcode.ReaderData TheReaderData = this.barcodeScanner.Reader.GetNextReaderData();
                if (TheReaderData.Result == Symbol.Results.SUCCESS)
                {
                    HandleData(TheReaderData);
                }
            }
        }
        private void myReader_StatusNotify(object Sender, EventArgs e)
        {
            // Checks if the Invoke method is required because the StatusNotify delegate is called by a different thread
            if (this.InvokeRequired)
            {
                // Executes the StatusNotify delegate on the main thread
                this.Invoke(myStatusNotifyHandler, new object[] { Sender, e });
            }
            else
            {
                // Get ReaderData
                Symbol.Barcode.BarcodeStatus TheStatusData = this.barcodeScanner.Reader.GetNextStatus();
            }
        }
        private void HandleData(Symbol.Barcode.ReaderData TheReaderData)
        {
            //write handling logic//
            if (!isPOScanned)
            {

                POScannedLbl.Text = TheReaderData.Text;
                isPOScanned = true;
                this.loadWebs();
                this.barcodeScanner.StartRead(false);
            }
            else
            {
                DeliveredToScannedLbl.Text = TheReaderData.Text;
                this.UnloadScanner();
                this.barcodeScanner.StartRead(false);
            }
        }

        private void frmHandOver_Load(object sender, EventArgs e)
        {
            this.locadScanner();
            BroughtByScannedLbl.Text = userBarcode;
        }
        private void UnloadScanner()
        {
            barcodeScanner.DetachReadNotify();
            barcodeScanner.DetachStatusNotify();
            barcodeScanner.TermReader();
        }
        private void loadWebs()
        {
            ListViewItem l1 = new ListViewItem("22222222");
            l1.SubItems.Add("#123");
            l1.SubItems.Add("123.45");
            listView1.Items.Add(l1);
            ListViewItem l2 = new ListViewItem("33333333");
            l2.SubItems.Add("#124");
            l2.SubItems.Add("124.45");
            listView1.Items.Add(l2);
            ListViewItem l3 = new ListViewItem("4792066523581");
            l3.SubItems.Add("#125");
            l3.SubItems.Add("125.45");
            listView1.Items.Add(l3);
            ListViewItem l4 = new ListViewItem("55555555");
            l4.SubItems.Add("#126");
            l4.SubItems.Add("126.45");
            listView1.Items.Add(l4);
            ListViewItem l5 = new ListViewItem("66666666");
            l5.SubItems.Add("#127");
            l5.SubItems.Add("127.45");
            listView1.Items.Add(l5);

        }
    }
}