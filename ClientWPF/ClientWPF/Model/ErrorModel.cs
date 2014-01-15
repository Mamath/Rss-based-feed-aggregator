using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using ClientWPF.ViewModel;
using ClientWPF.AccountService;
using System.ComponentModel;
//using ClientWPF.FeedService;
using System.IO;

namespace ClientWPF.Model
{
    class ErrorModel : BindableObject
    {
        static private ErrorModel instance = new ErrorModel();
        static public ErrorModel Instance { get { return instance; } }

        public Resultat.ErrorCode Error { get; set; }
        public string ErrorText { get; set; }

        private ErrorModel()
        {
            Error = Resultat.ErrorCode.SUCCESS;
            ErrorText = null;
        }

        public bool EvalResponse(AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
                return true;
            Error = Resultat.ErrorCode.INTERNAL_ERROR;
            ErrorText = e.Error.Message;
            RaisePropertyChange("Error");
            return false;
        }

        public bool EvalWebResult(AccountService.Resultat r)
        {
            if (r._error == Resultat.ErrorCode.SUCCESS)
                return true;
            Error = r._error;
            ErrorText = "WebService Error";
            RaisePropertyChange("Error");
            return false;
        }

        public bool EvalWebResult(FeedService.Resultat r)
        {
            if (r._error == FeedService.Resultat.ErrorCode.SUCCESS)
                return true;
            Error = (Resultat.ErrorCode)r._error;
            ErrorText = "WebService Error";
            RaisePropertyChange("Error");
            return false;
        }
    }
}
