namespace ComPortDemo
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.cmbComPortList = new System.Windows.Forms.ComboBox();
            this.serialPortCon = new System.IO.Ports.SerialPort(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtToSend = new System.Windows.Forms.TextBox();
            this.butSend = new System.Windows.Forms.Button();
            this.butOpen = new System.Windows.Forms.Button();
            this.butClearRecv = new System.Windows.Forms.Button();
            this.gbMode = new System.Windows.Forms.GroupBox();
            this.rbText = new System.Windows.Forms.RadioButton();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.rtfTerminal = new System.Windows.Forms.RichTextBox();
            this.butAnotherPort = new System.Windows.Forms.Button();
            this.gbMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbComPortList
            // 
            this.cmbComPortList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbComPortList.FormattingEnabled = true;
            this.cmbComPortList.Location = new System.Drawing.Point(82, 19);
            this.cmbComPortList.Name = "cmbComPortList";
            this.cmbComPortList.Size = new System.Drawing.Size(75, 21);
            this.cmbComPortList.TabIndex = 0;
            // 
            // serialPortCon
            // 
            this.serialPortCon.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPortCon_DataReceived);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "COM Port :";
            // 
            // txtToSend
            // 
            this.txtToSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToSend.Location = new System.Drawing.Point(12, 290);
            this.txtToSend.Name = "txtToSend";
            this.txtToSend.Size = new System.Drawing.Size(364, 20);
            this.txtToSend.TabIndex = 2;
            this.txtToSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtToSend_KeyDown);
            // 
            // butSend
            // 
            this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butSend.Location = new System.Drawing.Point(382, 290);
            this.butSend.Name = "butSend";
            this.butSend.Size = new System.Drawing.Size(75, 23);
            this.butSend.TabIndex = 3;
            this.butSend.Text = "Send";
            this.butSend.UseVisualStyleBackColor = true;
            this.butSend.Click += new System.EventHandler(this.butSend_Click);
            // 
            // butOpen
            // 
            this.butOpen.Location = new System.Drawing.Point(163, 17);
            this.butOpen.Name = "butOpen";
            this.butOpen.Size = new System.Drawing.Size(75, 23);
            this.butOpen.TabIndex = 3;
            this.butOpen.Text = "Open";
            this.butOpen.UseVisualStyleBackColor = true;
            this.butOpen.Click += new System.EventHandler(this.butOpen_Click);
            // 
            // butClearRecv
            // 
            this.butClearRecv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butClearRecv.Location = new System.Drawing.Point(382, 261);
            this.butClearRecv.Name = "butClearRecv";
            this.butClearRecv.Size = new System.Drawing.Size(75, 23);
            this.butClearRecv.TabIndex = 3;
            this.butClearRecv.Text = "Clear";
            this.butClearRecv.UseVisualStyleBackColor = true;
            this.butClearRecv.Click += new System.EventHandler(this.butClearRecv_Click);
            // 
            // gbMode
            // 
            this.gbMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMode.Controls.Add(this.rbText);
            this.gbMode.Controls.Add(this.rbHex);
            this.gbMode.Location = new System.Drawing.Point(382, 41);
            this.gbMode.Name = "gbMode";
            this.gbMode.Size = new System.Drawing.Size(77, 64);
            this.gbMode.TabIndex = 6;
            this.gbMode.TabStop = false;
            this.gbMode.Text = "Data &Mode";
            this.gbMode.Enter += new System.EventHandler(this.gbMode_Enter);
            // 
            // rbText
            // 
            this.rbText.AutoSize = true;
            this.rbText.Checked = true;
            this.rbText.Location = new System.Drawing.Point(12, 19);
            this.rbText.Name = "rbText";
            this.rbText.Size = new System.Drawing.Size(46, 17);
            this.rbText.TabIndex = 0;
            this.rbText.TabStop = true;
            this.rbText.Text = "Text";
            this.rbText.CheckedChanged += new System.EventHandler(this.rbText_CheckedChanged);
            // 
            // rbHex
            // 
            this.rbHex.AutoSize = true;
            this.rbHex.Location = new System.Drawing.Point(12, 39);
            this.rbHex.Name = "rbHex";
            this.rbHex.Size = new System.Drawing.Size(44, 17);
            this.rbHex.TabIndex = 1;
            this.rbHex.Text = "Hex";
            this.rbHex.CheckedChanged += new System.EventHandler(this.rbHex_CheckedChanged);
            // 
            // rtfTerminal
            // 
            this.rtfTerminal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfTerminal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtfTerminal.Location = new System.Drawing.Point(12, 46);
            this.rtfTerminal.Name = "rtfTerminal";
            this.rtfTerminal.Size = new System.Drawing.Size(364, 238);
            this.rtfTerminal.TabIndex = 7;
            this.rtfTerminal.Text = "";
            // 
            // butAnotherPort
            // 
            this.butAnotherPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butAnotherPort.Location = new System.Drawing.Point(382, 232);
            this.butAnotherPort.Name = "butAnotherPort";
            this.butAnotherPort.Size = new System.Drawing.Size(75, 23);
            this.butAnotherPort.TabIndex = 3;
            this.butAnotherPort.Text = "New Port";
            this.butAnotherPort.UseVisualStyleBackColor = true;
            this.butAnotherPort.Click += new System.EventHandler(this.butAnotherPort_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 322);
            this.Controls.Add(this.rtfTerminal);
            this.Controls.Add(this.gbMode);
            this.Controls.Add(this.butOpen);
            this.Controls.Add(this.butAnotherPort);
            this.Controls.Add(this.butClearRecv);
            this.Controls.Add(this.butSend);
            this.Controls.Add(this.txtToSend);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbComPortList);
            this.Name = "Form1";
            this.Text = "Carls Mini Coms Term";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbMode.ResumeLayout(false);
            this.gbMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbComPortList;
        private System.IO.Ports.SerialPort serialPortCon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtToSend;
        private System.Windows.Forms.Button butSend;
        private System.Windows.Forms.Button butOpen;
        private System.Windows.Forms.Button butClearRecv;
        private System.Windows.Forms.GroupBox gbMode;
        private System.Windows.Forms.RadioButton rbText;
        private System.Windows.Forms.RadioButton rbHex;
        private System.Windows.Forms.RichTextBox rtfTerminal;
        private System.Windows.Forms.Button butAnotherPort;
    }
}

