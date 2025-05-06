﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios.Domain.ValueObjects
{
    public class VORolePermissions
    {
        public List<string> PermissionIds { get; private set; }

        public VORolePermissions(List<string> permissionIds)
        {
            PermissionIds = permissionIds ?? new List<string>();
        }

        public bool HasPermission(string permissionId) => PermissionIds.Contains(permissionId);
    }
}
