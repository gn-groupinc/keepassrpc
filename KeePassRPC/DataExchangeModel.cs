﻿using System.Collections.Generic;
using Jayrock.Json.Conversion;

namespace KeePassRPC.DataExchangeModel
{
    public class Utilities
    {
        public static string FormFieldTypeToDisplay(FormFieldType fft, bool titleCase)
        {
            string type = "Text";
            if (fft == FormFieldType.FFTpassword)
                type = "Password";
            else if (fft == FormFieldType.FFTselect)
                type = "Select";
            else if (fft == FormFieldType.FFTradio)
                type = "Radio";
            else if (fft == FormFieldType.FFTtext)
                type = "Text";
            else if (fft == FormFieldType.FFTusername)
                type = "Username";
            else if (fft == FormFieldType.FFTcheckbox)
                type = "Checkbox";
            if (!titleCase)
                return type.ToLower();
            return type;
        }

        public static FormFieldType FormFieldTypeFromDisplay(string type)
        {
            type = type.ToLower();
            FormFieldType fft = FormFieldType.FFTusername;
            if (type == "password")
                fft = FormFieldType.FFTpassword;
            else if (type == "select")
                fft = FormFieldType.FFTselect;
            else if (type == "radio")
                fft = FormFieldType.FFTradio;
            else if (type == "text")
                fft = FormFieldType.FFTtext;
            else if (type == "username")
                fft = FormFieldType.FFTusername;
            else if (type == "checkbox")
                fft = FormFieldType.FFTcheckbox;
            return fft;
        }
    }

    public class AuthenticationResult
    {
        // private int _result;
        public int Result;// { get { return _result; } }
        //private string _name;
        public string Name;// { get { return _name; } }

        public AuthenticationResult() { }
        public AuthenticationResult(int res, string name)
        {
            Name = name;
            Result = res;
            //_name = name;
            //_result = res;
        }
    }

    public class Configuration
    {
        //bool allowUnencryptedMetaData; // doesn't affect encryption of passwords themselves
        //KPDatabaseList knownDatabases; // the MRU list (to expand this in v1+, maybe Firefox preferences can be used?)
        public string[] KnownDatabases;
        public bool AutoCommit; // whether KeePass should save the active database after every change

        public Configuration() { }
        public Configuration(string[] MRUList, bool autoCommit)
        {
            KnownDatabases = MRUList;
            AutoCommit = autoCommit;
        }
    }

    // This is no longer used but remains in the DTO spec for backwards compatibility
    public enum LoginSearchType { LSTall, LSTnoForms, LSTnoRealms }
    
    public enum FormFieldType { FFTradio, FFTusername, FFTtext, FFTpassword, FFTselect, FFTcheckbox } // ..., HTML 5, etc.
    // FFTusername is special type because bultin FF supports with only username and password

    public enum PlaceholderHandling { Default, Enabled, Disabled }

    public enum MatchAccuracyEnum
    {
        // Best = Non-URL match (i.e. we matched by UUID instead)
        // Best = Regex match (it is impossible for us to infer how
        // accurate a regex match is in comparison with other classes
        // of match so we always treat it as the best possible match
        // even if the regex itself is very loose)
        // Best = Same URL including query string

        // Close = Same URL excluding query string

        // HostnameAndPort = Same hostname and port

        // HostnameExcludingPort = Same hostname (domain + subdomains)

        // Domain = Same domain

        // None = No match (e.g. when we are being asked to return all entries)

        Best = 50,
        Close = 40,
        HostnameAndPort = 30,
        HostnameExcludingPort = 20,
        Domain = 10,
        None = 0
    } 

    public struct MatchAccuracy
    {
        // Best = Non-URL match (i.e. we matched by UUID instead)
        // Best = Regex match (it is impossible for us to infer how
        // accurate a regex match is in comparison with other classes
        // of match so we always treat it as the best possible match
        // even if the regex itself is very loose)
        // Best = Same URL including query string

        // Close = Same URL excluding query string

        // HostnameAndPort = Same hostname and port

        // HostnameExcludingPort = Same hostname (domain + subdomains)

        // Domain = Same domain

        // None = No match (e.g. when we are being asked to return all entries)

        public static readonly int Best = (int)MatchAccuracyEnum.Best;
        public static readonly int Close = (int)MatchAccuracyEnum.Close;
        public static readonly int HostnameAndPort = (int)MatchAccuracyEnum.HostnameAndPort;
        public static readonly int HostnameExcludingPort = (int)MatchAccuracyEnum.HostnameExcludingPort;
        public static readonly int Domain = (int)MatchAccuracyEnum.Domain;
        public static readonly int None = (int)MatchAccuracyEnum.None;
    } 

    public class FormField
    {
        public string Name;
        public string DisplayName;
        public string Value;
        public FormFieldType @Type;
        public string Id;
        public int Page;
        public PlaceholderHandling PlaceholderHandling;

        public FormField() { }

        public FormField(string name,
        string displayName,
        string value,
        FormFieldType @type,
        string id,
        int page,
        PlaceholderHandling placeholderHandling)
        {
            Name = name;
            DisplayName = displayName;
            Value = value;
            @Type = @type;
            Id = id;
            Page = page;
            PlaceholderHandling = placeholderHandling;
        }
    }

    public class Group
    {
        public string Title;
        public string UniqueID;
        public string IconImageData;
        public string Path;
        public string Notes;

        public Group[] ChildGroups;
        public Entry[] ChildEntries;
        public LightEntry[] ChildLightEntries;

        public Group() { }

        public Group(string title,
        string uniqueID,
        string iconImageData,
        string path,
        string notes)
        {
            Title = title;
            UniqueID = uniqueID;
            IconImageData = iconImageData;
            Path = path;
            Notes = notes;
        }
    }

    public class Entry : LightEntry
    {
        public string HTTPRealm;
        public FormField[] FormFieldList;

        // How accurately do the URLs in this entry match the URL we are looking for?
        // Higher = better match.
        // We don't consider protocol
        public int MatchAccuracy;

        public bool AlwaysAutoFill;
        public bool NeverAutoFill;
        public bool AlwaysAutoSubmit;
        public bool NeverAutoSubmit;
        public int Priority; // "Kee priority" = 1 (1 = 30000 relevancy score, 2 = 29999 relevancy score)

        public Group Parent;
        public Database Db;

        public Entry() { }

        public Entry(
            string[] urls,
            string hTTPRealm,
            string title,
            FormField[] formFieldList,
            string uniqueID,
            bool alwaysAutoFill,
            bool neverAutoFill,
            bool alwaysAutoSubmit,
            bool neverAutoSubmit,
            int priority,
            Group parent,
            string iconImageData,
            Database db,
            int matchAccuracy)
        {
            URLs = urls;
            HTTPRealm = hTTPRealm;
            Title = title;
            FormFieldList = formFieldList;
            UniqueID = uniqueID;
            AlwaysAutoFill = alwaysAutoFill;
            NeverAutoFill = neverAutoFill;
            AlwaysAutoSubmit = alwaysAutoSubmit;
            NeverAutoSubmit = neverAutoSubmit;
            Priority = priority;
            Parent = parent;
            IconImageData = iconImageData;
            Db = db;
            MatchAccuracy = matchAccuracy;
        }
    }

    public class LightEntry
    {
        public string[] URLs;
        public string Title;
        public string UniqueID;
        public string UsernameValue;
        public string UsernameName;
        public string IconImageData;

        public LightEntry() { }

        public LightEntry(
            string[] urls,
            string title,
            string uniqueID,
            string iconImageData,
            string usernameName,
            string usernameValue)
        {
            URLs = urls;
            Title = title;
            UniqueID = uniqueID;
            IconImageData = iconImageData;
            UsernameName = usernameName;
            UsernameValue = usernameValue;
        }
    }

    // We have no immutable properties and no known need for GetHashCode so default behaviour is as good as any
#pragma warning disable CS0659
    public class EntryConfig
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public int Version = 1;
        public string FormActionURL;
        public string HTTPRealm;
        public FormField[] FormFieldList;
        public bool AlwaysAutoFill;
        public bool NeverAutoFill;
        public bool AlwaysAutoSubmit;
        public bool NeverAutoSubmit;
        public int Priority;
        public string[] AltURLs;
        public bool Hide;
        public string[] BlockedURLs;
        public string[] RegExBlockedURLs;
        public string[] RegExURLs;

        /// <summary>
        /// Exact match required
        /// </summary>
        /// <remarks>This has to be public because Jayrock</remarks>
        public bool BlockHostnameOnlyMatch;

        /// <summary>
        /// Hostname/port match required
        /// </summary>
        /// <remarks>This has to be public because Jayrock</remarks>
        public bool BlockDomainOnlyMatch;

        /// <remarks>This has to be a method because Jayrock</remarks>
        public MatchAccuracyMethod GetMatchAccuracyMethod()
        {
            if (BlockHostnameOnlyMatch) return MatchAccuracyMethod.Exact;
            else if (BlockDomainOnlyMatch) return MatchAccuracyMethod.Hostname;
            else return MatchAccuracyMethod.Domain;
        }

        /// <remarks>This has to be a method because Jayrock</remarks>
        public void SetMatchAccuracyMethod(MatchAccuracyMethod value)
        {
            if (value == MatchAccuracyMethod.Domain)
            {
                BlockDomainOnlyMatch = false;
                BlockHostnameOnlyMatch = false;
            }
            else if (value == MatchAccuracyMethod.Hostname)
            {
                BlockDomainOnlyMatch = true;
                BlockHostnameOnlyMatch = false;
            } else
            {
                BlockDomainOnlyMatch = false;
                BlockHostnameOnlyMatch = true;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="EntryConfig"/> class.
        /// Match configuration depends on defaults in DB settings
        /// </summary>
        public EntryConfig(MatchAccuracyMethod accuracyMethod)
        {
            switch (accuracyMethod)
            {
                case MatchAccuracyMethod.Exact: BlockDomainOnlyMatch = false; BlockHostnameOnlyMatch = true; break;
                case MatchAccuracyMethod.Hostname: BlockDomainOnlyMatch = true; BlockHostnameOnlyMatch = false; break;
                case MatchAccuracyMethod.Domain: BlockDomainOnlyMatch = false; BlockHostnameOnlyMatch = false; break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryConfig"/> class.
        /// Match configuration defaults to MatchAccuracyMethod.Domain. In practice 
        /// this is only called by Jayrock deserialisation methods so the match accuracy
        /// method will be set to whatever value is stored in the JSON being used to
        /// represent this EntryConfig when at rest inside a custom string.
        /// </summary>
        public EntryConfig()
        {
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
                return false;

            EntryConfig p = obj as EntryConfig;
            if ((System.Object)p == null)
                return false;

            return Version == p.Version
                && FormActionURL == p.FormActionURL
                && HTTPRealm == p.HTTPRealm
                && AlwaysAutoFill == p.AlwaysAutoFill
                && NeverAutoFill == p.NeverAutoFill
                && AlwaysAutoSubmit == p.AlwaysAutoSubmit
                && NeverAutoSubmit == p.NeverAutoSubmit
                && Priority == p.Priority
                && Hide == p.Hide
                && BlockHostnameOnlyMatch == p.BlockHostnameOnlyMatch
                && BlockDomainOnlyMatch == p.BlockDomainOnlyMatch
                && AreEqual(FormFieldList, p.FormFieldList)
                && AreEqual(AltURLs, p.AltURLs)
                && AreEqual(BlockedURLs, p.BlockedURLs)
                && AreEqual(RegExBlockedURLs, p.RegExBlockedURLs)
                && AreEqual(RegExURLs, p.RegExURLs);
        }

        bool AreEqual<T>(T[] a, T[] b)
        {
            return AreEqual(a, b, EqualityComparer<T>.Default);
        }

        bool AreEqual<T>(T[] a, T[] b, IEqualityComparer<T> comparer)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a == null || b == null)
            {
                return false;
            }

            if (a.Length != b.Length)
            {
                return false;
            }

            for (int i = 0; i < a.Length; i++)
            {
                if (!comparer.Equals(a[i], b[i]))
                {
                    return false;
                }
            }
            return true;
        }

    }

    public class Database
    {
        public string Name;
        public string FileName;
        public Group Root;
        public bool Active;
        public string IconImageData;

        public Database() { }

        public Database(string name,
        string fileName,
        Group root,
        bool active,
        string iconImageData)
        {
            Name = name;
            Root = root;
            FileName = fileName;
            Active = active;
            IconImageData = iconImageData;
        }
    }

    public class IconCache<T>
    {
        private static object iconCacheLock = new object();
        public static Dictionary<T, string> _icons = new Dictionary<T, string>();
        // public static Dictionary<PwUuid, string> Icons { get { } set { } }
        public static void AddIcon(T iconId, string base64representation)
        {
            lock (iconCacheLock)
            {
                if (!_icons.ContainsKey(iconId))
                    _icons.Add(iconId, base64representation);
            }
        }

        public static string GetIconEncoding(T iconId)
        {
            string base64representation = null;
            lock (iconCacheLock)
            {
                if (!_icons.TryGetValue(iconId, out base64representation))
                    return null;
                return base64representation;
            }
        }



    }

    public enum Signal
    {
        /// <summary>
        /// 
        /// </summary>
        PLEASE_AUTHENTICATE = 0,
        /// <summary>
        /// deprecated?
        /// </summary>
        JSCALLBACKS_SETUP = 1,
        /// <summary>
        /// deprecated?
        /// </summary>
        ICECALLBACKS_SETUP = 2,

        DATABASE_OPENING = 3,
        DATABASE_OPEN = 4,
        DATABASE_CLOSING = 5,
        DATABASE_CLOSED = 6,
        DATABASE_SAVING = 7,
        DATABASE_SAVED = 8,
        DATABASE_DELETING = 9,
        DATABASE_DELETED = 10,
        DATABASE_SELECTED = 11,
        EXITING = 12
    }

    public enum ErrorCode
    {
        SUCCESS = 0, // Convention suggests we should not use 0 as an error condition
        UNKNOWN = 1, // A catchall - hopefully won't ever need to use this
        INVALID_MESSAGE = 2,
        UNRECOGNISED_PROTOCOL = 3,
        VERSION_CLIENT_TOO_LOW = 4,
        VERSION_CLIENT_TOO_HIGH = 5,
        AUTH_CLIENT_SECURITY_LEVEL_TOO_LOW = 6,
        AUTH_SERVER_SECURITY_LEVEL_TOO_LOW = 7,
        AUTH_FAILED = 8,
        AUTH_RESTART = 9,
        AUTH_EXPIRED = 10,
        AUTH_INVALID_PARAM = 11,
        AUTH_MISSING_PARAM = 12
    }

    public class KPRPCMessage
    {
        public string protocol;
        public JSONRPCContainer jsonrpc;
        public SRPParams srp;
        public KeyParams key;
        public int version;
        public string clientDisplayName;
        public string clientDisplayDescription;
        public string clientTypeId;
        public Error error;
        public string[] features;
    }

    public class JSONRPCContainer
    {
        public string message;
        public string iv;
        public string hmac;
    }

    public class KeyParams
    {
        public string username;
        public int securityLevel;
        public string cc;
        public string cr;
        public string sc;
        public string sr;
    }

    public class Error
    {
        public ErrorCode code;
        public string[] messageParams;

        public Error() { }
        public Error(ErrorCode code, string[] messageParams) { this.code = code; this.messageParams = messageParams; }
        public Error(ErrorCode code) { this.code = code; }
    }

    public class SRPParams
    {
        [JsonMemberName("I")]
        public string I;
        [JsonMemberName("A")]
        public string A;
        [JsonMemberName("S")]
        public string S;
        [JsonMemberName("B")]
        public string B;
        [JsonMemberName("M")]
        public string M;
        [JsonMemberName("M2")]
        public string M2;
        [JsonMemberName("s")]
        public string s;
        public string stage;
        public int securityLevel;
    }

    public class ApplicationMetadata
    {
        public string KeePassVersion;
        public bool IsMono;
        public string NETCLR;
        public string NETversion;
        public string MonoVersion;

        public ApplicationMetadata() { }
        public ApplicationMetadata(string keePassVersion, bool isMono, string nETCLR, string nETversion, string monoVersion)
        {
            KeePassVersion = keePassVersion;
            IsMono = isMono;
            NETCLR = nETCLR;
            NETversion = nETversion;
            MonoVersion = monoVersion;
        }
    }

}