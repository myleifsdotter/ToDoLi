using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ToDoLy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Path to the file where the To Do List is stored
            //-------- edit this to your convenience --------
            //string filePath = @"C:\Users\mil\OneDrive\Documents\Lexicon\taskFile.txt";
            string filePath = @"./taskFile.txt";
            //-----------------------------------------

            //A bunch of variables used along the way
            List<Task> sortedTasks;
            bool taskListEmpty;
            Task taskToEdit;
            int totalNumberOfTasks, numberOfCompletedTasks;
            string answer = "0"; //user input
            string description, project;
            DateTime date;

            Console.WriteLine("\r\nWelcome to ToDoLy");
            Console.WriteLine("-----------------");

            //Retrieve task list from file. A new one is created if it does not exist.
            List<Task> tasks = new List<Task>();
            try
            {
                string dataFromFile = File.ReadAllText(filePath);
                tasks = JsonConvert.DeserializeObject<List<Task>>(dataFromFile);
                taskListEmpty = false;
            }
            catch 
            { 
                Console.WriteLine("Could not read task file. Creating a new one.");
                taskListEmpty = true;
            }

            //Menu of choices will run until user chooses to quit.
            while (true)
            {
                totalNumberOfTasks = tasks.Count;
                numberOfCompletedTasks = tasks.Where(task => task.IsCompleted).Count();
                Console.WriteLine($"\r\nYou have {totalNumberOfTasks} tasks todo and {numberOfCompletedTasks} are done!");
                Console.WriteLine(" Pick an option: ");
                Console.WriteLine(" ( 1 ) Show Task List (by date or project)");
                Console.WriteLine(" ( 2 ) Add New Task");
                Console.WriteLine(" ( 3 ) Edit Task (update, mark as done, remove)");
                Console.WriteLine(" ( 4 ) Save and Quit");
                answer = Console.ReadLine();

                if (answer.Trim() == "4") { break; } //save and quit

                //Display a collection of tasks sorted by date or by project.
                //If no choice is made the unsorted list will be displayed.
                if (answer.Trim() == "1") //show list
                {
                    sortedTasks = tasks;
                    Console.WriteLine(" ( 1 ) Show tasks sorted by date: ");
                    Console.WriteLine(" ( 2 ) Show tasks sorted by project: ");
                    answer = Console.ReadLine();
                    if (answer.Trim() == "1")
                    {
                        sortedTasks = tasks.OrderBy(task => task.DueDate).ThenBy(task => task.Project).ToList();
                    }
                    else if (answer.Trim() == "2")
                    {
                        sortedTasks = tasks.OrderBy(task => task.Project).ThenBy(task => task.DueDate).ToList();
                    }
                    else { Console.WriteLine("That choise doesn't excist. Here's the unsorted list."); }
                    Methods.printTaskList(sortedTasks);
                    answer = "0"; //So that previous answer will not interfere with the future
                }

                //Add new task to list
                if (answer.Trim() == "2") //add task
                {
                    int newTaskID;
                    if(taskListEmpty == true) { newTaskID = 1; }
                    else { newTaskID = tasks.Max(task => task.ID) + 1; }
                    Console.Write("\r\nAdd task description: ");
                    description = Console.ReadLine().Trim();
                    date = Methods.getDate();
                    Console.WriteLine("\r\nWhich project does this task belong to? ");
                    Console.WriteLine("The following projects exist already: ");
                    Console.WriteLine("--------------------------------------");
                    var projects = tasks.Select(task => task.Project).Distinct();
                    foreach (var p in projects) { Console.WriteLine(p); }
                    Console.WriteLine("--------------------------------------");
                    Console.Write("\r\nEnter project: ");
                    project = Console.ReadLine().Trim();
                    Task inputTask = new(description, date, false, project,newTaskID);
                    tasks.Add(inputTask);
                }

                //Edit task (update, mark as done, remove) 
                if (answer.Trim() == "3") //edit task
                {
                    //show the list first
                    sortedTasks = tasks.OrderBy(task => task.DueDate).ThenBy(task => task.Project).ToList();
                    Console.WriteLine("\r\nThese are your tasks: ");
                    Methods.printTaskList(sortedTasks);

                    int editID;
                    string inputID;
                    while (true) //get the task to edit from user
                    {
                        Console.Write("Enter the ID of the task you wish to edit: ");
                        inputID = Console.ReadLine();
                        if (int.TryParse(inputID.Trim(), out editID))
                        {
                            if (tasks.Select(x => x.ID).Contains(editID)) { break; }
                            else { Console.WriteLine("That ID does not excist."); }
                        }
                        else { Console.WriteLine("Could not parse to number."); }
                    }
                    taskToEdit = Methods.getTaskToEdit(tasks, editID);
                    
                    if (taskToEdit != null)
                    {
                        Console.WriteLine("How would you like to edit your task?");
                        Console.WriteLine(" ( 1 ) Update task");
                        Console.WriteLine(" ( 2 ) Mark as done");
                        Console.WriteLine(" ( 3 ) Remove task");
                        answer = Console.ReadLine();
                    }
                    
                    if (answer.Trim() == "1") //update task
                    {
                        Console.WriteLine("Which field would you like to update?");
                        Console.WriteLine(" ( 1 ) Task Description");
                        Console.WriteLine(" ( 2 ) Due Date");
                        Console.WriteLine(" ( 3 ) Project");
                        answer = Console.ReadLine();
                        if(answer.Trim() == "1") //update description
                        {
                            Console.Write("Enter new description: ");
                            description = Console.ReadLine();
                            taskToEdit.editTitle(description);
                        }
                        else if (answer.Trim() == "2") //update due date
                        {
                            date = Methods.getDate();
                            taskToEdit.editDate(date);
                        }
                        else if (answer.Trim() == "3") // update project
                        {
                            Console.Write("Enter new project: ");
                            project = Console.ReadLine();
                            taskToEdit.editProject(project);
                        }
                        else { Console.WriteLine("Invalid choice"); }
                    }
                    else if (answer.Trim() == "2") //mark task as done
                    {
                        taskToEdit.markAsDone();
                    }
                    else if (answer.Trim() == "3") //remove task
                    {
                        Methods.removeTask(tasks, editID);
                    }
                    else { Console.WriteLine("Invalid choice"); }
                    answer = "0"; //So that previous answer will not interfere with next choise
                } 
            } 

            //Save task list to file. FilePath is set at top of Main()
            string listData = JsonConvert.SerializeObject(tasks);
            File.WriteAllText(filePath, listData);

        }
    }
}
