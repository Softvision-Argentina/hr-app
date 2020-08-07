// <copyright file="StageItem.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using Core;

    public class StageItem : Entity<int>
    {
        public string Description { get; set; }

        public string AssociatedContent { get; set; }

        public int StageId { get; set; }

        public Stage Stage { get; set; }
    }
}
