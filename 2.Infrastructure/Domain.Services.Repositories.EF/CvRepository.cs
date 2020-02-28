using Domain.Model;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.Cv;
using Domain.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Services.Repositories.EF
{
    public class CvRepository : ICvRepository
    {
        private readonly DataBaseContext _context;
        public CvRepository(DataBaseContext context)
        {
            _context = context;
        }
        public Cv GetCv(int id)
        {
            var cv = _context.Cv.FirstOrDefault(p => p.Id == id);

            return cv;
        }

        public bool SaveAll(Cv cv)
        {
            if (cv.Id == 0)
            {
                _context.Cv.Add(cv);
            }
            else
            {
                _context.Cv.Attach(cv);
                _context.Entry(cv).State = EntityState.Modified;
            }

            var save = _context.SaveChanges() > 0;
            return save;
        }
    }
}
