using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
  public class Task
  {
    public int Id {get;set;}
    public string Description {get;set;}

    public Task(string description, int id = 0)
    {
      this.Id = id;
      Description = description;
    }
    public override bool Equals(System.Object otherTask)
    {
      if (!(otherTask is Task))
      {
        return false;
      }
      else
      {
        Task newTask = (Task) otherTask;
        bool idEquality = (this.Id == newTask.Id);
        bool descriptionEquality = (this.Description == newTask.Description);
        return (idEquality && descriptionEquality);
      }
    }
    public static Task Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `tasks` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int taskId = 0;
      string taskDescription = "";

      while (rdr.Read())
      {
          taskId = rdr.GetInt32(0);
          taskDescription = rdr.GetString(1);
      }
     Task foundTask= new Task(taskDescription, taskId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
     return foundTask;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO `tasks` (`description`) VALUES (@TaskDescription);";

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@TaskDescription";
      description.Value = this.Description;
      cmd.Parameters.Add(description);
      cmd.ExecuteNonQuery();
      this.Id = (int)cmd.LastInsertedId;

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<Task> GetAll()
      {
        List<Task> allTasks = new List<Task> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM tasks;";
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int taskId = rdr.GetInt32(0);
          string taskDescription = rdr.GetString(1);
          Task newTask = new Task(taskDescription, taskId);
          allTasks.Add(newTask);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return allTasks;
      }
      public static void ClearAll()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM tasks;";
        cmd.ExecuteNonQuery();

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }
  }
}
