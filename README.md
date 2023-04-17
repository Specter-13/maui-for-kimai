# MAUI for Kimai


<!-- images -->

<img src="assets/screens.png" alt="screens" >

<img src="assets/logo.png" alt="logo" width="100" >
<img src="assets/kimai_logo.png" alt="logo" width="100" >


MAUI for Kimai is multi-platform time-tracking app companion for open-source  [Kimai time-tracker](https://github.com/kimai/kimai). This application is fully focused on time-tracking use-case and aims to simplify time-tracking within Kimai.

| Platform      | Supported      |
|--------------|-----------------|
| Android      | <span style="color:green">yes</span>     |
| Windows      | <span style="color:green">yes</span>     |
| IOS          | ? (need testing) |
| MacCatalysts | ? (need testing) |

I don't have access to Mac build tools, therefore IOS/MacOS is untested, but in theory it should work. If anyone would be willing to test it, I would deeply appreciate it.

## Features
---
### Kimai features

- Timesheet management (start, stop, delete, recent, all)
- Favourites timesheets
    - set timesheet as favourite and store it in local database
- Timesheet quick start
- Reports and graphs for activities, projects, customers
    - today, this week, this month
- Multiple Kimai servers support
- Possibility to set default server for automatic sign-in
- Multiple Kimai users support
- Role based time-tracking
    - possibility to show timesheet entries based on user role
- Integration with Gitlab Kimai plugin [GitLabBundle](https://github.com/LibreCodeCoop/GitLabBundle)

### MAUI application features
- Dark/Light mode 
- Form validation with [Fluent validation](https://github.com/FluentValidation/FluentValidation)
- MySQL local database, Secure storage for api passwords
- Graphs by [LiveCharts2](https://github.com/beto-rodriguez/LiveCharts2)

## Limitations
---
- **Kimai v2 is not supported yet**
- Dark/light theme can be only set by setting system-wide dark/light them (is not possible to change it in app yet).
- No support for management of project/customers/teams within app
- Missing support for teams management and team reports

## Installation
---
Since this is early release, only raw files are available. When more features are added, there is a plan to publish apps in Google play and Microsoft store.

### Android

1. Download and manually install `signed apk` file from Github release.

### Windows
1. Download `.msix` file from Github release.

2. Follow installation instructions from [here](https://learn.microsoft.com/en-us/dotnet/maui/windows/deployment/publish-cli?view=net-maui-7.0#installing-the-app).

## What is next 

---

There is following plan to add features:
- Add integration within the team (team management, team reports)
- Add support for Kimai v2 API interface
- Add support for basic user management
- Add possibility to offline time-tracking and synchronize time-tracked timesheets when online again


## Contribution 
---
Feel free to share your ideas and create issues. PR's are welcome.


## License 
This project is licensed under the MIT License - see the LICENSE file for more.

---
<img src="assets/logo.png" alt="logo" width="30"  > 

MAUI for Kimai

Contact: david.spavor@gmail.com