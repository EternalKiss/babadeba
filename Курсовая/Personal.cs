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
    public partial class Personal : Form
    {
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bSource = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        public Personal()
        {
            InitializeComponent();
        }
        MySqlConnection conn;

        private void Form4_Load(object sender, EventArgs e)
        {
            Program.Connection connection = new Program.Connection();
            conn = new MySqlConnection(connection.connStr);
            GetlistPersonal();

            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;
            dataGridView1.Columns[4].Visible = true;

            dataGridView1.Columns[0].FillWeight = 15;
            dataGridView1.Columns[1].FillWeight = 40;
            dataGridView1.Columns[2].FillWeight = 15;
            dataGridView1.Columns[3].FillWeight = 15;
            dataGridView1.Columns[4].FillWeight = 15;

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = true;

            ChangeCOLOR();
        }

        public bool InsertPersonal(string Ifio, int Iage, string IPosition, int Izarabatok)
        {
            int InsertCount = 0;
            bool result = false;
            conn.Open();
            string query = $"INSERT INTO Personal (fio, Age, Position, Zarabatok) VALUES ('{Ifio}', '{Iage}', '{IPosition}', '{Izarabatok}')";
            try
            {
                MySqlCommand command = new MySqlCommand(query, conn);
                InsertCount = command.ExecuteNonQuery();
            }
            catch
            {
                InsertCount = 0;
            }
            finally
            {
                {
                    conn.Close();
                    if (InsertCount != 0)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public void GetlistPersonal()
        {
            string commandStr = "SELECT id AS 'ID', fio AS 'ФИО', Age AS 'Возраст', Position AS 'Должность', Zarabatok AS 'Зарплата' FROM Personal";
            conn.Open();
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MyDA.Fill(table);
            bSource.DataSource = table;
            dataGridView1.DataSource = bSource;
            conn.Close();
            int count_rows = dataGridView1.RowCount - 1;
            ComboPersonale();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" | textBox2.Text == "" | textBox3.Text == "" | textBox4.Text == "")
            {
                MessageBox.Show("Не все данные введены.");
            }
            else
            {
                string Fio = textBox1.Text;
                int Age = Convert.ToInt32(textBox2.Text);
                string Position = textBox3.Text;
                int Zarabatok = Convert.ToInt32(textBox4.Text);
                InsertPersonal(Fio, Age, Position, Zarabatok);
                reload_list();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string comm = $"delete from Personal where fio='{textBox1.Text}'";
            MySqlCommand com = new MySqlCommand(comm, conn);
            try
            {
                conn.Open();
                com.ExecuteNonQuery();
                MessageBox.Show("Удаление прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления строки \n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
            }
            reload_list();
        }
        public void reload_list()
        {
            table.Clear();
            GetlistPersonal();
            ChangeCOLOR();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" | textBox2.Text == "" | textBox3.Text == "" | textBox4.Text == "")
            {
                MessageBox.Show("Введены не все данные");
            }
            else
            {
                string izm = $"UPDATE Personal SET Age='{textBox2.Text}', fio ='{textBox1.Text}', Position ='{textBox4.Text}', Zarabatok='{textBox3.Text}'  WHERE id='{comboBox1.Text}'";
                MySqlCommand command = new MySqlCommand(izm, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                reload_list();
            }
        }
        private void ComboPersonale()
        {
            comboBox1.Items.Clear();
            string drugs = $"SELECT id from Personal";
            MySqlCommand com = new MySqlCommand(drugs, conn);
            conn.Open();
            MySqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
            }
            conn.Close();
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            reload_list();
        }
        private void ChangeCOLOR()
        {
            int count_rows = dataGridView1.RowCount - 1;
            for (int i = 0; i < count_rows; i++)
            {
                string status = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value);
                if (status == "Врач")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
                if (status == "Главврач")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
                if (status == "Уборщик")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.HotPink;
                }
                if (status == "В отпуске")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
                if (status == "Медбрат")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Blue;
                }
            }
        }
    }
}