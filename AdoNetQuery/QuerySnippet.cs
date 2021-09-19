/* 
 * DBO-Tools collection.
 * ADO.NET Query tool.
 * Simple application to execute SQL queries via ADO.NET interface.
 * 
 * Query snippet holder.
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace AdoNetQuery
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
