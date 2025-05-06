using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Domain.Entities;
using Usuarios.Domain.ValueObjects;

namespace Usuarios.Domain.Repositories
{
    public interface IRoleRepository
    {
        //Task AddAsync(Role role);  // Insertar un nuevo rol
        Task<Role?> GetByIdAsync(string roleId); // Obtener un rol por su ID
        Task<bool> UpdatePermissionsAsync(VORoleId roleId, VORolePermissions permissionIds); // Modificar permisos
        //Task<bool> DeleteAsync(RoleId roleId);  // Eliminar un rol
    }
}
