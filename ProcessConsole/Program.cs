using ProcessCommon;
using ProcessConsole;
using R_APIClient;
using R_APICommonDTO;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;

internal class Program
{
    static HttpClient loHttpClient = null;
    static R_HTTPClient? loClient;
    private static void Main(string[] args)
    {
        loHttpClient = new HttpClient();
        loHttpClient.BaseAddress = new Uri("http://localhost:5137");
        R_HTTPClient.R_CreateInstanceWithName("DEFAULT", loHttpClient);
        loClient = R_HTTPClient.R_GetInstanceWithName("DEFAULT");

        //Task.Run(() => ServiceAttachFile());
        Task.Run(() => ServiceProcess());
        Console.ReadKey();
    }

    static async Task ServiceAttachFile()
    {
        R_APIException loException = new R_APIException();
        List<R_KeyValue> loUserParameters;
        R_UploadPar loUploadPar;
        R_ProcessAndUploadClient loCls;
        R_IProcessProgressStatus loProcessProgressStatus;

        try
        {
            //persiapkan User Par

            loUserParameters = new List<R_KeyValue>();
            loUserParameters.Add(new R_KeyValue() { Key = ProcessConstant.EMPLOYEE_ID, Value = "1" });

            //persiapkan upload Parameter
            loUploadPar = new R_UploadPar();
            loUploadPar.UserParameters = loUserParameters;

            loUploadPar.USER_ID = "User01";
            loUploadPar.COMPANY_ID = "C001";
            loUploadPar.ClassName = "ProcessBack.AttachFileCls";

            loUploadPar.FilePath = $@"C:\Users\User\Pictures\spontan.png";
            loUploadPar.File = new R_File();
            loUploadPar.File.FileId = Path.GetFileNameWithoutExtension(loUploadPar.FilePath);
            loUploadPar.File.FileDescription = $"Description of {Path.GetFileNameWithoutExtension(loUploadPar.File.FileId)}";
            loUploadPar.File.FileExtension = Path.GetExtension(loUploadPar.FilePath);

            //mempersiapkan proses upload
            loProcessProgressStatus = new ProcessStatus();
            loCls = new R_ProcessAndUploadClient(poProcessProgressStatus: loProcessProgressStatus, plSendWithContext: false, plSendWithToken: false);
            //loCls = new R_ProcessAndUploadClient(plSendWithContext: false, plSendWithToken: false);

            await loCls.R_AttachFile<Object>(loUploadPar);
        }
        catch (Exception ex)
        {

            loException.add(ex);
        }

    EndBlock:
        loException.ThrowExceptionIfErrors();
    }

    static async Task ServiceProcess()
    {
        R_APIException loException = new R_APIException();
        List<R_KeyValue> loUserParameters;
        R_BatchParameter loBarchPar;
        R_ProcessAndUploadClient loCls;
        ProcessStatus loProgressStatus;
        string lcGuid;

        try
        {
            //persiapkan User Par

            loUserParameters = new List<R_KeyValue>();
            loUserParameters.Add(new R_KeyValue() { Key = ProcessConstant.LOOP, Value = 10 });
            loUserParameters.Add(new R_KeyValue() { Key = ProcessConstant.IS_ERROR, Value = false });
            loUserParameters.Add(new R_KeyValue() { Key = ProcessConstant.IS_ERROR_STATEMENT, Value = false });

            //persiapkan upload Parameter
            loBarchPar = new();
            loBarchPar.UserParameters = loUserParameters;

            loBarchPar.USER_ID = "User01";
            loBarchPar.COMPANY_ID = "C001";
            loBarchPar.ClassName = "ProcessBack.BatchProcessCls";


            //progress status
            loProgressStatus = new ProcessStatus();
            ((ProcessStatus)loProgressStatus).CompanyId = loBarchPar.COMPANY_ID;
            ((ProcessStatus)loProgressStatus).UserId = loBarchPar.USER_ID;

            //mempersiapkan proses upload
            loCls = new R_ProcessAndUploadClient(poProcessProgressStatus: loProgressStatus, plSendWithContext: false, plSendWithToken: false);

            lcGuid = await loCls.R_BatchProcess<Object>(loBarchPar, 10);
            Console.WriteLine($"Process With Return GUID {lcGuid}");
        }
        catch (Exception ex)
        {

            loException.add(ex);
        }

    EndBlock:
        loException.ThrowExceptionIfErrors();
    }
}
