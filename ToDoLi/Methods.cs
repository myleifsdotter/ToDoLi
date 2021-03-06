using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoLy
{
    internal class Methods
    {
        //get task deadline from user
        public static DateTime getDate()
        {
            DateTime parseDate;
            string userInputDate;
            while (true)
            {
                Console.Write("\r\nEnter task deadline: 'YYYY-MM-DD': ");
                userInputDate = Console.ReadLine();
                if (DateTime.TryParse(userInputDate, out parseDate)) {  break; }
                else { Console.WriteLine("Input date on format YYYY-MM-DD"); }
            }
            return parseDate;
        }

        //printing the taskList to console
        public static void printTaskList(List<Task> taskList)
        {
            string isDone;
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine("ID".PadRight(5) + "Title".PadRight(40) + "Deadline".PadRight(15) + "Project".PadRight(20) + "Done?");
            Console.WriteLine("-------------------------------------------------------------------");
            foreach (var task in taskList)
            {
                if (task.IsCompleted) { isDone = "Done"; }
                else { isDone = ""; }
                Console.WriteLine(task.ID.ToString().PadRight(5) 
                    + task.Title.PadRight(40)
                    + task.DueDate.ToShortDateString().PadRight(15)
                    + task.Project.PadRight(20)
                    + isDone);
            }
            Console.WriteLine("-------------------------------------------------------------------");
        }
        
        //returns the Task whith editID from the tasklist
        public static Task getTaskToEdit(List<Task> tasks, int editID)
        {
            Task taskToEdit = null;
            try
            {
                taskToEdit = tasks.SingleOrDefault(r => r.ID == editID);
                if (taskToEdit == null) { Console.WriteLine($"Could not find task with id {editID}."); }
            }
            catch
            {
                Console.WriteLine("Error in retrieving task. There seems to be duplicates.");
            }
            return taskToEdit;
        }

        //removes the task with editID from the tasklist
        //in the case of duplicates the choice is to remove all or none
        public static void removeTask(List<Task> tasks, int editID)
        {
            string answer;
            try
            {
                var taskToRemove = tasks.SingleOrDefault(r => r.ID == editID);
                if (taskToRemove != null) { tasks.Remove(taskToRemove); }
                else { Console.WriteLine($"Could not find task with id {editID}."); }
            }
            catch
            {
                Console.WriteLine("There seems to be duplicates");
                Console.Write("Would you like to remove all? Y/N: ");
                answer = Console.ReadLine();
                if (answer.Trim().ToUpper() == "Y") { tasks.RemoveAll(r => r.ID == editID); }
                else { Console.WriteLine("Keeping all duplicates."); }
            }
        }
    }
}
