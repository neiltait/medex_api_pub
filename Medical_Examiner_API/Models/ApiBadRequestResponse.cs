using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Medical_Examiner_API.Models
{
    public class ApiBadRequestResponse : ApiResponse
    {
        public ApiBadRequestResponse(ModelStateDictionary modelState)
            : base(400)
        {
            if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));
        }

        public IEnumerable<string> Errors { get; }
    }
}