using System.Collections.Generic;

namespace Agenda
{
    public class Contatos
    {
        private readonly List<Contato> agenda = new List<Contato>();

        // Acesso somente leitura para a interface listar os contatos.
        public IReadOnlyList<Contato> Agenda { get { return agenda; } }

        public bool adicionar(Contato c)
        {
            if (c == null || agenda.Contains(c)) return false;
            agenda.Add(c);
            return true;
        }

        public Contato pesquisar(Contato c)
        {
            int i = agenda.IndexOf(c);
            return i >= 0 ? agenda[i] : null;
        }

        public bool alterar(Contato c)
        {
            int i = agenda.IndexOf(c);
            if (i < 0) return false;
            agenda[i] = c;
            return true;
        }

        public bool remover(Contato c)
        {
            return agenda.Remove(c);
        }
    }
}
