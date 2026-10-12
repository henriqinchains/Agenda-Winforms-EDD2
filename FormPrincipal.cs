using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Agenda
{
    // Formulário único, montado em código (não precisa do Designer).
    public class FormPrincipal : Form
    {
        private readonly Contatos contatos = new Contatos();
        private List<Telefone> telefonesAtuais = new List<Telefone>();

        private TextBox txtNome, txtEmail, txtNumero;
        private DateTimePicker dtpNasc;
        private ComboBox cmbTipo;
        private CheckBox chkPrincipal;
        private ListBox lstTelefones;
        private ListView lvContatos;

        public FormPrincipal()
        {
            Text = "Agenda de Contatos";
            ClientSize = new Size(900, 470);
            StartPosition = FormStartPosition.CenterScreen;
            MinimizeBox = true;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            MontarTela();
            AtualizarLista();
        }

        private void MontarTela()
        {
            // ---------- Dados do contato ----------
            GroupBox gbDados = new GroupBox { Text = "Contato", Location = new Point(10, 10), Size = new Size(380, 450) };
            Controls.Add(gbDados);

            gbDados.Controls.Add(new Label { Text = "Nome:", Location = new Point(10, 28), AutoSize = true });
            txtNome = new TextBox { Location = new Point(10, 46), Width = 355 };
            gbDados.Controls.Add(txtNome);

            gbDados.Controls.Add(new Label { Text = "E-mail (identifica o contato):", Location = new Point(10, 78), AutoSize = true });
            txtEmail = new TextBox { Location = new Point(10, 96), Width = 355 };
            gbDados.Controls.Add(txtEmail);

            gbDados.Controls.Add(new Label { Text = "Data de nascimento:", Location = new Point(10, 128), AutoSize = true });
            dtpNasc = new DateTimePicker
            {
                Location = new Point(10, 146),
                Width = 140,
                Format = DateTimePickerFormat.Short,
                MaxDate = DateTime.Today,
                MinDate = new DateTime(1900, 1, 1),
                Value = DateTime.Today
            };
            gbDados.Controls.Add(dtpNasc);

            // ---------- Telefones ----------
            GroupBox gbTel = new GroupBox { Text = "Telefones", Location = new Point(10, 180), Size = new Size(360, 200) };
            gbDados.Controls.Add(gbTel);

            gbTel.Controls.Add(new Label { Text = "Tipo:", Location = new Point(10, 22), AutoSize = true });
            cmbTipo = new ComboBox { Location = new Point(10, 40), Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipo.Items.AddRange(new object[] { "Celular", "Residencial", "Comercial" });
            cmbTipo.SelectedIndex = 0;
            gbTel.Controls.Add(cmbTipo);

            gbTel.Controls.Add(new Label { Text = "Número:", Location = new Point(120, 22), AutoSize = true });
            txtNumero = new TextBox { Location = new Point(120, 40), Width = 120 };
            gbTel.Controls.Add(txtNumero);

            chkPrincipal = new CheckBox { Text = "Principal", Location = new Point(250, 42), AutoSize = true };
            gbTel.Controls.Add(chkPrincipal);

            Button btnAddTel = new Button { Text = "Adicionar telefone", Location = new Point(10, 72), Size = new Size(140, 27) };
            btnAddTel.Click += BtnAddTel_Click;
            gbTel.Controls.Add(btnAddTel);

            Button btnRemTel = new Button { Text = "Remover selecionado", Location = new Point(160, 72), Size = new Size(140, 27) };
            btnRemTel.Click += BtnRemTel_Click;
            gbTel.Controls.Add(btnRemTel);

            lstTelefones = new ListBox { Location = new Point(10, 106), Size = new Size(340, 84) };
            gbTel.Controls.Add(lstTelefones);

            // ---------- Botões (opções do seletor) ----------
            int y = 392;
            gbDados.Controls.Add(CriarBotao("Adicionar", 10, y, BtnAdicionar_Click));
            gbDados.Controls.Add(CriarBotao("Pesquisar", 80, y, BtnPesquisar_Click));
            gbDados.Controls.Add(CriarBotao("Alterar", 150, y, BtnAlterar_Click));
            gbDados.Controls.Add(CriarBotao("Remover", 220, y, BtnRemover_Click));
            gbDados.Controls.Add(CriarBotao("Limpar", 290, y, (s, e) => LimparFormulario()));
            Button btnListar = CriarBotao("Listar", 10, y + 28, (s, e) => AtualizarLista());
            gbDados.Controls.Add(btnListar);
            Button btnSair = CriarBotao("Sair", 290, y + 28, (s, e) => Close());
            gbDados.Controls.Add(btnSair);

            // ---------- Lista de contatos ----------
            GroupBox gbLista = new GroupBox { Text = "Contatos cadastrados", Location = new Point(400, 10), Size = new Size(490, 450) };
            Controls.Add(gbLista);

            lvContatos = new ListView
            {
                Location = new Point(10, 22),
                Size = new Size(470, 418),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false
            };
            lvContatos.Columns.Add("Nome", 120);
            lvContatos.Columns.Add("E-mail", 130);
            lvContatos.Columns.Add("Nasc.", 70);
            lvContatos.Columns.Add("Idade", 45);
            lvContatos.Columns.Add("Tel. principal", 100);
            lvContatos.SelectedIndexChanged += LvContatos_SelectedIndexChanged;
            gbLista.Controls.Add(lvContatos);
        }

        private Button CriarBotao(string texto, int x, int y, EventHandler handler)
        {
            Button b = new Button { Text = texto, Location = new Point(x, y), Size = new Size(65, 27) };
            b.Click += handler;
            return b;
        }

        // ---------- Telefones ----------
        private void BtnAddTel_Click(object sender, EventArgs e)
        {
            string numero = txtNumero.Text.Trim();
            if (numero.Length == 0)
            {
                MessageBox.Show("Informe o número do telefone.");
                return;
            }

            Telefone t = new Telefone(cmbTipo.Text, numero, chkPrincipal.Checked);
            if (t.Principal)
                foreach (Telefone outro in telefonesAtuais) outro.Principal = false;
            else if (telefonesAtuais.Count == 0)
                t.Principal = true;

            telefonesAtuais.Add(t);
            txtNumero.Clear();
            chkPrincipal.Checked = false;
            AtualizarListaTelefones();
        }

        private void BtnRemTel_Click(object sender, EventArgs e)
        {
            int i = lstTelefones.SelectedIndex;
            if (i < 0) return;
            bool eraPrincipal = telefonesAtuais[i].Principal;
            telefonesAtuais.RemoveAt(i);
            if (eraPrincipal && telefonesAtuais.Count > 0)
                telefonesAtuais[0].Principal = true;
            AtualizarListaTelefones();
        }

        private void AtualizarListaTelefones()
        {
            lstTelefones.Items.Clear();
            foreach (Telefone t in telefonesAtuais)
                lstTelefones.Items.Add(t.ToString());
        }

        // ---------- Opções do seletor ----------
        private Contato LerFormulario()
        {
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (nome.Length == 0 || email.Length == 0)
            {
                MessageBox.Show("Nome e e-mail são obrigatórios.");
                return null;
            }

            DateTime d = dtpNasc.Value;
            Contato c = new Contato(email, nome, new Data(d.Day, d.Month, d.Year));
            foreach (Telefone t in telefonesAtuais)
                c.Telefones.Add(new Telefone(t.Tipo, t.Numero, t.Principal));
            return c;
        }

        private void BtnAdicionar_Click(object sender, EventArgs e)
        {
            Contato c = LerFormulario();
            if (c == null) return;

            if (contatos.adicionar(c))
            {
                AtualizarLista();
                LimparFormulario();
            }
            else
                MessageBox.Show("Já existe um contato com esse e-mail.");
        }

        private void BtnPesquisar_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (email.Length == 0)
            {
                MessageBox.Show("Digite o e-mail do contato a pesquisar.");
                return;
            }

            Contato achado = contatos.pesquisar(new Contato(email));
            if (achado == null)
                MessageBox.Show("Contato não encontrado.");
            else
            {
                PreencherFormulario(achado);
                MessageBox.Show(achado.ToString(), "Contato encontrado");
            }
        }

        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            Contato c = LerFormulario();
            if (c == null) return;

            if (contatos.alterar(c))
                AtualizarLista();
            else
                MessageBox.Show("Contato não encontrado (o e-mail identifica o contato e não pode ser alterado).");
        }

        private void BtnRemover_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (email.Length == 0)
            {
                MessageBox.Show("Informe ou selecione o contato a remover.");
                return;
            }

            if (MessageBox.Show("Remover o contato " + email + "?", "Confirmar",
                    MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            if (contatos.remover(new Contato(email)))
            {
                AtualizarLista();
                LimparFormulario();
            }
            else
                MessageBox.Show("Contato não encontrado.");
        }

        // ---------- Auxiliares ----------
        private void AtualizarLista()
        {
            lvContatos.Items.Clear();
            foreach (Contato c in contatos.Agenda)
            {
                ListViewItem item = new ListViewItem(c.Nome);
                item.SubItems.Add(c.Email);
                item.SubItems.Add(c.DtNasc.ToString());
                item.SubItems.Add(c.getIdade().ToString());
                item.SubItems.Add(c.getTelefonePrincipal());
                item.Tag = c;
                lvContatos.Items.Add(item);
            }
        }

        private void LvContatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvContatos.SelectedItems.Count == 0) return;
            PreencherFormulario((Contato)lvContatos.SelectedItems[0].Tag);
        }

        private void PreencherFormulario(Contato c)
        {
            txtNome.Text = c.Nome;
            txtEmail.Text = c.Email;
            dtpNasc.Value = c.DtNasc.ToDateTime();

            telefonesAtuais = new List<Telefone>();
            foreach (Telefone t in c.Telefones)
                telefonesAtuais.Add(new Telefone(t.Tipo, t.Numero, t.Principal));
            AtualizarListaTelefones();
        }

        private void LimparFormulario()
        {
            txtNome.Clear();
            txtEmail.Clear();
            txtNumero.Clear();
            chkPrincipal.Checked = false;
            dtpNasc.Value = DateTime.Today;
            telefonesAtuais = new List<Telefone>();
            AtualizarListaTelefones();
            lvContatos.SelectedItems.Clear();
        }
    }
}
