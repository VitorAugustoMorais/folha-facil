using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Folha_Fácil
{
    public partial class ProcurarColaborador : Form
    {
        private const string PlaceholderText = "Insira o CPF do Colaborador";
        private bool _placeholderActive = true;
        private bool colaboradorEncontrado = false;
        private string cpfColaboradorEncontrado;
        public ProcurarColaborador()
        {
            InitializeComponent();
            SetPlaceholder();
            txtCpfPesquisa.KeyPress += new KeyPressEventHandler(txtCpfPesquisa_KeyPress);
            txtCpfPesquisa.KeyDown += new KeyEventHandler(txtCpfPesquisa_KeyDown);
            txtCpfPesquisa.Click += new EventHandler(txtCpfPesquisa_Click);
            pictureBoxLupa.Click += new EventHandler(pictureBoxLupa_Click);
        }

        //CONFIGURANDO PLACEHOLDER + TEXTBOXCPF
        private void SetPlaceholder()
        {
            txtCpfPesquisa.Text = PlaceholderText;
            txtCpfPesquisa.ForeColor = Color.Gray;
            _placeholderActive = true;
        }

        private void txtCpfPesquisa_Enter(object sender, EventArgs e)
        {
            if (_placeholderActive)
            {
                txtCpfPesquisa.Text = "";
                txtCpfPesquisa.ForeColor = Color.Black;
                _placeholderActive = false;
            }
        }
        private void txtCpfPesquisa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_placeholderActive && e.KeyChar != (char)Keys.Back)
            {
                _placeholderActive = false;
                txtCpfPesquisa.Text = "";
                txtCpfPesquisa.ForeColor = Color.Black;
                e.Handled = false;
            }
        }
        private void txtCpfPesquisa_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCpfPesquisa.Text))
            {
                SetPlaceholder();
            }
        }
        private void txtCpfPesquisa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                txtCpfPesquisa.Text = "";
                SetPlaceholder(); 
                colaboradorEncontrado = false; 
                LimparCamposDetalhes(); 
                cpfColaboradorEncontrado = ""; 
                e.SuppressKeyPress = true; 
            }
            else if (e.KeyCode == Keys.Enter)
            {
                RealizarPesquisa();
                e.SuppressKeyPress = true; 
            }
        }
        private void RemovePlaceholder()
        {
            if (_placeholderActive)
            {
                txtCpfPesquisa.Text = "";
                txtCpfPesquisa.ForeColor = Color.Black;
                _placeholderActive = false;
            }
        }
        private void txtCpfPesquisa_Click(object sender, EventArgs e)
        {
            RemovePlaceholder();
        }

        private void pictureBoxLupa_Click(object sender, EventArgs e)
        {
            RealizarPesquisa();
        }

        //PESQUISA POR CPF
        private void RealizarPesquisa()
        {
            string cpf = txtCpfPesquisa.Text.Trim();
            colaboradorEncontrado = false;

            if (!string.IsNullOrWhiteSpace(cpf))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(@"Data Source=LUCAS; integrated security=SSPI; initial Catalog=FOLHAFACIL"))
                    {
                        connection.Open();
                        string query = @"
                    SELECT 
                        f.NomeCompleto, 
                        d.NomeDepartamento, 
                        c.NomeCargo, 
                        f.Telefone, 
                        f.Email, 
                        f.Endereco, 
                        f.EstadoCivil, 
                        f.CEP 
                    FROM Funcionario f
                    INNER JOIN Departamento d ON f.DepartamentoID = d.DepartamentoID
                    INNER JOIN Cargo c ON f.CargoID = c.CargoID
                    WHERE f.CPF = @CPF";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CPF", cpf);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    txtNomeCompleto.Text = reader["NomeCompleto"].ToString();
                                    txtDepartamento.Text = reader["NomeDepartamento"].ToString();
                                    txtCargo.Text = reader["NomeCargo"].ToString();
                                    txtTelefone.Text = reader["Telefone"].ToString();
                                    txtEmail.Text = reader["Email"].ToString();
                                    txtEndereco.Text = reader["Endereco"].ToString();
                                    txtEstadoCivil.Text = reader["EstadoCivil"].ToString();
                                    txtCEP.Text = reader["CEP"].ToString();
                                    colaboradorEncontrado = true;
                                    cpfColaboradorEncontrado = cpf;
                                }
                                else
                                {
                                    MessageBox.Show("Colaborador não encontrado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LimparCamposDetalhes();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) // Captura qualquer tipo de exceção
                {
                    MessageBox.Show("Erro ao acessar os dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, insira o CPF do colaborador para a pesquisa.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetPlaceholder();
                txtCpfPesquisa.Text = "";
            }
        }

        private void LimparCamposDetalhes()
        {
            txtNomeCompleto.Text = "";
            txtDepartamento.Text = "";
            txtCargo.Text = "";
            txtTelefone.Text = "";
            txtEmail.Text = "";
            txtEndereco.Text = "";
            txtEstadoCivil.Text = "";
            txtCEP.Text = "";
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (colaboradorEncontrado)
            {
                EditarColaborador formEditar = new EditarColaborador();
                formEditar.CarregarDados(cpfColaboradorEncontrado); // Use a variável armazenada
                this.Hide();
                var result = formEditar.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RealizarPesquisa();
                }
            }
            else
            {
                MessageBox.Show("Por favor, realize uma pesquisa e selecione um colaborador para editar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (colaboradorEncontrado && !string.IsNullOrWhiteSpace(txtNomeCompleto.Text))
            {
                if (MessageBox.Show($"ATENÇÃO: Você está prestes a excluir o colaborador: {txtNomeCompleto.Text}.\nEsta ação não pode ser desfeita.\nDeseja continuar?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(@"Data Source=LUCAS; integrated security=SSPI; initial Catalog=FOLHAFACIL"))
                        {
                            connection.Open();
                            string query = "DELETE FROM Funcionario WHERE CPF = @CPF";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@CPF", txtCpfPesquisa.Text);

                                int result = command.ExecuteNonQuery();
                                if (result > 0)
                                {
                                    MessageBox.Show("Colaborador excluído com sucesso.", "Excluído", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LimparCamposDetalhes();
                                }
                                else
                                {
                                    MessageBox.Show("Erro ao excluir o colaborador.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao acessar os dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, realize uma pesquisa e selecione um colaborador para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
  
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            menu proximaTela = new menu();
            proximaTela.Show(this);
            this.Hide();
        }

        private void ProcurarColaborador_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
