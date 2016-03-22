using System;

namespace ImportDataFromExternalSystem
{
    /// <summary>
    /// The external news item class.
    /// </summary>
    internal class ExternalNewsItem
    {
        /// <summary>
        /// Gets or sets the title of the news item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the html content of the news item.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the url name of the news item (usually the same as the title).
        /// </summary>
        public string UrlName { get; set; }

        /// <summary>
        /// Gets or sets the author of the news item.
        /// </summary>
        public string Author { get; set; }
    }
}
