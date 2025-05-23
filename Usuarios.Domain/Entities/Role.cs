using Usuarios.Domain.ValueObjects;

namespace Usuarios.Domain.Entities
{
    public class Role
    {
        public VORoleId Id { get; private set; }
        public VORoleName Name { get; private set; }
        public VORolePermissions PermissionIds { get; private set; }

        public Role(VORoleId id, VORoleName name, VORolePermissions permissionIds)
        {
            Id = id;
            Name = name;
            PermissionIds = permissionIds;
        }
    }
}
