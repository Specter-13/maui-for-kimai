using MauiForKimai.ApiClient;
using MauiForKimai.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MauiForKimai.Tests.Data;
public static class TestData
{

    private static DateTime _begin = DateTime.Now;
    private static DateTime _end = DateTime.Now.AddMinutes(60);
    //apistateprovider
    public static readonly ApiLoginContext Asp = new ApiLoginContext
      {
          IsAuthenticated = true,
          TimetrackingPermissions = new PermissionsTimetrackingModel(false,false,false),
           

      };

    public static readonly ServerModel Server = new ServerModel
    {
        Id = 1,
        Url = "http://localhost:8001",
        Name = "test",
        ApiPasswordKey = "password",
        Username = "username",
        IsDefault = false,
        CanEditBillable = false,
        CanEditExport = false,
        CanEditRate = false,
        
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
          Duration = 3600
      };

    public static readonly TimesheetEntity TestTimesheetEntity = new TimesheetEntity
    {
        Id = 1,
        Activity = TestActivity.Id,
        Project = TestProject.Id,
        Duration = 3600,
        User = 1,
        Tags = new[] {"ahoj", "cau"},
        Begin = _begin,
        End = _end,
        Description = "Test description",
        Exported = true,
        Billable = true
    };

    public static readonly TimesheetEditForm TestTimesheetEditFormNormalUser = new TimesheetEditForm
    {
        Activity = TestActivity.Id.Value,
        Project = TestProject.Id.Value,
        Tags = "ahoj, cau",
        Begin = _begin,
        End = _end,
        Description = "Test description",
    };

    public static readonly TimesheetEditForm TestTimesheetEditFormAdminUser = new TimesheetEditForm
    {
        Activity = TestActivity.Id.Value,
        Project = TestProject.Id.Value,
        Tags = "ahoj, cau",
        Begin = _begin,
        End = _end,
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
        Duration = 3600,
        Tags = new[] {"tag1", "tag2", "tag3"},
        Begin = _begin,
        End = _end,
        Description = "Test description",
        Exported = true,
        Billable = true
    };


    public static readonly TimesheetModel TestTimsheetModelRecent = new TimesheetModel
    {
        Id = 1,
        ActivityName = TestActivityExpanded.Name,
        ProjectName = TestProjectExpanded.Name,
        CustomerName= TestProjectExpanded.Customer.Name,
        CustomerId = TestProjectExpanded.Customer.Id.Value,

        Duration = 3600.ToString(),
        IsRecent = true,
        ActivityId = TestActivityExpanded.Id.Value,
        ProjectId = TestProjectExpanded.Id.Value,
        Begin = _begin,
        End = _end,
        Tags = "tag1, tag2, tag3",

        Description = "Test description",
        Exported = true,
        Billable = true
    };

}
