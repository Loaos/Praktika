using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
    public partial class AdminPanel : Form
    {
        DB database = new DB();
        public AdminPanel()
        {
            InitializeComponent();
            database.openConnection();
        }
        public void Upload(PictureBox pictureBox)
        {
            if (pictureBox.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    pictureBox.Image.Save(memoryStream, ImageFormat.Jpeg);

                    using (SqlCommand command = new SqlCommand("INSERT INTO Блюда (Картинка) VALUES (@Картинка)", database.getSqlConnection()))
                    {
                        command.Parameters.AddWithValue("@Картинка", memoryStream.ToArray());
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Upload(pictureBox1);
            using (SqlCommand command = new SqlCommand($"INSERT INTO [Блюда] (Название, Описание, Рецепт, Картинка, Калорийность_ингредиентов, Вес_ингредиентов) values (@Название, @Описание, @Рецепт, @Картинка, @Калорийность_ингредиентов, @Вес_ингредиентов)", database.getSqlConnection()))
            {
                command.Parameters.AddWithValue("@Название", textBox1.Text);
                command.Parameters.AddWithValue("@Описание", richTextBox2.Text);
                command.Parameters.AddWithValue("@Рецепт", richTextBox1.Text);
                command.Parameters.AddWithValue("@Калорийность_ингредиентов", richTextBox3.Text);
                command.Parameters.AddWithValue("@Вес_ингредиентов", richTextBox4.Text);

                if (pictureBox1.Image != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        pictureBox1.Image.Save(memoryStream, ImageFormat.Jpeg);
                        command.Parameters.AddWithValue("@Картинка", memoryStream.ToArray());
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@Картинка", DBNull.Value);
                }

                MessageBox.Show(command.ExecuteNonQuery().ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("DELETE FROM Блюда WHERE id = @id", database.getSqlConnection());
            command.Parameters.AddWithValue("@id", textBox10.Text);
            MessageBox.Show(command.ExecuteNonQuery().ToString());
        }

        private void AdminPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Authorization openform = new Authorization();
            openform.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Authorization openform = new Authorization();
            openform.Show();
            this.Hide();
        }
    }
}