﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Carroll.Data.Services.Models.Validation
{
    public class ModelStateWrapper:IValidationDictionary
    {
        private ModelStateDictionary _modelState;

        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            _modelState = modelState;
        }

        public void AddError(string key, string errorMessage)
        {
            _modelState.AddModelError(key, errorMessage);
           
        }

        public bool IsValid
        {
            get { return _modelState.IsValid; }
            
        }

        public ModelStateDictionary GetUnderlyingDictionary { get { return _modelState; } }

        
    }
}
