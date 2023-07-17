﻿using System;
using System.Collections.Generic;
using System.Data;
using AgentCheker.Interfaces;

namespace AgentCheker.DataBase
{
    public class DateBaseEset : DateBase
    {

        #region CTORs

        public DateBaseEset(
            string server,
            string userName,
            string dataBase,
            int port,
            string pass)
            : base(server, userName, dataBase, port, pass)
        {
            ServerName = server;
            ConnectionString = string.Format($"Server = {server},{port}; Database = {dataBase}; User Id = aku0\\{userName}; Password = {pass}");
            Query = @"
                        select	t1.computer_name,
                            t2.[computer_connected] + '03:00:00' as 'lastConnected'
                        from tbl_computers as t1
                        inner join [era_db].[dbo].[tbl_computers_aggr] as t2
                        on t1.computer_id = t2.computer_id
                        where t2.[computer_connected] is not NULL 
                        and t2.[computer_connected] <= DATEADD(DAY,-14,GETDATE())
                        order by t2.[computer_connected] desc
                        ";
        }

        #endregion CTORs

        #region PROPERTIES

        public override string Query
        {
            get { return Query; }
        }

        #endregion PROPERTIES

    }
}
