using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class UserForm : Form
    {
        DB database = new DB();
        private SqlDataAdapter adapter1 = new SqlDataAdapter();
        private DataTable table1 = new DataTable();
        public UserForm()
        {
            InitializeComponent();
            database.openConnection();
            LoadRecipesIntoComboBox1();
        }
        private void LoadRecipesIntoComboBox1()
        {
            using (SqlCommand command = new SqlCommand($"SELECT Название FROM Блюда", database.getSqlConnection()))
            {
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["Название"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке рецептов: " + ex.Message);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = comboBox1.SelectedItem.ToString();

            try
            {
                SqlCommand command = new SqlCommand("SELECT Описание FROM Блюда WHERE Название = @Name", database.getSqlConnection());
                command.Parameters.AddWithValue("@Name", selectedName);
                table1.Clear();
                adapter1.SelectCommand = command;
                adapter1.Fill(table1);
                richTextBox1.Text = table1.Rows[0]["Описание"].ToString();

                SqlCommand command1 = new SqlCommand("SELECT Картинка FROM Блюда WHERE Название = @Name", database.getSqlConnection());
                command1.Parameters.AddWithValue("@Name", selectedName);
                using (var reader = command1.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var imageData = (byte[])reader["Картинка"];
                        using (var memoryStream = new MemoryStream(imageData))
                        {
                            pictureBox1.Image = Image.FromStream(memoryStream);
                        }
                    }
                }

                SqlCommand command2 = new SqlCommand("SELECT Рецепт FROM Блюда WHERE Название = @Name", database.getSqlConnection());
                command2.Parameters.AddWithValue("@Name", selectedName);
                table1.Clear();
                adapter1.SelectCommand = command2;
                adapter1.Fill(table1);
                richTextBox2.Text = table1.Rows[0]["Рецепт"].ToString();


                //SqlCommand command3 = new SqlCommand("SELECT Вес_ингредиентов FROM Блюда WHERE Название = @Name", database.getSqlConnection());
                //command3.Parameters.AddWithValue("@Name", selectedName);
                //table1.Clear();
                //adapter1.SelectCommand = command3;
                //adapter1.Fill(table1);
                //richTextBox3.Text = table1.Rows[0]["Вес_ингредиентов"].ToString();


                SqlCommand command4 = new SqlCommand("SELECT Калорийность_ингредиентов, Вес_ингредиентов FROM Блюда WHERE Название = @Name", database.getSqlConnection());
                command4.Parameters.AddWithValue("@Name", selectedName);
                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(command4))
                {
                    da.Fill(dt);
                }
                table1.Clear();
                table1.Rows.Add(dt.Rows[0].ItemArray);

                if (dt.Rows.Count > 0)
                {
                    float calor = Convert.ToSingle(dt.Rows[0]["Калорийность_ингредиентов"]);
                    float weight = Convert.ToSingle(dt.Rows[0]["Вес_ингредиентов"]);

                    if (calor != 0)
                    {
                        float gramcal = calor / weight;
                        float calor1por = gramcal * 136;
                        float calor100gr = gramcal * 100;

                        richTextBox4.Text = calor1por.ToString();
                        richTextBox3.Text = calor100gr.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке рецептов: " + ex.Message);
            }

        }
        

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainPage back = new MainPage();
            back.Show();
            this.Hide();
        }
    }
}