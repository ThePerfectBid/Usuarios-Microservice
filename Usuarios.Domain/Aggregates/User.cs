using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Domain.Entities;
using Usuarios.Domain.ValueObjects;

namespace Usuarios.Domain.Aggregates
{
    public class User
    {
        public VOId Id { get; private set; }
        public VOName Name { get; private set; }
        public VOLastName LastName { get; private set; }
        public VOEmail Email { get; private set; }
        public VORoleId RoleId { get; private set; }
        public VOAddress? Address { get; private set; } // Opcional
        public VOPhone? Phone { get; private set; } // Opcional

        private User() { } // Constructor privado para serialización

        public User(VOId id, VOName name, VOLastName lastName, VOEmail email, VORoleId roleid, VOAddress? address = null, VOPhone? phone = null)
        {
            Id = id;
            Name = name;
            LastName = lastName ;
            Email = email;
            Address = address;
            Phone = phone;
            RoleId = roleid;
        }
    }
}