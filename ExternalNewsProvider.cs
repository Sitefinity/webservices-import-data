using System;
using System.Collections.Generic;

namespace ImportDataFromExternalSystem
{
    /// <summary>
    /// A sample mock provider for fetching the data from an external system.
    /// </summary>
    internal class ExternalNewsProvider
    {
        public IEnumerable<ExternalNewsItem> GetNews()
        {
            var ret = new List<ExternalNewsItem>();

            for (var i = 0; i < 10; i++)
            {
                var toImport = new ExternalNewsItem()
                {
                    Title = Guid.NewGuid().ToString(),
                    Content = Guid.NewGuid().ToString(),
                    UrlName = Guid.NewGuid().ToString(),
                    Author = Guid.NewGuid().ToString(),
                };

                ret.Add(toImport);
            }

            return ret;
        }
    }
}
