using MauiForKimai.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MauiForKimai.Tests.Data;
public static class TestData
{
    //apistateprovider
    public static readonly ApiStateProvider Asp = new ApiStateProvider
      {
          IsAuthenticated = true,
      };

    //normal
      public static readonly Customer TestCustomer = new Customer
      {
          Id = 1,
          Name = "Test customer",
          Number = "123",
          Comment = "Test comment",
          Visible = true,
          Billable = true,
          Color = "red"
      };

    public static readonly Project TestProject = new Project
      {
          Customer = TestCustomer,
          Id = 1,
          Name = "Test project",
          Comment = "Test comment",
          Visible = true,
          Billable = true,
          Color = "red",
          GlobalActivities = true
      };

    public static readonly Activity TestActivity = new Activity
    {
        Project = TestProject.Id,
        Id = 1,
        Name = "Test activity",
        Comment = "Test comment",
        Visible = true,
        Billable = true,
        Color = "red"
    };

     public static readonly TimesheetCollection TestTimesheetCollection = new TimesheetCollection
      {
          Id = 1,
          Activity = TestActivity.Id,
          Project = TestProject.Id,
          Duration = 1000
      };

    public static readonly TimesheetEntity TestTimesheetEntity = new TimesheetEntity
    {
        Id = 1,
        Activity = TestActivity.Id,
        Project = TestProject.Id,
        Duration = 3000,
        User = 1,
        Tags = new[] {"ahoj", "cau"},
        Begin = DateTimeOffset.Now,
        End = DateTimeOffset.Now.AddSeconds(3000),
        Description = "Test description",
        Exported = true,
        Billable = true
    };

    public static readonly TimesheetEditForm TestTimesheetEditFormNormalUser = new TimesheetEditForm
    {
        Activity = TestActivity.Id.Value,
        Project = TestProject.Id.Value,
        Tags = "ahoj, cau",
        Begin = DateTimeOffset.Now,
        End = DateTimeOffset.Now.AddSeconds(3000),
        Description = "Test description",
    };

    public static readonly TimesheetEditForm TestTimesheetEditFormAdminUser = new TimesheetEditForm
    {
        Activity = TestActivity.Id.Value,
        Project = TestProject.Id.Value,
        Tags = "ahoj, cau",
        Begin = DateTimeOffset.Now,
        End = DateTimeOffset.Now.AddSeconds(3000),
        Description = "Test description",
        Exported= true,
        Billable= true
    };


    //expanded
    public static readonly ActivityExpanded TestActivityExpanded = new ActivityExpanded
    {
        Project = TestProject,
        Id = 1,
        Name = "Test activity",
        Comment = "Test comment",
        Visible = true,
        Billable = true,
        Color = "red"
    };

      public static readonly ProjectExpanded TestProjectExpanded = new ProjectExpanded
      {
          Customer = TestCustomer,
          Id = 1,
          Name = "Test project",
          Comment = "Test comment",
          Visible = true,
          Billable = true,
          Color = "red",
          GlobalActivities = true
      };

    public static readonly TimesheetCollectionExpanded TestTimesheetCollectionExpanded = new TimesheetCollectionExpanded
    {
        Id = 1,
        Activity = TestActivityExpanded,
        Project = TestProjectExpanded,
        Duration = 3000,
        Tags = new[] {"tag1", "tag2", "tag3"},
        Begin = DateTimeOffset.Now,
        End = DateTimeOffset.Now.AddSeconds(3000),
        Description = "Test description",
        Exported = true,
        Billable = true
    };

}
