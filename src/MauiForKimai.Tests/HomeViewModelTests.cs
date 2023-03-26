namespace MauiForKimai.Tests;

using MauiForKimai.ApiClient;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.Interfaces;
using MauiForKimai.Tests.Mocks;
using MauiForKimai.ViewModels;
using MauiForKimai.Wrappers;
using Moq;

public class HomeViewModelTests
{
    [Fact]
    public async Task Test1()
    {
        var asp = new ApiStateProvider();
        var routeMock = new Mock<IRoutingService>();
        var loginMock = new Mock<ILoginService>();
        var timesheetMock = new Mock<ITimesheetService>();
        var dispatcherMock = new Mock<FakeDispatcherWrapper>();
   

        var vm = new HomeViewModel(routeMock.Object,loginMock.Object,timesheetMock.Object,asp ,dispatcherMock.Object);

        var customer = new Customer();
        customer.Name = "cau";

        var activity = new ActivityExpanded();
        activity.Id = 1;
        activity.Name = "Test";

        var project = new ProjectExpanded();
        project.Id = 1;
        project.Name = "Test";
        project.Customer = customer;

        var item = new TimesheetCollectionExpanded();
        item.Id = 1;
        item.Activity = activity;
        item.Project = project;
        item.Duration = 1;

        List<TimesheetCollectionExpanded> list = new List<TimesheetCollectionExpanded>();
        list.Add(item);


        timesheetMock.Setup(x => x.GetTenRecentTimesheetsAsync()).ReturnsAsync(list);


        await vm.GetTimeSheetsCommand.ExecuteAsync(true);

        var x = vm.RecentTimesheets.FirstOrDefault();

        Assert.NotNull(x);


    }
}