using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using ProjectManager.API.Application.Interfaces;

namespace ProjectManager.API.Application.Services
{
    public class TeamService : ItemsFeature
    {
        private readonly ITeamRepository _teamRepository;
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
    }
}