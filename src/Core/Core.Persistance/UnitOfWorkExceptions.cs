using System;

// TODO: Change namespace to match folder/project structure,
// or change location of this class to a new "Exceptions" folder or project.
namespace Core.Persistance.Exceptions
{
    public class UnitOfWorkException : Exception
    {
        public UnitOfWorkException()
            : base()
        {
        }

        public UnitOfWorkException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class RepositoryNotFoundException : UnitOfWorkException
    {
    }

    public class RepositoryAlreadyRegisteredException : UnitOfWorkException
    {
    }

    public class TransactionException : UnitOfWorkException
    {
        public TransactionException()
            : base()
        {
        }

        public TransactionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class CannotOpenTransactionException : TransactionException
    {
        public CannotOpenTransactionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CannotOpenTransactionException()
            : base("Cannot Open Transaction", null)
        {

        }
    }

    public class CannotCommitTransactionException : TransactionException
    {
        public CannotCommitTransactionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CannotCommitTransactionException()
            : base("Cannot Commit Transaction", null)
        {
        }
    }

    public class CannotRollbackTransactionException : TransactionException
    {
    }
}
