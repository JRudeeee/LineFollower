using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineFollower
{
    public partial class Form1 : Form
    {
        bool runRobot = false, runSerial = true;
        int input1 = 0, input2 = 0, count = 0, controlMode = 0;
        double curTime = 0, prevTime = 0, totTime = 0, timeDiff = 0;
        byte leftSensor, rightSensor;
        private Stopwatch stopwatch = new Stopwatch();

        bool run = false;

        byte[] outputs = new byte[4];
        byte[] inputs = new byte[4];

        const byte START = 255;
        const byte ZERO = 0;
        const int LOWER_DEADBAND = 1;
        const int LEFT_SENSOR = 0;
        const int RIGHT_SENSOR = 1;
        const int LEFT_MOTOR = 2;
        const int RIGHT_MOTOR = 3;
        const int SENSOR_MIN = 0;
        const int SENSOR_MAX = 255;
        const int MAX_FWD = 100;
        const int STD_FWD = 70;
        const int MAX_REV = 40;
        const int MOTOR_STOP = 50;
        const int BB_FWD = 0;
        const int BB_REV = 1;
        const int PID_FWD = 2;
        const int PID_REV = 3;
        const float DUTY_STEP = (float)244 / (float)100;
        const float CENTER = (float)0.2;


        const double Kp = 0.25;
        const double Ki = 0;
        const double Kd = 0.002;
        const float SET_POSITION = 0;

        double accError = 0;
        double lastE = 0;

        double correction, position;
        double leftDuty, rightDuty;


        public Form1()
        {
            InitializeComponent();
            stopwatch.Start();


            if (runSerial)
            {
                if (!serial.IsOpen)
                {
                    try
                    {
                        serial.Open();
                        toolStripSerialStatus.Text = "Connected.";
                    }
                    catch
                    {
                        toolStripSerialStatus.Text = "Error: Failed to connect.";
                    }
                }
            }


        }

        // Main loop run at end of timer tick.
        private void loop()
        {
            curTime = (float) stopwatch.ElapsedMilliseconds;
            toolStripStatusLabel1.Text = curTime.ToString();
            timeDiff = curTime - prevTime;
            if (!runRobot)
            {
                MotorDrive(MOTOR_STOP, MOTOR_STOP);
            }
            else
            {
                switch (controlMode)
                {
                    case BB_FWD:
                        BBFwd();
                        break;
                    case BB_REV:
                        BBRev();
                        break;
                    case PID_FWD:
                        vehicle_control();
                        break;
                    case PID_REV:
                        
                        break;
                }
            }
            prevTime = curTime;
        }

        private void BBFwd()
        {
            float position = FindPosition(leftSensor, rightSensor);

            if (position > -CENTER && position < CENTER)
            {
                position = 0;
            }

            if (position > 0)
            {
                MotorDrive(80, 30);
            }
            else if (position < 0)
            {
                MotorDrive(30, 80);
            }
            else
            {
                MotorDrive(75, 75);
            }
        }

        private void BBRev()
        {
            float position = FindPosition(leftSensor, rightSensor);

            if (position > -CENTER && position < CENTER)
            {
                position = 0;
            }

            if (position > 0)
            {
                MotorDrive(MAX_REV, MAX_FWD);
            }
            else if (position < 0)
            {
                MotorDrive(MAX_FWD, MAX_REV);
            }
            else
            {
                MotorDrive(MAX_REV, MAX_REV);
            }
        }

        // Send a four byte message to the Arduino via serial.
        private void SendIO(byte PORT, byte DATA)
        {
            outputs[0] = START; // Set the first byte to the start value that indicated the beginning of the message.
            outputs[1] = PORT;  // Set the second byte to represend the port where, leftSensor = 0, rightSensor = 1, leftMotor = 2, rightMotor = 3.
            outputs[2] = DATA;  // Set the third data byte to the value to be assigned to the port. 
            outputs[3] = (byte)(START + PORT + DATA); // Calculate the checksum byte.

            if (serial.IsOpen)
            {
                serial.Write(outputs, 0, 4); // Send all four bytes to the IO Card.
            }
        }

        // Find the position of the vehicle with relation to the line.
        private float FindPosition(byte leftSensor, byte rightSensor)
        {
            float difference = leftSensor - rightSensor;
            float position = difference / SENSOR_MAX; // -1 when left of line, 0 when center on line, 1 when right of line

            return position;
        }

        // Convert the commanded duty cycle to a bit output.
        private byte ConvertDutyCycle(double inDuty)
        {
            double result;
            if (inDuty < LOWER_DEADBAND) // For duty cycles commanded below 1%
            {
                result = ZERO;
            }
            else
            {
                result = DUTY_STEP * inDuty;
            }
            result = (float)Math.Floor(result);
            return (byte)result;
        }

        // Command motors to required duty cycle.
        private void MotorDrive(double leftDuty, double rightDuty)
        {
            SendIO(LEFT_MOTOR, ConvertDutyCycle(leftDuty));
            SendIO(RIGHT_MOTOR, ConvertDutyCycle(rightDuty));
        }

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (!runRobot)
            {
                runRobot = true;
                buttonStartStop.BackColor = Color.Green;
            }
            else
            {
                runRobot = false;
                buttonStartStop.BackColor = Color.Red;
            }
        }

        private void getIOtimer_Tick(object sender, EventArgs e)
        {
            if (serial.IsOpen)
            {
                SendIO((byte)count, ZERO); // Trigger sensor read, cycle between sensors every timer tick
                if (count == LEFT_SENSOR)
                {
                    count = RIGHT_SENSOR;
                }
                else
                {
                    count = LEFT_SENSOR;
                }

                if (serial.BytesToRead >= 4) // Check that the buffer contains a full four byte package.
                {
                    //toolStripSerialStatus.Text = "Incoming";
                    inputs[0] = (byte)serial.ReadByte(); // Read the first byte of the package.

                    // Check that the first byte is in fact the start byte.
                    if (inputs[0] == START)
                    {
                        //toolStripSerialStatus.Text = "Start Accepted";

                        // Read the rest of the package.
                        inputs[1] = (byte)serial.ReadByte();
                        inputs[2] = (byte)serial.ReadByte();
                        inputs[3] = (byte)serial.ReadByte();

                        // Claculate the checksum.
                        byte checkSum = (byte)(inputs[0] + inputs[1] + inputs[2]);

                        // Check that the calculated check sum matches the check sum sent with the message.
                        if (inputs[3] == checkSum)
                        {
                            //toolStripSerialStatus.Text = "CheckSum Accepted";

                            // Check which port the incoming data is associated with.
                            switch (inputs[1])
                            {
                                case LEFT_SENSOR:
                                    //toolStripSerialStatus.Text = "Left Sensor Read";
                                    leftSensor = inputs[2];
                                    maskedTextBoxSensorLeft.Text = leftSensor.ToString();
                                    break;
                                case RIGHT_SENSOR:
                                    //toolStripSerialStatus.Text = "Right Sensor Read";
                                    rightSensor = inputs[2];
                                    maskedTextBoxSensorRight.Text = rightSensor.ToString();
                                    break;
                            }
                        }
                    }
                }
            }

            loop();
        }

        private void buttonBBFwd_Click(object sender, EventArgs e)
        {
            controlMode = BB_FWD;
            buttonBBFwd.BackColor = Color.FromArgb(204, 255, 255, 1);
            buttonBBRev.BackColor = SystemColors.ControlLightLight;
            buttonPIDFwd.BackColor = SystemColors.ControlLightLight;
            buttonPIDRev.BackColor = SystemColors.ControlLightLight;
        }

        private void buttonBBRev_Click(object sender, EventArgs e)
        {
            controlMode = BB_REV;
            buttonBBFwd.BackColor = SystemColors.ControlLightLight;
            buttonBBRev.BackColor = Color.FromArgb(204, 255, 255, 1);
            buttonPIDFwd.BackColor = SystemColors.ControlLightLight;
            buttonPIDRev.BackColor = SystemColors.ControlLightLight;
        }

        private void buttonPIDFwd_Click(object sender, EventArgs e)
        {
            controlMode = PID_FWD;
            buttonBBFwd.BackColor = SystemColors.ControlLightLight;
            buttonBBRev.BackColor = SystemColors.ControlLightLight;
            buttonPIDFwd.BackColor = Color.FromArgb(204, 255, 255, 1);
            buttonPIDRev.BackColor = SystemColors.ControlLightLight;
        }

        private void buttonPIDRev_Click(object sender, EventArgs e)
        {
            controlMode = PID_REV;
            buttonBBFwd.BackColor = SystemColors.ControlLightLight;
            buttonBBRev.BackColor = SystemColors.ControlLightLight;
            buttonPIDFwd.BackColor = SystemColors.ControlLightLight;
            buttonPIDRev.BackColor = Color.FromArgb(204, 255, 255, 1);
        }

        void vehicle_control()
        {
            PID_Control();
            MotorDrive(leftDuty, rightDuty);
        }

        private void PID_Control()
        {
            position = leftSensor - rightSensor;

            if (position > -CENTER && position < CENTER)
            {
                position = 0;
            }
            float elapsedTime = (float) curTime - (float)prevTime;
            double error =  position - SET_POSITION;
            double rateError = (error - lastE) / elapsedTime;
            accError += error * elapsedTime;

            double controlSig = ((Kp * error) + (Ki * accError) + (Kd * rateError));
            
            
            correction = controlSig;

            leftDuty = leftDuty + correction;
            rightDuty = rightDuty + correction;

            prevTime = curTime;
            lastE = error;

            leftDuty = Math.Max(MAX_REV, Math.Min(MAX_FWD, leftDuty));
            rightDuty = Math.Max(MAX_REV, Math.Min(MAX_FWD, rightDuty));



        }
    }
}
