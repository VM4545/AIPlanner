using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AIPlanner.Models
{
	public class DailyPlan
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		public DateOnly Date { get; set; }

		[Required]
		public string Title { get; set; } = string.Empty;

		public string? Notes { get; set; }

		public List<PlanTask> Tasks { get; set; } = new();

		public int Priority { get; set; } = 3;

		public bool IsCompleted => Tasks.Count > 0 && Tasks.All(t => t.IsCompleted);

		public int TotalEstimatedMinutes => Tasks.Sum(t => t.EstimatedMinutes);
	}

	public class PlanTask
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		public string Description { get; set; } = string.Empty;

		public bool IsCompleted { get; set; }
		public int EstimatedMinutes { get; set; }

		public string? Category { get; set; }
		public int Order { get; set; }
	}
}

