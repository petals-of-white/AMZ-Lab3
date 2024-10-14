using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FellowOakDicom;

namespace Lab1.ExtensionMethods;

public static class DicomDatasetExtensions
{
    public static T? TryMaybeValue<T>(this DicomDataset dicomDs, DicomTag tag)
    {
        if (dicomDs.TryGetSingleValue(tag, out T result))
            return result;
        else
            return default;
    }
}
