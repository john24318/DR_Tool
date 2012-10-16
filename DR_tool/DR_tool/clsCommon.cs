using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace GL_Utility
{
    #region common function for Form (UI)
    
    public interface Common
    {
        void ReadFile_ExcludeEOF(string path);
        void WriteFile(string path, string new_record);
    }

    public class IniCommon : Common
    {
        public event AddIniEventHandler OnAddIniEvent;
        private int intID;
        private static IniCommon common;
        private bool isRefresh = false;
        private Dictionary<int, string> SourcePath;
        private const string EOF_Mark = "[EOF]";
        public IniCommon() { }
        public IniCommon(int intID) { this.ID = intID; }

        public int ID { get { return intID; } set { intID = value; } }

        public static IniCommon GetInstance(int intID)
        {
            if (common == null)
                common = new IniCommon(intID);

            return common;
        }

        public void ReadFile_ExcludeEOF(string path)
        {
            if (SourcePath == null)
            {
                SourcePath = new Dictionary<int, string>();
                if (!File.Exists(path))
                    return;

                string strLine = "";
                FileInfo f = new FileInfo(path);
                StreamReader srFile = f.OpenText();
                int i = 0;
                while ((strLine = srFile.ReadLine()) != null)
                {
                    if (strLine.Trim().Length > 0 && strLine != EOF_Mark)
                        SourcePath.Add(i, strLine.Trim());

                    i++;
                }
                srFile.Close();
            }
        }

        public void WriteFile(string path, string new_record)
        {
            if (!File.Exists(path))
            {
                FileStream f = File.Create(path);
                f.Close();
            }

            StreamWriter w = File.AppendText(path);
            w.WriteLine(new_record);
            w.Flush();
            w.Close();
            isRefresh = false;
        }

        public void RefreshFile(string path)
        {
            //if ini file is not sorted, then rewrite the ini file
            if (!isRefresh)
            {
                List<string> list = new List<string>();
                SourcePath = new Dictionary<int, string>();
                if (!File.Exists(path))
                    return;

                string strLine = "";
                FileInfo f = new FileInfo(path);
                StreamReader srFile = f.OpenText();
                while ((strLine = srFile.ReadLine()) != null)
                {
                    if (strLine.Trim().Length > 0)
                        list.Add(strLine.Trim());
                }
                srFile.Close();

                if (list.Count != 0)
                {
                    if (list[list.Count - 1] == EOF_Mark)
                    {
                        list.Remove(EOF_Mark);
                        list.Sort();
                        int i = 0;
                        foreach (string record in list)
                        {
                            SourcePath.Add(i, record);
                            i++;
                        }
                    }
                    else
                    {
                        list.Remove(EOF_Mark);
                        list.Sort();

                        File.Delete(path);
                        StreamWriter w = File.AppendText(path);
                        int i = 0;
                        foreach (string record in list)
                        {
                            SourcePath.Add(i, record);
                            w.WriteLine(record);
                            i++;
                        }
                        w.WriteLine(EOF_Mark);
                        w.Flush();
                        w.Close();
                    }
                }
            }
            isRefresh = true;
        }

        public void ValidateNewRecord(string New_Record)
        {
            List<int> Existed_Index = new List<int>();
            List<string> New_Dir_list = new List<string>();

            if (!Directory.Exists(New_Record))
            {
                OnAddIniEvent(this, new AddIniEventArgs(this.ID, new int[] { -1 }, New_Record));
                return;
            }
            else
            {
                string[] new_dirs = null;
                try
                {
                    new_dirs = Directory.GetDirectories(New_Record, "*", SearchOption.AllDirectories);
                }
                catch (Exception ex)
                {
                    OnAddIniEvent(this, new AddIniEventArgs(this.ID, new int[] { -1 }, New_Record));
                    return;
                }

                New_Dir_list.Add(New_Record.ToLower());
                foreach (string new_dir in new_dirs)
                {
                    New_Dir_list.Add(new_dir.ToLower());
                }
            }

            //Check if existed path list contain the new path
            Dictionary<int, string> dic = GetFileInfo();
            foreach (KeyValuePair<int, string> path in dic)
            {
                List<string> list = new List<string>();

                string[] dirs = null;
                try
                {
                    dirs = Directory.GetDirectories(path.Value, "*", SearchOption.AllDirectories);
                }
                catch (Exception ex)
                {
                    OnAddIniEvent(this, new AddIniEventArgs(this.ID, new int[] { -1 }, New_Record));
                    return;
                }

                list.Add(path.Value.ToLower());
                foreach (string dir in dirs)
                {
                    list.Add(dir.ToLower());
                }

                if (list.Contains(New_Record.ToLower()))
                {
                    Existed_Index.Add(path.Key);
                }
                else if (New_Dir_list.Contains(path.Value.ToLower()))
                {
                    Existed_Index.Add(path.Key);
                }
            }

            OnAddIniEvent(this, new AddIniEventArgs(this.ID, Existed_Index.ToArray(), New_Record));
        }

        public void WriteDictionaryToFile(string path)
        {
            File.Delete(path);
            StreamWriter w = File.AppendText(path);
            w.AutoFlush = true;
            Dictionary<int, string> dic = this.GetFileInfo();
            foreach (KeyValuePair<int, string> record in dic)
            {
                w.WriteLine(record.Value);
            }
            w.Close();
        }

        public Dictionary<int, string> GetFileInfo()
        {
            if (SourcePath == null) { return new Dictionary<int, string>(); }
            else { return SourcePath; }
        }

        public void AddIniDictionary(string new_record)
        {
            Dictionary<int, string> dic = this.GetFileInfo();

            if (isRefresh)
            {
                dic.Add(dic.Count, new_record);
            }
            else
            {
                int i = 0;
                foreach (KeyValuePair<int, string> record in dic)
                {
                    i = i <= record.Key ? record.Key + 1 : i;
                }
                dic.Add(i, new_record);
            }
            isRefresh = false;
        }

        public void RemoveIniDictionary(int[] Path_Index)
        {
            Dictionary<int, string> dic = this.GetFileInfo();
            foreach (int index in Path_Index)
                dic.Remove(index);
            isRefresh = false;
        }
    }

    public class StructureCommon : IniCommon
    {
        private static StructureCommon common;
        private StructureCommon(int intID) { this.ID = intID; }

        public new static StructureCommon GetInstance(int intID)
        {
            if (common == null)
                common = new StructureCommon(intID);

            return common;
        }
    }

    public class DeploymentCommon : IniCommon
    {
        private static DeploymentCommon common;
        private DeploymentCommon(int intID) { this.ID = intID; }

        public new static DeploymentCommon GetInstance(int intID)
        {
            if (common == null)
                common = new DeploymentCommon(intID);

            return common;
        }
    }

    public class SourceCodeCommon : IniCommon
    {
        private static SourceCodeCommon common;
        private SourceCodeCommon(int intID) { this.ID = intID; }

        public new static SourceCodeCommon GetInstance(int intID)
        {
            if (common == null)
                common = new SourceCodeCommon(intID);

            return common;
        }
    }

    public class Settings_Email_Common : IniCommon
    {
        public new event AddIniEventHandler OnAddIniEvent;
        private static Settings_Email_Common common;
        private bool bolEnableSend;
        private string strSMTP;
        private int intPort;
        private string strFrom;
        private const string SMTP_Mark = "[SMTP]";
        private Settings_Email_Common(int intID) { this.ID = intID; }

        public bool EnableSend { get { return bolEnableSend; } set { bolEnableSend = value; } }
        public string SMTP { get { return strSMTP; } set { strSMTP = value; } }
        public int Port { get { return intPort; } set { intPort = value; } }
        public string From { get { return strFrom; } set { strFrom = value; } }

        public new static Settings_Email_Common GetInstance(int intID)
        {
            if (common == null)
                common = new Settings_Email_Common(intID);

            return common;
        }

        public void ReadFile_Settings(string path)
        {
            string strLine = "";
            FileInfo f = new FileInfo(path);
            StreamReader srFile = f.OpenText();
            while ((strLine = srFile.ReadLine()) != null)
            {
                if (strLine.Trim().Length > 0 && strLine == SMTP_Mark)
                {
                    if ((strLine = srFile.ReadLine()) != null)
                        bolEnableSend = Convert.ToInt32(strLine.Trim()) == 1 ? true : false;
                    if ((strLine = srFile.ReadLine()) != null)
                        strSMTP = strLine.Trim();
                    if ((strLine = srFile.ReadLine()) != null)
                        intPort = Convert.ToInt32(strLine.Trim());
                    if ((strLine = srFile.ReadLine()) != null)
                        strFrom = strLine.Trim();
                    break;
                }
            }
            srFile.Close();
        }

        public new void ValidateNewRecord(string New_Record)
        {
            List<int> Existed_Index = new List<int>();

            Dictionary<int, string> dic = this.GetFileInfo();
            foreach (KeyValuePair<int, string> record in dic)
            {
                if (record.Value.ToLower().Equals(New_Record.ToLower()))
                {
                    OnAddIniEvent(this, new AddIniEventArgs(this.ID, new int[] { -1 }, New_Record));
                    return;
                }
            }
            OnAddIniEvent(this, new AddIniEventArgs(this.ID, Existed_Index.ToArray(), New_Record));
        }

        public void WriteFile_Settings(string path, string[] new_record)
        {
            StringBuilder sb = new StringBuilder();
            string strLine = "";
            FileInfo f = new FileInfo(path);
            StreamReader srFile = f.OpenText();
            while ((strLine = srFile.ReadLine()) != null)
            {
                sb.Append(strLine + "\r\n");

                if (strLine == SMTP_Mark)
                {
                    foreach (string record in new_record)
                        sb.Append(record + "\r\n");

                    break;
                }
            }
            srFile.Close();

            File.Delete(path);
            StreamWriter w = File.AppendText(path);
            w.AutoFlush = true;
            w.Write(sb.ToString());
            w.Close();

            bolEnableSend = Convert.ToInt32(new_record[0]) == 1 ? true : false;
            strSMTP = new_record[1];
            intPort = Convert.ToInt32(new_record[2]);
            strFrom = new_record[3];
        }

        public void SendMail(string subject, string body, string from, string SMTP, int port)
        {
            if (SMTP.Length != 0)
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.Subject = subject;
                message.From = new System.Net.Mail.MailAddress(from);
                message.Body = body;
                foreach (string address in this.GetFileInfo().Values)
                {
                    message.To.Add(address);
                }

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(SMTP, port);
                smtp.Send(message);
            }
        }
    }

    public class Settings_Char_Common : IniCommon
    {
        public new event AddIniEventHandler OnAddIniEvent;
        private static Settings_Char_Common common;
        private Settings_Char_Common(int intID) { this.ID = intID; }

        public new static Settings_Char_Common GetInstance(int intID)
        {
            if (common == null)
                common = new Settings_Char_Common(intID);

            return common;
        }

        public new void ValidateNewRecord(string New_Record)
        {
            List<int> Existed_Index = new List<int>();

            Dictionary<int, string> dic = this.GetFileInfo();
            foreach (KeyValuePair<int, string> record in dic)
            {
                if (record.Value.ToLower().Contains(New_Record.ToLower()))
                {
                    Existed_Index.Add(record.Key);
                }
                else if (New_Record.ToLower().Contains(record.Value.ToLower()))
                {
                    OnAddIniEvent(this, new AddIniEventArgs(this.ID, new int[] { -1 }, New_Record));
                    return;
                }
            }
            OnAddIniEvent(this, new AddIniEventArgs(this.ID, Existed_Index.ToArray(), New_Record));
        }
    }

    public class Settings_Parallel_Common : Common
    {
        private int intID;
        private static Settings_Parallel_Common common;
        private int intParallel_Num;
        private const string Parallel_Number_Mark = "[Parallel Number]";
        private Settings_Parallel_Common(int intID) { this.ID = intID; }

        public int ID { get { return intID; } set { intID = value; } }
        public int Parallel_Num { get { return intParallel_Num; } set { intParallel_Num = value; } }

        public static Settings_Parallel_Common GetInstance(int intID)
        {
            if (common == null)
                common = new Settings_Parallel_Common(intID);

            return common;
        }

        public void ReadFile_ExcludeEOF(string path)
        {
        }

        public void ReadFile_Settings(string path)
        {
            string strLine = "";
            FileInfo f = new FileInfo(path);
            StreamReader srFile = f.OpenText();
            while ((strLine = srFile.ReadLine()) != null)
            {
                if (strLine.Trim().Length > 0 && strLine == Parallel_Number_Mark)
                {
                    if ((strLine = srFile.ReadLine()) != null)
                        Parallel_Num = Convert.ToInt32(strLine.Trim());
                    break;
                }
            }
            srFile.Close();
        }

        public void WriteFile(string path, string new_record)
        {
            if (!File.Exists(path))
            {
                FileStream f = File.Create(path);
                f.Close();
            }

            StreamWriter w = File.AppendText(path);
            w.WriteLine(new_record);
            w.Flush();
            w.Close();
        }

        public void WriteFile_Settings(string path, string[] new_record)
        {
            StringBuilder sb = new StringBuilder();
            string strLine = "";
            FileInfo f = new FileInfo(path);
            StreamReader srFile = f.OpenText();
            while ((strLine = srFile.ReadLine()) != null)
            {
                sb.Append(strLine + "\r\n");

                if (strLine == Parallel_Number_Mark)
                {
                    foreach (string record in new_record)
                        sb.Append(record + "\r\n");
                    srFile.ReadLine();
                }
            }
            srFile.Close();
            File.Delete(path);

            StreamWriter w = File.AppendText(path);
            w.AutoFlush = true;
            w.Write(sb.ToString());
            w.Close();

            intParallel_Num = Convert.ToInt32(new_record[0]);
        }
    }

    public class SecureCommon : Common
    {
        public event AddKeyEventHandler OnAddKeyEvent;
        private int intID;
        private static SecureCommon common;
        private bool isRefresh = false;
        private Dictionary<string, KeyFileObject> CredentialDic; //key string must Lower letters
        private const string EOF_Mark = "[EOF]";
        private List<string> liAdd_ComboBox = new List<string>();
        public SecureCommon() { }
        public SecureCommon(int intID) { this.ID = intID; }

        public int ID { get { return intID; } set { intID = value; } }

        public static SecureCommon GetInstance(int intID)
        {
            if (common == null)
                common = new SecureCommon(intID);

            return common;
        }

        public void ReadFile_ExcludeEOF(string path)
        {
            if (CredentialDic == null)
            {
                CredentialDic = new Dictionary<string, KeyFileObject>();
                if (!File.Exists(path))
                    return;

                string strLine = "";
                FileInfo f = new FileInfo(path);
                StreamReader srFile = f.OpenText();
                while ((strLine = srFile.ReadLine()) != null)
                {
                    if (strLine.Trim().Length > 0 && strLine != EOF_Mark)
                    {
                        string[] strWord = strLine.Split('\t');
                        KeyFileObject key = ReadAESKey(strWord[0], strWord[1]);
                        if (key == null)
                        {
                            srFile.Close();
                            File.Delete(path);
                            return;
                        }
                        CredentialDic.Add(strWord[0].ToLower(), key);
                        liAdd_ComboBox.Add(strWord[0]);
                    }
                }
                srFile.Close();
            }
        }

        public void WriteFile(string path, string new_record)
        {
            if (!File.Exists(path))
            {
                FileStream f = File.Create(path);
                f.Close();
            }

            StreamWriter w = File.AppendText(path);
            w.WriteLine(new_record);
            w.Flush();
            w.Close();
            isRefresh = false;
        }

        public void RefreshFile(string path)
        {
            //if key file is not sorted, then rewrite the key file
            if (!isRefresh)
            {
                List<KeyFileObject> list = new List<KeyFileObject>();
                CredentialDic = new Dictionary<string, KeyFileObject>();
                if (!File.Exists(path))
                    return;

                string strLine = "";
                FileInfo f = new FileInfo(path);
                StreamReader srFile = f.OpenText();
                while ((strLine = srFile.ReadLine()) != null)
                {
                    if (strLine.Trim().Length > 0)
                    {
                        if (strLine != EOF_Mark)
                        {
                            string[] strWord = strLine.Split('\t');
                            KeyFileObject key = ReadAESKey(strWord[0], strWord[1]);
                            if (key == null)
                            {
                                srFile.Close();
                                File.Delete(path);
                                return;
                            }
                            list.Add(key);
                        }
                        else
                        {
                            list.Add(new KeyFileObject(EOF_Mark, "", "", "", ""));
                        }
                    }
                }
                srFile.Close();

                if (list.Count != 0)
                {
                    KeyFileObject EOF_Mark_Object = new KeyFileObject(EOF_Mark, "", "", "", "");
                    if (list[list.Count - 1] == EOF_Mark_Object)
                    {
                        list.Remove(EOF_Mark_Object);
                        list.Sort();
                        foreach (KeyFileObject record in list)
                        {
                            CredentialDic.Add(record.Path, record);
                        }
                    }
                    else
                    {
                        list.Remove(EOF_Mark_Object);
                        list.Sort();

                        File.Delete(path);
                        StreamWriter w = File.AppendText(path);
                        AESEncrypt AES = new AESEncrypt();
                        foreach (KeyFileObject record in list)
                        {
                            CredentialDic.Add(record.Path, record);
                            w.WriteLine(record.Path + "\t" + AES.Encrypt_AES(record));
                        }
                        w.WriteLine(EOF_Mark);
                        w.Flush();
                        w.Close();
                    }
                }
            }
            isRefresh = true;
        }

        public void ValidateNewRecord(KeyFileObject New_Record, bool Add_ComboBox)
        {
            List<string> Existed_Pathes = new List<string>();
            Dictionary<string, KeyFileObject> dic = GetFileInfo();
            List<string> MachineName_Pathes = new List<string>();
            List<string> UNC_Pathes = new List<string>();
            foreach (KeyValuePair<string, KeyFileObject> path in dic)
            {
                if (path.Key.StartsWith("\\") || path.Key.StartsWith("ftp://"))
                {
                    UNC_Pathes.Add(path.Key.ToLower());
                }
                else
                {
                    MachineName_Pathes.Add(path.Key.ToLower());
                }
            }

            //Check if existed path list contain the new path
            bool PathIsMachineName = false;
            if (!(New_Record.Path.Contains("\\") || New_Record.Path.ToLower().StartsWith("ftp://")))
                PathIsMachineName = true;

            if (PathIsMachineName)
            {
                if (MachineName_Pathes.Contains(New_Record.Path.ToLower()))
                    Existed_Pathes.Add(New_Record.Path.ToLower());
            }
            else
            {
                foreach (string unc_path in UNC_Pathes)
                {
                    if(unc_path.Contains(New_Record.Path.ToLower()))
                    {
                        Existed_Pathes.Add(unc_path);
                    }
                    else if (New_Record.Path.ToLower().Contains(unc_path))
                    {
                        Existed_Pathes.Add(unc_path);
                    }
                }
            }

            OnAddKeyEvent(this, new AddKeyEventArgs(this.ID, Existed_Pathes.ToArray(), New_Record, Add_ComboBox)); //For refresh ComboBox in DR_Tool Form
        }

        public void WriteDictionaryToFile(string path)
        {
            File.Delete(path);
            StreamWriter w = File.AppendText(path);
            w.AutoFlush = true;
            Dictionary<string, KeyFileObject> dic = this.GetFileInfo();
            AESEncrypt AES = new AESEncrypt();
            foreach (KeyValuePair<string, KeyFileObject> record in dic)
            {
                w.WriteLine(record.Key + "\t" + AES.Encrypt_AES(record.Value));
            }
            w.Close();
        }

        public Dictionary<string, KeyFileObject> GetFileInfo()
        {
            if (CredentialDic == null) { return new Dictionary<string, KeyFileObject>(); }
            else { return CredentialDic; }
        }

        public void AddKeyDictionary(KeyFileObject new_record)
        {
            Dictionary<string, KeyFileObject> dic = this.GetFileInfo();
            dic.Add(new_record.Path.ToLower(), new_record);
            isRefresh = false;
        }

        public void RemoveKeyDictionary(string[] Path_Key)
        {
            Dictionary<string, KeyFileObject> dic = this.GetFileInfo();
            foreach (string index in Path_Key)
                dic.Remove(index);
            isRefresh = false;
        }

        private KeyFileObject ReadAESKey(string Key, string Value)
        {
            AESEncrypt AES = new AESEncrypt();
            string[] secure_value = AES.Decrypt_AES(Value).Split('\t');
            if (secure_value.Length != 6 || secure_value[5] != "@" + System.Windows.Forms.SystemInformation.ComputerName)
            {
                return null;
            }
            else
            {
                return new KeyFileObject(secure_value[0], secure_value[1], secure_value[2], secure_value[3], secure_value[4]);
            }
        }

        public void SetAdd_ComboBox(string record)
        {
            liAdd_ComboBox.Add(record);
        }

        public List<string> GetAdd_ComboBox()
        {
            return liAdd_ComboBox;
        }
    }

    public class AddIniEventArgs : EventArgs
    {
        public int CommonID;
        public int[] Record_Index; //duplicate index
        public string New_Record;

        public AddIniEventArgs(int CommonID, int[] Record_Index, string New_Record)
        {
            this.CommonID = CommonID;
            this.Record_Index = Record_Index;
            this.New_Record = New_Record;
        }
    }

    public class AddKeyEventArgs : EventArgs
    {
        public int CommonID;
        public string[] Record_Pathes; //duplicate path
        public KeyFileObject New_Record;
        public bool Save_Pwd;

        public AddKeyEventArgs(int CommonID, string[] Record_Pathes, KeyFileObject New_Record, bool Save_Pwd)
        {
            this.CommonID = CommonID;
            this.Record_Pathes = Record_Pathes;
            this.New_Record = New_Record;
            this.Save_Pwd = Save_Pwd;
        }
    }

    #endregion


    class AESEncrypt
    {
        private static readonly String strAesKey = "GL-team0T10237ParkRoad0123456789";//32-bits Key

        //Customize by John
        public String Encrypt_AES(KeyFileObject key)
        {
            return Encrypt_AES(key.Path + "\t" + key.MethodType + "\t" + key.Domain + "\t" + key.Username + "\t" + key.Password + "\t" + "@" + System.Windows.Forms.SystemInformation.ComputerName);
        }

        private String Encrypt_AES(String str)
        {
            Byte[] keyArray = System.Text.UTF8Encoding.UTF8.GetBytes(strAesKey);
            Byte[] toEncryptArray = System.Text.UTF8Encoding.UTF8.GetBytes(str);

            System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = System.Security.Cryptography.CipherMode.ECB;
            rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        
        public String Decrypt_AES(String str)
        {
            Byte[] keyArray = System.Text.UTF8Encoding.UTF8.GetBytes(strAesKey);
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = System.Security.Cryptography.CipherMode.ECB;
            rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return System.Text.UTF8Encoding.UTF8.GetString(resultArray);
        }


    }

}
