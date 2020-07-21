using Core;
using System;

namespace Domain.Model.Exceptions.Stage
{
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
            StageId = stageId;
        }

        public int StageId { get; set; }
    }

    public class StageDeletedException : InvalidStageException
    {
        protected override int SubErrorCode => (int)StageErrorSubCodes.StageDeleted;

        public StageDeletedException(int stageId, string name)
            : base($"The stage {name} was deleted")
        {
            StageId = stageId;
            Name = name;
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
            StageId = stageId;
            ClientSystemId = clientSystemId;
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
            StageId = stageId;
            ClientSystemId = clientSystemId;
        }

        public int StageId { get; }
        public Guid ClientSystemId { get; }
    }

    public class StageNotFoundException : InvalidStageException
    {
        protected override int SubErrorCode => (int)StageErrorSubCodes.UpdateStageNotFound;

        public StageNotFoundException(int stageId) : base($"The Stage {stageId} was not found.")
        {
            StageId = stageId;
        }

        public int StageId { get; }
    }
}
