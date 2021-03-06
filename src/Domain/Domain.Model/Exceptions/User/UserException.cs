﻿// <copyright file="UserException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.User
{
    using System;
    using Core;

    public class UserException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.User;

        public UserException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a user related error" : message)
        {
        }
    }

    public class InvalidUserException : UserException
    {
        public InvalidUserException(string message)
            : base(string.IsNullOrEmpty(message) ? "The user is not valid" : message)
        {
        }
    }

    public class InvalidUpdateException : InvalidUserException
    {
        protected override int SubErrorCode => (int)UserErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the user.")
        {
        }
    }

    public class DeleteUserNotFoundException : InvalidUserException
    {
        protected override int SubErrorCode => (int)UserErrorSubCodes.DeleteUserNotFound;

        public DeleteUserNotFoundException(int userId)
            : base($"User not found for the UserId: {userId}")
        {
            this.UserId = userId;
        }

        public int UserId { get; set; }
    }

    public class UserDeletedException : InvalidUserException
    {
        protected override int SubErrorCode => (int)UserErrorSubCodes.UserDeleted;

        public UserDeletedException(int id, string name)
            : base($"The user {name} was deleted")
        {
            this.UserId = id;
            this.Name = name;
        }

        public int UserId { get; set; }

        public string Name { get; set; }
    }

    public class UpdateUserNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)UserErrorSubCodes.UpdateUserNotFound;

        public UpdateUserNotFoundException(int userId, Guid clientSystemId)
            : base($"User {userId} and Client System Id {clientSystemId} was not found.")
        {
            this.UserId = userId;
            this.ClientSystemId = clientSystemId;
        }

        public int UserId { get; }

        public Guid ClientSystemId { get; }
    }

    public class UserNotFoundException : InvalidUserException
    {
        protected override int SubErrorCode => (int)UserErrorSubCodes.UserNotFound;

        public UserNotFoundException(int userId) : base($"The User {userId} was not found.")
        {
            this.UserId = userId;
        }

        public int UserId { get; }
    }
}
