using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFPGeneric
{
    // Summary:
    //     Represents the result of an operation
    public class Result
    {
        private Boolean _success;
        private List<Exception> _errors;

        // Summary:
        //     Parameterless Constructor that defaults to a false (failure result)
        //
        // Parameters:
        //   success:
        private Result()
            : this(false)
        {
        }

        //
        // Summary:
        //     Failure constructor that takes errors
        //
        // Parameters:
        //   errors:
        public Result(params Exception[] Errors)
            : this(false, Errors)
        {
        }

        //
        // Summary:
        //     Failure constructor that takes errors
        //
        // Parameters:
        //   errors:
        public Result(List<Exception> Errors)
            : this(false, Errors)
        {
        }

        //
        // Summary:
        //     Failure constructor that takes error messages
        //
        // Parameters:
        //   ErrorMessages:
        public Result(params String[] ErrorMessages)
            : this(ErrorMessages.ToList<String>())
        {
        }

        //
        // Summary:
        //     Failure constructor that takes error messages
        //
        // Parameters:
        //   ErrorMessages:
        public Result(List<String> ErrorMessages)
            : this(false, new List<Exception>())
        {
            foreach (var errMsg in ErrorMessages)
                _errors.Add(new Exception(errMsg));
        }

        // Summary:
        //     Constructor that takes whether the result is successful
        //
        // Parameters:
        //   success: (default is false)
        private Result(bool Success)
            : this(Success, new List<Exception>())
        {
        }

        public Result(Boolean Success, params Exception[] Errors)
        {
            _success = Success;
            _errors = Errors.ToList<Exception>();
        }

        public Result(Boolean Success, List<Exception> Errors)
        {
            _success = Success;
            _errors = Errors;
        }

        public static Result Uninitialized
        {
            get { return new Result("Uninitialized"); }
        }

        //
        // Summary:
        //     True if the operation was successful
        public Boolean Success
        {
            get { return _success; }
        }

        //
        // Summary:
        //     True if the operation succeeded
        public Boolean Succeeded
        {
            get { return Success; }
        }

        //
        // Summary:
        //     True if the operation was a failure
        public Boolean Failure
        {
            get { return !_success; }
        }

        //
        // Summary:
        //     True if the operation failed
        public Boolean Failed
        {
            get { return Failure; }
        }

        //
        // Summary:
        //     True if the result has errors
        public Boolean HasErrors
        {
            get
            {
                if (_errors.Count < 1) { return false; }
                else { return true; }
            }
        }

        //
        // Summary:
        //     True if the result has errorMessages
        public Boolean HasErrorMessages
        {
            get { return HasErrors; }
        }

        // Summary:
        //     List of errors
        public List<Exception> Errors
        {
            get { return _errors; }
        }

        // Summary:
        //     List of errorMessages
        public List<String> ErrorMessages
        {
            get
            {
                var errMsgs = new List<string>();
                foreach (var excp in _errors) { errMsgs.Add(excp.Message); }
                return errMsgs;
            }
        }

        // Summary:
        //     First of errors
        public Exception Error
        {
            get
            {
                if (_errors.Count >= 1) { return _errors[0]; }
                else { return new Exception("No error exists"); }
            }
        }

        // Summary:
        //     First of errorMessages
        public String ErrorMessage
        {
            get
            {
                if (_errors.Count >= 1) { return _errors[0].Message; }
                else { return "No error message exists"; }
            }
        }

        // Summary:
        //     Adds error to errors
        public void AddError(Exception Error)
        {
            if (_errors == null) { _errors = new List<Exception>(); }

            _errors.Add(Error);
        }

        // Summary:
        //     Adds errorMessage to errorMessages
        public void AddErrorMessage(String ErrorMessage)
        {
            AddError(new Exception(ErrorMessage));
        }

        // Summary:
        //     Adds errors to errors
        public void AddErrors(List<Exception> Errors)
        {
            if (_errors == null) { _errors = new List<Exception>(); }

            _errors.AddRange(Errors);
        }

        // Summary:
        //     Adds errors to errors
        public void AddErrors(params Exception[] Errors)
        {
            AddErrors(Errors.ToList<Exception>());
        }

        // Summary:
        //     Adds errorMessages to errorMessages
        public void AddErrorMessages(List<String> ErrorMessages)
        {
            var errs = new List<Exception>();
            foreach (var errMsg in ErrorMessages)
                errs.Add(new Exception(errMsg));

            AddErrors(errs);
        }

        // Summary:
        //     Adds errorMessages to errorMessages
        public void AddErrorMessages(params String[] ErrorMessages)
        {
            AddErrorMessages(ErrorMessages.ToList());
        }
    }
}
