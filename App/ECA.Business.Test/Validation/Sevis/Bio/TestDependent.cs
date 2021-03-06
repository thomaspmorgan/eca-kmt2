﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Sevis.Model;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    public class TestDependent : Dependent
    {
        public TestDependent()
            : base(
                  fullName: null,
                  birthCity: null,
                  birthCountryCode: null,
                  birthCountryReasonCode: null,
                  birthDate: null,
                  citizenshipCountryCode: null,
                  emailAddress: null,
                  gender: null,
                  permanentResidenceCountryCode: null,
                  phoneNumber: null,
                  relationship: null,
                  mailAddress: null,
                  usAddress: null,
                  printForm: true,
                  personId: 10,
                  participantId: 20,
                  isTravelingWithParticipant: true,
                  isDeleted: false
                  )
        {

        }

        public bool IgnoreValidation { get; set; }

        public override RequestId GetRequestId()
        {
            return new RequestId(this.PersonId, RequestIdType.Dependent, RequestActionType.Create);
        }

        public override object GetSevisExhangeVisitorDependentInstance()
        {
            return new SEVISEVBatchTypeExchangeVisitorDependent
            {
                Item = null,
                UserDefinedA = null,
                UserDefinedB = null
            };
        }

        public override bool IgnoreDependentValidation()
        {
            return IgnoreValidation;
        }
    }
}
