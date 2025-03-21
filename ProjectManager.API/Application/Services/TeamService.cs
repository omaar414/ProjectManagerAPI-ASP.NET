using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public Task<List<Team>> GetMyTeams(int userId)
        {
            var teams = _teamRepository.GetUserTeamsAsync(userId);
            return teams;
        }
    }
}