from commons.dll_utils import get_wrapper
from commons.error_codes import STATUS_CODE, resolve_error_code

@when(u'agent receives message with subject {subject} and sender {sender}')
def step_impl(context,subject, sender):
    parser = get_wrapper(context.account.login, context.account.password)
    result = ""
    context.result_code = resolve_error_code(parser.ParseALLMessage(subject, sender, result)[0])
    context.result = result