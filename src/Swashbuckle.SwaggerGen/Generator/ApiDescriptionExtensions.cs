using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Swashbuckle.SwaggerGen.Generator
{
    public static class ApiDescriptionExtensions
    {
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

        public static IEnumerable<object> GetControllerAttributes(this ApiDescription apiDescription)
        {
            var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
            return (actionDescriptor != null)
                ? actionDescriptor.ControllerTypeInfo.GetCustomAttributes(false)
                : Enumerable.Empty<object>();
        }

        public static IEnumerable<object> GetActionAttributes(this ApiDescription apiDescription)
        {
            var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
            return (actionDescriptor != null)
                ? actionDescriptor.MethodInfo.GetCustomAttributes(false)
                : Enumerable.Empty<object>();
        }



        internal static string RelativePathSansQueryString(this ApiDescription apiDescription)
        {
            return apiDescription.RelativePath.Split('?').First();
        }

        internal static bool IsObsolete(this ApiDescription apiDescription)
        {
            return apiDescription.GetActionAttributes().OfType<ObsoleteAttribute>().Any();
        }
    }
}