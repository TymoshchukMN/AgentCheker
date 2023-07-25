namespace FiredProcessing
{
    using System.DirectoryServices;

    public class LDAP
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

        public void GetPCinform(string pcName)
        {
            SearchResultCollection results;
            DirectorySearcher directorySearcher;

            using (DirectoryEntry directoryEntry = new DirectoryEntry(_domainName))
            {
                directorySearcher = new DirectorySearcher(directoryEntry);

                string ldapPCInform = $"(&(objectCategory=computer)(name={pcName}))";

                directorySearcher.Filter = ldapPCInform;
                const int PAGE_SIZE = 1000;
                directorySearcher.PageSize = PAGE_SIZE;
                directorySearcher.PropertiesToLoad.Add("sAMAccountName");
                results = directorySearcher.FindAll();

                foreach (SearchResult sr in results)
                {
                        string.Format($"{sr.Properties["OperatingSystem"][0]}");
                }

                directorySearcher.Dispose();
            }
        }

        /// <summary>
        /// Ger root domain.
        /// </summary>
        /// <returns>
        /// Domain name.
        /// </returns>
        private string GetCurrentDomainPath()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");

            return "LDAP://OU=Trash," + de.Properties["defaultNamingContext"][0].ToString();
        }

        #endregion METHODS
    }
}
