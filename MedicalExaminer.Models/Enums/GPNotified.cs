﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GPNotified
    {
        GPUnabledToBeNotified,
        GPNotified,
        NA
    }
}
