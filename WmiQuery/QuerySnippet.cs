/* 
 * DBO-Tools collection.
 * WMI Query tool.
 * Simple application to execute WQL/WMI queries.
 * 
 * Query snippet holder.
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace WmiQuery
{
    public class QuerySnippet
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Query { get; set; }
        public string Parameters { get; set; }

        public void Assign(QuerySnippet pSource)
        {
            Name = pSource.Name;
            Description = pSource.Description;
            Query = pSource.Query;
            Parameters = pSource.Parameters;
        }
    }
}
