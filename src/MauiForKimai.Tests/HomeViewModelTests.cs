namespace MauiForKimai.Tests;

using MauiForKimai.ApiClient;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.Interfaces;
using MauiForKimai.Tests.Data;
using MauiForKimai.Tests.Mocks;
using MauiForKimai.ViewModels;
using MauiForKimai.Wrappers;
using Moq;

public class HomeViewModelTests
{

    private HomeViewModel vm {get; set;}
    private Mock<HomeViewModel> mockVm {get; set;}

    private Mock<IRoutingService> routeMock = new Mock<IRoutingService>();
    private Mock<ILoginService> loginMock = new Mock<ILoginService>();
    private Mock<ITimesheetService> timesheetMock = new Mock<ITimesheetService>();
    private Mock<IServerService> serverServiceMock = new Mock<IServerService>();
    private Mock<ISecureStorageService> secureStorageMock = new Mock<ISecureStorageService>();
    private Mock<FakeDispatcherWrapper> dispatcherMock = new Mock<FakeDispatcherWrapper>();


    public HomeViewModelTests()
    {
       // var x = new HomeViewModel(routeMock.Object, loginMock.Object, timesheetMock.Object, dispatcherMock.Object,serverServiceMock.Object,secureStorageMock.Object);
        mockVm = new Mock<HomeViewModel>(routeMock.Object, loginMock.Object, timesheetMock.Object, dispatcherMock.Object,serverServiceMock.Object,secureStorageMock.Object);
        mockVm.Setup(x => x.GetConnectivity()).Returns(NetworkAccess.Internet);
        vm = mockVm.Object;
    }

    [Fact]
    public async Task GetRecentTimesheets_ListNotEmpty_Success()
    {

        //arrange
        List<TimesheetCollectionExpanded> list = new List<TimesheetCollectionExpanded>();
        list.Add(TestData.TestTimesheetCollectionExpanded);

        timesheetMock.Setup(x => x.GetTenRecentTimesheetsAsync()).ReturnsAsync(list);

        //act
        await vm.GetTimeSheetsCommand.ExecuteAsync(true);

        var x = vm.RecentTimesheets.FirstOrDefault();

        //assert
        Assert.Contains(x, vm.RecentTimesheets);


    }


}