using System;

namespace ToDoLy
{
    internal class Task
    {
        public Task(string title, DateTime dueDate, bool isCompleted, string project, int id)
        {
            Title = title;
            DueDate = dueDate;
            IsCompleted = isCompleted;
            Project = project;
            ID = id;
        }

        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Project { get; set; }
        public int ID { get; set; }

        public void editTitle(string newString)
        {
            Title = newString;
        }
        public void editProject(string newString)
        {
            Project = newString;
        }
        public void editDate(DateTime newDate)
        {
            DueDate = newDate;
        }
        public void markAsDone()
        {
            IsCompleted = true;
        }
    }
}
