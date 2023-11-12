using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;


namespace Folha_Fácil
{
    public partial class CadastrarColaborador : Form
    {
        public CadastrarColaborador()
        {
            InitializeComponent();
            CarregarDepartamentos();
            CarregarCargos();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            menu proximaTela = new menu();
            proximaTela.Show(this);
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
 
                    SqlConnection cnt = new SqlConnection(@"Data Source=LUCAS; integrated security=SSPI; initial Catalog=FOLHAFACIL");
                    SqlCommand cmd = new SqlCommand();
                    {
                        cnt.Open();
                        string query = "INSERT INTO Funcionario ( NomeCompleto, DataNascimento, RG, CPF, CEP, Endereco,EstadoCivil, Telefone, Email, DataAdmissao, Status, CargoID, DepartamentoID) VALUES (@NomeCompleto, @DataNascimento, @RG, @CPF, @CEP, @Endereco, @EstadoCivil, @Telefone, @Email, @DataAdmissao, 'Ativo', @CargoID, @DepartamentoID)";
                        SqlCommand command = new SqlCommand(query, cnt);

                        command.Parameters.AddWithValue("@NomeCompleto", txtNomeCompleto.Text);
                        command.Parameters.AddWithValue("@DataNascimento", dteNascimento.Value);
                        command.Parameters.AddWithValue("@RG", txtRG.Text);
                        command.Parameters.AddWithValue("@CPF", txtCPF.Text);
                        command.Parameters.AddWithValue("@CEP", txtCEP.Text);
                        command.Parameters.AddWithValue("@Endereco", txtEndereco.Text);
                        command.Parameters.AddWithValue("@EstadoCivil", txtEstadoCivil.Text);
                        command.Parameters.AddWithValue("@Telefone", txtTelefone.Text);
                        command.Parameters.AddWithValue("@Email", textEmail.Text);
                        command.Parameters.AddWithValue("@DataAdmissao", dateTimePicker1.Value); 
                        command.Parameters.AddWithValue("@CargoID", comboBox2.SelectedValue);
                        command.Parameters.AddWithValue("@DepartamentoID", comboBox1.SelectedValue);

                        command.ExecuteNonQuery();


         

                        AbrirMenuPrincipal();
                     }
                        MessageBox.Show("Dados enviados com sucesso para o banco de dados.");
                }
          
        private void AbrirMenuPrincipal()
        {
            menu abriMenu = new menu();
            abriMenu.Show(this);
            this.Hide();
        }
        private void CarregarDepartamentos()
        {
            using (SqlConnection cnt = new SqlConnection(@"Data Source=LUCAS; integrated security=SSPI; initial Catalog=FolhaFacil"))
            {
                string query = "SELECT DepartamentoID, NomeDepartamento FROM Departamento";
                SqlCommand cmd = new SqlCommand(query, cnt);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Insere um item no início com uma instrução para seleção
                DataRow newRow = dt.NewRow();
                newRow["DepartamentoID"] = 0;
                newRow["NomeDepartamento"] = "Selecione um Departamento";
                dt.Rows.InsertAt(newRow, 0);

                comboBox1.DisplayMember = "NomeDepartamento";
                comboBox1.ValueMember = "DepartamentoID";
                comboBox1.DataSource = dt;

                // Definir o item selecionado para o item de instrução
                comboBox1.SelectedIndex = 0;
            }
        }

        private void CarregarCargos()
        {
            using (SqlConnection cnt = new SqlConnection(@"Data Source=LUCAS; integrated security=SSPI; initial Catalog=FolhaFacil"))
            {
                string query = "SELECT CargoID, NomeCargo FROM Cargo";
                SqlCommand cmd = new SqlCommand(query, cnt);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Insere um item no início com uma instrução para seleção
                DataRow newRow = dt.NewRow();
                newRow["CargoID"] = 0;
                newRow["NomeCargo"] = "Selecione um Cargo";
                dt.Rows.InsertAt(newRow, 0);

                comboBox2.DisplayMember = "NomeCargo";
                comboBox2.ValueMember = "CargoID";
                comboBox2.DataSource = dt;

                // Definir o item selecionado para o item de instrução
                comboBox2.SelectedIndex = 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}



