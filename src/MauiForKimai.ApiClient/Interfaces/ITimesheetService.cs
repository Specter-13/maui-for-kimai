using MauiForKimai.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Interfaces;
public interface ITimesheetService : IBaseService, ICrudOperations<TimesheetEntity, TimesheetEditForm>
{
     Task<ICollection<TimesheetCollectionExpanded>> GetTimesheetsIncrementalyAsync(int page, int sizePerPage);
     Task<ICollection<TimesheetCollectionExpanded>> GetTenRecentTimesheetsAsync();

     Task<ICollection<TimesheetCollectionExpanded>> GetActive();
     Task<TimesheetEntity> StopActive(int id);
}
