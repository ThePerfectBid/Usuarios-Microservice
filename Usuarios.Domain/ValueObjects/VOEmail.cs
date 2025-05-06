using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Usuarios.Domain.ValueObjects
{
    public class VOEmail
    {
        public string Value { get; private set; }

        public VOEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                throw new ArgumentException("El correo es inválido.");

            Value = value;
        }

        public override string ToString() => Value;


    }
}
