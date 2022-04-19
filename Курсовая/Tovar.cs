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
    public partial class Tovar : Form
    {
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bSource = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        public Tovar()
        {
            InitializeComponent();
        }
        MySqlConnection conn;
        private void Form3_Load(object sender, EventArgs e)
        {
            Program.Connection connection = new Program.Connection();
            conn = new MySqlConnection(connection.connStr);
            GetListDrugs();

            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;

            dataGridView1.Columns[0].FillWeight = 25;
            dataGridView1.Columns[1].FillWeight = 25;
            dataGridView1.Columns[2].FillWeight = 25;
            dataGridView1.Columns[3].FillWeight = 25;

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = true;
        }
        public bool InsertDrugs(string IName, string IRestrictions, int IQuantity)
        {
            int InsertCount = 0;
            bool result = false;
            conn.Open();
            string query = $"INSERT INTO Drugs (Name, Restrictions, Quantity) VALUES ('{IName}', '{IRestrictions}', '{IQuantity}')";
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
        public void GetListDrugs()
        {
            string commandStr = "SELECT id AS 'id', Name AS 'Название препарата', Restrictions AS 'Противопоказания', Quantity AS 'Количество' FROM Drugs";
            conn.Open();
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MyDA.Fill(table);
            bSource.DataSource = table;
            dataGridView1.DataSource = bSource;
            conn.Close();
            ComboDrugs();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string Name = textBox1.Text;
            int Quantity = Convert.ToInt32(textBox3.Text);
            string Restriction = textBox2.Text;
            InsertDrugs(Name, Restriction, Quantity);
            reload_list();
               
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string izm = $"UPDATE Drugs SET Quantity='{textBox3.Text}', Name ='{textBox1.Text}', Restrictions ='{textBox2.Text}'  WHERE id='{comboBox1.Text}'";
            MySqlCommand command = new MySqlCommand(izm, conn);
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
            reload_list();
        }
        public void reload_list()
        {
            table.Clear();
            GetListDrugs();
        }
        public void ComboDrugs()
        {
            comboBox1.Items.Clear();
            string drugs = $"SELECT id from Drugs";
            MySqlCommand com = new MySqlCommand(drugs, conn);
            conn.Open();
            MySqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
            }
            conn.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string comm = $"delete from Drugs where Name='{textBox1.Text}'";
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

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            reload_list();
        }
    }   
}
