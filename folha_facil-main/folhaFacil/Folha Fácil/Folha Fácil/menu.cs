using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Folha_Fácil
{
    public partial class menu : Form
    {
        public menu()
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            login sairLogin = new login();
            sairLogin.Show(this);
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e) //botao de minimizar
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e) //botao de fechar
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Criar uma nova instância do formulário de destino
            CadastrarColaborador proximaTela = new CadastrarColaborador();
            proximaTela.Show(this);
            this.Hide();

            
        }
    }
}
