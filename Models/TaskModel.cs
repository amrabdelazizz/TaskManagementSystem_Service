using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class TaskModel
    {
        [Key]
        public int TaskId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string AssignedTo { get; set; }


        [EnumDataType(typeof(PriorityLevel))]
        public PriorityLevel priority{ get; set; }
        public DateTime duedate { get; set; }

        [EnumDataType(typeof(TaskStatus))]
        public TaskStatus status { get; set; }

    }

    public enum PriorityLevel
    {
        Low,
        Medium,
        High
    }

    public enum TaskStatus
    {
        New,
        InProgress,
        Completed
    }
}
