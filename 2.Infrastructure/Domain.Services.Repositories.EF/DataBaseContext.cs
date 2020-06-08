﻿using Domain.Model;
using Domain.Model.Seed;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;

namespace Domain.Services.Repositories.EF
{
    public class DataBaseContext : DbContextBase
    {
        public virtual DbSet<Dummy> Dummies { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<CandidateProfile> Profiles { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Postulant> Postulants { get; set; }
        public DbSet<CandidateSkill> CandidateSkills { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<HrStage> HrStages { get; set; }
        public DbSet<TechnicalStage> TechnicalStages { get; set; }
        public DbSet<ClientStage> ClientStages { get; set; }
        public DbSet<Interview> Interview { get; set; }
        public DbSet<PreOfferStage> PreOfferStages { get; set; }
        public DbSet<OfferStage> OfferStages { get; set; }
        public DbSet<StageItem> StageItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SkillType> SkillTypes { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Community> Community { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<HireProjection> HireProjection { get; set; }
        public DbSet<EmployeeCasualty> EmployeeCasualty { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<DaysOff> DaysOff { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<CompanyCalendar> CompanyCalendar { get; set; }
        public DbSet<DeclineReason> DeclineReasons { get; set; }
        public DbSet<PreOffer> PreOffer { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Cv> Cv { get; set; }
        public DbSet<UserDashboard> UserDashboards { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CandidateSkill>()
                .HasKey(cs => new { cs.CandidateId, cs.SkillId });

            modelBuilder.Entity<CandidateSkill>()
                .HasOne(cs => cs.Candidate)
                .WithMany(cs => cs.CandidateSkills)
                .HasForeignKey(cs => cs.CandidateId);

            modelBuilder.Entity<CandidateSkill>()
                .HasOne(cs => cs.Skill)
                .WithMany(cs => cs.CandidateSkills)
                .HasForeignKey(cs => cs.SkillId);

            modelBuilder.Entity<SkillType>()
                .HasMany(st => st.Skills)
                .WithOne(s => s.Type)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientStage>()
                .HasMany(cs => cs.Interviews)
                .WithOne(cs => cs.ClientStage)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Task>()
                .HasMany(t => t.TaskItems)
                .WithOne(ti => ti.Task)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserDashboard>()
                .HasKey(ud => new { ud.UserId, ud.DashboardId });

            modelBuilder.Entity<UserDashboard>()
                .HasOne<User>(ud => ud.User)
                .WithMany(u => u.UserDashboards)
                .HasForeignKey(ud => ud.UserId);

            modelBuilder.Entity<UserDashboard>()
                .HasOne<Dashboard>(ud => ud.Dashboard)
                .WithMany(d => d.UserDashboards)
                .HasForeignKey(ud => ud.DashboardId);
        }
    }
}
