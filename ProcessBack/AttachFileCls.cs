using ProcessCommon;
using R_BackEnd;
using R_Common;

namespace ProcessBack
{
    public class AttachFileCls : R_IAttachFile
    {
        public void R_AttachFile(R_AttachFilePar poAttachFile)
        {
            R_Exception loException = new R_Exception();
            string lcEmployeeId;
            string lcCmd;
            R_Db loDb;

            try
            {
                //mendapatkan user pameter, dan kita butuh untuk memvalidasi nya, sebelum di input
                //harus ada user Parameter
                if (poAttachFile.UserParameters.Count > 0)
                {
                    //pengecekan apakah user parameter yang sesuai dengan employe id yang ada
                    var loVar = poAttachFile.UserParameters.Where(x => x.Key.Equals(ProcessConstant.EMPLOYEE_ID)).FirstOrDefault().Value;
                    if (loVar == null)
                    {
                        loException.Add("001", "Employee ID parameter not found");
                        goto EndBlock;
                    }

                    //pengecekan parameter user tersebut yang sudah di convert, dan dicari hanya 1, memang tersedia atau tidak
                    lcEmployeeId = ((System.Text.Json.JsonElement)loVar).GetString();
                    if (lcEmployeeId == null)
                    {
                        loException.Add("001", "Employee ID parameter not found");
                        goto EndBlock;
                    }
                }

                lcCmd = $"Insert into TestEmployeeAttachment(CoId,EmpId,FileName,oData,FileExtension) " +
                    $"values('{poAttachFile.Key.COMPANY_ID}','{poAttachFile.Key.USER_ID}','{poAttachFile.File.FileDescription}', " +
                    $"dbo.RFN_CombineByte('{poAttachFile.Key.COMPANY_ID}','{poAttachFile.Key.USER_ID}','{poAttachFile.Key.KEY_GUID}')" +
                    $",'{poAttachFile.File.FileExtension}' )";



                loDb = new R_Db();
                loDb.SqlExecNonQuery(lcCmd);

                /*
                lcCmd = "Insert into TestEmployeeAttachment(CoId, EmpId, FileName, FileExtension, oData) Values(@CoId, @EmpId, @FileName, @FileExtension, dbo.RFN_CombineByte(@CoId,@UserId,@KeyGUID))";
                loDb = new R_Db();
                loCommand = loDb.GetCommand();
                loCommand.CommandText = lcCmd;
                loCommand.CommandType =CommandType.Text;

                loDb.R_AddCommandParameter(loCommand, "CoId", DbType.String, 50, poAttachFile.Key.COMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "EmpId", DbType.String, 50, lcEmplId);
                loDb.R_AddCommandParameter(loCommand, "FileName", DbType.String, 50, poAttachFile.File.FileId);
                loDb.R_AddCommandParameter(loCommand, "FileExtension", DbType.String, 50, poAttachFile.File.FileExtension);

                loDb.R_AddCommandParameter(loCommand, "UserId", DbType.String, 50, poAttachFile.Key.USER_ID);
                loDb.R_AddCommandParameter(loCommand, "KeyGUID", DbType.String, 50, poAttachFile.Key.KEY_GUID);

                loDb.SqlExecNonQuery(loDb.GetConnection(), loCommand, true);
                */

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

        EndBlock:
            loException.ThrowExceptionIfErrors();
        }

    }
}