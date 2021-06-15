﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AtomicSeller.Helpers.ValidationAttributes
{
    public class MinIntegerValueAttribute : ValidationAttribute
    {
        private readonly bool inclusive;
        private readonly int minValue;

        public MinIntegerValueAttribute(int minValue, bool inclusive = false)
        {
            this.minValue = minValue;
            this.inclusive = inclusive;

            ErrorMessage = "Entrez une valeur " + (inclusive ? "supérieure ou égale" : "strictement supérieure") + " à " +
                           minValue;
        }

        public override bool IsValid(object value)
        {
            var integer = (int) value;

            return inclusive ? integer >= minValue : integer > minValue;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = ErrorMessage;
            rule.ValidationParameters.Add("min", minValue);
            rule.ValidationParameters.Add("max", Int32.MaxValue);
            rule.ValidationType = "range";
            yield return rule;
        }
    }
}
