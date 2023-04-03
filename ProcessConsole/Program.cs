using ProcessCommon;
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
        loHttpClient.BaseAddress = new Uri("http://localhost:5047");
        R_HTTPClient.R_CreateInstanceWithName("DEFAULT", loHttpClient);
        loClient = R_HTTPClient.R_GetInstanceWithName("DEFAULT");

        Task.Run(() => ServiceAttachFile());

        Console.ReadKey();
    }

    static async Task ServiceAttachFile()
    {
        R_APIException loException = new R_APIException();
        List<R_KeyValue> loUserParameters;
        R_UploadPar loUploadPar;
        R_ProcessAndUploadClient loCls;

        try
        {
            //persiapkan User Pa

            loUserParameters = new List<R_KeyValue>();
            loUserParameters.Add(new R_KeyValue() { Key = ProcessConstant.EMPLOYEE_ID, Value = "EMPLOYEE_ID" });

            //persiapkan upload Parameter
            loUploadPar = new R_UploadPar();
            loUploadPar.UserParameters = loUserParameters;

            loUploadPar.USER_ID = "User01";
            loUploadPar.COMPANY_ID = "C001";
            loUploadPar.ClassName = "ProcessBack.AttachFileCls";

            loUploadPar.FilePath = $@"C:\Users\alvan\Pictures\Test1.pdf";
            loUploadPar.File = new R_File();
            loUploadPar.File.FileId = Path.GetFileNameWithoutExtension(loUploadPar.FilePath);
            loUploadPar.File.FileDescription = $"Description of {Path.GetFileNameWithoutExtension(loUploadPar.File.FileId)}";
            loUploadPar.File.FileExtension = Path.GetExtension(loUploadPar.FilePath);

            //mempersiapkan proses upload
            loCls = new R_ProcessAndUploadClient(plSendWithContext: false, plSendWithToken: false);

            await loCls.R_AttachFile<Object>(loUploadPar);
        }
        catch (Exception ex)
        {

            loException.add(ex);
        }

    EndBlock:
        loException.ThrowExceptionIfErrors();
    }

}