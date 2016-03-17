﻿using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis.ErrorPaths;
using FluentValidation;
using System;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// A FullName validator is used to validate a sevis exchange visitor's name.
    /// </summary>
    public class FullNameValidator : AbstractValidator<FullName>
    {
        /// <summary>
        /// The max length of the first name.
        /// </summary>
        public const int FIRST_NAME_MAX_LENGTH = 80;

        /// <summary>
        /// The max length of the last name.
        /// </summary>
        public const int LAST_NAME_MAX_LENGTH = 40;

        /// <summary>
        /// The max length of the passport name.
        /// </summary>
        public const int PASSPORT_NAME_MAX_LENGTH = 39;

        /// <summary>
        /// The max length of the preferred or alias name.
        /// </summary>
        public const int PREFERRED_NAME_MAX_LENGTH = 145;

        /// <summary>
        /// The error message to return when a person's first name is to long.
        /// </summary>
        public static string FIRST_NAME_ERROR_MESSAGE = string.Format("The person's first name can be up to {0} characters.", FIRST_NAME_MAX_LENGTH);

        /// <summary>
        /// The error message to return when a person's last name is to long.
        /// </summary>
        public static string LAST_NAME_ERROR_MESSAGE = string.Format("The person's last name can be up to {0} characters.", LAST_NAME_MAX_LENGTH);

        /// <summary>
        /// The error message to return when a person's passport name is to long.
        /// </summary>
        public static string PASSPORT_NAME_ERROR_MESSAGE = string.Format("The person's passport name can be up to {0} characters.", PASSPORT_NAME_MAX_LENGTH);

        /// <summary>
        /// The error message to return when a person's preferred name is to long.
        /// </summary>

        public static string PREFFERED_NAME_ERROR_MESSAGE = string.Format("Full Name: Preferred Name can be up to {0} characters.", PREFERRED_NAME_MAX_LENGTH);

        /// <summary>
        /// The sevis junior suffix.
        /// </summary>
        public static string JUNIOR_SUFFIX = NameSuffixCodeType.Jr.ToString();

        /// <summary>
        /// The sevis senior suffix.
        /// </summary>
        public static string SENIOR_SUFFIX = NameSuffixCodeType.Sr.ToString();

        /// <summary>
        /// The sevis first suffix.
        /// </summary>
        public static string FIRST_SUFFIX = NameSuffixCodeType.I.ToString();

        /// <summary>
        /// The sevis second suffix.
        /// </summary>
        public static string SECOND_SUFFIX = NameSuffixCodeType.II.ToString();

        /// <summary>
        /// The sevis third suffix.
        /// </summary>
        public static string THIRD_SUFFIX = NameSuffixCodeType.III.ToString();

        /// <summary>
        /// The sevis fourth suffix.
        /// </summary>
        public static string FOURTH_SUFFIX = NameSuffixCodeType.IV.ToString();

        /// <summary>
        /// The possible suffix values to match.
        /// </summary>
        public static string SUFFIX_MATCHES_STRING = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", JUNIOR_SUFFIX, SENIOR_SUFFIX, FIRST_SUFFIX, SECOND_SUFFIX, THIRD_SUFFIX, FOURTH_SUFFIX);

        /// <summary>
        /// The error message to format when a suffix is not a valid value.
        /// </summary>
        public const string SUFFIX_VALUE_ERROR_MESSAGE = "The person's name suffix '{0}' is invalid and must be one of the following values:  '{1}'.";

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public FullNameValidator()
        {
            RuleFor(visitor => visitor.FirstName)
                .Length(1, FIRST_NAME_MAX_LENGTH)
                .WithMessage(FIRST_NAME_ERROR_MESSAGE)
                .WithState(x => new FullNameErrorPath());

            RuleFor(visitor => visitor.LastName)
                .NotNull()
                .WithMessage(LAST_NAME_ERROR_MESSAGE)
                .WithState(x => new FullNameErrorPath())
                .Length(1, LAST_NAME_MAX_LENGTH)
                .WithMessage(LAST_NAME_ERROR_MESSAGE)
                .WithState(x => new FullNameErrorPath());

            Func<FullName, object> getValidSuffixValues = (o) =>
            {
                return String.Join(JUNIOR_SUFFIX, SENIOR_SUFFIX, FIRST_SUFFIX, SECOND_SUFFIX, THIRD_SUFFIX, FOURTH_SUFFIX);
            };

            Func<FullName, object> getNameSuffixValue = (n) =>
            {
                return n.Suffix;
            };

            RuleFor(visitor => visitor.Suffix)
                .Matches(SUFFIX_MATCHES_STRING)
                .WithMessage(SUFFIX_VALUE_ERROR_MESSAGE, getNameSuffixValue, getValidSuffixValues)
                .WithState(x => new FullNameErrorPath());

            RuleFor(visitor => visitor.PassportName)
                .Length(0, PASSPORT_NAME_MAX_LENGTH)
                .WithMessage(PASSPORT_NAME_ERROR_MESSAGE)
                .WithState(x => new FullNameErrorPath());

            RuleFor(visitor => visitor.PreferredName)
                .Length(0, PREFERRED_NAME_MAX_LENGTH)
                .WithMessage(PREFFERED_NAME_ERROR_MESSAGE)
                .WithState(x => new FullNameErrorPath());
        }
    }
}