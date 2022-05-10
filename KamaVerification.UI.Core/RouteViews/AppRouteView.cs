using KamaVerification.UI.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Net;

namespace KamaVerification.UI.Core.RouteViews
{
    public class AppRouteView : RouteView
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public ICustomerRepository? CustomerRepository { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            var authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;
            if (authorize && CustomerRepository?.Customer == null)
            {
                var returnUrl = WebUtility.UrlEncode(new Uri(NavigationManager!.Uri).PathAndQuery);
                NavigationManager.NavigateTo($"customer/login?returnUrl={returnUrl}");
            }
            else
            {
                base.Render(builder);
            }
        }
    }
}
