using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

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
            // TODO : Load this from a file so we remember the selected com port
			try
			{
				cmbComPortList.SelectedIndex = 0;
			}
			catch (Exception)
			{
				
		
			}
            
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
                    serialPortCon.BaudRate = 38400;
                    serialPortCon.Open();
                    cmbComPortList.Enabled = false;
                    butOpen.Text = "Close";
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
                    serialPortCon.Write(txtToSend.Text);

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
                rtfTerminal.SelectionFont = new Font(rtfTerminal.SelectionFont, FontStyle.Bold);
                rtfTerminal.SelectionColor = LogMsgTypeColor[(int)msgtype];
                rtfTerminal.AppendText(msg);
                rtfTerminal.ScrollToCaret();
            }));
        }


        //private delegate void UpdateFormCallBack(Byte[] bytes, int length);

        //void SerialDataReceived(Byte[] bytes, int length)
        //{
        //    try
        //    {
        //        string tempstr = System.Text.Encoding.Default.GetString(bytes, 0, length);
        //        txtRecvd.Text = tempstr + txtRecvd.Text;
        //    }
        //    catch (Exception)
        //    {
                
        //        throw;
        //    }

        //}

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


    }
}