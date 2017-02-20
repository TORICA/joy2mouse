using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace joy2mouse_host
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
                Console.WriteLine(port);
                comboBox1.SelectedItem = port;
                serialPort1.Close();
                serialPort1.PortName = port;
                serialPort1.Open();
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort1.ReadLine();
                if (!string.IsNullOrEmpty(data))
                {
                    String[] elements=data.Split(',');
                    int valueX = int.Parse(elements[0]);
                    int valueY = int.Parse(elements[1]);

                    int newCursorX = System.Windows.Forms.Cursor.Position.X + valueX/20;
                    int newCursorY = System.Windows.Forms.Cursor.Position.Y - valueY/20;
                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(newCursorX, newCursorY);

                    Console.WriteLine("{0} -> X={1}, Y={2}",data,valueX,valueY);
                    //SendKeys.SendWait("{ENTER}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.Close();
            string port = (String)comboBox1.SelectedItem;
            Console.WriteLine(port);
            serialPort1.PortName = port;
            serialPort1.Open();
        }
    }
}