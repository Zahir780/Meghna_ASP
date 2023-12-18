////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ISP_Call.cs
//FileType: Visual C# Source file
//Author : Zahid Sarmon
//Created On : 02-11-2020
//Copy Rights : Evision Soft. Ltd.
//Description : Class for Call Procedure Dynamically
////////////////////////////////////////////////////////////////////////////////////////////////////////
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.DataAccess.Repository.IRepository
{
    /*Create Implement of Store procedure 
 * Author: Zahid Sarmon
 * Create date:26-07-2020
 */
    public interface ISP_Call : IDisposable
    {
        T Single<T>(string procedureName, DynamicParameters param = null);

        void Execute(string procedureName, DynamicParameters param = null);

        T OneRecord<T>(string procedureName, DynamicParameters param = null);

        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null);

        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null);
    }
}
