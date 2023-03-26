using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient;

public partial interface IApiClient
{
    System.Threading.Tasks.Task<System.Collections.Generic.ICollection<TimesheetCollectionExpanded>> TimesheetsAllExpandedAsync(string user, string customer, string customers, string project, string projects, string activity, string activities, string page, string size, string tags, string orderBy, string order, string begin, string end, string exported, string active, string billable, string full, string term, string modified_after);
    System.Threading.Tasks.Task<System.Collections.Generic.ICollection<TimesheetCollectionExpanded>> TimesheetsAllExpandedAsync(string user, string customer, string customers, string project, string projects, string activity, string activities, string page, string size, string tags, string orderBy, string order, string begin, string end, string exported, string active, string billable, string full, string term, string modified_after, System.Threading.CancellationToken cancellationToken);

}

public partial class ApiClient
{

    public virtual System.Threading.Tasks.Task<System.Collections.Generic.ICollection<TimesheetCollectionExpanded>> TimesheetsAllExpandedAsync(string user, string customer, string customers, string project, string projects, string activity, string activities, string page, string size, string tags, string orderBy, string order, string begin, string end, string exported, string active, string billable, string full, string term, string modified_after)
    {
        return TimesheetsAllExpandedAsync(user, customer, customers, project, projects, activity, activities, page, size, tags, orderBy, order, begin, end, exported, active, billable, full, term, modified_after, System.Threading.CancellationToken.None);
    }
     /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// Returns a collection of timesheet records
        /// </summary>
        /// <param name="user">User ID to filter timesheets. Needs permission 'view_other_timesheet', pass 'all' to fetch data for all user (default: current user)</param>
        /// <param name="customer">DEPRECATED: Customer ID to filter timesheets (will be removed with 2.0)</param>
        /// <param name="customers">Comma separated list of customer IDs to filter timesheets</param>
        /// <param name="project">DEPRECATED: Project ID to filter timesheets (will be removed with 2.0)</param>
        /// <param name="projects">Comma separated list of project IDs to filter timesheets</param>
        /// <param name="activity">DEPRECATED: Activity ID to filter timesheets (will be removed with 2.0)</param>
        /// <param name="activities">Comma separated list of activity IDs to filter timesheets</param>
        /// <param name="page">The page to display, renders a 404 if not found (default: 1)</param>
        /// <param name="size">The amount of entries for each page (default: 50)</param>
        /// <param name="tags">Comma separated list of tag names</param>
        /// <param name="orderBy">The field by which results will be ordered. Allowed values: id, begin, end, rate (default: begin)</param>
        /// <param name="order">The result order. Allowed values: ASC, DESC (default: DESC)</param>
        /// <param name="begin">Only records after this date will be included (format: HTML5)</param>
        /// <param name="end">Only records before this date will be included (format: HTML5)</param>
        /// <param name="exported">Use this flag if you want to filter for export state. Allowed values: 0=not exported, 1=exported (default: all)</param>
        /// <param name="active">Filter for running/active records. Allowed values: 0=stopped, 1=active (default: all)</param>
        /// <param name="billable">Filter for non-/billable records. Allowed values: 0=non-billable, 1=billable (default: all)</param>
        /// <param name="full">Allows to fetch fully serialized objects including subresources. Allowed values: true (default: false)</param>
        /// <param name="term">Free search term</param>
        /// <param name="modified_after">Only records changed after this date will be included (format: HTML5). Available since Kimai 1.10 and works only for records that were created/updated since then.</param>
        /// <returns>Returns a collection of timesheets records. Be aware that the datetime fields are given in the users local time including the timezone offset via ISO 8601.</returns>
        /// <exception cref="KiamiApiException">A server side error occurred.</exception>
        public virtual async System.Threading.Tasks.Task<System.Collections.Generic.ICollection<TimesheetCollectionExpanded>> TimesheetsAllExpandedAsync(string user, string customer, string customers, string project, string projects, string activity, string activities, string page, string size, string tags, string orderBy, string order, string begin, string end, string exported, string active, string billable, string full, string term, string modified_after, System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append("api/timesheets?");
            if (user != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("user") + "=").Append(System.Uri.EscapeDataString(ConvertToString(user, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (customer != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("customer") + "=").Append(System.Uri.EscapeDataString(ConvertToString(customer, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (customers != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("customers") + "=").Append(System.Uri.EscapeDataString(ConvertToString(customers, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (project != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("project") + "=").Append(System.Uri.EscapeDataString(ConvertToString(project, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (projects != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("projects") + "=").Append(System.Uri.EscapeDataString(ConvertToString(projects, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (activity != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("activity") + "=").Append(System.Uri.EscapeDataString(ConvertToString(activity, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (activities != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("activities") + "=").Append(System.Uri.EscapeDataString(ConvertToString(activities, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (page != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("page") + "=").Append(System.Uri.EscapeDataString(ConvertToString(page, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (size != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("size") + "=").Append(System.Uri.EscapeDataString(ConvertToString(size, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (tags != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("tags") + "=").Append(System.Uri.EscapeDataString(ConvertToString(tags, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (orderBy != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("orderBy") + "=").Append(System.Uri.EscapeDataString(ConvertToString(orderBy, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (order != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("order") + "=").Append(System.Uri.EscapeDataString(ConvertToString(order, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (begin != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("begin") + "=").Append(System.Uri.EscapeDataString(ConvertToString(begin, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (end != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("end") + "=").Append(System.Uri.EscapeDataString(ConvertToString(end, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (exported != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("exported") + "=").Append(System.Uri.EscapeDataString(ConvertToString(exported, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (active != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("active") + "=").Append(System.Uri.EscapeDataString(ConvertToString(active, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (billable != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("billable") + "=").Append(System.Uri.EscapeDataString(ConvertToString(billable, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (full != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("full") + "=").Append(System.Uri.EscapeDataString(ConvertToString(full, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (term != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("term") + "=").Append(System.Uri.EscapeDataString(ConvertToString(term, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (modified_after != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("modified_after") + "=").Append(System.Uri.EscapeDataString(ConvertToString(modified_after, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<System.Collections.Generic.ICollection<TimesheetCollectionExpanded>>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new KiamiApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new KiamiApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

}
