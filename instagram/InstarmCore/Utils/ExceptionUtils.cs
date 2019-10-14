using System;

namespace InstarmCore.Utils
{
    public static class ExeptionUtils
    {
        public static Error currentState = Error.UNKNOWN;
        public static string ErrorMessage { get; set; } = "";

        public static void SetState(Error state)
        {
            currentState = state;
            if (state == Error.S_OK)
            {
                ErrorMessage = "";
            }
        }
        public static void SetState(Error state, string message)
        {
            currentState = state;
            ErrorMessage = message;
            if (state == Error.S_OK)
            {
                ErrorMessage = "";
            }
        }

        public static int GetCode()
        {
            return Convert.ToInt32(ExeptionUtils.GetState());
        }

        public static void Throw(string message)
        {
            throw new BaseException(message);
        }
        public static void Throw(Error state, string message)
        {
            SetState(state, message);
            throw new BaseException(message);
        }

        public static Enum GetState() { return currentState; }
        

    }

    public enum Error
    {
    UNKNOWN = 998,
    DEFAULT = 999,
    S_OK = 0,
    E_DATA_NOT_FOUND = 1,
    E_EMAIL_PROVIDER_NOT_FOUND = 2,
    E_INVALID_EMAIL = 3,

    E_DB_CONNECTION = 4, //связь с бд
    E_DB_DATA_NOT_FOUND = 5, //нет данных в бд
    E_DB_READING = 6, // при чтении
    E_DB_WRITING = 7, // при записи 
    E_DB_CREATING = 8, // при создании бд
    E_DB_EXECUTING = 9, // при исполнении sql запроса

    E_ACC_MEDIA_URL = 29,
    E_ACC_SIGNIN = 30,
    E_ACC_AVATAR = 31,
    E_ACC_SETPOST = 32,
    E_ACC_COMMENT = 33,
    E_ACC_LIKE = 34, 
    E_ACC_FOLLOW = 35,
    E_ACC_DIRECT = 36,
    E_CHALLENGE = 40,
           

    }
}
