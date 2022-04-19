using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Курсовая
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }
        MySqlConnection conn;
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            CloseButton.ForeColor = Color.Red;
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            CloseButton.ForeColor = Color.White;
        }
        Point lastPoint;
        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void Loggin_Click(object sender, EventArgs e)
        {
            string loginUser = Logg.Text;
            string passuser = Pass.Text;

            Program.Connection connection = new Program.Connection();
            conn = new MySqlConnection(connection.connStr);

            conn.Open();
            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand($"Select logg AS 'login', Pass AS 'password' from Avtorizacia WHERE logg = '{Logg.Text}' AND Pass = '{Pass.Text}'", conn);
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                listBox1.Items.Add(reader[0].ToString());
            }
            conn.Close();
            if (listBox1.Items.Count > 0)
            {
                MessageBox.Show("Авторизация прошла успешно.");
                Menu menu = new Menu();
                this.Hide();
                menu.ShowDialog();
                this.Show();
            }

            else
            {
                MessageBox.Show("Неверный логин и/или пароль.");
            }
        }
    }
}
