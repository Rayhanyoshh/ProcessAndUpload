using R_APICommonDTO;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessConsole
{
    public class ProcessStatus : R_IProcessProgressStatus
    {
        public string CompanyId { get; set; }
        public string UserId { get; set; }

        public Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            if (poProcessResultMode == eProcessResultMode.Success)
            {
                Console.WriteLine($"Process Complete with no error With GUID {pcKeyGuid}");
            }
            else
            {
                Console.WriteLine($"Process Complete with GUID {pcKeyGuid}");
            }
            return Task.CompletedTask;
        }

        public Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            Console.WriteLine($"Process Fail with GUID {pcKeyGuid}");
            foreach (R_Error item in ex.ErrorList)
            {
                Console.WriteLine($"{item.ErrDescp}");
            }
            return Task.CompletedTask;
        }

        public Task ReportProgress(int pnProgress, string pcStatus)
        {
            Console.WriteLine($"Step {pnProgress} with status :{pcStatus}");
            return Task.CompletedTask;
        }
    }
}