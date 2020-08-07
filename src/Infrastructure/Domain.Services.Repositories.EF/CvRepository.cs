// <copyright file="CvRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Domain.Model;
    using Domain.Services.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class CvRepository : ICvRepository
    {
        private readonly DataBaseContext context;

        public CvRepository(DataBaseContext context)
        {
            this.context = context;
        }

        public Cv GetCv(int id)
        {
            var cv = this.context.Cv.FirstOrDefault(p => p.Id == id);

            return cv;
        }

        public bool SaveAll(Cv cv)
        {
            if (cv.Id == 0)
            {
                this.context.Cv.Add(cv);
            }
            else
            {
                this.context.Cv.Attach(cv);
                this.context.Entry(cv).State = EntityState.Modified;
            }

            var save = this.context.SaveChanges() > 0;
            return save;
        }
    }
}
