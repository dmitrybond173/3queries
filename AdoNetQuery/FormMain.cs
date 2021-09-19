/* 
 * DBO-Tools collection.
 * ADO.NET Query tool.
 * Simple application to execute SQL queries via ADO.NET interface.
 * 
 * Application MainForm UI.
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using XService.Utils;
using PAL;
using XService.Data;
using XService.UI;
using XService.UI.CommonForms;
using AdoNetQuery.Properties;
using XService.Snippets;

namespace AdoNetQuery
{
    public partial class FormMain : Form
    {
        public static TraceSwitch TrcLvl = new TraceSwitch("TraceLevel", "TraceLevel");

		private const string HELP_LICENSE_TEXT =
			"ADO.NET Query. \r\n" +
			" \r\n" +
			"Copyright (c) 2012-2021 Dmitry Bondarenko, Kyiv, Ukraine                        \r\n" +
			" \r\n" +
            "Distributed under MIT License\r\n" +
			"                                                                                \r\n" +
			"Welcome to send your feedbacks, bug reports, sugestiongs to dima_ben@ukr.net    \r\n" +
			"or to Dmitry_Bond@hotmail.com                                                   \r\n" +
			"";

        private const string HELP_TEXT_USAGE =
            "Usage: $(AppName) [options]\r\n" +
            "";

        private const string HELP_TEXT =
            HELP_TEXT_USAGE +
            "\r\n" +
            "Supported options:\r\n" +
            "  -?  - print this message\r\n" +
            "  -copyright, -eula  - print a license agreement\r\n" +
            "  -cp, --conn-param={text}  - add specified line to connection parameters\r\n" +
            "  -eev, --expand-env-vars[=1|0]  - set Expand Environment Variables flag\r\n" +
            "  -exec, --execute  - execute current query with current ADO.NET parameters\r\n" +
            "  -exit - exit application\r\n" +
            "  -hc, --hold-connection  - switch hold-connection flag\r\n" +
            "  -q, --query={SqlQuery}  - SQL query to execute\r\n" +
            "  -qf, --query-file={filename}  - filename with SQL queries to execute\r\n" +
            "  -ite, --ignore-to-end  - ignore all cmdline parameters after this parameter\r\n" +
            "  -rcp, --reset-conn-params  - cleanup all connection parameters\r\n" +
            "  -sn, --snippet={SnippetName}  - choose snippet with specified name\r\n" +
            "  -snqry, --snippet-query={SnippetName}  - use query from snippet with specified name\r\n" +
            "  -snprm, --snippet-params={SnippetName}  - use parameters from snippet with specified name\r\n" +
            "  -sprm, --set-param={name,value}  - set value of specified parameter or add if parameter not found\r\n" +
            "  -srx, --save-result-xml={filename}  - same query results into specified XML file\r\n" +
            "  -src, --save-result-csv={filename}  - same query results into specified CSV file\r\n" +
            "\r\n" +
            "Example:\r\n" +
            "  AdoNetQuery.exe -query=\"SELECT userid FROM usr ORDER BY userid\" -execute -save-result-csv=users-list.txt -exit\r\n" +
            "  AdoNetQuery.exe -snippet=\"ListOfUsers\" -execute -save-result-csv=users-list.txt -exit\r\n" +
            "\r\n" +
            "";

        public FormMain(string[] pArgs)
        {
            this.args = pArgs;
            InitializeComponent();
        }

        private const string STR_PROVIDER_AND_CS_DELIM = "~";

        private string[] args;
        private int maxLoggerItems = 300;
        private bool executing = false;
        private List<string> csUsedParams = new List<string>();
        private DataTable dataObject;
        private Dictionary<string, string> queryParams = new Dictionary<string, string>();
        private XmlRegistryKey regKeySavedQueries;
        private bool lockDbConnection = false;
        private DbProviderFactory dbFactory = null;
        private DbConnection dbConnection = null;
        private string connectionString = null;
        private DataTable adoNetProviders = null;
        private ExecutionContext execCtx = null;
        private ManualResetEvent evtExecPrepared = new ManualResetEvent(false);
        private ManualResetEvent evtExec = new ManualResetEvent(false);
        private ManualResetEvent evtDataReady = new ManualResetEvent(false);
        private SnippetsStorage snippets;
        private string snippetsRootKeyPath = "Snippets";
        private string userConfigPath = null;

        private static string DEFAULT_Parameters = ""
            + "AdoNetProviderName = System.Data.OleDb" + Environment.NewLine
            + "ConnectionString = Provider=IBMDADB2.1; Location=$(DbServer):50000; Data Source=$(DbName); User ID=$(DbUser); Password=$(DbPassword);" + Environment.NewLine
            + "" + Environment.NewLine
            + "DbName = PT2010" + Environment.NewLine
            + "DbUser = myDbUzver" + Environment.NewLine
            + "DbPassword = myDbPazzworD" + Environment.NewLine
            + "DbServer = vmDemoProjX64" + Environment.NewLine
            + "" + Environment.NewLine
            + "";

        private void displayUsefulLinks()
        {
            Dictionary<string, string> links = (Dictionary<string, string>)ConfigurationManager.GetSection("UsefulLinks");
            foreach (KeyValuePair<string, string> link in links)
            {
                ToolStripItem mi = mnuUsefulLinks.DropDownItems.Add(link.Key);
                mi.Tag = link.Value;
                mi.Click += this.miUsefulLink_Click;
            }
        }

        private void displayConnectionStrings()
        {
            Dictionary<string, string> links = (Dictionary<string, string>)ConfigurationManager.GetSection("ConnectionStrings");
            foreach (KeyValuePair<string, string> link in links)
            {
                ToolStripItem mi = mnuConnectionStrings.DropDownItems.Add(link.Key);
                mi.Tag = link.Value;
                mi.Click += this.miConnectionString_Click;
            }
        }

        private string collectPrmName(string pParamName)
        {
            csUsedParams.Add(pParamName);
            return "";
        }

        private void miConnectionString_Click(object sender, EventArgs e)
        {
            ToolStripItem mi = (ToolStripItem)sender;
            string s = mi.Tag.ToString();
            string prov = StrUtils.GetToPattern(s, STR_PROVIDER_AND_CS_DELIM);
            string cs = StrUtils.GetAfterPattern(s, STR_PROVIDER_AND_CS_DELIM);

            updateParameterInGui("AdoNetProviderName", prov);
            updateParameterInGui("ConnectionString", cs);

            csUsedParams.Clear();
            CollectionUtils.ReplaceParameters(s, "$(,)", this.collectPrmName);
            foreach (string prm in this.csUsedParams)
            {
                updateParameterInGui(prm, null, true);
            }
        }

        private void executeQuery()
        {
			lock (this)
			{
				if (this.executing)
				{
					Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
						"! Application is already executing query!") : "");
					return;
				}

                pmiResizeByHeader.Checked = false;
                pmiResizeByValue.Checked = false;

                this.updateText(stLab1, "Executing...");
				this.updateText(stLab3, "Started at " + StrUtils.NskTimestampOf(DateTime.Now));
                submitUiCommand(EUiCommand.ExecuteButtonState, false);

				// Note: it seems that exactly this call is affecting if it will work! :-\
				updateDataView(this, null);

				this.executing = true;

                if (this.execCtx == null)
                {
                    evtExecPrepared.Reset();
                    submitUiCommand(EUiCommand.PrepareExecutionContext, null);
                    waitFor(evtExecPrepared);
                }
                
                this.evtExec.Reset();
                this.evtDataReady.Reset();
                ThreadPool.QueueUserWorkItem(this.doExecuteQuery, this.execCtx);
			}
        }

        private void runConnectionStringBuilder()
        {
            try
            {
                string provId, paramsText = txtParams.Text;
                paramsText = StrUtils.TrimText(paramsText, ETrimOption.Both);
                paramsText = removeCommentedLines(paramsText, "//");
                // need to replace all EOLs with '\n' only!
                paramsText = paramsText.Replace("\r\n", "\n").Replace('\r', '\n');
                paramsText = paramsText.Trim(StrUtils.CH_SPACES);

                Dictionary<string, string> prms = new Dictionary<string, string>();
                CollectionUtils.ParseParametersStrEx(prms, paramsText, true, '\n', "=");

                bool hasNamedValue = prms.TryGetValue("adonetprovidername", out provId);
                if (hasNamedValue)
                    hasNamedValue = !string.IsNullOrEmpty(provId.Trim(StrUtils.CH_SPACES));
                if (hasNamedValue)
                {
                    provId = provId.Trim(StrUtils.CH_SPACES);
                    initAdoNetProvider(provId);
                }

                string cs = "";
                hasNamedValue = prms.TryGetValue("connectionstring", out cs);

                DbConnectionStringBuilder builder = this.dbFactory.CreateConnectionStringBuilder();
                if (builder == null)
                    throw new ToolError(string.Format("ADO.NET Provider ({0}) does not support ConnectionStringBuilder!", provId));

                if (FormCsBuilder.Execute(this, provId, ref cs, builder))
                {
                    updateParameterInGui("ConnectionString", cs);
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? String.Format(
                    "   * ConnectionStringBuilder->{0}\n{1}",
                    ErrorUtils.FormatErrorMsg(exc), ErrorUtils.FormatStackTrace(exc)
                    ) : "");
            }
        }

        private void updateParameterInGui(string pPrmName, string pValue)
        {
            updateParameterInGui(pPrmName, pValue, false);
        }

        private void updateParameterInGui(string pPrmName, string pValue, bool pAutoAdd)
        {
            bool isChanged = false;
            bool isFound = false;
            string text = "";
            string[] lines = txtParams.Text.Replace(Environment.NewLine, "\n").Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim(StrUtils.CH_SPACES);
                if (string.IsNullOrEmpty(line))
                {
                    text += (lines[i] + Environment.NewLine);
                    continue;
                }

                string name = StrUtils.GetToPattern(line, "=").Trim(StrUtils.CH_SPACES);
                if (StrUtils.IsSameText(name, pPrmName))
                {
                    isFound = true;
                    if (pValue != null)
                    {
                        lines[i] = pPrmName + "=" + pValue.Trim(StrUtils.CH_SPACES);
                        isChanged = true;
                    }
                }

                text += (lines[i] + Environment.NewLine);
            }
            if (pAutoAdd && !isFound)
            {
                text = text.TrimEnd(Environment.NewLine.ToCharArray()) + (Environment.NewLine + pPrmName + " = ");
                isChanged = true;
            }
            if (isChanged)
                txtParams.Text = text;
        }

        private delegate void UpdateTextMethod(object pSender, string pData);

        private void updateText(object pSender, string pData)
        {
            if (this.InvokeRequired)
            {                
                this.BeginInvoke(new UpdateTextMethod(this.updateText), pSender, pData);
            }
            else
            {
                bool isAdd = pData.StartsWith("+");
                if (isAdd) pData = pData.Remove(0, 1);

                if (pSender is Control)
                {
                    Control ctrl = (Control)pSender;
                    if (isAdd)
                        ctrl.Text += pData;
                    else
                        ctrl.Text = pData;
                    ctrl.Invalidate();
                }
                else if (pSender is ToolStripItem)
                {
                    ToolStripItem ctrl = (ToolStripItem)pSender;
                    if (isAdd)
                        ctrl.Text += pData;
                    else
                        ctrl.Text = pData;
                    ctrl.Invalidate();
                }
            }
        }

        private delegate void UpdateDataViewMethod(object pSender, DataTable pData);

        private void updateDataView(object pSender, DataTable pData)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateDataViewMethod(this.updateDataView), pSender, this.dataObject);
            }
            else
            {
                if (pData == null)
                {
                    this.bsData.DataMember = null;
                    this.bsData.DataSource = null;
                    this.bsData.ResetBindings(true);

                    if (this.dataObject != null)
                    {
                        this.dataObject.Clear();
                        CommonUtils.DisposeObject(this.dataObject);
                        this.dataObject = null;

                        /*
                        // remove old DataTable from DataSet
                        int n = this.ds.Tables.IndexOf("Record");
                        if (n >= 0)
                            this.ds.Tables.RemoveAt(n);
                        */
                    }
                }

                if (pData != null)
                {
                    Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                        "= Updating DataGridView...") : "");

                    dgvResultset.DataSource = this.dataObject;

                    Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                        "= DataGridView Updated.") : "");

                    this.executing = false;

                    stLab1.Text = "Ready.";
                    stLab3.Text = string.Format("{0} columns x {1} rows. Loaded at {2}.", 
                        this.dataObject.Columns.Count, this.dataObject.Rows.Count, StrUtils.NskTimestampOf(DateTime.Now));
                    stbarMain.Invalidate();

                    this.evtDataReady.Set();
                    Thread.Sleep(10);
                }
                dgvResultset.Invalidate();
            }
        }

        private DbConnection createSearcher(string pConnStr)
        {
            Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                "Connecting({0}): {1} ...", this.dbFactory, this.connectionString) : "");
            
            DbConnection conn = this.dbFactory.CreateConnection();
            conn.ConnectionString = this.connectionString;
            conn.Open();

            return conn;
        }

        private void doExecuteQuery(object pState)
        {
            Thread.CurrentThread.Name = "doExecuteQuery";
            DateTime t1 = DateTime.Now;
            try
            {
                try
                {
                    ExecutionContext ctx = (ExecutionContext)pState;
                    string s, cmdText = ctx.CmdText;
                    cmdText = StrUtils.TrimText(cmdText, ETrimOption.Both);
                    // assume for query we supporting both comments styles - with // (as for C/C++) and with -- (as for SQL)
                    cmdText = removeCommentedLines(cmdText, "--");
                    cmdText = removeCommentedLines(cmdText, "//");
                    cmdText = cmdText.Trim(StrUtils.CH_SPACES);
                    cmdText = cmdText.Replace("\r\n", " ");

                    if (this.execCtx.ExpandEnvVars)
                        cmdText = Environment.ExpandEnvironmentVariables(cmdText);

                    string paramsText = ctx.Params;
                    paramsText = StrUtils.TrimText(paramsText, ETrimOption.Both);
                    paramsText = removeCommentedLines(paramsText, "//");
                    // need to replace all EOLs with '\n' only!
                    paramsText = paramsText.Replace("\r\n", "\n").Replace('\r', '\n');
                    paramsText = paramsText.Trim(StrUtils.CH_SPACES);

                    if (this.execCtx.ExpandEnvVars)
                        paramsText = Environment.ExpandEnvironmentVariables(paramsText);

                    Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                        " - Execute:\n\tquery: {0} \n\tparams: {1}", cmdText, paramsText) : "");

                    this.queryParams.Clear();
                    this.connectionString = "";
                    CollectionUtils.ParseParametersStrEx(this.queryParams, paramsText, true, '\n', "=");

                    //if (this.queryParams.TryGetValue("expandenvironmentvars", out s))

                    cmdText = ensureExpandMacroValues(cmdText);
                    Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(" - Final Query: {0}", cmdText) : "");

                    // do this to cleanup DataGridView from any data before executing a new query
                    this.updateDataView(this, null);
                    Thread.Sleep(20); // to ensure data cleaned up

                    bool hasLockedConnection = (this.lockDbConnection && this.dbConnection != null);

                    if (!hasLockedConnection)
                    {
                        bool hasProvider = this.queryParams.TryGetValue("adonetprovidername", out s);
                        if (hasProvider)
                            hasProvider = !string.IsNullOrEmpty(s.Trim(StrUtils.CH_SPACES));
                        if (hasProvider)
                        {
                            s = s.Trim(StrUtils.CH_SPACES);
                            initAdoNetProvider(s);
                        }

                        bool hasConnStr = this.queryParams.TryGetValue("connectionstring", out s);
                        if (hasConnStr)
                            hasConnStr = !string.IsNullOrEmpty(s.Trim(StrUtils.CH_SPACES));
                        if (hasConnStr)
                        {
                            this.connectionString = s.Trim(StrUtils.CH_SPACES);
                            this.connectionString = ensureExpandMacroValues(this.connectionString);
                        }
                        if (string.IsNullOrEmpty(this.connectionString))
                        {
                            Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? String.Format(
                                "! Connection string is empty! Cannot continue.") : "");
                            return;
                        }
                        if (string.IsNullOrEmpty(cmdText))
                        {
                            Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? String.Format(
                                "! Query string is empty! Cannot continue.") : "");
                            return;
                        }
                        
                        Thread.Sleep(20); // need a pause to resolve ThreadPool jobs before executintg new query

                        // execute a query
                        if (this.lockDbConnection)
                        {
                            Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                                "! Create and save DbConnection...") : "");
                            this.dbConnection = createSearcher(cmdText);
                            using (DbCommand cmd = this.dbConnection.CreateCommand())
                            {
                                cmd.CommandText = cmdText;
                                ensureLoadDbParameters(cmd);

                                // this will load data from ManagementObjectCollection into a DataTable object in the "ds" DataSet
                                loadData(cmd);

                                Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                                    "= Loaded dataset has {0} columns x {1} rows", this.dataObject.Columns.Count, this.dataObject.Rows.Count) : "");

                                // display data in DataGridView
                                this.updateDataView(this, this.dataObject);
                            }
                        }
                        else
                        {
                            using (this.dbConnection = createSearcher(cmdText))
                            {
                                using (DbCommand cmd = this.dbConnection.CreateCommand())
                                {
                                    cmd.CommandText = cmdText;
                                    ensureLoadDbParameters(cmd);

                                    // this will load data from ManagementObjectCollection into a DataTable object in the "ds" DataSet
                                    loadData(cmd);

                                    Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                                        "= Loaded dataset has {0} columns x {1} rows", this.dataObject.Columns.Count, this.dataObject.Rows.Count) : "");

                                    // display data in DataGridView
                                    this.updateDataView(this, this.dataObject);
                                }
                            }
                            this.dbConnection = null;
                        }
                    }
                    else 
                    {
                        Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                            "! Re-use saved connection...") : "");
                        using (DbCommand cmd = this.dbConnection.CreateCommand())
                        {
                            cmd.CommandText = cmdText;
                            ensureLoadDbParameters(cmd);

                            // this will load data from ManagementObjectCollection into a DataTable object in the "ds" DataSet
                            loadData(cmd);

                            Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                                "= Loaded dataset has {0} columns x {1} rows", this.dataObject.Columns.Count, this.dataObject.Rows.Count) : "");

                            // display data in DataGridView
                            this.updateDataView(this, this.dataObject);
                        }
                    }
                }
                catch (Exception exc)
                {
                    this.evtDataReady.Set(); // to resolve wait-lock
                    Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? String.Format(
                        "   * ExecuteQuery->{0}\n{1}",
                        ErrorUtils.FormatErrorMsg(exc), ErrorUtils.FormatStackTrace(exc)
                        ) : "");
                }
            }
            finally 
            { 
                this.executing = false; 
            }
            DateTime t2 = DateTime.Now;
            this.updateText(stLab3, string.Format("+ Elapsed time {0} ms", (t2-t1).TotalMilliseconds));
            this.execCtx = null; // need to reset context after usage
            this.evtExec.Set();
            submitUiCommand(EUiCommand.ExecuteButtonState, true);
        }

        private void waitFor(EventWaitHandle pEvt)
        {
            string id = "???";
            if (pEvt.Equals(this.evtExecPrepared)) id = "ExecCtx";
            if (pEvt.Equals(this.evtExec)) id = "Exec";
            if (pEvt.Equals(this.evtDataReady)) id = "Data";
            int waitCount = 0;
            DateTime t1 = DateTime.Now;
            while (!pEvt.WaitOne(1))
            {
                waitCount++;
                if ((waitCount % 50) == 0)
                    Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(" . Waiting for {0} (#{1})", id, waitCount) : "");
                Thread.Sleep(100);
            }
            DateTime t2 = DateTime.Now;
            Trace.WriteLineIf(TrcLvl.TraceWarning, TrcLvl.TraceWarning ? String.Format(" . Wait of [{0}] completed: counter = {1}, time = {2} sec", 
                id, waitCount, (t2 - t1).TotalSeconds.ToString("N2")) : "");
        }

        private void ensureLoadDbParameters(DbCommand pCmd)
        {
            // 
            foreach (KeyValuePair<string, string> item in this.queryParams)
            {
                string key = item.Key.Trim(StrUtils.CH_SPACES);
                bool isSqlPrm = (key.ToLower().StartsWith("sql.prm.") || key.ToLower().StartsWith("sql.param."));
                if (!isSqlPrm) continue;

                if (key.ToLower().StartsWith("sql.prm."))
                    key = key.Remove(0, 8).Trim(StrUtils.CH_SPACES);
                else
                    key = key.Remove(0, 10).Trim(StrUtils.CH_SPACES);

                string prmName = key;
                DbParameter dbPrm = this.dbFactory.CreateParameter();
                if (!prmName.StartsWith("#"))
                    dbPrm.ParameterName = prmName;

                string value = item.Value.Trim(StrUtils.CH_SPACES);
                int p = value.IndexOf(':');
                if (p >= 0)
                {
                    string s = StrUtils.GetToPattern(value, ":");
                    value = StrUtils.GetAfterPattern(value, ":");
                    dbPrm.DbType = (DbType)Enum.Parse(typeof(DbType), s, true);
                }
                else
                    dbPrm.DbType = DbType.String;

                dbPrm.Value = value;

                pCmd.Parameters.Add(dbPrm);
                Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                    "   * Add SQL parameter[ {0} ]: {1} = {2}", dbPrm.ParameterName, dbPrm.DbType, dbPrm.Value.ToString()) : "");
            }
        }

        private void listAdoNetProviders()
        {
            string prov = "";
            if (FormDbProviders.Execute(this, ref prov))
            {
                updateParameterInGui("AdoNetProviderName", prov);
            }
        }

        private void initAdoNetProvider(string pProviderName)
        {
            string savedProviderName = pProviderName;
            try
            {
                this.dbFactory = DbProviderFactories.GetFactory(pProviderName);
            }
            catch (Exception exc)
            {
                Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? String.Format(
                    "   * InitAdoNetProvider->{0}", ErrorUtils.FormatErrorMsg(exc) ) : "");

                pProviderName = clarifyProviderName(pProviderName);
                if (pProviderName == null || StrUtils.IsSameStr(savedProviderName, pProviderName))
                    throw ;

                Trace.WriteLineIf(TrcLvl.TraceWarning, TrcLvl.TraceWarning ? String.Format(
                    "   ! Correct ADO.Net Provider name must be \"{0}\" but not the \"{1}\"", 
                    pProviderName, savedProviderName) : "");

                // 2nd attempt
                initAdoNetProvider(pProviderName);

                updateParameterInGui("AdoNetProviderName", pProviderName);
                Trace.WriteLineIf(TrcLvl.TraceWarning, TrcLvl.TraceWarning ? String.Format(
                    "   ! AdoNetProviderName was force changed to \"{0}\"!", pProviderName) : "");
            }
        }

        private string clarifyProviderName(string pProviderName)
        {
            if (this.adoNetProviders == null)
                this.adoNetProviders = DbProviderFactories.GetFactoryClasses();

            foreach (DataRow row in this.adoNetProviders.Rows)
            {
                string id = row["InvariantName"].ToString();
                if (StrUtils.IsSameText(pProviderName, id))
                {
                    return id;
                }
            }
            return null;
        }

        private void loadData(DbCommand pCmd)
        {
            this.dataObject = new DataTable("Record");
            Thread.Sleep(10);

            if (this.dataObject == null)
            {
                this.dataObject = new DataTable("Record"); 
            }

            this.dataObject.Clear();
            this.dataObject.Columns.Clear();

            using (DbDataAdapter da = this.dbFactory.CreateDataAdapter())
            {
                da.SelectCommand = pCmd;
                da.Fill(this.dataObject);
            }

			ensureToCreateCusomColumns();
            Thread.Sleep(10);
        }

        private void displayData(DataTable pTable)
        {
            this.dataObject = pTable;
            Thread.Sleep(10);

            ensureToCreateCusomColumns();
            Thread.Sleep(10);

            // display data in DataGridView
            this.updateDataView(this, this.dataObject);
        }

        private void ensureToCreateCusomColumns()
		{
			foreach (KeyValuePair<string, string> prm in this.queryParams)
			{
				if (prm.Key.ToUpper().StartsWith("TABLE.COLUMN."))
				{
					// table.column.NAME(int) = expression
					Type colType = typeof(string);
					string colName = prm.Key.Remove(0, "TABLE.COLUMN.".Length).Trim(StrUtils.CH_SPACES);
					if (colName.IndexOf("(") >= 0 && colName.IndexOf(")") >= 0)
					{ 
						string tn = StrUtils.GetToPattern(StrUtils.GetAfterPattern(colName, "("), ")").Trim(StrUtils.CH_SPACES);
						colName = StrUtils.GetToPattern(colName, "(").Trim(StrUtils.CH_SPACES);
						colType = AppUtils.StrToType(tn, colType);
					}
					string expr = prm.Value.Trim(StrUtils.CH_SPACES);

					// may need to fix column name to keep original characters case in a name
					string s = AppUtils.FindPrmLine(txtParams.Text, prm.Key);
					if (!string.IsNullOrEmpty(s))
					{
						s = StrUtils.GetToPattern(s, "=");
						s = s.Remove(0, "TABLE.COLUMN.".Length).Trim(StrUtils.CH_SPACES);
						colName = s;
					}
					
					Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
						" ! Table.AddColumn( {0}: {1}) as \"{2}\"", colName, colType, expr) : "");
					DataColumn dc = this.dataObject.Columns.Add(colName, colType);
					dc.Expression = expr;
				}
			}
		}

        private void writeLogger(string pMessage, bool pWriteLine)
        {
            string ts = StrUtils.NskTimestampOf(DateTime.Now).Substring(11, 12);

            while (lvLogger.Items.Count > maxLoggerItems)
            {
                lvLogger.Items.RemoveAt(0);
            }

            ListViewItem li = null;
            if (!pWriteLine)
            {
                if (lvLogger.Items.Count > 0)
                {
                    li = lvLogger.Items[lvLogger.Items.Count - 1];
                    li.SubItems[0].Text = ts;
                    li.SubItems[1].Text += pMessage;
                }
                else
                    pWriteLine = true;
            }
            if (pWriteLine)
            {
                if (pMessage.IndexOf('\n') >= 0)
                {
                    string[] lines = pMessage.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
                    foreach (string line in lines)
                    {
                        li = new ListViewItem(new string[] { ts, line });
                        lvLogger.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListViewItem(new string[] { ts, pMessage });
                    lvLogger.Items.Add(li);
                }
            }
            if (li != null)
                li.EnsureVisible();
        }

        private void onTraceWrite(string pMessage, bool pWriteLine)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CallbackTraceListener.OnWriteProc(this.writeLogger), pMessage, pWriteLine);
            }
            else
            {
                this.writeLogger(pMessage, pWriteLine);
            }
        }

        private string ensureExpandMacroValues(string pText)
        {
            return StrUtils.ExpandParameters(pText, this.queryParams, true);
        }

        private string removeCommentedLines(string pText, string pCommentPrefix)
        {
            pText = pText.Trim(StrUtils.CH_SPACES);
            StringBuilder sb = new StringBuilder(pText.Length);
            pText = pText.Replace("\r\n", "\n").Replace('\r', '\n');
            string[] lines = pText.Split('\n');
            pText = "";
            foreach (string line in lines)
            {
                string s = line.Trim(StrUtils.CH_SPACES);
                if (s.StartsWith(pCommentPrefix)) continue;
                if (string.IsNullOrEmpty(s)) continue;
                sb.Append(s + Environment.NewLine);
            }
            return sb.ToString();
        }

        private void saveResultsAsXml(string pFilename, bool pSaveSchema)
        {
            try
            {
                this.dataObject.TableName = "Record"; // <-- it may fail if this property not set!

                FileInfo fi = new FileInfo(pFilename);
                this.dataObject.WriteXml(pFilename);
                Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format("Data XML successfully saved to {0}", fi.FullName) : "");

                if (pSaveSchema)
                {
                    string fn = Path.ChangeExtension(pFilename, ".xsd");
                    fi = new FileInfo(fn);
                    this.dataObject.WriteXmlSchema(fn);
                    Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format("XML schema successfully saved to {0}", fi.FullName) : "");
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? String.Format(
                    " * SaveAsXML->{0}\n{1}",
                    ErrorUtils.FormatErrorMsg(exc), ErrorUtils.FormatStackTrace(exc)
                    ) : "");
            }
        }

        private void saveResultsAsCsl(string pFilename, bool pSaveHeader)
        {
            try
            {
                FileInfo fi = new FileInfo(pFilename);
                //this.dataObject.WriteS
                Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format("Data CSV successfully saved to {0}", fi.FullName) : "");

                string delim = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                using (StreamWriter sw = fi.CreateText())
                {
                    string txt = null;
                    if (pSaveHeader)
                    {
                        foreach (DataColumn col in this.dataObject.Columns)
                        {
                            if (txt != null)
                                txt += (delim + col.ColumnName);
                            else
                                txt = col.ColumnName;
                        }
                        sw.WriteLine(txt);
                    }
                    foreach (DataRow row in this.dataObject.Rows)
                    {
                        txt = null;
                        foreach (DataColumn col in this.dataObject.Columns)
                        {
                            string sv = row[col].ToString().TrimEnd();
                            if (sv.IndexOfAny(Shopfloor.Borland.Delphi5.RTL._EMPTY_CHARS) >= 0)
                                sv = Shopfloor.Borland.Delphi5.RTL.AnsiQuotedStr(sv, '\"');
                            if (txt != null)
                                txt += (delim + sv);
                            else
                                txt = sv;
                        }
                        sw.WriteLine(txt);
                    }
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? String.Format(
                    " * SaveAsCSV->{0}\n{1}",
                    ErrorUtils.FormatErrorMsg(exc), ErrorUtils.FormatStackTrace(exc)
                    ) : "");
            }
        }

        private void displayHelp()
        {
            string text = HELP_TEXT;
            text = text.Replace("$(AppName)", TypeUtils.ApplicationName);
            MessageBox.Show(text, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void displayEULA()
        {
            MessageBox.Show(HELP_LICENSE_TEXT, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void doAnalyzeCmdLine(object x)
        {
            analyzeCmdLine();
        }

        private void analyzeCmdLine()
        {
            Thread.CurrentThread.Name = "analyzeCmdLine";
            string cmdLine = StrUtils.Join(this.args, " ");
            Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(" * CmdLine: {0}", cmdLine) : "");
            for (int iArg = 0; iArg < this.args.Length; iArg++)
            {
                string arg = this.args[iArg];
                if (string.IsNullOrEmpty(arg)) continue;
                if (arg.StartsWith("/") || arg.StartsWith("-"))
                {
                    if (arg.StartsWith("--"))
                        arg = arg.Remove(0, 2);
                    else
                        arg = arg.Remove(0, 1);
                    string pn = arg, pv = null;
                    int n = pn.IndexOfAny("=:".ToCharArray());
                    if (n >= 0)
                    {
                        pn = pn.Substring(0, n);
                        pv = arg.Remove(0, n + 1);
                    }
                    if (StrUtils.IsSameText(pn, "?") || StrUtils.IsSameText(pn, "help"))
                    {
                        displayHelp();
                        submitUiCommand(EUiCommand.Close, null);
                    }
                    else if (StrUtils.IsSameText(pn, "license") || StrUtils.IsSameText(pn, "eula"))
                    {
                        displayEULA();
                        submitUiCommand(EUiCommand.Close, null);
                    }                        
                    else if (StrUtils.IsSameText(pn, "conn-param") || StrUtils.IsSameText(pn, "cp"))
                    {
                        if (!string.IsNullOrEmpty(pv))
                            submitUiCommand(EUiCommand.AddParamsLine, pv);
                    }
                    else if (StrUtils.IsSameText(pn, "expand-env-vars") || StrUtils.IsSameText(pn, "eev"))
                    {
                        if (string.IsNullOrEmpty(pv)) pv = "1";
                        submitUiCommand(EUiCommand.SetExpandEnvVars, pv);
                    }
                    else if (StrUtils.IsSameText(pn, "execute") || StrUtils.IsSameText(pn, "exec"))
                    {
                        this.evtExecPrepared.Reset();
                        submitUiCommand(EUiCommand.PrepareExecutionContext, null);
                        waitFor(this.evtExecPrepared);
                        executeQuery();
                        waitFor(evtDataReady);
                    }
                    else if (StrUtils.IsSameText(pn, "exit") || StrUtils.IsSameText(pn, "Close"))
                    {
                        submitUiCommand(EUiCommand.Close, null);
                    }
                    else if (StrUtils.IsSameText(pn, "hold-connection") || StrUtils.IsSameText(pn, "hc"))
                    {
                        if (string.IsNullOrEmpty(pv)) pv = "1";
                        tbHoldConnection.Checked = StrUtils.GetAsBool(pv);
                    }
                    else if (StrUtils.IsSameText(pn, "ignore-to-end") || StrUtils.IsSameText(pn, "ite"))
                    {
                        return;
                    }
                    else if (StrUtils.IsSameText(pn, "query") || StrUtils.IsSameText(pn, "Q"))
                    {
                        submitUiCommand(EUiCommand.SetQuery, pv);
                    }
                    else if (StrUtils.IsSameText(pn, "query-file") || StrUtils.IsSameText(pn, "QF"))
                    {
                        FileInfo fi = new FileInfo(pv);
                        if (fi.Exists)
                        {
                            using (StreamReader sr = fi.OpenText())
                            {
                                pv = sr.ReadToEnd();
                            }
                            submitUiCommand(EUiCommand.SetQuery, pv);
                        }
                        else
                            MessageBox.Show(string.Format("Query-file [{0}] is not found!", fi.FullName), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (StrUtils.IsSameText(pn, "snippet") || StrUtils.IsSameText(pn, "sn"))
                    {
                        SnippetRef snp = this.Snippets.FindSnippet(pv);
                        if (snp != null)
                        {
                            submitUiCommand(EUiCommand.SetQuery, snp["QUERY"]);
                            submitUiCommand(EUiCommand.SetParams, snp["PARAMETERS"]);
                        }
                    }
                    else if (StrUtils.IsSameText(pn, "snippet-query") || StrUtils.IsSameText(pn, "snqry"))
                    {
                        SnippetRef snp = this.Snippets.FindSnippet(pv);
                        if (snp != null)
                        {
                            submitUiCommand(EUiCommand.SetQuery, snp["QUERY"]);
                        }
                    }
                    else if (StrUtils.IsSameText(pn, "snippet-params") || StrUtils.IsSameText(pn, "snprm"))
                    {
                        SnippetRef snp = this.Snippets.FindSnippet(pv);
                        if (snp != null)
                        {
                            submitUiCommand(EUiCommand.SetParams, snp["PARAMETERS"]);
                        }
                    }
                    else if (StrUtils.IsSameText(pn, "set-param") || StrUtils.IsSameText(pn, "sprm"))
                    {
                        string[] items = pv.Split(',');
                        if (items.Length > 1)
                        { 
                            updateParameterInGui(items[0], items[1], true);
                        }
                    }
                    else if (StrUtils.IsSameText(pn, "reset-conn-params") || StrUtils.IsSameText(pn, "rcp"))
                    {
                        submitUiCommand(EUiCommand.ClearParams, null);
                    }
                    else if (StrUtils.IsSameText(pn, "save-result-xml") || StrUtils.IsSameText(pn, "SRX"))
                    {
                        if (string.IsNullOrEmpty(pv)) pv = "query-results.xml";
                        saveResultsAsXml(pv, true);
                    }
                    else if (StrUtils.IsSameText(pn, "save-result-csv") || StrUtils.IsSameText(pn, "SRC"))
                    {
                        if (string.IsNullOrEmpty(pv)) pv = "query-results.csv";
                        saveResultsAsCsl(pv, true);
                    }
                }
                else
                {
                    // nothing to do here
                }
            }
        }

        private enum EUiCommand
        { 
            Close,
            SetQuery,
            ClearParams,
            AddParamsLine,
            SetParams,
            PrepareExecutionContext,
            ExecuteButtonState,
            SetExpandEnvVars,
        }

        private delegate void submitUiCommandMethod(EUiCommand pCmd, object pData);
        private void submitUiCommand(EUiCommand pCmd, object pData)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new submitUiCommandMethod(this.submitUiCommand), pCmd, pData);
            }
            else 
            {
                Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format("UI-command: {0} [{1}]", pCmd, (pData == null ? "(null)" : pData.ToString())) : "");
                switch (pCmd)
                {
                    case EUiCommand.Close: this.Close(); break;
                    case EUiCommand.SetQuery: txtQuery.Text = pData.ToString(); break;
                    case EUiCommand.ClearParams: txtParams.Clear(); break;
                    case EUiCommand.AddParamsLine: txtParams.Text += (Environment.NewLine + pData.ToString()); break;
                    case EUiCommand.SetParams: txtParams.Text = pData.ToString(); break;
                    case EUiCommand.SetExpandEnvVars: miExpandEnvironmentVariables.Checked = StrUtils.GetAsBool(pData.ToString()); break;
                    case EUiCommand.ExecuteButtonState:
                        {
                            bool newState = (bool) pData;
                            mmiExecute.Enabled = newState;
                            tbExecute.Enabled = newState;
                        }
                        break;
                    case EUiCommand.PrepareExecutionContext:
                        this.execCtx = new ExecutionContext();
                        this.execCtx.CmdText = StrUtils.TrimText(txtQuery.Text.Trim(StrUtils.CH_SPACES), ETrimOption.Both);
                        this.execCtx.Params = StrUtils.TrimText(txtParams.Text.Trim(StrUtils.CH_SPACES), ETrimOption.Both);
                        this.execCtx.ExpandEnvVars = miExpandEnvironmentVariables.Checked;
                        this.evtExecPrepared.Set();
                        break;
                }
            }
            Thread.Sleep(10);
        }

        private void trimSpaced()
        {
            this.Enabled = false;
            this.Refresh();
            try
            {
                foreach (DataRow row in this.dataObject.Rows)
                {
                    foreach (DataColumn dc in this.dataObject.Columns)
                    {
                        if (dc.DataType.Equals(typeof(string)))
                        {
                            row[dc] = row[dc].ToString().TrimEnd();
                        }
                    }
                }
            }
            finally { this.Enabled = true; }
        }

        private string serializeExtraUiProps()
        {
            string txt = "";
            txt += string.Format("QueryHeight:{0}%; ", (100.0d * (double)panQuery.Height / this.Height).ToString("N2"));
            txt += string.Format("LogPanelHeight:{0}%; ", (100.0d * (double)lvLogger.Height / this.Height).ToString("N2"));
            txt += string.Format("HoldConnection:{0}; ", (tbHoldConnection.Checked ? "1" : "0"));
            txt += string.Format("ExpandEnvVars:{0}; ", (miExpandEnvironmentVariables.Checked ? "1" : "0"));
            return txt;
        }

        private void deserializeExtraUiProp(string pName, string pValue)
        {
            if (StrUtils.IsSameText(pName, "QueryHeight"))
            {
                try { panQuery.Height = (int)(Convert.ToDouble(pValue.Trim("%".ToCharArray())) / 100.0 * this.Height); }
                catch { }
            }
            if (StrUtils.IsSameText(pName, "LogPanelHeight"))
            {
                try { lvLogger.Parent.Height = (int)(Convert.ToDouble(pValue.Trim("%".ToCharArray())) / 100.0 * this.Height); }
                catch { }
            }            
            if (StrUtils.IsSameText(pName, "HoldConnection"))
            {
                try { tbHoldConnection.Checked = StrUtils.GetAsBool(pValue); }
                catch { }
            }
            if (StrUtils.IsSameText(pName, "ExpandEnvVars"))
            {
                try { miExpandEnvironmentVariables.Checked = StrUtils.GetAsBool(pValue); }
                catch { }
            }
        }

        private SnippetsStorage Snippets
        {
            get
            {
                if (this.snippets == null)
                {
                    this.snippets = new SnippetsStorage("AdoNetQuery.snippets", snippetsRootKeyPath, typeof(QuerySnippet));
                    this.snippets.LoadSnippets();
                }
                return this.snippets;
            }
        }

        private void loadResultsetDs(string pFilename)
        {
            try
            {
                List<DataTable> tables = new List<DataTable>();
                DacService.DeserializeTables(tables, pFilename);
                DataTable selectedTable = tables[0];
                if (tables.Count > 1)
                {
                    List<string> items = new List<string>();
                    List<string> selected = new List<string>();
                    foreach (DataTable t in tables)
                        items.Add(t.TableName);
                    bool isOk = FormSelectItems.Execute(this, "Select table", "Select table", items, selected, 
                        FormSelectItems.ESelectItemFlags.SingleItem | FormSelectItems.ESelectItemFlags.DetailsView
                        );
                    if (!isOk) return ;

                    string selectedName = selected[0];
                    selectedTable = tables.Find(it => StrUtils.IsSameText(it.TableName, selectedName));
                }
                if (selectedTable == null) return;

                displayData(selectedTable);
            }
            catch (Exception exc)
            { 
                MessageBox.Show(ErrorUtils.FormatErrorMsg(exc), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadResultsetXml(string pFilename)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(pFilename);

                DataTable selectedTable = ds.Tables[0];                

                displayData(selectedTable);
            }
            catch (Exception exc)
            {
                MessageBox.Show(ErrorUtils.FormatErrorMsg(exc), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadResultset(string pFilename)
        {
            string ext = Path.GetExtension(pFilename).Trim(".".ToCharArray());
            if (StrUtils.IsSameText(ext, "data"))
            {
                loadResultsetDs(pFilename);
            }
            else if (StrUtils.IsSameText(ext, "xml"))
            {
                loadResultsetXml(pFilename);
            }
        }

        #region Form Event Handlers

        private void FormMain_Load(object sender, EventArgs e)
        {
            CallbackTraceListener.OnWrite += this.onTraceWrite;

            string s = ConfigurationManager.AppSettings["QueryText"];
            if (!string.IsNullOrEmpty(s))
                txtQuery.Text = StrUtils.TrimText(s, ETrimOption.Both).Trim(StrUtils.CH_SPACES);

            s = ConfigurationManager.AppSettings["ParametersText"];
            if (!string.IsNullOrEmpty(s))
                txtParams.Text = StrUtils.TrimText(s, ETrimOption.Both).Trim(StrUtils.CH_SPACES);

            s = ConfigurationManager.AppSettings["MaxLoggerItems"];
            if (!string.IsNullOrEmpty(s))
                this.maxLoggerItems = Convert.ToInt32(s.Trim(StrUtils.CH_SPACES));

            s = ConfigurationManager.AppSettings["SnippetsRootKeyPath"];
            if (!string.IsNullOrEmpty(s))
                this.snippetsRootKeyPath = s;

            this.dgvResultset.AutoGenerateColumns = true;

            if (!string.IsNullOrEmpty(Settings.Default.QueryText))
            {
                txtQuery.Text = Settings.Default.QueryText;
            }

            if (!string.IsNullOrEmpty(Settings.Default.ParametersText))
            {
                txtParams.Text = Settings.Default.ParametersText;
            }

            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                this.userConfigPath = config.FilePath;
            }
            catch (Exception exc)
            {
                Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? string.Format("!ERR: {0}\nat {1}",
                    ErrorUtils.FormatErrorMsg(exc), ErrorUtils.FormatStackTrace(exc)) : "");
            }

            pgcQuery.TabIndex = 0;

            displayUsefulLinks();
            displayConnectionStrings();

            stLab1.Text = "Ready.";
            stLab1.Invalidate();

            if (this.args != null && this.args.Length > 0)
            {
                //analyzeCmdLine();
                ThreadPool.QueueUserWorkItem(this.doAnalyzeCmdLine);
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            // load MainForm UI settings (window position & state)
            if (!string.IsNullOrEmpty(Settings.Default.MainFormView))
                UiTools.DeserializeFormView(Settings.Default.MainFormView, this, this.deserializeExtraUiProp);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.MainFormView = UiTools.SerializeFormView(this) + serializeExtraUiProps();

            Settings.Default.QueryText = txtQuery.Text;
            Settings.Default.ParametersText = txtParams.Text;
            Settings.Default.Save();
        }

        private void dgvResultset_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            e.ThrowException = false;            
            Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? String.Format(
                "DataGridView->{0}\n{1}",
                ErrorUtils.FormatErrorMsg(e.Exception), ErrorUtils.FormatStackTrace(e.Exception)
                ) : "");
        }

        private void dgvResultset_SelectionChanged(object sender, EventArgs e)
        {
            int row = -1, col = -1;
            if (dgvResultset.SelectedCells.Count > 0) 
            {
                row = dgvResultset.SelectedCells[0].RowIndex;
                col = dgvResultset.SelectedCells[0].ColumnIndex;
                stLab2.Text = string.Format("R:{0}; C:{1}", row, col);
            }
        }

        private void miUsefulLink_Click(object sender, EventArgs e)
        {
            ToolStripItem mi = (ToolStripItem)sender;
            ProcessStartInfo st = new ProcessStartInfo();
            st.UseShellExecute = true;
            st.Verb = "open";
            st.FileName = mi.Tag.ToString();
            Process.Start(st);
        }

        private void mmiLoadResultset_Click(object sender, EventArgs e)
        {
            if (dlgOpen.ShowDialog() != System.Windows.Forms.DialogResult.OK) return ;
            loadResultset(dlgOpen.FileName);
        }

        private void mmiConnectionStringBuilder_Click(object sender, EventArgs e)
        {
            runConnectionStringBuilder();
        }

        private void mmiSaveResultsetToXML_Click(object sender, EventArgs e)
        {
            pmiSaveAsXml_Click(sender, e);
        }

        private void mmiSaveResultsetToCSV_Click(object sender, EventArgs e)
        {
            pmiSaveAsCsv_Click(sender, e);
        }

        private void mmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mmiExecute_Click(object sender, EventArgs e)
        {
            executeQuery();
        }

        private void mmiInsertDefaultParameters_Click(object sender, EventArgs e)
        {
            pgcQuery.SelectedIndex = 1;
            txtParams.Text = DEFAULT_Parameters;
        }

        private void mmiHoldConnection_Click(object sender, EventArgs e)
        {
            this.lockDbConnection = !this.lockDbConnection;
            mmiHoldConnection.Checked = this.lockDbConnection;
            tbHoldConnection.Checked = this.lockDbConnection;
            txtParams.Enabled = !this.lockDbConnection;
            txtParams.BackColor = (txtParams.Enabled ? SystemColors.Window : SystemColors.ButtonFace);
        }

        private void tbHoldConnection_Click(object sender, EventArgs e)
        {
            this.lockDbConnection = !this.lockDbConnection;
            mmiHoldConnection.Checked = this.lockDbConnection;
            tbHoldConnection.Checked = this.lockDbConnection;
            txtParams.Enabled = !this.lockDbConnection;
            txtParams.BackColor = (txtParams.Enabled ? SystemColors.Window : SystemColors.ButtonFace);
        }

        private void mmiHelp_Click(object sender, EventArgs e)
        {
            ProcessStartInfo st = new ProcessStartInfo();
            st.UseShellExecute = true;
            st.Verb = "open";
            st.FileName = TypeUtils.ApplicationHomePath + "AdoNetQueryHelp.html";
            if (File.Exists(st.FileName))
                Process.Start(st);
            else
                MessageBox.Show("Help file is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void mmiListAdoNetProviders_Click(object sender, EventArgs e)
        {
            listAdoNetProviders();
        }

        private void mmiAbout_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            TypeUtils.CollectVersionInfoAttributes(props, Assembly.GetEntryAssembly());
            props["ApplicationName"] = "ADO.NET Query";
            props["EOL"] = Environment.NewLine;
            props["url"] = "https://dmitrybond.wordpress.com/2012/10/20/three-queries/";
            props["userConfig"] = (string.IsNullOrEmpty(this.userConfigPath) ? "-" : this.userConfigPath);

            Assembly asm = Assembly.GetExecutingAssembly();
            props["HostInfo"] = CommonUtils.HostInfoStamp() + string.Format(" ProcessType:{0};", asm.GetName().ProcessorArchitecture);

            string info = ""
                + "$(ApplicationName).$(EOL)"
                + "Version $(Version) / $(FileVersion)$(EOL)"
                + "Written by Dmitry Bond. (dima_ben@ukr.net)$(EOL)"
                + "$(EOL)"
                + "$(HostInfo)$(EOL)"
                + "$(EOL)"
                + "$(url)$(EOL)"
                + "$(EOL)"
                + "$(userConfig)$(EOL)"
                + "";
            info = StrUtils.ExpandParameters(info, props, true);
            FormAbout.Execute(this, StrUtils.ExpandParameters("About $(ApplicationName)", props, true),
                info, HELP_LICENSE_TEXT);
        }

        private void mmiSnippetsManager_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values["QUERY"] = txtQuery.Text;
            values["PARAMETERS"] = txtParams.Text;
            if (FormSnippetManager.Execute(this, this.Snippets, ref values))
            {
                string s;
                if (values.TryGetValue("QUERY", out s))
                    txtQuery.Text = s;
                if (values.TryGetValue("PARAMETERS", out s))
                    txtParams.Text = s;
            }
        }

        private void pmiCopy_Click(object sender, EventArgs e)
        {
            bool first = true;
            StringBuilder sb = new StringBuilder(this.dataObject.Columns.Count * 64 * this.dataObject.Rows.Count);
            foreach (DataRow row in this.dataObject.Rows)
            {
                if (first)
                {
                    first = false;
                    foreach (DataColumn col in this.dataObject.Columns)
                    {
                        if (col.Ordinal > 0)
                            sb.Append('\t');
                        sb.Append(col.ColumnName);
                    }
                    sb.Append(Environment.NewLine);
                }
                foreach (DataColumn col in this.dataObject.Columns)
                {
                    if (col.Ordinal > 0)
                        sb.Append('\t');
                    sb.Append(row[col].ToString());
                }
                sb.Append(Environment.NewLine);
            }
            Clipboard.SetText(sb.ToString());
        }

        private void pmiCopyRow_Click(object sender, EventArgs e)
        {
            if (dgvResultset.SelectedRows.Count == 0)
            {
                Trace.WriteLineIf(TrcLvl.TraceWarning, TrcLvl.TraceWarning ? String.Format("There is no selected row DataGridView. Nothing to do.") : "");
                return;
            }

            StringBuilder sb = new StringBuilder(this.dataObject.Columns.Count * 64 * dgvResultset.SelectedRows.Count);
            foreach (DataGridViewRow r in dgvResultset.SelectedRows)
            {
                DataRow row = this.dataObject.Rows[r.Index];
                foreach (DataColumn col in this.dataObject.Columns)
                {
                    if (col.Ordinal > 0)
                        sb.Append('\t');
                    sb.Append(row[col].ToString());
                }
                sb.Append(Environment.NewLine);
            }
            Clipboard.SetText(sb.ToString());
        }

        private void pmiClear_Click(object sender, EventArgs e)
        {
            updateDataView(this, null);
        }

        private void pmiResizeByHeader_Click(object sender, EventArgs e)
        {
            pmiResizeByValue.Checked = false;
            pmiResizeByHeader.Checked = true;
            this.dgvResultset.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
        }

        private void pmiResizeByValue_Click(object sender, EventArgs e)
        {
            pmiResizeByHeader.Checked = false;
            pmiResizeByValue.Checked = true;
            this.dgvResultset.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void pmiSaveAsXml_Click(object sender, EventArgs e)
        {
            if (this.dataObject == null)
            {
                Trace.WriteLineIf(TrcLvl.TraceWarning, TrcLvl.TraceWarning ? String.Format("Nothing to save! No data loaded.") : "");
                return;
            }

            if (dlgSave.ShowDialog() != DialogResult.OK) return;

            saveResultsAsXml(dlgSave.FileName, true);
        }

        private void pmiSaveAsCsv_Click(object sender, EventArgs e)
        {
            if (this.dataObject == null)
            {
                Trace.WriteLineIf(TrcLvl.TraceWarning, TrcLvl.TraceWarning ? String.Format("Nothing to save! No data loaded.") : "");
                return;
            }

            if (dlgSaveCsv.ShowDialog() != DialogResult.OK) return;

            saveResultsAsCsl(dlgSaveCsv.FileName, true);
        }

        private void pmiCopyAll_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(lvLogger.Items.Count * 80);
            foreach (ListViewItem li in lvLogger.Items)
            {
                bool first = true;
                foreach (ListViewItem.ListViewSubItem subLi in li.SubItems)
                {
                    sb.Append((first ? "" : "\t") + subLi.Text);
                    first = false;
                }
                sb.Append(Environment.NewLine);
            }
            Clipboard.SetText(sb.ToString());
        }

        private void pmiCopyLine_Click(object sender, EventArgs e)
        {
            if (lvLogger.SelectedItems.Count == 0) return;

            StringBuilder sb = new StringBuilder(lvLogger.SelectedItems.Count * 80);
            foreach (ListViewItem li in lvLogger.SelectedItems)
            {
                bool first = true;
                foreach (ListViewItem.ListViewSubItem subLi in li.SubItems)
                {
                    sb.Append((first ? "" : "\t") + subLi.Text);
                    first = false;
                }
                sb.Append(Environment.NewLine);
            }
            Clipboard.SetText(sb.ToString());
        }

        private void pmiTrimSpaces_Click(object sender, EventArgs e)
        {
            trimSpaced();
        }

        private void tbExecute_Click(object sender, EventArgs e)
        {
            executeQuery();
        }

        private void tbConnStrBuilder_Click(object sender, EventArgs e)
        {
            runConnectionStringBuilder();
        }

        private void tbProvidersList_Click(object sender, EventArgs e)
        {
            listAdoNetProviders();
        }

        private void txtParams_TextChanged(object sender, EventArgs e)
        {
            if (this.dbConnection != null)
            {
                try { this.dbConnection.Dispose(); }
                catch { }
            }
            this.dbConnection = null;
        }

        #endregion // Form Event Handlers

        private class ExecutionContext
        {
            public string CmdText;
            public string Params;
            public bool ExpandEnvVars = false;
        }

    }
}
