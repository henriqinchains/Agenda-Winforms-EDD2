using System;

namespace Agenda
{
    public class Data
    {
        private int dia;
        private int mes;
        private int ano;

        public Data() : this(1, 1, 1900) { }

        public Data(int dia, int mes, int ano)
        {
            setData(dia, mes, ano);
        }

        public int Dia { get { return dia; } }
        public int Mes { get { return mes; } }
        public int Ano { get { return ano; } }

        public void setData(int dia, int mes, int ano)
        {
            if (ano < 1 || ano > 9999 || mes < 1 || mes > 12 ||
                dia < 1 || dia > DateTime.DaysInMonth(ano, mes))
                throw new ArgumentException("Data inválida.");

            this.dia = dia;
            this.mes = mes;
            this.ano = ano;
        }

        public DateTime ToDateTime()
        {
            return new DateTime(ano, mes, dia);
        }

        public override string ToString()
        {
            return string.Format("{0:00}/{1:00}/{2:0000}", dia, mes, ano);
        }
    }
}
