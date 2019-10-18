from commons.dll_utils import get_wrapper, get_insta_wrapper
from commons.error_codes import STATUS_CODE, resolve_error_code
from commons.const_contract import Contract

@when(u'agent receives message with subject {subject} and sender {sender}')
def step_impl(context,subject, sender):
    parser = get_wrapper("CommonTools.dll")
    result = ""
    context.result_code = resolve_error_code(parser.ParseALLMessage(context.account.login,context.account.password, subject, sender, result)[0])
    context.result = result


@when(u'account recive accountname')
def step_impl(context):
    parser = get_insta_wrapper("InstarmCore.dll")
    result = ""
    context.result_code = resolve_error_code(parser.GetAccount(context.account.login, result)[0])
    context.result = result

@when(u'account trying to like post')
def step_impl(context):
    parser = get_insta_wrapper("InstarmCore.dll")
    result = ""
    context.result_code = resolve_error_code(parser.LikeMedia(context.account.login, "http://instagram.com/media_blabla" ,result)[0])
    context.result = result

    
@when(u'account trying to singin')
def step_impl(context):
    parser = get_insta_wrapper("InstarmCore.dll")
    result = ""
    context.result_code = resolve_error_code(parser.SingIn(context.account.login, result)[0])
    context.result = result