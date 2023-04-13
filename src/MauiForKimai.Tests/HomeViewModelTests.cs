namespace MauiForKimai.Tests;

using MauiForKimai.ApiClient;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.Core.Models;
using MauiForKimai.Interfaces;
using MauiForKimai.Popups;
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


    private Mock<ICustomerService> customerServiceMock = new Mock<ICustomerService>();
    private Mock<PopupSizeConstants> sizeConstantsMock = new Mock<PopupSizeConstants>();
    private Mock<IFavouritesTimesheetService> favourtiteServiceMock = new Mock<IFavouritesTimesheetService>();

    private ApiLoginContext apiContext;
    public HomeViewModelTests()
    {

        apiContext = new ApiLoginContext();
        apiContext.IsAuthenticated = true;
        apiContext.SetAuthInfo(TestData.Server);
        loginMock.Setup(x => x.GetLoginContext()).Returns(apiContext);


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
        await vm.GetRecentTimesheetsCommand.ExecuteAsync(true);

        var x = vm.RecentTimesheets.FirstOrDefault();

        //assert
        Assert.Contains(x, vm.RecentTimesheets);


    }

      [Fact]
    public async Task StartRecentTimesheet_Success()
    {

        //arrange
        //var timesheetDetailViewModel = new Mock<TimesheetDetailViewModel>(routeMock.Object, loginMock.Object, customerServiceMock.Object, timesheetMock.Object,sizeConstantsMock.Object,favourtiteServiceMock.Object);
        List<TimesheetCollectionExpanded> list = new List<TimesheetCollectionExpanded>();
        list.Add(TestData.TestTimesheetCollectionExpanded);

        timesheetMock.Setup(x => x.Create(It.IsAny<TimesheetEditForm>())).ReturnsAsync(TestData.TestTimesheetEntity);
        timesheetMock.Setup(x => x.GetActive()).ReturnsAsync(list);



        //act
        await vm.StartRecentTimesheetCommand.ExecuteAsync(TestData.TestTimsheetModelRecent);

        

        //assert
        Assert.NotNull(vm.ActiveTimesheet);
        Assert.Equal(TestData.TestTimsheetModelRecent.ActivityName, vm.SelectedActivity);

        

    }


}