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
    public partial class Clients : Form
    {
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bSource = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        string id_selected_rows = "0";
        public Clients()
        {
            InitializeComponent();
        }
        MySqlConnection conn;

        private void Form2_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(this.button1, "Удаляет из базы данных информацию о бывшем пациенте.");
            toolTip2.SetToolTip(this.button3, "Добавляет в базу данных информацию о новом пациенте.");
            toolTip3.SetToolTip(this.button2, "Позволяет изменить какую-либо информацию о пациенте.");
            Program.Connection connection = new Program.Connection();
            conn = new MySqlConnection(connection.connStr);
            GetlistClients();

            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;
            dataGridView1.Columns[4].Visible = true;
            dataGridView1.Columns[5].Visible = true;

            dataGridView1.Columns[0].FillWeight = 15;
            dataGridView1.Columns[1].FillWeight = 40;
            dataGridView1.Columns[2].FillWeight = 15;
            dataGridView1.Columns[3].FillWeight = 15;
            dataGridView1.Columns[4].FillWeight = 15;
            dataGridView1.Columns[5].FillWeight = 15;

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = true;
        }
            
        private void button1_Click(object sender, EventArgs e)
        {
            string sql_delete_user = $"DELETE FROM Clients WHERE fio ='{textBox2.Text}'";
            MySqlCommand delete_user = new MySqlCommand(sql_delete_user, conn);
            try
            {
                conn.Open();
                delete_user.ExecuteNonQuery();
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
                reload_list();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string izm = $"UPDATE Clients SET fio='{textBox2.Text}', Complexity='{textBox3.Text}', TreatmentCost='{textBox5.Text}', Doctor='{textBox4.Text}', Age='{textBox1.Text}'  WHERE id='{comboBox1.Text}'";
            MySqlCommand command = new MySqlCommand(izm, conn);
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
            reload_list();
        }
        public bool InsertClients(string Ifio, string IComplexity, int ITreatmentCost, string IDoctor, int IAge)
        {
            int InsertCount = 0;
            bool result = false;
            conn.Open();
            string query = $"INSERT INTO Clients (fio, Complexity, TreatmentCost, Doctor, Age) VALUES ('{Ifio}', '{IComplexity}', '{ITreatmentCost}', '{IDoctor}', '{IAge}')";
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" | textBox2.Text == "" | textBox3.Text == "" | textBox4.Text == "" | textBox5.Text == "")
            {
                MessageBox.Show("Введены не все данные");
            }
            else
            {
                string fio = textBox2.Text;
                string Complexity = textBox3.Text;
                int TreatmentCost = Convert.ToInt32(textBox5.Text);
                string Doctor = textBox4.Text;
                int Age = Convert.ToInt32(textBox1.Text);
                if (InsertClients(fio, Complexity, TreatmentCost, Doctor, Age))
                reload_list();
            }
        }
        public void reload_list()
        {
            table.Clear();
            GetlistClients();
        }
        public void GetlistClients()
        {
            string commandStr = "SELECT id as 'id', fio AS 'ФИО', Complexity AS 'Стадия', TreatmentCost AS 'Стоимость лечения', Doctor AS 'Доктор', Age AS 'Возраст' FROM Clients";
            conn.Open();
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MyDA.Fill(table);
            bSource.DataSource = table;
            dataGridView1.DataSource = bSource;
            conn.Close();
            int count_rows = dataGridView1.RowCount - 1;
            ComboClients();
        }
        private void ComboClients()
        {
            comboBox1.Items.Clear();
            string drugs = $"SELECT id from Clients";
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
    }
}
    

