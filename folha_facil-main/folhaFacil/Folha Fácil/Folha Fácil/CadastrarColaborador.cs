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

namespace Folha_Fácil
{
    public partial class CadastrarColaborador : Form
    {
        public CadastrarColaborador()
        {
            InitializeComponent();
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
            SqlConnection cnt = new SqlConnection(@"Data Source=DESKTOP-UU3KD7O\SQLEXPRESS; integrated security=SSPI; initial Catalog=FolhaFacil");
            SqlCommand cmd = new SqlCommand();
            {
                cnt.Open();
                string query = "INSERT INTO Funcionario ( NomeCompleto, DataNascimento, RG, CPF, CTPS, CEP, Endereco,EstadoCivil, Telefone, Email, DataAdmissao) VALUES (@NomeCompleto, @DataNascimento, @RG, @CPF, @CTPS, @CEP, @Endereco, @EstadoCivil, @Telefone, @Email, @DataAdmissao)";
                SqlCommand command = new SqlCommand(query, cnt);
                
                command.Parameters.AddWithValue("@NomeCompleto", txtNomeCompleto.Text);
                command.Parameters.AddWithValue("@DataNascimento", dteNascimento.Value);
                command.Parameters.AddWithValue("@RG", txtRG.Text);
                command.Parameters.AddWithValue("@CPF", txtCPF.Text);
                command.Parameters.AddWithValue("@CTPS", txtCTPS.Text);
                command.Parameters.AddWithValue("@CEP", txtCEP.Text);
                command.Parameters.AddWithValue("@Endereco", txtEndereco.Text);
                command.Parameters.AddWithValue("@EstadoCivil", txtEstadoCivil.Text);
                command.Parameters.AddWithValue("@Telefone", txtTelefone.Text);
                command.Parameters.AddWithValue("@Email", textEmail.Text);
                command.Parameters.AddWithValue("@DataAdmissao", dateTimePicker1.Value);
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
    }
    }

