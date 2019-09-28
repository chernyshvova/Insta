from enum import Enum

class STATUS_CODE(Enum):
    S_OK = 0
    E_DATA_NOT_FOUND = 1
    E_EMAIL_PROVIDER_NOT_FOUND = 2
    E_INVALID_EMAIL = 3

def resolve_error_name(error_name):
    if error_name == "S_OK":
        return STATUS_CODE.S_OK
    if error_name == "E_DATA_NOT_FOUND":
        return STATUS_CODE.E_DATA_NOT_FOUND
    if error_name == "E_EMAIL_PROVIDER_NOT_FOUND":
        return STATUS_CODE.E_EMAIL_PROVIDER_NOT_FOUND
    if error_name == "E_INVALID_EMAIL":
        return STATUS_CODE.E_INVALID_EMAIL
    
    print("error {} is not handled".format(error_name))
    return error_name

def resolve_error_code(code):
    if code == STATUS_CODE.S_OK.value:
        return STATUS_CODE.S_OK
    if code == STATUS_CODE.E_DATA_NOT_FOUND.value:
        return STATUS_CODE.E_DATA_NOT_FOUND
    if code == STATUS_CODE.E_EMAIL_PROVIDER_NOT_FOUND.value:
        return STATUS_CODE.E_EMAIL_PROVIDER_NOT_FOUND
    if code == STATUS_CODE.E_INVALID_EMAIL.value:
        return STATUS_CODE.E_INVALID_EMAIL
    
    print("error {} is not handled".format(code))
    return code

    