using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Domain.Aggregates;
using Usuarios.Domain.Entities;
using Usuarios.Domain.ValueObjects;

namespace Usuarios.Domain.Factories
{
    public static class UserFactory
    {
        public static User Create(VOId id, VOName name, VOLastName lastName, VOEmail email, VORoleId roleId, VOAddress? address = null, VOPhone? phone = null)
        {
            return new User(id, name, lastName, email, roleId, address, phone);
        }
    }
}
