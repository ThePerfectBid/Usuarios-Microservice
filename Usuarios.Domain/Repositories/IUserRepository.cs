using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Domain.Aggregates;

namespace Usuarios.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
    }
}
