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

namespace Folha_Fácil
{
    public partial class EditarColaborador : Form
    {
        private string cpfDoColaborador;

        private string originalNomeCompleto;
        private int originalDepartamentoId;
        private int originalCargoId;
        private string originalTelefone;
        private string originalEmail;
        private string originalEndereco;
        private string originalEstadoCivil;
        private string originalCep;
        public EditarColaborador()
        {
            InitializeComponent();
            CarregarDadosDoComboBox();
            txtNomeCompleto.ReadOnly = true; // DEFININDO TEXTBOX NOME COMPLETO APENAS COMO LEITURA.
        } 

        private string connectionString = @"Data Source=LUCAS; integrated security=SSPI; initial Catalog=FOLHAFACIL";
        private void CarregarDadosDoComboBox()
        {
            CarregarDepartamentos();
            CarregarCargos();
        }

        // CONFIGURAÇÃO DOS COMBOBOX - DEPARTAMENTO/CARGO
        private void CarregarDepartamentos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT DepartamentoID, NomeDepartamento FROM Departamento";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comboBoxDepartamento.DataSource = dt;
                comboBoxDepartamento.ValueMember = "DepartamentoID";
                comboBoxDepartamento.DisplayMember = "NomeDepartamento";
            }
        }
        private void CarregarCargos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT CargoID, NomeCargo FROM Cargo";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comboBoxCargo.DataSource = dt;
                comboBoxCargo.ValueMember = "CargoID";
                comboBoxCargo.DisplayMember = "NomeCargo";
            }
        }

        // CARREGA OS DADOS DO COLABORADOR PESQUISADOS PARA OUTRO FORMULÁRIO.
        public void CarregarDados(string cpf)
        {
            cpfDoColaborador = cpf;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    f.NomeCompleto, 
                    d.DepartamentoID,
                    c.CargoID, 
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
                                comboBoxDepartamento.SelectedValue = reader.GetInt32(reader.GetOrdinal("DepartamentoID"));
                                comboBoxCargo.SelectedValue = reader.GetInt32(reader.GetOrdinal("CargoID"));
                                txtTelefone.Text = reader["Telefone"].ToString();
                                txtEmail.Text = reader["Email"].ToString();
                                txtEndereco.Text = reader["Endereco"].ToString();
                                txtEstadoCivil.Text = reader["EstadoCivil"].ToString();
                                txtCEP.Text = reader["CEP"].ToString();

                                originalNomeCompleto = reader["NomeCompleto"].ToString();
                                originalDepartamentoId = reader.GetInt32(reader.GetOrdinal("DepartamentoID"));
                                originalCargoId = reader.GetInt32(reader.GetOrdinal("CargoID"));
                                originalTelefone = reader["Telefone"].ToString();
                                originalEmail = reader["Email"].ToString();
                                originalEndereco = reader["Endereco"].ToString();
                                originalEstadoCivil = reader["EstadoCivil"].ToString();
                                originalCep = reader["CEP"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Colaborador não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao acessar os dados do colaborador: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // SALVAR DADOS EDITADOS
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Verifique se há alterações antes de tentar salvar
            if (HasChanges())
            {
                // Peça confirmação antes de salvar
                if (MessageBox.Show("Deseja salvar as alterações?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string query = @"
                UPDATE Funcionario 
                SET 
                    DepartamentoID = @DepartamentoID, 
                    CargoID = @CargoID, 
                    Telefone = @Telefone, 
                    Email = @Email, 
                    Endereco = @Endereco, 
                    EstadoCivil = @EstadoCivil, 
                    CEP = @CEP 
                WHERE CPF = @CPF";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // Obter os valores dos controles do formulário
                                int departamentoId = Convert.ToInt32(comboBoxDepartamento.SelectedValue);
                                int cargoId = Convert.ToInt32(comboBoxCargo.SelectedValue);
                                string telefone = txtTelefone.Text;
                                string email = txtEmail.Text;
                                string endereco = txtEndereco.Text;
                                string estadoCivil = txtEstadoCivil.Text;
                                string cep = txtCEP.Text;

                                // Adicionar parâmetros para a consulta SQL
                                command.Parameters.AddWithValue("@DepartamentoID", departamentoId);
                                command.Parameters.AddWithValue("@CargoID", cargoId);
                                command.Parameters.AddWithValue("@Telefone", telefone);
                                command.Parameters.AddWithValue("@Email", email);
                                command.Parameters.AddWithValue("@Endereco", endereco);
                                command.Parameters.AddWithValue("@EstadoCivil", estadoCivil);
                                command.Parameters.AddWithValue("@CEP", cep);
                                command.Parameters.AddWithValue("@CPF", cpfDoColaborador);

                                int result = command.ExecuteNonQuery();
                                if (result > 0)
                                {
                                    MessageBox.Show("As alterações foram salvas com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Nenhuma alteração foi feita.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao salvar as alterações: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Nenhuma alteração foi detectada.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Método para verificar se há alterações
        private bool HasChanges()
        {
            return comboBoxDepartamento.SelectedValue.ToString() != originalDepartamentoId.ToString() ||
           comboBoxCargo.SelectedValue.ToString() != originalCargoId.ToString() ||
           txtTelefone.Text != originalTelefone ||
           txtEmail.Text != originalEmail ||
           txtEndereco.Text != originalEndereco ||
           txtEstadoCivil.Text != originalEstadoCivil ||
           txtCEP.Text != originalCep;
        }

        // CANCELAR EDIÇÃO DOS DADOS
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja descartar as alterações?", "Cancelar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
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
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (this.DialogResult == DialogResult.OK || this.DialogResult == DialogResult.Cancel)
            {
                ProcurarColaborador formProcurar = new ProcurarColaborador();
                formProcurar.Show();
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ProcurarColaborador proximaTela = new ProcurarColaborador();
            proximaTela.Show(this);
            this.Hide();
        }

        private void pictureBoxVoltar_Click(object sender, EventArgs e)
        {
            if (HasChanges())
            {
                if (MessageBox.Show("Você tem alterações não salvas. Deseja voltar mesmo assim?", "Voltar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}