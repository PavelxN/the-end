using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;

namespace diplom
{
    public partial class Items : Form
    {
        static string index_selected_rows;
        static string id_selected_rows;
        public class Database
        {
            //Переменная соединения

            MySqlConnection conn = new MySqlConnection("server=chuc.caseum.ru;port=33333;user=st_1_18_19;database=is_1_18_st19_VKR;password=123123123;");
            //string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_18_19;database=is_1_18_st19_VKR;password=123123123;";

            DataTable dt = new DataTable();
            BindingSource bs = new BindingSource();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            private MySqlDataAdapter MyDA = new MySqlDataAdapter();
            //Объявление BindingSource, основная его задача, это обеспечить унифицированный доступ к источнику данных.
            private BindingSource bSource = new BindingSource();
            private DataSet ds = new DataSet();
            //Представляет одну таблицу данных в памяти.
            public DataTable table = new DataTable();


            public void GetListStaff(BindingSource bs1, DataGridView dg1)
            {

                //Запрос для вывода строк в БД
                string commandStr = "SELECT id AS 'ID', title 'Наименование', price 'Цена', date Дата_приобретения , invent_number 'Инвент.номер', information 'Описание' FROM items";
                //Открываем соединение
                conn.Open();
                //Объявляем команду, которая выполнить запрос в соединении conn
                MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
                //Заполняем таблицу записями из БД
                MyDA.Fill(table);
                //Указываем, что источником данных в bindingsource является заполненная выше таблица
                bs1.DataSource = table;
                //Указываем, что источником данных ДатаГрида является bindingsource
                dg1.DataSource = bs1;
                //Закрываем соединение
                conn.Close();
            }

            public void reload(BindingSource bs1, DataGridView dg1)
            {
                //Чистим виртуальную таблицу
                table.Clear();
                //Вызываем метод получения записей, который вновь заполнит таблицу
                Database DBO = new Database();
                DBO.GetListStaff(bs1, dg1);
            }
            public void GetCurrentID(DataGridView dataGridView1)
            {
                index_selected_rows = dataGridView1.SelectedCells[0].RowIndex.ToString();
                // MessageBox.Show("Индекс выбранной строки" + index_selected_rows);
                id_selected_rows = dataGridView1.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
                // MessageBox.Show("Содержимое поля Код, в выбранной строке" + id_selected_rows);
                class_edit_user.id = id_selected_rows;
            }
            public void DeleteEmpl(int id)
            {
                string del = "DELETE FROM items WHERE id = " + id;
                MySqlCommand del_empl = new MySqlCommand(del, conn);

                try
                {
                    conn.Open();
                    del_empl.ExecuteNonQuery();
                    MessageBox.Show("Удалено!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления пользователя \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
            public void Insert(TextBox textBox1, TextBox textBox2, DateTimePicker dateTimePicker1, TextBox textBox3, RichTextBox richTextBox1)
            {
                string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_18_19;database=is_1_18_st19_VKR;password=123123123;";

                MySqlConnection conn = new MySqlConnection(connStr);
                //Получение новых параметров пользователя
                string new_title = textBox1.Text;
                string new_price = textBox2.Text;
                string new_date = dateTimePicker1.Text;
                string new_invent_number = textBox3.Text;
                string new_information = richTextBox1.Text;

                if (textBox1.Text.Length > 0)
                {
                    //Формируем строку запроса на добавление строк
                    string sql_insert_clothes = " INSERT INTO  `items` (title, price,date,invent_number,information) " +
                        "VALUES ('" + new_title + "','" + new_price + "','" + new_date + "','" + new_invent_number + "','" + new_information + "')";


                    //Посылаем запрос на добавление данных
                    MySqlCommand insert_clothes = new MySqlCommand(sql_insert_clothes, conn);
                    try
                    {
                        conn.Open();
                        insert_clothes.ExecuteNonQuery();
                        MessageBox.Show("Успешно добавлено", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка добавления \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка!", "Информация");
                }
            }
            public void Update(TextBox textBox1, TextBox textBox2, DateTimePicker dateTimePicker1, TextBox textBox3, RichTextBox richTextBox1)
            {
                //Получаем ID пользователя
                string id = class_edit_user.id;
                string SQL_izm = "UPDATE items SET title=N'" + textBox1.Text + "', price=N'" + textBox2.Text+ "',  date=N'"+ dateTimePicker1.Text+"', invent_number=N'"+ textBox3.Text +"', information=N'"+ richTextBox1.Text +"' where id=" + id;
                MessageBox.Show(SQL_izm);
                MySqlConnection conn = new MySqlConnection("server=chuc.caseum.ru;port=33333;user=st_1_18_19;database=is_1_18_st19_VKR;password=123123123;");
                conn.Open();
                MySqlCommand command1 = new MySqlCommand(SQL_izm, conn);
                MySqlDataReader dr = command1.ExecuteReader();
                dr.Close();
                conn.Close();
                MessageBox.Show("Данные изменены");
                //this.Activate();
                textBox1.Text = "";
                textBox2.Text = "";
                dateTimePicker1.Text = "";
                textBox3.Text = "";
                richTextBox1.Text = "";
            }
        }
            public Items()
            {
            InitializeComponent();
            }

        private void Items_Load(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.GetListStaff(bindingSource1, dataGridView1);


            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;
            dataGridView1.Columns[4].Visible = true;
            dataGridView1.Columns[5].Visible = false;

            //Ширина полей
            dataGridView1.Columns[0].FillWeight = 3;
            dataGridView1.Columns[1].FillWeight = 12;
            dataGridView1.Columns[2].FillWeight = 3;
            dataGridView1.Columns[3].FillWeight = 8;
            dataGridView1.Columns[4].FillWeight = 5;
            //dataGridView1.Columns[5].FillWeight = 10;
            //dataGridView1.Columns[6].FillWeight = 10;


            //Растягивание полей грида
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            //Убираем заголовки строк
            dataGridView1.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView1.ColumnHeadersVisible = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.Insert(textBox1, textBox2, dateTimePicker1,textBox3, richTextBox1);
            DBO.reload(bindingSource1, dataGridView1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string fromDGtoTB = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox1.Text =
                dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox2.Text =
                dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            dateTimePicker1.Text =
               dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox3.Text =
                dataGridView1[4, dataGridView1.CurrentRow.Index].Value.ToString();
            richTextBox1.Text =
                dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.GetCurrentID(dataGridView1);
            DBO.Update(textBox1, textBox2, dateTimePicker1, textBox3, richTextBox1);
            DBO.reload(bindingSource1, dataGridView1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.GetCurrentID(dataGridView1);
            DBO.DeleteEmpl(Convert.ToInt32(id_selected_rows));
            DBO.reload(bindingSource1, dataGridView1);
        }

        private void export_Click(object sender, EventArgs e)
        {
            Excel.Application exapp = new Excel.Application();
            exapp.Workbooks.Add();
            Excel.Worksheet wsh = (Excel.Worksheet)exapp.ActiveSheet;
            wsh.Columns[3].ColumnWidth = 35;
            wsh.Columns[5].ColumnWidth = 15;
            wsh.Columns[6].ColumnWidth = 15;
            wsh.Columns[7].ColumnWidth = 170;

            int i, j;
            for (i = 0; i <= dataGridView1.RowCount - 2; i++)
            {
                for (j = 0; j <= dataGridView1.ColumnCount - 1; j++)
                {
                    wsh.Cells[1, j + 2] = dataGridView1.Columns[j].HeaderText.ToString();
                    wsh.Cells[i + 2, j + 2] = dataGridView1[j, i].Value.ToString();
                }
            }
            exapp.Visible = true;
        }
    }
}
