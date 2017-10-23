using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;

namespace ToDoList.Tests
{
  [TestClass]
  public class TaskTest : IDisposable
  {
    public void Dispose()
    {
      Task.ClearAll();
    }
    public void ToDoListTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=ToDoList_data_test;";
    }
    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      int result = Task.GetAll().Count;
      Assert.AreEqual(0, result);
    }
    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Task()
    {
      Task firstTask = new Task("Mow the lawn");
      Task secondTask = new Task("Mow the lawn");
      Assert.AreEqual(firstTask, secondTask);
    }
    [TestMethod]
    public void Save_SavesToDatabase_TaskList()
    {
      Task testTask = new Task("Mow the lawn");
      testTask.Save();
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Find_FindsTaskInDatabase_Task()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn");
      testTask.Save();

      //Act
      Task foundTask = Task.Find(testTask.Id);

      //Assert
      Assert.AreEqual(testTask, foundTask);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      Task testTask = new Task("Mow the lawn");

      testTask.Save();

      Task savedTask = Task.GetAll()[0];
      int result = savedTask.Id;
      int testId = testTask.Id;
      Assert.AreEqual(testId, result);
    }
  }
}
