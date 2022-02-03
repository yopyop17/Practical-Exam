using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Exam_Project.Api.Models
{
    public class Response
    {
        public Response()
        {
            this.watch = new Stopwatch();
            this.watch.Start();
        }

        private Stopwatch watch;
        private bool success;

        /// <summary>
        /// Duration of response in seconds
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Whether the operation completed successfully.
        /// </summary>
        public bool Success
        {
            get { return this.success; }
            set
            {
                this.success = value;
                if (this.watch != null)
                {
                    this.watch.Stop();
                    Duration = watch.ElapsedMilliseconds / 1000d;
                }
            }
        }

        /// <summary>
        /// HTTP response code (4xx/5xx) if applicable.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Resource key of error message.
        /// </summary>
        public string ErrorKey { get; set; }

        /// <summary>
        /// Checks if response has errors.
        /// </summary>
        public bool HasError { get { return !string.IsNullOrEmpty(ErrorKey) || !string.IsNullOrEmpty(ErrorMessage); } }

        private List<ValidationResult> _validationResults = new List<ValidationResult>();
        /// <summary>
        /// List of validation errors.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<ValidationResult> ValidationResults { get { return _validationResults; } }


        /// <summary>
        /// Copies properties from response.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="property"></param>
        public Response Copy(Response response, string property = null)
        {
            CopyState(response);
            return this;
        }

        protected void CopyState(Response response, string property = null)
        {
            this.Success = response.Success;
            this.ErrorMessage = response.ErrorMessage;
            this.ErrorKey = response.ErrorKey;
            foreach (var valError in response.ValidationResults)
            {
                var memberNames = new List<string>();
                foreach (var member in valError.MemberNames)
                    memberNames.Add(string.IsNullOrEmpty(property) ? member : property + "." + member);
                this._validationResults.Add(new ValidationResult(valError.ErrorMessage, memberNames));

            }
        }

        /// <summary>
        /// Sets the response to a failed state.
        /// </summary>
        /// <param name="response"></param>
        public Response Fail(string errorMessage)
        {
            SetFailState(errorMessage);
            return this;
        }

        protected void SetFailState(string errorMessage)
        {
            this.Success = false;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Adds validation error and sets to a failed state.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Response AddValidationError(string property, string message, string errorKey = "")
        {
            SetValidationError(property, message, errorKey);
            return this;
        }

        /// <summary>
        /// Adds validation error and sets to a failed state.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="message"></param>
        /// <param name="errorKey"></param>
        protected void SetValidationError(string property, string message, string errorKey = "")
        {
            this.Success = false;
            this._validationResults.Add(new ValidationResult(message, new string[] { property }));
            if (string.IsNullOrEmpty(this.ErrorMessage))
                this.ErrorMessage = message;
            this.ErrorKey = errorKey;
        }

    }
    public class Result<T>
    {
        /// <summary>
        /// The outout if the operation
        /// </summary>
        [Newtonsoft.Json.JsonProperty("data")]
        public List<T> Data { get; set; } = new List<T>();
        /// <summary>
        /// Total count of data
        /// </summary>
        [Newtonsoft.Json.JsonProperty("count")]
        public long Count { get; set; }
    }

    public class Response<T> : Response
    {
        /// <summary>
        /// The output of the operation.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Copies properties from response.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="property"></param>
        public new Response<T> Copy(Response response, string property = null)
        {
            CopyState(response, property: property);
            return this;
        }

        /// <summary>
        /// Sets the response to a failed state.
        /// </summary>
        /// <param name="response"></param>
        public new Response<T> Fail(string errorMessage)
        {
            SetFailState(errorMessage);
            return this;
        }

        /// <summary>
        /// Adds validation error and sets to a failed state.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Response<T> AddValidationError(string property, string message)
        {
            SetValidationError(property, message);
            return this;
        }

        public Response<T> AddValidationError<R>(Expression<Func<R>> property, string message)
        {
            var n = (property.Body as MemberExpression).Member.Name;
            SetValidationError(n, message, n);
            return this;
        }

        public string GetNameOf<R>(Expression<Func<R>> property)
        {
            return (property.Body as MemberExpression).Member.Name;
        }

        /// <summary>
        /// Adds validation error and sets to a failed state.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="message"></param>
        /// <param name="errorKey"></param>
        /// <returns></returns>
        public Response<T> AddValidationError(string property, string message, string errorKey = "")
        {
            SetValidationError(property, message, errorKey);
            return this;
        }
    }

    /// <summary>
    /// The validation error details for a given property
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// The property that failed validation
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Message for developer
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Message for end user
        /// </summary>
        public string FriendlyMessage { get; set; }
    }
}
