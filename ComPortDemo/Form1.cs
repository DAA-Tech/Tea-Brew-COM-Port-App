using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;



namespace ComPortDemo
{
    #region Public Enumerations
    public enum DataMode { Text, Hex }
    public enum LogMsgType { Incoming, Outgoing, Normal, Warning, Error };
    #endregion

    public partial class Form1 : Form
    {
        // Various colors for logging info
        private Color[] LogMsgTypeColor = { Color.Blue, Color.Green, Color.Black, Color.Orange, Color.Red };


        #region Local Properties
        private DataMode CurrentDataMode
        {
            get
            {
                if (rbHex.Checked) return DataMode.Hex;
                else return DataMode.Text;
            }
            set
            {
                if (value == DataMode.Text) rbText.Checked = true;
                else rbHex.Checked = true;
            }
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>
        /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
        /// <returns> Returns a well formatted string of hex digits with spacing. </returns>
        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }

        /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
        /// <param name="s"> The string containing the hex digits (with or without spaces). </param>
        /// <returns> Returns an array of bytes. </returns>
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];

            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }

            return buffer;
        }

        // List com ports avalible on this PC
        public void GetComPortNames()
        {
            string[] ports = SerialPort.GetPortNames();

            cmbComPortList.Items.Clear();

            foreach (string port in ports)
            {
                cmbComPortList.Items.Add(port);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetComPortNames();
            serialPortCon.ReadBufferSize = 65536;

            // TODO : Load this from a file so we remember the selected com port
			try
			{
				cmbComPortList.SelectedIndex = 0;
			}
			catch (Exception)
			{				
		
			}

            InitializeExcel();
            
        }

        private void butOpen_Click(object sender, EventArgs e)
        {
            if ("Open" == butOpen.Text)
            {
                try
                {
                    // You need to set the encoding to default or you will only get 7bits
                    serialPortCon.Encoding = System.Text.Encoding.Default;
                    serialPortCon.PortName = cmbComPortList.Text;
                    serialPortCon.BaudRate = 19200;
                    //serialPortCon.BaudRate = 57600;
                    serialPortCon.Open();
                    cmbComPortList.Enabled = false;
                    butOpen.Text = "Close";

                    for (int i = 0; i < 5; i++)
                    {
                        serialPortCon.WriteLine("AT\r\n");
                    }              

                    serialPortCon.WriteLine("AT+CMGF=1\r\n");

                }
                catch (Exception)
                {
                    Log(LogMsgType.Error, "Failed to open the COM port\n");
                }
            }
            else
            {
                try
                {
                    serialPortCon.Close();
                    cmbComPortList.Enabled = true;
                    butOpen.Text = "Open";
                }
                catch (Exception)
                {
                    Log(LogMsgType.Error, "Failed to close the COM port\n");
                }
            }
        }

        // -------------------------------------------------------------
        // Send data via coms
        // -------------------------------------------------------------
        private void butSend_Click(object sender, EventArgs e)
        {
            if (CurrentDataMode == DataMode.Text)
            {
                try
                {
                    // Send the user's text straight out the port
                    //serialPortCon.Write(txtToSend.Text);
                    serialPortCon.WriteLine(txtToSend.Text);

                    // Show in the terminal window the user's text
                    Log(LogMsgType.Outgoing, txtToSend.Text + "\n");
                }
                catch (Exception)
                {
                    Log(LogMsgType.Error, "Failed to send, COM port Open?\n");
                }
            }
            else
            {
                try
                {
                    // Convert the user's string of hex digits (ex: B4 CA E2) to a byte array
                    byte[] data = HexStringToByteArray(txtToSend.Text);

                    // Send the binary data out the port
                    serialPortCon.Write(data, 0, data.Length);

                    // Show the hex digits on in the terminal window
                    Log(LogMsgType.Outgoing, ByteArrayToHexString(data) + "\n");
                }
                catch (Exception)
                //catch (FormatException)
                {
                    // Inform the user if the hex string was not properly formatted
                    Log(LogMsgType.Error, "Not properly formatted hex string: " + txtToSend.Text + "\n");
                }
            }
            txtToSend.SelectAll();
        }

        // -------------------------------------------------------------
        // Read Coms data
        // -------------------------------------------------------------

        /// <summary> Log data to the terminal window. </summary>
        /// <param name="msgtype"> The type of message to be written. </param>
        /// <param name="msg"> The string containing the message to be shown. </param>
        private void Log(LogMsgType msgtype, string msg)
        {
            rtfTerminal.Invoke(new EventHandler(delegate
            {
                rtfTerminal.SelectedText = string.Empty;
                rtfTerminal.SelectionFont = new Font(rtfTerminal.SelectionFont, FontStyle.Regular);
                rtfTerminal.SelectionColor = LogMsgTypeColor[(int)msgtype];
                rtfTerminal.AppendText(msg);
                rtfTerminal.ScrollToCaret();
            }));
        }

        // -------------------------------------------------------------
        // Strip Data
        // -------------------------------------------------------------
        private void strip_string (string dat)
        {
            char[] delimiterChars = { ',', ' ', '\n', '\r' };
            //char[] delimiterChars = { ',', '.', '\n', ' '};
            string[] words = dat.Split(delimiterChars);
            int arrayLength = words.Length;

            words = GetArrayWithoutEmptyEntries(words);
            WriteToExcel(words);
        }

        // -------------------------------------------------------------
        // Remove Empty Spaces
        // -------------------------------------------------------------
        public static string[] GetArrayWithoutEmptyEntries(string[] input)
        {
            if (input == null || input.Length == 0)
                return input;

            var list = new List<string>(input.Length);
            for (int i = 0; i < input.Length; i++)
                // in .NET 4.0 you could use IsNullOrWhiteSpace()
                if (!String.IsNullOrEmpty(input[i]))
                    list.Add(input[i]);

            return list.ToArray();
        }
        // -------------------------------------------------------------
        // Declarations
        // -------------------------------------------------------------
        public static string DB_PATH = "C:\\Users\\Ayodele\\Desktop\\Software Test\\ComPortDemo\\sms_server.xlsx";  // Add your own path here
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;
        private static int lastRow = 0;

        // -------------------------------------------------------------
        // Init Excel
        // -------------------------------------------------------------
        public static void InitializeExcel()
        {

            //Excel.Workbook excelWorkbook = null;
            MyApp = new Excel.Application();
            MyApp.Visible = true;
            MyBook = MyApp.Workbooks.Open(DB_PATH);
            MySheet = (Excel.Worksheet)MyBook.Sheets[1]; 
            lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            try
            {
                MyBook = MyApp.Workbooks.Open(DB_PATH, 0,
                false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true,
                false, 0, true, false, false);
            }
            catch
            {
                //Create a new workbook if the existing workbook failed to open.
                MyBook = MyApp.Workbooks.Add();
            }
            MySheet = (Excel.Worksheet)MyBook.Sheets[1];
            lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
        }
        // -------------------------------------------------------------
        // Write to Excel
        // -------------------------------------------------------------
        public static void WriteToExcel(String[] emp)
        {
            int index2 = emp.Length;

            if (index2 > 20)
            {

            }

            else
            {
                lastRow += 1;
                for (int i = 0; i < index2; i++)
                {
                    MySheet.Cells[lastRow, (i + 1)] = emp[i];
                }
                MyBook.Save();
            }


        }
        // -------------------------------------------------------------
        // Close Excel
        // -------------------------------------------------------------
        public static void CloseExcel()
        {
            MyBook.Saved = true;
            MyApp.Quit();
        }

        // -------------------------------------------------------------
        // Data Received Handler
        // -------------------------------------------------------------
        private void serialPortCon_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            int bytesRecv = serialPortCon.BytesToRead;

            if (bytesRecv > 0)
            {
                // which mode (string or binary)
                if (CurrentDataMode == DataMode.Text)
                {
                    // Read all the data waiting in the buffer
                    string data = serialPortCon.ReadExisting();
                    // Display the text to the user in the terminal
                    Log(LogMsgType.Incoming, data);
                    strip_string(data);
                }
                else
                {
                    byte[] commsBytesIn = new byte[bytesRecv];
                    // Read the coms data into a byte array
                    serialPortCon.Read(commsBytesIn, 0, bytesRecv);
                    Log(LogMsgType.Incoming, ByteArrayToHexString(commsBytesIn));
                }
            }
        }

        // -------------------------------------------------------------
        // Button Clear
        // -------------------------------------------------------------
        private void butClear_Click(object sender, EventArgs e)
        {
            txtToSend.Clear();
        }

        private void butClearRecv_Click(object sender, EventArgs e)
        {
            rtfTerminal.Clear();
        }

        private void rbText_CheckedChanged(object sender, EventArgs e)
        {
            if (rbText.Checked) CurrentDataMode = DataMode.Text;
        }

        private void rbHex_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHex.Checked) CurrentDataMode = DataMode.Hex;
        }

        private void butAnotherPort_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath);
        }

        private void txtToSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                butSend_Click(null, null);
            }
        }

        private void gbMode_Enter(object sender, EventArgs e)
        {

        }

        private void cmbComPortList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}