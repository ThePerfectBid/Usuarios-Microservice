using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Usuarios.Domain.ValueObjects
{
    public class VOLastName
    {
        public string Value { get; private set; }

        public VOLastName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El apellido no puede estar vacío.");

            Value = value;
        }

        public override string ToString() => Value;
    }
}
