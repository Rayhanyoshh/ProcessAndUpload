using ProcessCommon;
using R_BackEnd;
using R_Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessBack
{
    public class BatchProcessCls : R_IBatchProcess
    {
        public void R_BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            R_Exception loEx = new R_Exception();
            int lnLoop;
            int lnLoop2;
            int lnLoop3;

            bool llIsError;
            bool llIsErrorStatement;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                //user param validation

                //validate LOOP
                var loVar = poBatchProcessPar.UserParameters.Where(x => x.Key.Equals(ProcessConstant.LOOP)).FirstOrDefault().Value;
                if (loVar == null)
                {
                    loEx.Add("001", "LOOP Param not found");
                }
                lnLoop = ((System.Text.Json.JsonElement)loVar).GetInt16();

                //validate isError
                var loVar1 = poBatchProcessPar.UserParameters.Where(x => x.Key.Equals(ProcessConstant.IS_ERROR)).FirstOrDefault().Value;
                if (loVar1 == null)
                {
                    loEx.Add("001", "IS_ERROR Param not found");
                }
                llIsError = ((System.Text.Json.JsonElement)loVar1).GetBoolean();

                //validate isErrorStatement
                var loVar2 = poBatchProcessPar.UserParameters.Where(x => x.Key.Equals(ProcessConstant.IS_ERROR_STATEMENT)).FirstOrDefault().Value;
                if (loVar2 == null)
                {
                    loEx.Add("001", "IS_ERROR_STATEMENT Param not found");
                }
                llIsErrorStatement = ((System.Text.Json.JsonElement)loVar2).GetBoolean();

                //handle isErrorStatement, if true then gotoendblock
                if (llIsErrorStatement == true)
                {
                    loEx.Add("002", "There is error statement");
                    goto EndBlock;
                }

                //prepare db
                loDb = new R_Db();
                loCommand = loDb.GetCommand();
                loCommand.CommandText = "SampleProcessBatch";//spName
                loCommand.CommandType = System.Data.CommandType.StoredProcedure; //command datatype: storedprocedure

                //prepare paramteter to use in storedprocedure
                loDb.R_AddCommandParameter(loCommand, "@CoId", System.Data.DbType.String, 50, poBatchProcessPar.Key.COMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@UserId", System.Data.DbType.String, 50, poBatchProcessPar.Key.USER_ID);
                loDb.R_AddCommandParameter(loCommand, "@KeyGUID", System.Data.DbType.String, 50, poBatchProcessPar.Key.KEY_GUID);
                loDb.R_AddCommandParameter(loCommand, "@Loop", System.Data.DbType.String, 50, lnLoop);
                loDb.R_AddCommandParameter(loCommand, "@IsError", System.Data.DbType.String, 50, llIsError);

                //execute command with open connection, and autoclose connection
                loDb.SqlExecNonQuery(loDb.GetConnection(), loCommand, true);
            }
            catch (Exception e)
            {
                loEx.Add(e);
            }
        EndBlock:
            loEx.ThrowExceptionIfErrors();

        }
    }
}