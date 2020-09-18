// <copyright file="ValidatorConstants.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators
{
    internal class ValidatorConstants
    {
        internal const string RULESETCREATE = "Create";
        internal const string RULESETUPDATE = "Update";
        internal const string RULESETDEFAULT = "default";
    }

    internal class ValidationConstants
    {
        internal const int MAXINPUT = 150;
        internal const int MAXTEXTAREA = 10000;
        internal const int MAXINPUTEMAIL = 60;
        internal const int MAXMONTHLYINCOME = 1000000;
        internal const int MAXDNI = 999999999;
        internal const int MAXPHONENUMBER = 16;
        internal const int MAXTEXTINTERVIEWFEEDBACK = 1000;
    }
}