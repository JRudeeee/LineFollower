using System;
using System.Collections.Generic;
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
        double curTime = 0, prevTime = 0, elapsed = 0;
        byte leftSensor, rightSensor;
        private Stopwatch stopwatch = new Stopwatch();

        double kp = 0, ki = 0, kd = 0, error = 0, oldError = 0, sum = 0, pidOut = 0, errorIntegral = 0, pidCommand = 0;
        double proportional = 0, integral = 0, derivative = 0;

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
        const int MAX_FWD = 75;
        const int MAX_REV = 25;
        const int MOTOR_STOP = 50;
        const int BB_FWD = 0;
        const int BB_REV = 1;
        const int PID_FWD = 2;
        const int PID_REV = 3;
        const double DUTY_STEP = (double)244 / (double)100;
        const double CENTER = (double)0.05;

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
            curTime = stopwatch.ElapsedMilliseconds;
            elapsed = curTime - prevTime;
            //toolStripStatusLabel1.Text = elapsed.ToString() + "s";
            toolStripStatusLabel1.Text = stopwatch.Elapsed.TotalSeconds.ToString() + "s";
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
                        PIDControlFWD();
                        break;
                    case PID_REV:

                        break;
                }
            }
            prevTime = curTime;
        }

        private void BBFwd()
        {
            double position = FindPosition(leftSensor, rightSensor);

            if (position >-CENTER && position < CENTER)
            {
                position = 0;
            }

            if (position < 0)
            {
                MotorDrive(MAX_FWD, 40);
            }
            else if (position > 0)
            {
                MotorDrive(40, MAX_FWD);
            }
            else
            {
                MotorDrive(MAX_FWD, MAX_FWD);
            }
        }

        private void BBRev()
        {
            double position = FindPosition(leftSensor, rightSensor);

            if (position > -CENTER && position < CENTER)
            {
                position = 0;
            }

            if (position > 0)
            {
                MotorDrive(MAX_REV, 60);
            }
            else if (position < 0)
            {
                MotorDrive(60, MAX_REV);
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
        private double FindPosition(byte leftSensor, byte rightSensor)
        {
            double difference = leftSensor - rightSensor;
            double position = difference / SENSOR_MAX; // -1 when left of line, 0 when center on line, 1 when right of line

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
            result = (double)Math.Floor(result);
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

        private void PID()
        {
            error = FindPosition(leftSensor,rightSensor);
            sum += error;
            proportional = kp * error;
            
            derivative = kd * (error - oldError) / elapsed;
            bool windUp = false, saturation = false;

            pidOut = proportional + integral + derivative;

            if (pidOut > MAX_FWD)
            {
                pidCommand = MAX_FWD;
                saturation = true;
            }
            else if (pidOut < MAX_REV)
            {
                pidCommand = MAX_REV;
                saturation = true;
            }
            else
            {
                pidCommand = pidOut;
                saturation= false;
            }

            if (error > 0 && pidOut > 0)
            {
                windUp = true;
            } else if (error < 0 &&  pidOut < 0)
            {
                windUp = true;
            } else
            {
                windUp = false;
            }

            if ((saturation) && (windUp))
            {
                integral = 0;
            } else
            {
                integral = ki * sum * elapsed;
            }

                oldError = error;

        }

        private void PIDControlFWD() {
            PID();
            MotorDrive(pidCommand, -pidCommand);
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
                else {
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

        private void buttonCalibrateWhite_Click(object sender, EventArgs e)
        {
            buttonCalibrateWhite.BackColor = Color.Green;
        }

        private void buttonCalibrateBlack_Click(object sender, EventArgs e)
        {
            buttonCalibrateBlack.BackColor = Color.Green;
        }
    }
}
