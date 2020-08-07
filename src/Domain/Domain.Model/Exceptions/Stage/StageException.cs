// <copyright file="StageException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Stage
{
    using System;
    using Core;

    public class StageException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Stage;

        public StageException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a stage related error" : message)
        {
        }
    }

    public class InvalidStageException : StageException
    {
        public InvalidStageException(string message)
            : base(string.IsNullOrEmpty(message) ? "The stage is not valid" : message)
        {
        }
    }

    public class DeleteStageNotFoundException : InvalidStageException
    {
        protected override int SubErrorCode => (int)StageErrorSubCodes.DeleteStageNotFound;

        public DeleteStageNotFoundException(int stageId)
            : base($"Stage not found for the StageId: {stageId}")
        {
            this.StageId = stageId;
        }

        public int StageId { get; set; }
    }

    public class StageDeletedException : InvalidStageException
    {
        protected override int SubErrorCode => (int)StageErrorSubCodes.StageDeleted;

        public StageDeletedException(int stageId, string name)
            : base($"The stage {name} was deleted")
        {
            this.StageId = stageId;
            this.Name = name;
        }

        public int StageId { get; }

        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidStageException
    {
        protected override int SubErrorCode => (int)StageErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the stage.")
        {
        }
    }

    public class UpdateStageNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)StageErrorSubCodes.UpdateStageNotFound;

        public UpdateStageNotFoundException(int stageId, Guid clientSystemId)
            : base($"Stage {stageId} and Client System Id {clientSystemId} was not found.")
        {
            this.StageId = stageId;
            this.ClientSystemId = clientSystemId;
        }

        public int StageId { get; }

        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)StageErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int stageId, Guid clientSystemId, string name)
            : base($"Stage {name} has not changes.")
        {
            this.StageId = stageId;
            this.ClientSystemId = clientSystemId;
        }

        public int StageId { get; }

        public Guid ClientSystemId { get; }
    }

    public class StageNotFoundException : InvalidStageException
    {
        protected override int SubErrorCode => (int)StageErrorSubCodes.UpdateStageNotFound;

        public StageNotFoundException(int stageId) : base($"The Stage {stageId} was not found.")
        {
            this.StageId = stageId;
        }

        public int StageId { get; }
    }
}
