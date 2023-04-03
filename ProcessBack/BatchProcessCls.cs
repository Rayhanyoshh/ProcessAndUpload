﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessCommon;
using R_BackEnd;
using R_Common;

namespace ProcessBack
{
    public class BatchProcessCls : R_IBatchProcess
    {
        public void R_BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            R_Exception loException = new();
            int lnLoop;
            bool llIsError;
            bool llIsErrorStatement = false;
            R_Db loDb;
            DbCommand loCommand;

            try
            {
                var loVar = poBatchProcessPar.UserParameters.Where(x => x.Key.Equals(ProcessConstant.LOOP)).FirstOrDefault().Value;
                if (loVar == null)
                {
                    loException.Add("001", "Loop parameter not found");
                    goto EndBlock;
                }
                lnLoop = ((System.Text.Json.JsonElement)loVar).GetInt16();


                loVar = poBatchProcessPar.UserParameters.Where(x => x.Key.Equals(ProcessConstant.IS_ERROR)).FirstOrDefault().Value;
                if (loVar == null)
                {
                    loException.Add("001", "IS_ERROR parameter not found");
                    goto EndBlock;
                }
                llIsError = ((System.Text.Json.JsonElement)loVar).GetBoolean();

                loVar = poBatchProcessPar.UserParameters.Where(x => x.Key.Equals(ProcessConstant.IS_ERROR_STATEMENT)).FirstOrDefault().Value;
                if (loVar == null)
                {
                    loException.Add("001", "IS_ERROR_STATEMENT parameter not found");
                    goto EndBlock;
                }
                llIsError = ((System.Text.Json.JsonElement)loVar).GetBoolean();

                if (llIsErrorStatement == true)
                {
                    loException.Add("002", "Ada Error Statement");
                    goto EndBlock;
                }

                loDb = new();
                loCommand = loDb.GetCommand();
                loCommand.CommandText = "SamplePRocessBatch";
                loCommand.CommandType = System.Data.CommandType.StoredProcedure;


                loDb.R_AddCommandParameter(loCommand, "@CoId", System.Data.DbType.String, 50, poBatchProcessPar.Key.COMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@UserId", System.Data.DbType.String, 50, poBatchProcessPar.Key.USER_ID);
                loDb.R_AddCommandParameter(loCommand, "@KeyGUID", System.Data.DbType.String, 50, poBatchProcessPar.Key.KEY_GUID);
                loDb.R_AddCommandParameter(loCommand, "@Loop", System.Data.DbType.Int16, 0, lnLoop);
                loDb.R_AddCommandParameter(loCommand, "@IsError", System.Data.DbType.Boolean, 0, llIsError);


                loDb.SqlExecNonQuery(loDb.GetConnection(), loCommand, true);
            }
            catch (Exception e)
            {
                loException.Add(e);
            }

        EndBlock:
            loException.ThrowExceptionIfErrors(); ;
        }
    }
}