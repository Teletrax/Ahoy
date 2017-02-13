using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Swashbuckle.AspNetCore.SwaggerGen
{
    public static class ApiDescriptionExtensions
    {
        public static IEnumerable<object> ControllerAttributes(this ApiDescription apiDescription)
        {
            var controllerActionDescriptor = apiDescription.ControllerActionDescriptor();
            return (controllerActionDescriptor == null)
                ? Enumerable.Empty<object>()
                : controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(false);
        }

        public static IEnumerable<object> ActionAttributes(this ApiDescription apiDescription)
        {
            var controllerActionDescriptor = apiDescription.ControllerActionDescriptor();
            return (controllerActionDescriptor == null)
                ? Enumerable.Empty<object>()
                : controllerActionDescriptor.MethodInfo.GetCustomAttributes(false);
        }

        internal static string ControllerName(this ApiDescription apiDescription)
        {
            var controllerActionDescriptor = apiDescription.ControllerActionDescriptor();
            return (controllerActionDescriptor == null) ? null : controllerActionDescriptor.ControllerName;
        }

        internal static string FriendlyId(this ApiDescription apiDescription)
        {
            var parts = (apiDescription.RelativePathSansQueryString() + "/" + apiDescription.HttpMethod.ToLower())
                .Split('/');

            var builder = new StringBuilder();
            foreach (var part in parts) 
            {
                var trimmed = part.Trim('{', '}');
                builder.AppendFormat("{0}{1}",
                    (part.StartsWith("{") ? "By" : string.Empty),
                    trimmed.ToTitleCase()
                );
            }

            return builder.ToString();
        }

        internal static string RelativePathSansQueryString(this ApiDescription apiDescription)
        {
            return apiDescription.RelativePath.Split('?').First();
        }

        internal static IEnumerable<string> SupportedRequestMediaTypes(this ApiDescription apiDescription)
        {
            if (apiDescription.SupportedRequestFormats.Any())
                return apiDescription.SupportedRequestFormats
                    .Select(requestFormat => requestFormat.MediaType);
            var action = apiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (action==null)
                return Enumerable.Empty<string>();
            return action.MethodInfo.GetCustomAttributes<ConsumesAttribute>().SelectMany(a => a.ContentTypes);
        }

        internal static IEnumerable<string> SupportedRequestMediaTypes(this IEnumerable<ApiDescription> apiDescriptions)
        {
            return apiDescriptions.SelectMany(a => a.SupportedRequestMediaTypes());
        }

        internal static IEnumerable<string> SupportedResponseMediaTypes(this ApiDescription apiDescription)
        {
            return apiDescription.SupportedResponseTypes
                .SelectMany(responseType => responseType.ApiResponseFormats)
                .Select(responseFormat => responseFormat.MediaType)
                .Distinct();
        }

        internal static IEnumerable<string> SupportedResponseMediaTypes(this IEnumerable<ApiDescription> apiDescriptions)
        {
            return apiDescriptions.SelectMany(a=>a.SupportedResponseMediaTypes());
        }

        internal static bool IsObsolete(this ApiDescription apiDescription)
        {
            return apiDescription.ActionAttributes().OfType<ObsoleteAttribute>().Any();
        }

        private static ControllerActionDescriptor ControllerActionDescriptor(this ApiDescription apiDescription)
        {
            var controllerActionDescriptor = apiDescription.GetProperty<ControllerActionDescriptor>();
            if (controllerActionDescriptor == null)
            {
                controllerActionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
                apiDescription.SetProperty(controllerActionDescriptor);
            }
            return controllerActionDescriptor; 
        }

    }
}