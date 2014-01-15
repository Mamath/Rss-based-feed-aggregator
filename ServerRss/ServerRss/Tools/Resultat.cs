using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ServerRss.Tools
{
    public class Resultat
    {

        public enum ErrorCode
        {
            SUCCESS = 0x0,
            USER_NOT_FOUND,
            USER_ALREADY_EXIST,
            INFORMATION_REQUIRED,
            NOT_LOGUED,
            INTERNAL_ERROR,
            NEED_PRIVILEGE,
            ALREADY_LOGUED,
            CANNOT_CREATE_FEED,
            CANNOT_GET_FEED,
            ITEM_NOT_FOUND,
            PARAMETER_ERROR,
            INVALID_PARAMETER,
        }
        public ErrorCode _error { get; set; }

        public Resultat()
        {
            _error = ErrorCode.SUCCESS;
        }

        public Resultat(ErrorCode error)
        {
            this._error = error;
        }
    }

    public class Resultat<T> : Resultat
    {
        public T _val { get; set; }

        public Resultat()
            : base()
        {

        }

        public Resultat(ErrorCode error)
            : base(error)
        {

        }

        public Resultat(T val)
            : base()
        {
            this._val = val;
        }

        public Resultat(T val, ErrorCode err)
            : base(err)
        {
            this._val = val;
        }
    }

    public class Resultat<T1, T2> : Resultat
    {
        public T1 _val1 { get; set; }
        public T2 _val2 { get; set; }

        public Resultat()
            : base()
        {

        }

        public Resultat(ErrorCode error)
            : base(error)
        {

        }

        public Resultat(T1 val1, T2 val2)
            : base()
        {
            this._val1 = val1;
            this._val2 = val2;
        }

        public Resultat(T1 val1, T2 val2, ErrorCode err)
            : base(err)
        {
            this._val1 = val1;
            this._val2 = val2;
        }
    }
}