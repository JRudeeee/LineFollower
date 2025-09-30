using System.Drawing;
using System.Windows.Forms;

namespace LineFollower
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.getIOtimer = new System.Windows.Forms.Timer(this.components);
            this.buttonStartStop = new System.Windows.Forms.Button();
            this.labelCalib = new System.Windows.Forms.Label();
            this.buttonCalibrateWhite = new System.Windows.Forms.Button();
            this.buttonCalibrateBlack = new System.Windows.Forms.Button();
            this.buttonBBRev = new System.Windows.Forms.Button();
            this.buttonBBFwd = new System.Windows.Forms.Button();
            this.labelMode = new System.Windows.Forms.Label();
            this.buttonPIDFwd = new System.Windows.Forms.Button();
            this.buttonPIDRev = new System.Windows.Forms.Button();
            this.maskedTextBoxSensorLeft = new System.Windows.Forms.MaskedTextBox();
            this.labelSensorLeft = new System.Windows.Forms.Label();
            this.labelSensorRight = new System.Windows.Forms.Label();
            this.maskedTextBoxSensorRight = new System.Windows.Forms.MaskedTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripSerialLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSerialStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.serial = new System.IO.Ports.SerialPort(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // getIOtimer
            // 
            this.getIOtimer.Enabled = true;
            this.getIOtimer.Interval = 5;
            this.getIOtimer.Tick += new System.EventHandler(this.getIOtimer_Tick);
            // 
            // buttonStartStop
            // 
            this.buttonStartStop.BackColor = System.Drawing.Color.Red;
            this.buttonStartStop.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStartStop.Location = new System.Drawing.Point(20, 10);
            this.buttonStartStop.Name = "buttonStartStop";
            this.buttonStartStop.Size = new System.Drawing.Size(107, 40);
            this.buttonStartStop.TabIndex = 0;
            this.buttonStartStop.Text = "Run / Stop";
            this.buttonStartStop.UseVisualStyleBackColor = false;
            this.buttonStartStop.Click += new System.EventHandler(this.buttonStartStop_Click);
            // 
            // labelCalib
            // 
            this.labelCalib.AutoSize = true;
            this.labelCalib.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelCalib.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCalib.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelCalib.Location = new System.Drawing.Point(2, 84);
            this.labelCalib.Name = "labelCalib";
            this.labelCalib.Size = new System.Drawing.Size(150, 21);
            this.labelCalib.TabIndex = 1;
            this.labelCalib.Text = "Sensor Calibration";
            // 
            // buttonCalibrateWhite
            // 
            this.buttonCalibrateWhite.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCalibrateWhite.Location = new System.Drawing.Point(19, 105);
            this.buttonCalibrateWhite.Name = "buttonCalibrateWhite";
            this.buttonCalibrateWhite.Size = new System.Drawing.Size(107, 38);
            this.buttonCalibrateWhite.TabIndex = 2;
            this.buttonCalibrateWhite.Text = "White Surface";
            this.buttonCalibrateWhite.UseVisualStyleBackColor = true;
            this.buttonCalibrateWhite.Click += new System.EventHandler(this.buttonCalibrateWhite_Click);
            // 
            // buttonCalibrateBlack
            // 
            this.buttonCalibrateBlack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCalibrateBlack.Location = new System.Drawing.Point(19, 148);
            this.buttonCalibrateBlack.Name = "buttonCalibrateBlack";
            this.buttonCalibrateBlack.Size = new System.Drawing.Size(108, 38);
            this.buttonCalibrateBlack.TabIndex = 3;
            this.buttonCalibrateBlack.Text = "Black Surface";
            this.buttonCalibrateBlack.UseVisualStyleBackColor = true;
            this.buttonCalibrateBlack.Click += new System.EventHandler(this.buttonCalibrateBlack_Click);
            // 
            // buttonBBRev
            // 
            this.buttonBBRev.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBBRev.Location = new System.Drawing.Point(198, 148);
            this.buttonBBRev.Name = "buttonBBRev";
            this.buttonBBRev.Size = new System.Drawing.Size(108, 38);
            this.buttonBBRev.TabIndex = 6;
            this.buttonBBRev.Text = "Bang-Bang Reverse";
            this.buttonBBRev.UseVisualStyleBackColor = true;
            this.buttonBBRev.Click += new System.EventHandler(this.buttonBBRev_Click);
            // 
            // buttonBBFwd
            // 
            this.buttonBBFwd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBBFwd.Location = new System.Drawing.Point(199, 105);
            this.buttonBBFwd.Name = "buttonBBFwd";
            this.buttonBBFwd.Size = new System.Drawing.Size(107, 38);
            this.buttonBBFwd.TabIndex = 5;
            this.buttonBBFwd.Text = "Bang-Bang Forward";
            this.buttonBBFwd.UseVisualStyleBackColor = true;
            this.buttonBBFwd.Click += new System.EventHandler(this.buttonBBFwd_Click);
            // 
            // labelMode
            // 
            this.labelMode.AutoSize = true;
            this.labelMode.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelMode.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelMode.Location = new System.Drawing.Point(202, 84);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(104, 21);
            this.labelMode.TabIndex = 4;
            this.labelMode.Text = "Mode Select";
            // 
            // buttonPIDFwd
            // 
            this.buttonPIDFwd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPIDFwd.Location = new System.Drawing.Point(199, 192);
            this.buttonPIDFwd.Name = "buttonPIDFwd";
            this.buttonPIDFwd.Size = new System.Drawing.Size(108, 38);
            this.buttonPIDFwd.TabIndex = 7;
            this.buttonPIDFwd.Text = "PID Forward";
            this.buttonPIDFwd.UseVisualStyleBackColor = true;
            this.buttonPIDFwd.Click += new System.EventHandler(this.buttonPIDFwd_Click);
            // 
            // buttonPIDRev
            // 
            this.buttonPIDRev.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPIDRev.Location = new System.Drawing.Point(198, 235);
            this.buttonPIDRev.Name = "buttonPIDRev";
            this.buttonPIDRev.Size = new System.Drawing.Size(108, 38);
            this.buttonPIDRev.TabIndex = 8;
            this.buttonPIDRev.Text = "PID Reverse";
            this.buttonPIDRev.UseVisualStyleBackColor = true;
            this.buttonPIDRev.Click += new System.EventHandler(this.buttonPIDRev_Click);
            // 
            // maskedTextBoxSensorLeft
            // 
            this.maskedTextBoxSensorLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maskedTextBoxSensorLeft.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.maskedTextBoxSensorLeft.Location = new System.Drawing.Point(241, 10);
            this.maskedTextBoxSensorLeft.Name = "maskedTextBoxSensorLeft";
            this.maskedTextBoxSensorLeft.ReadOnly = true;
            this.maskedTextBoxSensorLeft.Size = new System.Drawing.Size(65, 23);
            this.maskedTextBoxSensorLeft.TabIndex = 9;
            // 
            // labelSensorLeft
            // 
            this.labelSensorLeft.AutoSize = true;
            this.labelSensorLeft.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSensorLeft.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelSensorLeft.Location = new System.Drawing.Point(145, 10);
            this.labelSensorLeft.Name = "labelSensorLeft";
            this.labelSensorLeft.Size = new System.Drawing.Size(94, 21);
            this.labelSensorLeft.TabIndex = 10;
            this.labelSensorLeft.Text = "Left Sensor";
            // 
            // labelSensorRight
            // 
            this.labelSensorRight.AutoSize = true;
            this.labelSensorRight.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSensorRight.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelSensorRight.Location = new System.Drawing.Point(133, 37);
            this.labelSensorRight.Name = "labelSensorRight";
            this.labelSensorRight.Size = new System.Drawing.Size(106, 21);
            this.labelSensorRight.TabIndex = 11;
            this.labelSensorRight.Text = "Right Sensor";
            // 
            // maskedTextBoxSensorRight
            // 
            this.maskedTextBoxSensorRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maskedTextBoxSensorRight.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.maskedTextBoxSensorRight.Location = new System.Drawing.Point(241, 37);
            this.maskedTextBoxSensorRight.Name = "maskedTextBoxSensorRight";
            this.maskedTextBoxSensorRight.Size = new System.Drawing.Size(65, 23);
            this.maskedTextBoxSensorRight.TabIndex = 12;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSerialLabel,
            this.toolStripSerialStatus,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 357);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(375, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripSerialLabel
            // 
            this.toolStripSerialLabel.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSerialLabel.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.toolStripSerialLabel.Name = "toolStripSerialLabel";
            this.toolStripSerialLabel.Size = new System.Drawing.Size(79, 17);
            this.toolStripSerialLabel.Text = "Serial Status : ";
            // 
            // toolStripSerialStatus
            // 
            this.toolStripSerialStatus.Name = "toolStripSerialStatus";
            this.toolStripSerialStatus.Size = new System.Drawing.Size(112, 17);
            this.toolStripSerialStatus.Text = "toolStripSerialStatus";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(375, 379);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.maskedTextBoxSensorRight);
            this.Controls.Add(this.labelSensorRight);
            this.Controls.Add(this.labelSensorLeft);
            this.Controls.Add(this.maskedTextBoxSensorLeft);
            this.Controls.Add(this.buttonPIDRev);
            this.Controls.Add(this.buttonPIDFwd);
            this.Controls.Add(this.buttonBBRev);
            this.Controls.Add(this.buttonBBFwd);
            this.Controls.Add(this.labelMode);
            this.Controls.Add(this.buttonCalibrateBlack);
            this.Controls.Add(this.buttonCalibrateWhite);
            this.Controls.Add(this.labelCalib);
            this.Controls.Add(this.buttonStartStop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Line Following Robot";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer getIOtimer;
        private Button buttonStartStop;
        private Label labelCalib;
        private Button buttonCalibrateWhite;
        private Button buttonCalibrateBlack;
        private Button buttonBBRev;
        private Button buttonBBFwd;
        private Label labelMode;
        private Button buttonPIDFwd;
        private Button buttonPIDRev;
        private MaskedTextBox maskedTextBoxSensorLeft;
        private Label labelSensorLeft;
        private Label labelSensorRight;
        private MaskedTextBox maskedTextBoxSensorRight;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripSerialLabel;
        private ToolStripStatusLabel toolStripSerialStatus;
        private System.IO.Ports.SerialPort serial;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}

