using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace LearnRevitAPI.Lib
{
    public class DeleteWarningSuper : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            IList<FailureMessageAccessor> failList = failuresAccessor.GetFailureMessages();
            //if (failList.Count > 0)
            if (failList.Any())
            {
                foreach (FailureMessageAccessor failure in failList)
                {
                    FailureSeverity s = failure.GetSeverity();
                    if (s == FailureSeverity.Warning)
                    {
                        failuresAccessor.DeleteWarning(failure);
                    }
                    else if (s == FailureSeverity.Error)
                    {
                        failuresAccessor.ResolveFailure(failure);
                    }
                }

                return FailureProcessingResult.ProceedWithCommit;
            }
            else
            {
                return  FailureProcessingResult.Continue;
            }
        }
    }
}
