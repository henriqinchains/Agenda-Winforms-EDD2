namespace Agenda
{
    public class Telefone
    {
        private string tipo;
        private string numero;
        private bool principal;

        public Telefone() : this("", "", false) { }

        public Telefone(string tipo, string numero, bool principal)
        {
            this.tipo = tipo;
            this.numero = numero;
            this.principal = principal;
        }

        public string Tipo { get { return tipo; } set { tipo = value; } }
        public string Numero { get { return numero; } set { numero = value; } }
        public bool Principal { get { return principal; } set { principal = value; } }

        public override string ToString()
        {
            return string.Format("{0}: {1}{2}", tipo, numero, principal ? " (principal)" : "");
        }
    }
}
