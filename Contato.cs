using System;
using System.Collections.Generic;

namespace Agenda
{
    public class Contato
    {
        private string email;
        private string nome;
        private Data dtNasc;
        private List<Telefone> telefones;

        // Usado nas pesquisas/remoções: o e-mail identifica o contato.
        public Contato(string email) : this(email, "", new Data()) { }

        public Contato(string email, string nome, Data dtNasc)
        {
            this.email = email;
            this.nome = nome;
            this.dtNasc = dtNasc;
            this.telefones = new List<Telefone>();
        }

        public string Email { get { return email; } set { email = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
        public Data DtNasc { get { return dtNasc; } set { dtNasc = value; } }
        public List<Telefone> Telefones { get { return telefones; } set { telefones = value; } }

        public int getIdade()
        {
            DateTime hoje = DateTime.Today;
            int idade = hoje.Year - dtNasc.Ano;
            if (hoje.Month < dtNasc.Mes || (hoje.Month == dtNasc.Mes && hoje.Day < dtNasc.Dia))
                idade--;
            return idade;
        }

        public void adicionarTelefone(Telefone t)
        {
            if (t.Principal)
                foreach (Telefone outro in telefones)
                    outro.Principal = false;      // só pode haver um principal
            else if (telefones.Count == 0)
                t.Principal = true;               // o primeiro vira principal

            telefones.Add(t);
        }

        public string getTelefonePrincipal()
        {
            foreach (Telefone t in telefones)
                if (t.Principal)
                    return string.Format("({0}) {1}", t.Tipo, t.Numero);
            return "(sem telefone)";
        }

        public override string ToString()
        {
            return string.Format("{0} | {1} | Nasc.: {2} ({3} anos) | Tel.: {4}",
                nome, email, dtNasc, getIdade(), getTelefonePrincipal());
        }

        // Dois contatos são iguais quando têm o mesmo e-mail.
        public override bool Equals(object obj)
        {
            Contato outro = obj as Contato;
            if (outro == null) return false;
            return string.Equals(email, outro.email, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (email ?? "").ToLowerInvariant().GetHashCode();
        }
    }
}
