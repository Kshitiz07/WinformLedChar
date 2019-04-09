using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LedControl;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Timer timer1 = new Timer();

        bool clock = false;
        bool scroll = false;
        bool hide = false;


        public Form1()
        {
            InitializeComponent();

            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            this.timer1.Enabled = false;
            this.timer1.Stop();

            ledDisplay1.Scroll = false;
        }

        private void designButton_Click(object sender, EventArgs e)
        {
            DesignForm lf = new DesignForm(ledDisplay1);
            lf.ShowDialog();
            lf.Dispose();
        }

        private void clockButton_Click(object sender, EventArgs e)
        {
            clock = clock == false ? true : false;          // toogle clock state
            this.timer1.Enabled = clock;
        }

        private void scrollButton_Click(object sender, EventArgs e)
        {
            scroll = scroll == false ? true : false;        // toggle scroll state
            ledDisplay1.Scroll = scroll;
        }


        private void hideButton_Click(object sender, EventArgs e)
        {
            hide = hide == false ? true : false;            // toggle hide form state

            if (hide)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.TransparencyKey = this.BackColor;
                this.designButton.Hide();
                this.clockButton.Hide();
                this.scrollButton.Hide();
                //this.hideButton.Hide();
                this.label1.Hide();
                this.textBox1.Hide();
            }
            else // show
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TransparencyKey = Color.FromArgb(0, Color.FromArgb(0, 0, 0));
                this.designButton.Show();
                this.clockButton.Show();
                this.scrollButton.Show();
                this.hideButton.Show();
                this.textBox1.Show();
                this.label1.Show();
            }

        }

        // on timer tick, set display text to current time
        private void timer1_Tick(object sender, EventArgs e)
        {
            ledDisplay1.Text = DateTime.Now.ToLongTimeString();
        }

        // on property text changed event, change text in display
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ledDisplay1.Text = textBox1.Text;
        }

        private void ledDisplay1_Click(object sender, EventArgs e)
        {

        }
    }
}
