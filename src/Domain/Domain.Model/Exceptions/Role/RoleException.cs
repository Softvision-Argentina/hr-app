using Core;
using System;

namespace Domain.Model.Exceptions.Role
{
    public class RoleException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Role;

        public RoleException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a Role related error" : message)
        {
        }
    }

    public class InvalidRoleException : RoleException
    {
        public InvalidRoleException(string message)
            : base(string.IsNullOrEmpty(message) ? "The Role is not valid" : message)
        {
        }
    }

    public class DeleteRoleNotFoundException : InvalidRoleException
    {
        protected override int SubErrorCode => (int)RoleErrorSubCodes.DeleteRoleNotFound;

        public DeleteRoleNotFoundException(int roleId)
            : base($"Role not found for the Role Id: {roleId}")
        {
            RoleId = roleId;
        }

        public int RoleId { get; set; }
    }

    public class RoleDeletedException : InvalidRoleException
    {
        protected override int SubErrorCode => (int)RoleErrorSubCodes.RoleDeleted;

        public RoleDeletedException(int id, string name)
            : base($"The Role {name} was deleted")
        {
            RoleId = id;
            Name = name;
        }

        public int RoleId { get; set; }
        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidRoleException
    {
        protected override int SubErrorCode => (int)RoleErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the Role.")
        {
        }
    }

    public class UpdateRoleNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)RoleErrorSubCodes.UpdateRoleNotFound;

        public UpdateRoleNotFoundException(int roleId, Guid clientSystemId)
            : base($"Role {roleId} and Client System Id {clientSystemId} was not found.")
        {
            RoleId = roleId;
            ClientSystemId = clientSystemId;
        }
  
        public int RoleId { get; }
        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)RoleErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int roleId, Guid clientSystemId, string name)
            : base($"Role {name} has not changes.")
        {
            RoleId = roleId;
            ClientSystemId = clientSystemId;
        }

        public int RoleId { get; }
        public Guid ClientSystemId { get; }
    }

    public class RoleNotFoundException : InvalidRoleException
    {
        protected override int SubErrorCode => (int)RoleErrorSubCodes.RoleNotFound;

        public RoleNotFoundException(int roleId) : base($"The Role {roleId} was not found.")
        {
            RoleId = roleId;
        }

        public int RoleId { get; }
    }
}
