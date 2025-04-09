using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;



        public TaskItem(string title, string description, DateTime? dueDate, int projectId)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            ProjectId = projectId;
        }

        
    }
}