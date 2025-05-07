using System;
using System.Collections.Generic;

namespace WalletService.Domain.Authorization
{
    public class Role
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Permission Permissions { get; private set; }
        public bool IsSystem { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        
        private Role() { }
        
        public Role(string name, string description, Permission permissions, bool isSystem = false)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Permissions = permissions;
            IsSystem = isSystem;
            CreatedAt = DateTime.UtcNow;
        }
        
        public void UpdatePermissions(Permission permissions)
        {
            if (IsSystem)
                throw new InvalidOperationException("Cannot modify system role permissions");
                
            Permissions = permissions;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public bool HasPermission(Permission permission)
        {
            return (Permissions & permission) == permission;
        }
        
        public static IEnumerable<Role> GetDefaultRoles()
        {
            return new[]
            {
                new Role("User", "Standard user role", Permission.User, true),
                new Role("Admin", "Administrative role", Permission.Admin, true),
                new Role("SuperAdmin", "Super administrative role", Permission.SuperAdmin, true)
            };
        }
    }
}