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

namespace Forum_PDP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private MySqlConnectionStringBuilder conexaoBanco()
        {
            MySqlConnectionStringBuilder conexaoBD = new MySqlConnectionStringBuilder();
            conexaoBD.Server = "localhost";
            conexaoBD.Database = "howsdog";
            conexaoBD.UserID = "root";
            conexaoBD.Password = "ffm#7276";
            conexaoBD.SslMode = 0;
            return conexaoBD;
        }

        private void limparCampos()
        {
            tbID.Clear();
            tbNome.Clear();
            tbLogradouro.Clear();
            tbNumero.Clear();
            tbNome.Focus();
        }

        private void atualizarDataGrid()
        {
            MySqlBaseConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexaoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexaoBD.Open();

                MySqlCommand comandoMySql = realizaConexaoBD.CreateCommand();
                comandoMySql.CommandText = "select idCliente, nmCliente, concat(logradouro, ', ', enderecoNumero) as endereco, bairro, cidade, CEP from cliente";
                MySqlDataReader reader = comandoMySql.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone(); // Faz um CAST e clona a linha da Tabela 
                    row.Cells[0].Value = reader.GetInt32("idCliente"); //ID
                    row.Cells[1].Value = reader.GetString("nmCliente"); //Nome
                    row.Cells[2].Value = getSafeRead(reader, "endereco"); //Endereço

                    dataGridView1.Rows.Add(row); //Adiciona a linha na tabela
                }
                realizaConexaoBD.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open Connection ! " + ex.Message);
                Console.WriteLine(ex.Message);
            }

        }

        private static string getSafeRead(MySqlDataReader reader, string colname)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(colname)))
                return reader.GetString(colname);
            return string.Empty;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            atualizarDataGrid();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            MySqlBaseConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexaoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexaoBD.Open();

                MySqlCommand comandoMySql = realizaConexaoBD.CreateCommand(); 
                comandoMySql.CommandText = "insert into cliente (nmCliente, logradouro, enderecoNumero) values " +
                    "('" + tbNome.Text + "', '" + tbLogradouro.Text + "', '" + tbNumero.Text + "')";
                comandoMySql.ExecuteNonQuery();
                realizaConexaoBD.Close();
                atualizarDataGrid();
                limparCampos();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível inserir um novo registro! \n" + ex.Message);
            }
        }
    }
}
