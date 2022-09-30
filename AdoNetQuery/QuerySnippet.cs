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
        /// <summary>Name of code/SQL snippet</summary>
        public string Name { get; set; }

        /// <summary>Description of snippet</summary>
        public string Description { get; set; }

        /// <summary>The source code of snippet</summary>
        public string Query { get; set; }

        /// <summary>Parameters for snippet</summary>
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
