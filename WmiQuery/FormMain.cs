/* 
 * DBO-Tools collection.
 * WMI Query tool.
 * Simple application to execute WQL/WMI queries.
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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using XService.Utils;
using PAL;
using XService.UI;
using XService.UI.CommonForms;
using XService.Snippets;
using WmiQuery.Properties;

namespace WmiQuery
{
    public partial class FormMain : Form
    {
        public static TraceSwitch TrcLvl = new TraceSwitch("TraceLevel", "TraceLevel");

        private const string HELP_LICENSE_TEXT =
            "WMI Query.                                                                      \r\n" +
            "                                                                                \r\n" +
            "Copyright (c) 2012-2016 Dmitry Bondarenko, Kyiv, Ukraine                        \r\n" +
            "                                                                                \r\n" +
            "This software is provided 'as-is', without any express or implied warranty.     \r\n" +
            "In no event will the authors be held liable for any damages arising from        \r\n" +
            "the use of this software.                                                       \r\n" +
            "                                                                                \r\n" +
            "Permission is granted to anyone to use this software for any purpose,           \r\n" +
            "including commercial applications, and to redistribute it freely,               \r\n" +
            "subject to the following restrictions:                                          \r\n" +
            "                                                                                \r\n" +
            "1. The origin of this software must not be misrepresented; you must not claim   \r\n" +
            "   that you wrote the original software. If you use this software in a product, \r\n" +
            "   an acknowledgment in the product documentation would be appreciated but      \r\n" +
            "   is not required.                                                             \r\n" +
            "                                                                                \r\n" +
            "2. Altered source versions must be plainly marked as such, and must not be      \r\n" +
            "   misrepresented as being the original software.                               \r\n" +
            "                                                                                \r\n" +
            "3. This notice may not be removed or altered from any source distribution.      \r\n" +
            "                                                                                \r\n" +
            "========================================================================        \r\n" +
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
            "  -exec, --execute  - execute current query with current parameters\r\n" +
            "  -exit - exit application\r\n" +
            "  -hc, --hold-connection  - switch hold-connection flag (lock WMI searcher)\r\n" +
            "  -q, --query={WqlQuery}  - WQL query to execute\r\n" +
            "  -qf, --query-file={filename}  - filename with WQL query to execute\r\n" +
            "  -ite, --ignore-to-end  - ignore all cmdline parameters after this parameter\r\n" +
            "  -rp, --reset-params  - cleanup all parameters\r\n" +
            "  -sn, --snippet={SnippetName}  - choose snippet with specified name\r\n" +
            "  -snqry, --snippet-query={SnippetName}  - use query from snippet with specified name\r\n" +
            "  -snprm, --snippet-params={SnippetName}  - use parameters from snippet with specified name\r\n" +
            "  -sprm, --set-param={name,value}  - set value of specified parameter or add if parameter not found\r\n" +
            "  -srx, --save-result-xml={filename}  - same query results into specified XML file\r\n" +
            "  -src, --save-result-csv={filename}  - same query results into specified CSV file\r\n" +
            "\r\n" +
            "Example:\r\n" +
            "  WmiQuery.exe -query=\"SELECT * FROM Win32_Process WHERE name=\'devenv.exe\'\" -execute -save-result-csv=dev-proc-list.txt -exit\r\n" +
            "  WmiQuery.exe -query=\"Win32_Process;name=\'devenv.exe\'\" -execute -save-result-xml=dev-proc-list.xml -exit\r\n" +
            "  WmiQuery.exe -snippet=\"ListOfUsers\" -execute -save-result-csv=users-list.txt -exit\r\n" +
            "\r\n" +
            "";

        public FormMain(string[] pArgs)
        {
            this.args = pArgs;

            InitializeComponent();
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
                        tbLockSearcher.Checked = StrUtils.GetAsBool(pv);
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
                        /*SnippetRef snp = this.Snippets.FindSnippet(pv);
                        if (snp != null)
                        {
                            submitUiCommand(EUiCommand.SetQuery, snp["QUERY"]);
                            submitUiCommand(EUiCommand.SetParams, snp["PARAMETERS"]);
                        }*/
                    }
                    else if (StrUtils.IsSameText(pn, "snippet-query") || StrUtils.IsSameText(pn, "snqry"))
                    {
                        /*SnippetRef snp = this.Snippets.FindSnippet(pv);
                        if (snp != null)
                        {
                            submitUiCommand(EUiCommand.SetQuery, snp["QUERY"]);
                        }*/
                    }
                    else if (StrUtils.IsSameText(pn, "snippet-params") || StrUtils.IsSameText(pn, "snprm"))
                    {
                        /*SnippetRef snp = this.Snippets.FindSnippet(pv);
                        if (snp != null)
                        {
                            submitUiCommand(EUiCommand.SetParams, snp["PARAMETERS"]);
                        }*/
                    }
                    else if (StrUtils.IsSameText(pn, "set-param") || StrUtils.IsSameText(pn, "sprm"))
                    {
                        string[] items = pv.Split(',');
                        if (items.Length > 1)
                        {
                            //updateParameterInGui(items[0], items[1], true);
                        }
                    }
                    else if (StrUtils.IsSameText(pn, "reset-conn-params") || StrUtils.IsSameText(pn, "rcp"))
                    {
                        submitUiCommand(EUiCommand.ClearParams, null);
                    }
                    else if (StrUtils.IsSameText(pn, "save-result-xml") || StrUtils.IsSameText(pn, "SRX"))
                    {
                        if (string.IsNullOrEmpty(pv)) pv = "wmi-results.xml";
                        saveResultsAsXml(pv, true);
                    }
                    else if (StrUtils.IsSameText(pn, "save-result-csv") || StrUtils.IsSameText(pn, "SRC"))
                    {
                        if (string.IsNullOrEmpty(pv)) pv = "wmi-results.csv";
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
                            bool newState = (bool)pData;
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

        private void miUsefulLink_Click(object sender, EventArgs e)
        {
            ToolStripItem mi = (ToolStripItem)sender;
            ProcessStartInfo st = new ProcessStartInfo();
            st.UseShellExecute = true;
            st.Verb = "open";
            st.FileName = mi.Tag.ToString();
            Process.Start(st);
        }

        private void executeQuery()
        {
            if (this.executing)
            {
                Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                    "! Application is already executing query!") : "");
                return;
            }

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
                this.BeginInvoke(new UpdateDataViewMethod(this.updateDataView), this, this.dataObject);
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
                        this.dataObject = null;

                        // remove old DataTable from DataSet
                        int n = this.ds.Tables.IndexOf("Record");
                        if (n >= 0)
                            this.ds.Tables.RemoveAt(n);
                    }
                }

                if (pData != null)
                {
                    Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                        "= Updating DataGridView...") : "");

                    this.bsData.DataSource = this.ds;
                    this.bsData.DataMember = "Record";
                    this.bsData.ResetBindings(true);

                    // Note: there is stupid bug in DataGridView which raising lot of "DataGridView System.IndexOutOfRangeException Index does not have a value" errors
                    // I still do not see how to avoid such error. So, code below is just an attempt...
                    this.bsData.CurrencyManager.Refresh();

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

        private ManagementObjectSearcher createSearcher(string pCmdText, ManagementScope pScope)
        {
            SelectQuery query = null;
            string[] queryItems = pCmdText.Split(';');
            if (queryItems.Length == 2)
            {
                query = new SelectQuery(queryItems[0], queryItems[1]);
            }
            else
                query = new SelectQuery(pCmdText);

            ManagementObjectSearcher searcher = null;
            if (pScope != null)
            {
                Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                    "Scoped.Exec: {0}", pCmdText) : "");
                searcher = new ManagementObjectSearcher(pScope, query);
            }
            else
            {
                Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                    "Local.Exec: {0}", pCmdText) : "");
                searcher = new ManagementObjectSearcher(query);
            }

            return searcher;
        }

        private void doExecuteQuery(object pState)
        {
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
                    CollectionUtils.ParseParametersStrEx(this.queryParams, paramsText, true, '\n', "=");

                    cmdText = ensureExpandMacroValues(cmdText);
                    Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(" - Final Query: {0}", cmdText) : "");

                    // do this to cleanup DataGridView from any data before executing a new query
                    this.updateDataView(this, null);
                    Thread.Sleep(20); // to ensure data cleaned up

                    bool hasLockedSearcher = (this.lockWmiSearcher && this.wmiSearcher != null);

                    if (!hasLockedSearcher)
                    {
                        ManagementScope scope = null;

                        bool hasScope = this.queryParams.TryGetValue("scope", out s);
                        if (!hasScope)
                            hasScope = this.queryParams.TryGetValue("scope.name", out s);
                        if (hasScope)
                            hasScope = !string.IsNullOrEmpty(s.Trim(StrUtils.CH_SPACES));
                        if (hasScope)
                        {
                            s = s.Trim(StrUtils.CH_SPACES);
                            loadWmiScope(s, out scope);
                        }

                        Thread.Sleep(20); // need a pause to resolve ThreadPool jobs before executintg new query

                        // execute a query
                        if (this.lockWmiSearcher)
                        {
                            Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                                "! Create and save WMI searcher...") : "");
                            this.wmiSearcher = createSearcher(cmdText, scope);
                            using (ManagementObjectCollection items = this.wmiSearcher.Get())
                            {
                                // this will load data from ManagementObjectCollection into a DataTable object in the "ds" DataSet
                                wmiLoadDataObject(items);

                                Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                                    "= Loaded dataset has {0} columns x {1} rows", this.dataObject.Columns.Count, this.dataObject.Rows.Count) : "");

                                // display data in DataGridView
                                this.updateDataView(this, this.dataObject);
                            }
                        }
                        else
                        {
                            using (ManagementObjectSearcher searcher = createSearcher(cmdText, scope))
                            {
                                using (ManagementObjectCollection items = searcher.Get())
                                {
                                    // this will load data from ManagementObjectCollection into a DataTable object in the "ds" DataSet
                                    wmiLoadDataObject(items);

                                    Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                                        "= Loaded dataset has {0} columns x {1} rows", this.dataObject.Columns.Count, this.dataObject.Rows.Count) : "");

                                    // display data in DataGridView
                                    this.updateDataView(this, this.dataObject);
                                }
                            }
                        }
                    }
                    else 
                    {
                        Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                            "! Re-use saved WMI searcher...") : "");
                        this.wmiSearcher.Query = new SelectQuery(cmdText);
                        using (ManagementObjectCollection items = this.wmiSearcher.Get())
                        {
                            // this will load data from ManagementObjectCollection into a DataTable object in the "ds" DataSet
                            wmiLoadDataObject(items);

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
                        "   * ExecuteWmiQuery->{0}\n{1}",
                        ErrorUtils.FormatErrorMsg(exc), ErrorUtils.FormatStackTrace(exc)
                        ) : "");
                }
            }
            finally { this.executing = false; }
            DateTime t2 = DateTime.Now;
            this.execCtx = null; // need to reset context after usage
            this.evtExec.Set();
            this.updateText(stLab3, string.Format("+ Elapsed time {0} ms", (t2 - t1).TotalMilliseconds));
            submitUiCommand(EUiCommand.ExecuteButtonState, true);
        }

        private void loadWmiScope(string pScopeDef, out ManagementScope pScope)
        {
            pScope = null;
            try
            {
                ConnectionOptions options = new ConnectionOptions();

                string s;
                if (this.queryParams.TryGetValue("scope.authentication", out s) && !string.IsNullOrEmpty(s.Trim(StrUtils.CH_SPACES)))
                {
                    s = s.Trim(StrUtils.CH_SPACES);
                    options.Authentication = (AuthenticationLevel)Enum.Parse(typeof(AuthenticationLevel), ensureExpandMacroValues(s), true);
                }

                if (this.queryParams.TryGetValue("scope.authority", out s) && !string.IsNullOrEmpty(s.Trim(StrUtils.CH_SPACES)))
                {
                    s = s.Trim(StrUtils.CH_SPACES);
                    options.Authority = ensureExpandMacroValues(s);
                }

                if (this.queryParams.TryGetValue("scope.impersonation", out s) && !string.IsNullOrEmpty(s.Trim(StrUtils.CH_SPACES)))
                {
                    s = s.Trim(StrUtils.CH_SPACES);
                    options.Impersonation = (ImpersonationLevel)Enum.Parse(typeof(ImpersonationLevel), ensureExpandMacroValues(s), true);
                }

                string pwd = "";
                if (this.queryParams.TryGetValue("scope.password", out s) && !string.IsNullOrEmpty(s.Trim(StrUtils.CH_SPACES)))
                {
                    s = s.Trim(StrUtils.CH_SPACES);
                    pwd = ensureExpandMacroValues(s);
                    options.Password = pwd;
                }

                if (this.queryParams.TryGetValue("scope.username", out s) && !string.IsNullOrEmpty(s.Trim(StrUtils.CH_SPACES)))
                {
                    s = s.Trim(StrUtils.CH_SPACES);
                    options.Username = ensureExpandMacroValues(s);
                }

                if (this.queryParams.TryGetValue("scope.timeoutsec", out s) && !string.IsNullOrEmpty(s.Trim(StrUtils.CH_SPACES)))
                {
                    s = s.Trim(StrUtils.CH_SPACES);
                    options.Timeout = new TimeSpan(0, 0, Convert.ToInt32(ensureExpandMacroValues(s)));
                }

                pScopeDef = ensureExpandMacroValues(pScopeDef);

                Trace.WriteLineIf(TrcLvl.TraceInfo, TrcLvl.TraceInfo ? String.Format(
                    "   * Connecting ManagementScope( {0}; authLvl={1}; auth={2}; imp={3}; login={4}/{5}; ) ...",
                    pScopeDef, options.Authentication, options.Authority, options.Impersonation,
                    options.Username, pwd
                    ) : "");

                pScope = new ManagementScope(pScopeDef, options);

                pScope.Connect();
            }
            catch (Exception exc)
            {
                Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? String.Format(
                    "   * Connecting ManagementScope->{0}\n{1}",
                    ErrorUtils.FormatErrorMsg(exc), ErrorUtils.FormatStackTrace(exc)
                    ) : "");
                throw ;
            }
        }

        private void wmiLoadDataObject(ManagementObjectCollection pItems)
        {
            if (this.dataObject == null)
            {
                // remove old DataTable from DataSet
                int n = this.ds.Tables.IndexOf("Record");
                if (n >= 0)
                    this.ds.Tables.RemoveAt(n);
                // create new DataTable in DataSet
                this.dataObject = this.ds.Tables.Add("Record");
            }

            this.dataObject.Clear();
            this.dataObject.Columns.Clear();

            if (pItems.Count > 0)
            {
                bool isFirst = (this.dataObject.Columns.Count == 0);
                foreach (ManagementObject item in pItems)
                {
                    if (isFirst)
                    {
                        foreach (PropertyData prop in item.Properties)
                        {
                            object value = prop.Value;
                            if (value == null) value = "";
                            this.dataObject.Columns.Add(prop.Name, value.GetType());
                        }
                        isFirst = false;
                    }
                    int i = 0;
                    object[] values = new object[this.dataObject.Columns.Count];
                    foreach (PropertyData prop in item.Properties)
                    {
                        DataColumn dc = null;
                        if (i < this.dataObject.Columns.Count) dc = this.dataObject.Columns[i];
                        try
                        {
                            object v = prop.Value;
                            bool isColumnMismatch = (string.Compare(dc.ColumnName, prop.Name, true) != 0);
                            if (isColumnMismatch)
                            {
                                int idx = this.dataObject.Columns.IndexOf(prop.Name);
                                if (idx < 0)
                                {
                                    Trace.WriteLineIf(TrcLvl.TraceWarning, TrcLvl.TraceWarning ? String.Format(
                                        "   * row# {0}, value# {1} - column name mismatch ({2}, but {3} expected). Cannot find appropriate target column in DataTable!",
                                        this.dataObject.Rows.Count, i, prop.Name, dc.ColumnName) : "");
                                    continue;
                                }
                                i = idx;
                                dc = this.dataObject.Columns[i];
                            }
                            values[i] = v;
                            if (v != null && dc != null)
                            {
                                if (dc.DataType != v.GetType())
                                {
                                    Trace.WriteLineIf(TrcLvl.TraceWarning, TrcLvl.TraceWarning ? String.Format(
                                        "   * row# {0}, value# {1} - value type mismatch ({2}, but {3} expected)!",
                                        this.dataObject.Rows.Count, i, v.GetType(), dc.DataType) : "");
                                    values[i] = null;
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? String.Format(
                                "   * row# {0}, value# {1}: fail to add value -> {2}",
                                this.dataObject.Rows.Count, i, ErrorUtils.FormatErrorMsg(exc)) : "");
                            break;
                        }
                        i++;
                    }
                    try
                    {
                        this.dataObject.Rows.Add(values);
                    }
                    catch (Exception exc)
                    {
                        Trace.WriteLineIf(TrcLvl.TraceError, TrcLvl.TraceError ? String.Format(
                            "   * row# {0}, value# {1}: fail to add row -> {2}",
                            this.dataObject.Rows.Count, i, ErrorUtils.FormatErrorMsg(exc)) : "");
                    }
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

        /*private bool validateSaveQueryInput(SavedQuery pQuery)
        {
            if (this.regKeySavedQueries.ValueExists(pQuery.Name))
                return false;
            return true;
        }*/

        private string ensureExpandMacroValues(string pText)
        {
            return StrUtils.ExpandParameters(pText, this.queryParams, true);
        }

        /*private void loadSavedQueries()
        {
            this.regKeySavedQueries = this.registry.OpenKey("SavedQueries", true);
            foreach (string vn in this.regKeySavedQueries.ValueNames)
            {
                SavedQuery Q = new SavedQuery();
                Q.Name = vn;
                string text = this.regKeySavedQueries.ReadString(vn);
                string delimiter = SavedQuery.STR_PARAMS_DELIMITER;
                if (text.IndexOf(delimiter) >= 0)
                {
                    Q.SaveParams = true;
                    Q.Parameters = StrUtils.GetAfterPattern(text, delimiter);
                    text = StrUtils.GetToPattern(text, delimiter);
                }
                Q.Query = text;
                this.savedQueries.Add(Q);
            }
        }*/

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
                            if (sv.IndexOfAny(_EMPTY_CHARS) >= 0)
                                sv = AnsiQuotedStr(sv, '\"');
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

        // just all chars in range \x00..\x20 and '\"' and ','
        public static char[] _EMPTY_CHARS = new char[] 
					{
						'\x00','\x01','\x02','\x03', '\x04','\x05','\x06','\x07',
						'\x08','\x09','\x0A','\x0B', '\x0C','\x0D','\x0E','\x0F',
						'\x10','\x11','\x12','\x13', '\x14','\x15','\x16','\x17',
						'\x18','\x19','\x1A','\x1B', '\x1C','\x1D','\x1E','\x1F',
						'\x20','\"',','
					};

        public static string AnsiQuotedStr(string S, char Quote)
        {
            int add_count = 0;
            int p = S.IndexOf(Quote);
            while (p >= 0)
            {
                add_count++;
                p = S.IndexOf(Quote, p + 1);
            }
            if (add_count == 0)
                return Quote + S + Quote;

            string text = S;
            p = text.IndexOf(Quote);
            while (p >= 0)
            {
                text = text.Insert(p, "" + Quote);
                p = text.IndexOf(Quote, p + 2);
            }
            return Quote + text + Quote;
        }

        private string serializeExtraUiProps()
        {
            string txt = "";
            txt += string.Format("QueryHeight:{0}%; ", (100.0d * (double)panQuery.Height / this.Height).ToString("N2"));
            txt += string.Format("LogPanelHeight:{0}%; ", (100.0d * (double)lvLogger.Height / this.Height).ToString("N2"));
            txt += string.Format("HoldConnection:{0}; ", (tbLockSearcher.Checked ? "1" : "0"));
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
                try { tbLockSearcher.Checked = StrUtils.GetAsBool(pValue); }
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
                    this.snippets = new SnippetsStorage("WmiQuery.snippets", snippetsRootKeyPath, typeof(QuerySnippet));
                    this.snippets.LoadSnippets();
                }
                return this.snippets;
            }
        }

        #region Form Event Handlers

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

        private void tbExecute_Click(object sender, EventArgs e)
        {
            executeQuery();
        }

        private void mmiSaveResultsetToXML_Click(object sender, EventArgs e)
        {
            pmiSaveAsXml_Click(sender, e);
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

        private void mmiLockWMISearcher_Click(object sender, EventArgs e)
        {
            this.lockWmiSearcher = !this.lockWmiSearcher;
            mmiLockWMISearcher.Checked = this.lockWmiSearcher;
            tbLockSearcher.Checked = this.lockWmiSearcher;
            txtParams.Enabled = !this.lockWmiSearcher;
            txtParams.BackColor = (txtParams.Enabled ? SystemColors.Window : SystemColors.ButtonFace);
        }

        private void tbLockSearcher_Click(object sender, EventArgs e)
        {
            this.lockWmiSearcher = !this.lockWmiSearcher;
            mmiLockWMISearcher.Checked = this.lockWmiSearcher;
            tbLockSearcher.Checked = this.lockWmiSearcher;
            txtParams.Enabled = !this.lockWmiSearcher;
            txtParams.BackColor = (txtParams.Enabled ? SystemColors.Window : SystemColors.ButtonFace);
        }

        private void mmiHelp_Click(object sender, EventArgs e)
        {
            ProcessStartInfo st = new ProcessStartInfo();
            st.UseShellExecute = true;
            st.Verb = "open";
            st.FileName = TypeUtils.ApplicationHomePath + "WmiQueryHelp.html";
            if (File.Exists(st.FileName))
                Process.Start(st);
            else
                MessageBox.Show("Help file is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /*private void mmiLoadSavedQuery_Click(object sender, EventArgs e)
        {
            SavedQuery Q = new SavedQuery();
            if (FormSaveQuery.ExecuteLoadQuery(this, Q, this.savedQueries))
            {
                txtQuery.Text = Q.Query;
                if (Q.SaveParams)
                    this.txtParams.Text = Q.Parameters;
            }
        }

        private void mmiSaveCurrentQueryAs_Click(object sender, EventArgs e)
        {
            SavedQuery Q = new SavedQuery();
            Q.Query = txtQuery.Text.Trim(StrUtils.CH_SPACES);
            if (FormSaveQuery.ExecuteSaveQuery(this, Q, this.validateSaveQueryInput))
            {
                string query = Q.Query;
                query = "-- " + Q.Description + Environment.NewLine + query;
                if (Q.SaveParams)
                    query += SavedQuery.STR_PARAMS_DELIMITER + txtParams.Text;
                this.regKeySavedQueries.WriteString(Q.Name, query);
                SavedQuery newQ = new SavedQuery();
                newQ.Assign(Q);
                this.savedQueries.Add(newQ);
            }
            this.registry.Save();
        }*/

        private void mmiAbout_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            TypeUtils.CollectVersionInfoAttributes(props, Assembly.GetEntryAssembly());
            props["ApplicationName"] = "WMI Query";
            props["EOL"] = Environment.NewLine;
            props["url"] = "https://dmitrybond.wordpress.com/2012/10/20/three-queries/";

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
                + "";
            info = StrUtils.ExpandParameters(info, props, true);
            FormAbout.Execute(this, StrUtils.ExpandParameters("About $(ApplicationName)", props, true),
                info, HELP_LICENSE_TEXT);
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
            this.dgvResultset.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
        }

        private void pmiResizeByValue_Click(object sender, EventArgs e)
        {
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

        private void mmiSaveResultsetToCSV_Click(object sender, EventArgs e)
        {
            if (this.dataObject == null)
            {
                Trace.WriteLineIf(TrcLvl.TraceWarning, TrcLvl.TraceWarning ? String.Format("Nothing to save! No data loaded.") : "");
                return;
            }

            if (dlgSaveCsv.ShowDialog() != DialogResult.OK) return;

            saveResultsAsCsl(dlgSave.FileName, true);
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

        private void txtParams_TextChanged(object sender, EventArgs e)
        {
            if (this.wmiSearcher != null)
            {
                try { this.wmiSearcher.Dispose(); }
                catch { }
            }
            this.wmiSearcher = null;
        }

        private void tbSnippets_Click(object sender, EventArgs e)
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

        private void FormMain_Load(object sender, EventArgs e)
        {
            CallbackTraceListener.OnWrite += this.onTraceWrite;

            string s = ConfigurationManager.AppSettings["QueryText"];
            if (!string.IsNullOrEmpty(s))
                txtQuery.Text = StrUtils.AdjustLineBreaks(StrUtils.TrimText(s, ETrimOption.Both).Trim(StrUtils.CH_SPACES), Environment.NewLine);

            s = ConfigurationManager.AppSettings["ParametersText"];
            if (!string.IsNullOrEmpty(s))
                txtParams.Text = StrUtils.AdjustLineBreaks(StrUtils.TrimText(s, ETrimOption.Both).Trim(StrUtils.CH_SPACES), Environment.NewLine);

            s = ConfigurationManager.AppSettings["MaxLoggerItems"];
            if (!string.IsNullOrEmpty(s))
                this.maxLoggerItems = Convert.ToInt32(s.Trim(StrUtils.CH_SPACES));            

            this.dgvResultset.AutoGenerateColumns = true;

            if (!string.IsNullOrEmpty(Settings.Default.QueryText))
            {
                txtQuery.Text = Settings.Default.QueryText;
            }

            if (!string.IsNullOrEmpty(Settings.Default.ParametersText))
            {
                txtParams.Text = Settings.Default.ParametersText;
            }

            //this.registry = new XmlRegistry(TypeUtils.ApplicationHomePath + "WmiQueryRegistry.xml", true);
            //loadSavedQueries();

            pgcQuery.TabIndex = 0;

            displayUsefulLinks();

            if (this.args != null && this.args.Length > 0)
            {
                //analyzeCmdLine();
                ThreadPool.QueueUserWorkItem(this.doAnalyzeCmdLine);
            }

            stLab1.Text = "Ready.";
            stLab1.Invalidate();
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

        #endregion // Form Event Handlers

        private int maxLoggerItems = 300;
        private string[] args;
        private bool executing = false;
        private DataSet ds = new DataSet("Data");
        private DataTable dataObject;
        private ExecutionContext execCtx = null;
        private ManualResetEvent evtExecPrepared = new ManualResetEvent(false);
        private ManualResetEvent evtExec = new ManualResetEvent(false);
        private ManualResetEvent evtDataReady = new ManualResetEvent(false);
        private Dictionary<string, string> queryParams = new Dictionary<string, string>();
        private List<SavedQuery> savedQueries = new List<SavedQuery>();
        //private XmlRegistry registry;
        //private XmlRegistryKey regKeySavedQueries;
        private bool lockWmiSearcher = false;
        private ManagementObjectSearcher wmiSearcher = null;
        private SnippetsStorage snippets;
        private string snippetsRootKeyPath = "Snippets";

        private static string DEFAULT_Parameters = ""
            + @"//scope.name = \\$(computer)\root\cimv2" + Environment.NewLine
            + "//scope.username = alta" + Environment.NewLine
            + "//scope.password = Forget1" + Environment.NewLine
            + "//scope.authentication = Unchanged|Default|None|Connect|Call|Packet|PacketIntegrity|PacketPrivacy" + Environment.NewLine
            + "//scope.authority = NTLMDOMAIN:$(Domain) | Kerberos:$(PrincipalName)" + Environment.NewLine
            + "//scope.impersonation = Default|Anonymous|Identify|Impersonate|Delegate" + Environment.NewLine
            + "//scope.timeoutsec = $(Seconds)" + Environment.NewLine
            + "" + Environment.NewLine
            + "//Computer=" + Environment.NewLine
            + "//Domain=" + Environment.NewLine
            + "//PrincipalName=" + Environment.NewLine
            + "//Seconds=30" + Environment.NewLine
            + "" + Environment.NewLine;

        private class ExecutionContext
        {
            public string CmdText = null;
            public string Params = null;
            public bool ExpandEnvVars = false;
        }


    }
}
