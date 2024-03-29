﻿using MauiForKimai.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Interfaces;
public interface ITimesheetService : IBaseService, ICrudOperations<TimesheetEntity, TimesheetEditForm>
{
     Task<ICollection<TimesheetCollectionExpanded>> GetTimesheetsIncrementalyAsync(int page, int sizePerPage);
     Task<ICollection<TimesheetCollectionExpanded>> GetTodayTimesheetsAsync();
     Task<ICollection<TimesheetCollectionExpanded>> GetTenRecentTimesheetsAsync();
     Task<ICollection<TimesheetCollectionExpanded>> GetTimesheetsForReportsAsync(string begin, string end);
     Task<ICollection<TimesheetCollectionExpanded>> GetActive();
     Task<TimesheetEntity> StopActive(int id);

     Task<TimesheetEntity> SetMetaField(int id, Body5 body);
}
