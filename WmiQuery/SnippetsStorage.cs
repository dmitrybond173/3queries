/* 
 * DBO-Tools collection.
 * WMI Query tool.
 * Simple application to execute WQL/WMI queries.
 * 
 * Query snippens manager.
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using PAL;
using XService.Utils;

namespace XService.Snippets
{
    /// <summary>
    /// SnippetsStorage - engine to maintain storage of code snippets
    /// </summary>
    public class SnippetsStorage
    {
        public SnippetsStorage(string pRegistryFilename, string pSnippetsRootKeyPath, Type pSnippetType)
        {
            string fn = Path.GetFileName(pRegistryFilename);
            string targetDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + TypeUtils.ApplicationName;
            if (!Directory.Exists(targetDir))
            {
                DirectoryInfo di = Directory.CreateDirectory(targetDir);
                EnableIoFullControl(di);
            }
            string targetFn = PathUtils.IncludeTrailingSlash(targetDir) + fn;

            FileInfo sourceFi = new FileInfo(TypeUtils.ApplicationHomePath + fn);
            FileInfo targetFi = new FileInfo(targetFn);

            if (!sourceFi.Exists)
            {
                try
                {
                    using (StreamWriter sw = sourceFi.CreateText())
                    {
                        sw.WriteLine(DEFAULT_SNIPPETS_FILE);
                    }
                }
                catch (Exception exc)
                {
                    Trace.WriteLineIf(XmlRegistry.TrcLvl.TraceError, XmlRegistry.TrcLvl.TraceError ? String.Format(
                        "DefaultSnippets.ERR({0}): {1}", exc.GetType(), exc.Message) : "");
                }
            }

            bool needToUpdate = (!targetFi.Exists || targetFi.LastWriteTime < sourceFi.LastWriteTime);
            if (needToUpdate)
            {
                if (targetFi.Exists)
                {
                    try
                    {
                        string bakFn = PathUtils.IncludeTrailingSlash(targetDir) + Path.ChangeExtension(fn, StrUtils.StripNskTimestamp(StrUtils.NskTimestampOf(DateTime.Now)).Substring(0, 14).Insert(8, "-") + ".txt");
                        Trace.WriteLineIf(XmlRegistry.TrcLvl.TraceWarning, XmlRegistry.TrcLvl.TraceWarning ? String.Format(
                            "! Save backup of snippets at: {0}", bakFn) : "");
                        targetFi.CopyTo(bakFn, true);
                    }
                    catch (Exception exc)
                    {
                        Trace.WriteLineIf(XmlRegistry.TrcLvl.TraceError, XmlRegistry.TrcLvl.TraceError ? String.Format(
                            "BackupSnippets.ERR({0}): {1}", exc.GetType(), exc.Message) : "");
                    }
                }

                try
                {
                    Trace.WriteLineIf(XmlRegistry.TrcLvl.TraceVerbose, XmlRegistry.TrcLvl.TraceVerbose ? String.Format("Copy [{0}] into [{1}]...", sourceFi.FullName, targetFi.FullName) : "");
                    sourceFi.CopyTo(targetFi.FullName, true);
                }
                catch (Exception exc)
                {
                    Trace.WriteLineIf(XmlRegistry.TrcLvl.TraceError, XmlRegistry.TrcLvl.TraceError ? String.Format(
                        "InstallSnippets.ERR({0}): {1}", exc.GetType(), exc.Message) : "");
                }

                EnableIoFullControl(targetFi);
            }

            this.registryFilename = targetFi.FullName;

            this.snippetsRootKeyPath = pSnippetsRootKeyPath;
            this.snippetType = pSnippetType;
        }

        public static void EnableIoFullControl(DirectoryInfo pInfo)
        {
            try
            {
                Trace.WriteLineIf(XmlRegistry.TrcLvl.TraceVerbose, XmlRegistry.TrcLvl.TraceVerbose ? String.Format("Set ACL for({0})...", pInfo.FullName) : "");
                DirectorySecurity sec = pInfo.GetAccessControl();
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.FullControl, AccessControlType.Allow));
                pInfo.SetAccessControl(sec);
            }
            catch (Exception exc)
            {
                Trace.WriteLineIf(XmlRegistry.TrcLvl.TraceError, XmlRegistry.TrcLvl.TraceError ? String.Format(
                    "SetACL.ERR({0}): {1}", exc.GetType(), exc.Message) : "");
            }
        }

        public static void EnableIoFullControl(FileInfo pInfo)
        {
            try
            {
                Trace.WriteLineIf(XmlRegistry.TrcLvl.TraceVerbose, XmlRegistry.TrcLvl.TraceVerbose ? String.Format("Set ACL for({0})...", pInfo.FullName) : "");
                FileSecurity sec = pInfo.GetAccessControl();
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.FullControl, AccessControlType.Allow));
                pInfo.SetAccessControl(sec);
            }
            catch (Exception exc)
            {
                Trace.WriteLineIf(XmlRegistry.TrcLvl.TraceError, XmlRegistry.TrcLvl.TraceError ? String.Format(
                    "SetACL.ERR({0}): {1}", exc.GetType(), exc.Message) : "");
            }
        }

        public SnippetRef Root { get { return this.rootSnippet; } }

        public string Filename { get { return this.registryFilename; } }

        public XmlRegistry Registry { get { return this.registry; } }

        public void LoadSnippets()
        {
            discoverSnippetProps();
            loadSnippets();
        }

        public void SaveChanges()
        {
            this.registry.Save();
        }

        public SnippetRef FindSnippet(string pSnippetName)
        {
            bool isPath = (pSnippetName.IndexOfAny("\\/".ToCharArray()) >= 0);
            if (isPath)
            {
                pSnippetName = pSnippetName.Replace('\\', '/');
                List<string> pathItems = new List<string>(pSnippetName.Split('/'));
                if (pathItems.Count == 0) 
                    return null;
                SnippetRef snp = this.Root;
                if (!StrUtils.IsSameText(pathItems[0], this.Root.Caption))
                    pathItems.Insert(0, this.Root.Caption);
                for (int i = 0; i < pathItems.Count; i++)
                {
                    snp = findChildSnippetByName(pathItems[i], snp);
                    if (snp == null) 
                        return null;
                }
                return snp;
            }
            else
            {
                return findSnippetByName(pSnippetName, this.Root);
            }
        }

        #region Implementation Details

        internal object createSnippetInstance()
        {
            object snippet = this.snippetConstructor.Invoke(new object[] { });
            return snippet;
        }

        internal string getSnippetProp(SnippetRef pRef, string pPropName)
        {
            if (pRef.Snippet == null)
            {
                if (pPropName.ToUpper().CompareTo("NAME") == 0)
                    return pRef.RegKey.Name;
                return null;
            }
            PropertyInfo prop;
            if (!this.snippetProps.TryGetValue(pPropName.ToUpper(), out prop))
                return null;
            object x = prop.GetValue(pRef.Snippet, null);
            if (x != null)
                return x.ToString();
            return null;
        }

        internal void setSnippetProp(SnippetRef pRef, string pPropName, string pValue)
        {
            if (pRef.Snippet == null)
            {
                if (pPropName.ToUpper().CompareTo("NAME") == 0)
                    pRef.RegKey.Name = pValue;
                return;
            }
            PropertyInfo prop;
            if (!this.snippetProps.TryGetValue(pPropName.ToUpper(), out prop))
                return;

            object v = pValue;
            if (!prop.PropertyType.Equals(typeof(string)))
                v = Convert.ChangeType(v, prop.PropertyType);
            prop.SetValue(pRef.Snippet, v, null);

            if (pPropName.ToUpper().CompareTo("NAME") == 0)
                pRef.RegKey.Name = pValue;
            else
                pRef.RegKey.WriteText(pPropName, new StringBuilder(pValue));
        }

        private SnippetRef findSnippetByName(string pSnippetName, SnippetRef pSearchPoint)
        {
            if (StrUtils.IsSameText(pSearchPoint.Caption, pSnippetName))
                return pSearchPoint;

            if (pSearchPoint.ChildSnippets != null)
            {
                foreach (SnippetRef snp in pSearchPoint.ChildSnippets)
                {
                    SnippetRef item = findSnippetByName(pSnippetName, snp);
                    if (item != null)
                        return item;
                }
            }
            return null;
        }

        private SnippetRef findChildSnippetByName(string pSnippetName, SnippetRef pSearchPoint)
        {
            if (pSearchPoint.ChildSnippets != null)
            {
                foreach (SnippetRef snp in pSearchPoint.ChildSnippets)
                {
                    if (StrUtils.IsSameText(snp.Caption, pSearchPoint.Caption))
                        return snp;
                }
            }
            return null;
        }

        private void discoverSnippetProps()
        {
            this.snippetConstructor = this.snippetType.GetConstructor(new Type[] { });

            this.snippetProps.Clear();
            PropertyInfo[] props = this.snippetType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            foreach (PropertyInfo prop in props)
            {
                this.snippetProps.Add(prop.Name.ToUpper(), prop);
            }
        }

        /// <summary>
        /// Load snippets from specified storage.
        /// Agreement is:
        /// * snippet type should have one default constructor without parameters
        /// * snippet type should have public property "Name" with public setter
        /// * snippet loader only loads public properties which have a public setter
        /// </summary>
        private void loadSnippets()
        {
            if (this.snippetConstructor == null) return;
            if (this.registry != null) return;

            this.registry = new XmlRegistry(this.registryFilename, true);
            this.rootKey = this.registry.OpenKey(this.snippetsRootKeyPath, true);
            this.rootSnippet = new SnippetRef(this) { ChildSnippets = new List<object>(), RegKey = this.rootKey };

            tryLoadSnippetsFrom(this.rootKey, this.rootSnippet);
        }

        private void tryLoadSnippetsFrom(XmlRegistryKey pKey, SnippetRef pTargetSnippet)
        {
            foreach (string keyName in pKey.KeyNames)
            {
                XmlRegistryKey regKeySnp = this.registry.OpenKey(pKey.Path + "/" + keyName, false);
                if (regKeySnp != null)
                {
                    SnippetRef snRef;
                    if (tryLoadSnippetFrom(regKeySnp, out snRef))
                    {
                        if (pTargetSnippet.ChildSnippets == null)
                            pTargetSnippet.ChildSnippets = new List<object>();
                        pTargetSnippet.ChildSnippets.Add(snRef);
                        if (pKey.KeyNames.Length > 0)
                        {                            
                            tryLoadSnippetsFrom(regKeySnp, snRef);
                        }
                    }
                    if (snRef != null)
                        snRef.RegKey = regKeySnp;
                    else
                        regKeySnp.Close();
                }
            }
        }

        private bool tryLoadSnippetFrom(XmlRegistryKey pKey, out SnippetRef pRef)
        {
            pRef = null;
            string name = pKey.Name;
            if (pKey.ValueNames.Length > 0)
            {
                object snippet = this.snippetConstructor.Invoke(new object[] { });
                if (snippet != null)
                {
                    PropertyInfo prop;
                    if (this.snippetProps.TryGetValue("NAME", out prop))
                    {
                        prop.SetValue(snippet, name, null);
                        int propsCount = 0;
                        foreach (string propName in pKey.ValueNames)
                        {
                            if (!this.snippetProps.TryGetValue(propName.ToUpper(), out prop)) continue;

                            string sv = pKey.ReadString(propName);
                            object v = sv;
                            if (!prop.PropertyType.Equals(typeof(String)))
                                v = Convert.ChangeType(sv, prop.PropertyType);
                            prop.SetValue(snippet, v, null);
                            propsCount++;
                        }
                        if (propsCount == 0)
                            snippet = null;
                        pRef = new SnippetRef(this) { Caption = name, Snippet = snippet };
                    }
                }
            }
            else
            {
                pRef = new SnippetRef(this) { Caption = name, Snippet = null, ChildSnippets = new List<object>() };
            }
            return (pRef != null);
        }

        private static string DEFAULT_SNIPPETS_FILE = ""
            + "<?xml version=\"1.0\"?>\n" 
            + "<Registry savedAt=\"2015-12-14:14:03:01.021000\" filename=\"WmiQuery.snippets\" application=\"AdoNetQuery_x86\">\n"
            + "    <Key name=\"default\">\n"
            + "        <Item name=\"default\" type=\"string\">@</Item>\n"
            + "    </Key>\n"
            + "    <Key name=\"Snippets\">\n"
            + "    </Key>\n"
            + "</Registry>\n";

        private string registryFilename;
        private string snippetsRootKeyPath;
        private XmlRegistry registry = null;
        private XmlRegistryKey rootKey;

        private Type snippetType;
        private ConstructorInfo snippetConstructor;
        private Dictionary<string, PropertyInfo> snippetProps = new Dictionary<string, PropertyInfo>();

        private SnippetRef rootSnippet;

        #endregion // Implementation Details
    }


    /// <summary>
    /// SnippetRef - wrapper for snippet storage object
    /// </summary>
    public class SnippetRef
    {
        public SnippetRef(SnippetsStorage pStorage)
        {
            this.Storage = pStorage;
        }

        public SnippetsStorage Storage { get; protected set; }
        public XmlRegistryKey RegKey;
        public string Caption;
        public object Snippet;
        public List<object> ChildSnippets;

        public bool IsFolder { get { return (this.Snippet == null); } }

        public string this[string pPropName]
        {
            get { return this.Storage.getSnippetProp(this, pPropName); }
            set { this.Storage.setSnippetProp(this, pPropName, value); }
        }

        public void CreateSnippetInstance()
        {
            if (this.Snippet != null) return;

            this.Snippet = this.Storage.createSnippetInstance();
        }
    }
}
