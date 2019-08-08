using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RouteOfAdmission
    {
        AccidentAndEmergency,
        DirectToWard,
        GPReferral,
        TransferFromAnotherHospital,
        ParamedicToSpecialistCenter,
        FromOutpatientClinic,
        Unknown
    }
}
