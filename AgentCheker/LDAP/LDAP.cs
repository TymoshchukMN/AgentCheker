using System.DirectoryServices;
using AgentCheker.Interfaces;

namespace AgentChecker
{
    public class LDAP : IisClientOS
    {
        #region FIELDS

        /// <summary>
        /// Current domain name.
        /// </summary>
        private string _domainName;

        #endregion FIELDS

        #region CTORs

        public LDAP()
        {
            _domainName = GetCurrentDomainPath();
        }

        #endregion CTORs

        #region METHODS

        public bool IsClientOS(string pcName)
        {
            SearchResultCollection results;
            DirectorySearcher directorySearcher;
            bool isClientOS = false;

            using (DirectoryEntry directoryEntry = new DirectoryEntry(_domainName))
            {
                directorySearcher = new DirectorySearcher(directoryEntry);

                string ldapPCInform = $"(&(objectCategory=computer)" +
                    $"(name={pcName.Replace(".AKU.COM", string.Empty).Replace(".COMFY.UA", string.Empty)}))";

                directorySearcher.Filter = ldapPCInform;
                const int PAGE_SIZE = 1000;
                directorySearcher.PageSize = PAGE_SIZE;
                directorySearcher.PropertiesToLoad.Add("OperatingSystem");
                results = directorySearcher.FindAll();
                const string ExcludeOS = "Server";

                if (results.Count > 0)
                {
                    string os = results[0].Properties["OperatingSystem"][0].ToString();
                    if (!os.Contains(ExcludeOS))
                    {
                        isClientOS = true;
                    }
                }

                directorySearcher.Dispose();
                return isClientOS;
            }
        }

        /// <summary>
        /// Get root domain.
        /// </summary>
        /// <returns>
        /// Domain name.
        /// </returns>
        private string GetCurrentDomainPath()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");

            return "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();
        }

        #endregion METHODS
    }
}
