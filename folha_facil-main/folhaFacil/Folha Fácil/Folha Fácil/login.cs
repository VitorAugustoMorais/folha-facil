using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Imaging;

namespace Folha_Fácil
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // ESTABELECENDO CONEXÃO COM O BANCO DE DADOS SQL SERVER
        SqlConnection cnt = new SqlConnection(@"Data Source=LUCAS; integrated security=SSPI; initial Catalog=FOLHAFACIL");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader leitura;

        private void btnVerSenha_MouseDown(object sender, MouseEventArgs e)
        {
            txtSenha.UseSystemPasswordChar = false;
        }

        private void btnVerSenha_MouseUp(object sender, MouseEventArgs e)
        {
            txtSenha.UseSystemPasswordChar = true;
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if(txtUsuario.Text == "" & txtSenha.Text == "")
            {
                MessageBox.Show("Preencha os campos usuario e senha.", "Atenção!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txtUsuario.Text == "")
            {
                MessageBox.Show("Preencha o campo usuario.", "Atenção!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txtSenha.Text == "")
            {
                MessageBox.Show("Preencha o campo senha.", "Atenção!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    cnt.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM UsuarioLogin WHERE Usuario = @Usuario AND Senha = @Senha", cnt))
                    {
                        cmd.Parameters.AddWithValue("@Usuario", txtUsuario.Text);
                        cmd.Parameters.AddWithValue("@Senha", txtSenha.Text);
                        using (SqlDataReader leitura = cmd.ExecuteReader())
                        {

                            if (leitura.HasRows)
                            {
                                menu abriMenu = new menu();
                                abriMenu.Show(this);
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Usuario ou senha invalido.", "Acesso negado!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtUsuario.Clear();
                                txtSenha.Clear();
                                txtUsuario.Focus();
                            }
                        }
                    }
                }

                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    cnt.Close();
                }
                finally
                {
                    cnt.Close();
                }
            }      
       }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void login_Load(object sender, EventArgs e)
        {

        }
    }
}
