using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;
using BOOL = System.Boolean;
using DWORD = System.UInt32;
using LPWSTR = System.String;

namespace clsDR_tool
{
    public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);
    public delegate void LogEventHandler(object sender, LogEventArgs e);

    public enum Method : int { Method_LocalHost = 1, Method_FTP = 2, Method_NetDisk = 3 };

    public class ProgressEventArgs : EventArgs
    {
        public string Stage;
        public string Current_Dir;
        public long[] Bytes;
        public int[] Dir_Info;
        public int Position;

        public ProgressEventArgs(string Stage, string Current_Dir, long[] Bytes, int[] Dir_Info, int Position)
        {
            this.Stage = Stage;
            this.Current_Dir = Current_Dir;
            this.Bytes = Bytes;
            this.Dir_Info = Dir_Info;
            this.Position = Position;
        }
    }

    public class LogEventArgs : EventArgs
    {
        public string Datetime;
        public bool InfoType;
        public string Message;

        public LogEventArgs(string dt, bool type, string msg)
        {
            this.Datetime = dt;
            this.InfoType = type;
            this.Message = msg;
        }
    }

    public class FileReplicator
    {
        private bool _CloneStructure;
        private Dictionary<int, string> _SourcePath;
        private string _LocalPath;
        private string _NetDiskPath;
        private string _FTP_IP;
        private string _Username;
        private string _Password;
        private Method _MethodType;
        private bool _CreateFolder = true;
        private int _THREAD_NUM = 1;
        private string _ExcludeChar_FilePath = "";
        private bool _Cancel;
        

        public bool CloneStructure { set { _CloneStructure = value; } get { return _CloneStructure; } }
        public Dictionary<int, string> SourcePath { set { _SourcePath = value; } get { return _SourcePath; } }
        public string LocalPath { set { _LocalPath = value; } get { return _LocalPath; } }
        public string NetDiskPath { set { _NetDiskPath = value; } get { return _NetDiskPath; } }
        public string FTP_IP { set { _FTP_IP = value; } get { return _FTP_IP; } }
        public string Username { set { _Username = value; } get { return _Username; } }
        public string Password { set { _Password = value; } get { return _Password; } }
        public Method MethodType { set { _MethodType = value; } get { return _MethodType; } }
        public bool CreateFolder { set { _CreateFolder = value; } get { return _CreateFolder; } }
        public int THREAD_NUM { set { _THREAD_NUM = value; } get { return _THREAD_NUM; } }
        public string ExcludeChar_FilePath { set { _ExcludeChar_FilePath = value; } get { return _ExcludeChar_FilePath; } } 
        public bool Cancel { set { _Cancel = value; } get { return _Cancel; } }

        public event ProgressEventHandler OnStartEvent;
        public event ProgressEventHandler OnDoEvent;
        public event ProgressEventHandler OnEndEvent;
        public event LogEventHandler OnLogEvent;

        public int DIR_NUM = 0;
        public long TOTAL_BYTE = 0;
        private int current_dir_num = 0;
        private long current_dir_byte = 0;
        private int current_thread_num = 0;

        public void SearchFolder(object args)
        {
            int i = 0;

            if (current_dir_num == 0)
            {
                Output_Log(new LogEventArgs(DateTime.Now.ToString(), true, "Raplicate start"));

                foreach(string record in SourcePath.Values)
                {
                    DIR_NUM++;
                    DirectoryInfo dir = new DirectoryInfo(record);
                    IEnumerable<FileInfo> files = dir.GetFiles("*", SearchOption.AllDirectories);
                    foreach (FileInfo file in files)
                    {
                        TOTAL_BYTE += GetFileLength(file);
                    }
                }
                SendEvents(new ProgressEventArgs("start", "", new long[] { current_dir_byte, TOTAL_BYTE }, new int[] { current_dir_num, DIR_NUM }, 0));
            }

            while (current_dir_num < DIR_NUM)
            {
                if (current_thread_num < THREAD_NUM && i < this.SourcePath.Count)
                {
                    current_dir_byte = 0;
                    DirectoryInfo dir = new DirectoryInfo(this.SourcePath[i]);
                    IEnumerable<FileInfo> files = dir.GetFiles("*", SearchOption.AllDirectories);
                    foreach (FileInfo file in files)
                    {
                        current_dir_byte += GetFileLength(file);
                    }

                    //StartCopy(new string[] { this.SourcePath[i], _CreateFolder.ToString() });
                    Thread thd = new Thread(new ThreadStart(delegate() { StartCopy(new string[] { this.SourcePath[i], _CreateFolder.ToString() }); }));
                    thd.IsBackground = true;
                    thd.Start();
                    current_thread_num++;
                    SendEvents(new ProgressEventArgs("do", this.SourcePath[i], new long[] { current_dir_byte, TOTAL_BYTE }, new int[] { current_dir_num, DIR_NUM }, (int)(100 * (double)current_dir_num / (double)DIR_NUM)));
                }
                
                //User cancel the process
                if (this.Cancel)
                {
                    SendEvents(new ProgressEventArgs("end", this.SourcePath[i], new long[] { current_dir_byte, TOTAL_BYTE }, new int[] { current_dir_num, DIR_NUM }, (int)(100 * (double)current_dir_num / (double)DIR_NUM)));
                    return;
                }
                i++;
            }
            Output_Log(new LogEventArgs(DateTime.Now.ToString(), true, "Complete"));
            SendEvents(new ProgressEventArgs("end", "", new long[] { current_dir_byte, TOTAL_BYTE }, new int[] { current_dir_num, DIR_NUM }, 100));
        }

        private void SendEvents(ProgressEventArgs e)
        {
            switch (e.Stage)
            {
                case "start":
                    OnStartEvent(this, e);
                    break;
                case "do":
                    OnDoEvent(this, e);
                    break;
                case "end":
                    OnEndEvent(this, e);
                    break;
                default:
                    OnDoEvent(this, e);
                    break;
            }
        }

        private void StartCopy(object args)
        {
            string[] param = (string[])args;
            string strSourcePath = param[0];
            bool bolCreateFolder = param[1].ToLower() == "true" ? true : false;

            if (this.MethodType == Method.Method_LocalHost)
            {
                ProcessXcopy(strSourcePath, this.LocalPath, _ExcludeChar_FilePath, _CreateFolder);
            }
            else if (this.MethodType == Method.Method_FTP)
            {
                ProcessXcopy(strSourcePath, this.FTP_IP, _ExcludeChar_FilePath, _CreateFolder);
            }
            else if (this.MethodType == Method.Method_NetDisk) //(include Domain Machine)
            {
                string dest_dir = this.NetDiskPath;
                if (_CloneStructure)
                {
                    CreateFolder = false;
                    dest_dir = ReplaceDestDirectory(strSourcePath, this.NetDiskPath);
                    DirectoryInfo dest_info = new DirectoryInfo(dest_dir);
                    if (!dest_info.Exists)
                    {
                        dest_info.Create();
                        dest_info.Refresh();
                    }
                }
                ProcessXcopy(strSourcePath, dest_dir, _ExcludeChar_FilePath, _CreateFolder);
            }

            Add_DIR_NUM();
        }

        private void ProcessXcopy(string SourceDirectory, string DestDirectory, string ExcludeChar_FilePath, bool CreateFolder)
        {
            if (CreateFolder)
            {
                DirectoryInfo info = new DirectoryInfo(SourceDirectory);
                DestDirectory += "\\" + info.Name;
                info = new DirectoryInfo(DestDirectory);
                if (!info.Exists)
                    info.Create();
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false; //if show cmd window
            startInfo.UseShellExecute = false;
            startInfo.FileName = "xcopy";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            if (this.ExcludeChar_FilePath.Length == 0)
                startInfo.Arguments = "\"" + SourceDirectory + "\"" + " " + "\"" + DestDirectory + "\"" + @" /Y/E/R/D";
            else
                startInfo.Arguments = "\"" + SourceDirectory + "\"" + " " + "\"" + DestDirectory + "\"" + " " + @"/exclude:" + this.ExcludeChar_FilePath + @" /Y/E/R/D";
            
            if (ExcludeChar_FilePath.Length != 0)
            {
            }

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (IOException ex)
            {
                Output_Log(new LogEventArgs(DateTime.Now.ToString(), false, "Destination deny to access " + SourceDirectory));
            }
            catch (Exception ex)
            {
                Output_Log(new LogEventArgs(DateTime.Now.ToString(), false, "Unexpected error. " + SourceDirectory));
            }
        }

        private string ReplaceDestDirectory(string SourceDirectory, string DestDirectory)
        {
            string source_disk;
            string dest_disk;
            if (SourceDirectory.Contains(":\\"))
            {
                //exmaple: D:\
                source_disk = SourceDirectory.Substring(0, SourceDirectory.IndexOf(":\\") + 2);
                dest_disk = SourceDirectory.Substring(0, SourceDirectory.IndexOf(":\\")) + "$";
            }
            else
            {
                //example: \\glap01\D$
                source_disk = SourceDirectory.Substring(0, SourceDirectory.IndexOf("$") + 1);
                dest_disk = SourceDirectory.Substring(SourceDirectory.TrimStart('\\').IndexOf("\\") + 3, SourceDirectory.IndexOf("$") + 1);
            }
            string dest_dir = SourceDirectory.Replace(source_disk, "\\\\" + DestDirectory + "\\" + dest_disk + "\\");
            return dest_dir;
        }

        private void Output_Log(LogEventArgs e)
        {
            lock(this)
            {
                OnLogEvent(this, e);
            }
        }

        private void Add_DIR_NUM()
        {
            lock(this)
            {
                current_dir_num++;
            }

            lock (this)
            {
                current_thread_num--;
            }
        }

        public void CancelReplicate(CancelEventArgs e)
        {
            this.Cancel = e.Cancel;
        }

        private long GetFileLength(FileInfo FI)
        {
            long retval;
            try
            {
                retval = FI.Length;
            }
            catch (System.IO.FileNotFoundException)
            {
                retval = 0;
            }
            return retval;
        }
    }

    public class StructureCreater
    {
        private Dictionary<int, string> _SourcePath;
        private string _NetDiskPath;
        private string _Username;
        private string _Password;
        private string _ExcludeChar_FilePath = "";
        private bool _Cancel;

        public Dictionary<int, string> SourcePath { set { _SourcePath = value; } get { return _SourcePath; } }
        public string NetDiskPath { set { _NetDiskPath = value; } get { return _NetDiskPath; } }
        public string Username { set { _Username = value; } get { return _Username; } }
        public string Password { set { _Password = value; } get { return _Password; } }
        public string ExcludeChar_FilePath { set { _ExcludeChar_FilePath = value; } get { return _ExcludeChar_FilePath; } }
        public bool Cancel { set { _Cancel = value; } get { return _Cancel; } }

        public event ProgressEventHandler OnStartEvent;
        public event ProgressEventHandler OnDoEvent;
        public event ProgressEventHandler OnEndEvent;
        public event LogEventHandler OnLogEvent;

        public int DIR_NUM = 0;
        private int current_dir_num = 0;

        public void SearchFolder(object args)
        {
            int i = 0;

            if (current_dir_num == 0)
            {
                foreach (string record in SourcePath.Values)
                {
                    DIR_NUM++;
                }
                Output_Log(new LogEventArgs(DateTime.Now.ToString(), true, "Create structure start"));
                SendEvents(new ProgressEventArgs("start", "", null, new int[] { current_dir_num, DIR_NUM }, 100));
            }

            while (current_dir_num < DIR_NUM)
            {
                StartCreate(new string[] { this.SourcePath[i] });
                
                SendEvents(new ProgressEventArgs("do", this.SourcePath[i], null, new int[] { current_dir_num, DIR_NUM }, (int)(100 * (double)current_dir_num / (double)DIR_NUM)));

                //User cancel the process
                if (this.Cancel)
                {
                    SendEvents(new ProgressEventArgs("end", this.SourcePath[i], null, new int[] { current_dir_num, DIR_NUM }, (int)(100 * (double)current_dir_num / (double)DIR_NUM)));
                    return;
                }
                i++;
            }
            Output_Log(new LogEventArgs(DateTime.Now.ToString(), true, "Complete"));
            SendEvents(new ProgressEventArgs("end", "", null, new int[] { current_dir_num, DIR_NUM }, (int)(100 * (double)current_dir_num / (double)DIR_NUM)));
        }

        private void SendEvents(ProgressEventArgs e)
        {
            switch (e.Stage)
            {
                case "start":
                    OnStartEvent(this, e);
                    break;
                case "do":
                    OnDoEvent(this, e);
                    break;
                case "end":
                    OnEndEvent(this, e);
                    break;
                default:
                    OnDoEvent(this, e);
                    break;
            }
        }

        private void StartCreate(object args)
        {
            string[] param = (string[])args;
            string strSourcePath = param[0];
            
            ProcessCreate(strSourcePath, this.NetDiskPath, _ExcludeChar_FilePath);
            Add_DIR_NUM();
        }

        private void ProcessCreate(string SourceDirectory, string DestDirectory, string ExcludeChar_FilePath)
        {
            string[] temp_dirs = Directory.GetDirectories(SourceDirectory, "*", SearchOption.AllDirectories);
            List<string> source_dirs = new List<string>();
            if (ExcludeChar_FilePath.Length != 0)
            {
                string[] exclude_chars = File.ReadAllLines(this.ExcludeChar_FilePath);
                foreach (string exclude_char in exclude_chars)
                {
                    foreach (string temp_dir in temp_dirs)
                    {
                        if (!temp_dir.Contains(exclude_char))
                            source_dirs.Add(temp_dir);
                    }                        
                }
            }
            else
            {
                foreach (string temp_dir in temp_dirs)
                    source_dirs.Add(temp_dir);
            }

            foreach (string source_dir in source_dirs)
            {
                string dest_dir = ReplaceDestDirectory(source_dir, DestDirectory);
                DirectoryInfo dest_info = new DirectoryInfo(dest_dir);
                if (!dest_info.Exists)
                {
                    dest_info.Create();
                    dest_info.Refresh();
                }
            }
        }

        private string ReplaceDestDirectory(string SourceDirectory, string DestDirectory)
        {
            string source_disk;
            string dest_disk;
            if (SourceDirectory.Contains(":\\"))
            {
                //exmaple: D:\
                source_disk = SourceDirectory.Substring(0, SourceDirectory.IndexOf(":\\") + 2);
                dest_disk = SourceDirectory.Substring(0, SourceDirectory.IndexOf(":\\")) + "$";
            }
            else
            {
                //example: \\glap01\D$
                source_disk = SourceDirectory.Substring(0, SourceDirectory.IndexOf("$") + 1);
                dest_disk = SourceDirectory.Substring(SourceDirectory.TrimStart('\\').IndexOf("\\") + 3, SourceDirectory.IndexOf("$") + 1);
            }
            string dest_dir = SourceDirectory.Replace(source_disk, "\\\\" + DestDirectory + "\\" + dest_disk + "\\");
            return dest_dir;
        }

        private void Output_Log(LogEventArgs e)
        {
            lock (this)
            {
                OnLogEvent(this, e);
            }
        }

        private void Add_DIR_NUM()
        {
            lock (this)
            {
                current_dir_num++;
            }

            //lock (this)
            //{
            //    current_thread_num--;
            //}
        }

        public void CancelReplicate(CancelEventArgs e)
        {
            this.Cancel = e.Cancel;
        }
    }

    public class UNCAccess : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct USE_INFO_2
        {
            internal LPWSTR ui2_local;
            internal LPWSTR ui2_remote;
            internal LPWSTR ui2_password;
            internal DWORD ui2_status;
            internal DWORD ui2_asg_type;
            internal DWORD ui2_refcount;
            internal DWORD ui2_usecount;
            internal LPWSTR ui2_username;
            internal LPWSTR ui2_domainname;
        }

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern NET_API_STATUS NetUseAdd(
            LPWSTR UncServerName,
            DWORD Level,
            ref USE_INFO_2 Buf,
            out DWORD ParmError);

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern NET_API_STATUS NetUseDel(
            LPWSTR UncServerName,
            LPWSTR UseName,
            DWORD ForceCond);

        [DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
        private static extern NET_API_STATUS NetShareEnum(
            StringBuilder serverName,
            int level,
            ref IntPtr bufPtr,
            uint prefMaxLen,
            ref int entriesRead,
            ref int totalEntries,
            ref int resumeHandle
            );

        [DllImport("Netapi32.dll", SetLastError = true)]
        static extern int NetApiBufferFree(IntPtr Buffer);

        public enum NET_API_STATUS : uint
        {
            NERR_Success = 0,
            NERR_InvalidComputer = 2351,
            NERR_NotPrimary = 2226,
            NERR_SpeGroupOp = 2234,
            NERR_LastAdmin = 2452,
            NERR_BadPassword = 2203,
            NERR_PasswordTooShort = 2245,
            NERR_UserNotFound = 2221,
            ERROR_ACCESS_DENIED = 5,
            ERROR_NOT_ENOUGH_MEMORY = 8,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_INVALID_NAME = 123,
            ERROR_INVALID_LEVEL = 124,
            ERROR_MORE_DATA = 234,
            ERROR_SESSION_CREDENTIAL_CONFLICT = 1219
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct _SHARE_INFO_0
        {
            [MarshalAsAttribute(UnmanagedType.LPWStr)]
            public string shi0_netname;
        }

        private bool disposed = false;

        private string sUNCPath;
        private string sUser;
        private string sPassword;
        private string sDomain;
        private int iLastError;

        /// <summary>
        /// A disposeable class that allows access to a UNC resource with credentials.
        /// </summary>
        public UNCAccess()
        {
        }

        /// <summary>
        /// The last system error code returned from NetUseAdd or NetUseDel.  Success = 0
        /// </summary>
        public int LastError
        {
            get { return iLastError; }
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                NetUseDelete();
            }
            disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Connects to a UNC path using the credentials supplied.
        /// </summary>
        /// <param name="UNCPath">Fully qualified domain name UNC path</param>
        /// <param name="User">A user with sufficient rights to access the path.</param>
        /// <param name="Domain">Domain of User.</param>
        /// <param name="Password">Password of User</param>
        /// <returns>True if mapping succeeds.  Use LastError to get the system error code.</returns>
        public bool NetUseWithCredentials(string UNCPath, string User, string Domain, string Password)
        {
            sUNCPath = UNCPath;
            sUser = User;
            sPassword = Password;
            sDomain = Domain;
            return NetUseWithCredentials();
        }

        private bool NetUseWithCredentials()
        {
            NET_API_STATUS returncode;
            try
            {
                USE_INFO_2 useinfo = new USE_INFO_2();

                useinfo.ui2_remote = sUNCPath;
                useinfo.ui2_username = sUser;
                useinfo.ui2_domainname = sDomain;
                useinfo.ui2_password = sPassword;
                useinfo.ui2_asg_type = 0;
                useinfo.ui2_usecount = 1;
                uint paramErrorIndex;
                returncode = NetUseAdd(null, 2, ref useinfo, out paramErrorIndex);
                iLastError = (int)returncode;
                return returncode == 0;
            }
            catch
            {
                iLastError = Marshal.GetLastWin32Error();
                return false;
            }
        }

        /// <summary>
        /// Ends the connection to the remote resource 
        /// </summary>
        /// <returns>True if it succeeds.  Use LastError to get the system error code</returns>
        public bool NetUseDelete()
        {
            NET_API_STATUS returncode;
            try
            {
                returncode = NetUseDel(null, sUNCPath, 2);
                iLastError = (int)returncode;
                return (returncode == 0);
            }
            catch
            {
                iLastError = Marshal.GetLastWin32Error();
                return false;
            }
        }

        /// <summary>
        /// Get remote share folder name
        /// </summary></returns>
        /// <returns>Array of share folder name<returns>
        public string[] EnumNetShares(string remoteMachineName)
        {
            List<string> shares = new List<string>();
            StringBuilder serverName = new StringBuilder(remoteMachineName);
            int level = 0;
            IntPtr bufPtr = IntPtr.Zero;
            uint prefMaxLen = 0xFFFFFFFF;
            int entriesRead = 0;
            int totalEntries = 0;
            int resumeHandle = 0;
            int structSize = Marshal.SizeOf(typeof(_SHARE_INFO_0));
            NET_API_STATUS result = NetShareEnum(serverName, level, ref bufPtr, prefMaxLen, ref entriesRead, ref totalEntries, ref resumeHandle);
            if (result == NET_API_STATUS.NERR_Success)
            {
                IntPtr current = bufPtr;
                for (int i = 0; i < entriesRead; i++)
                {
                    _SHARE_INFO_0 shareInfo = (_SHARE_INFO_0)Marshal.PtrToStructure(current, typeof(_SHARE_INFO_0));
                    shares.Add(shareInfo.shi0_netname);
                    current = new IntPtr(current.ToInt32() + structSize);
                }
            }
            else if (result == NET_API_STATUS.ERROR_MORE_DATA)
            {
                NetApiBufferFree(bufPtr);
            }

            return shares.ToArray();
        }

        ~UNCAccess()
        {
            Dispose();
        }

    }
}
