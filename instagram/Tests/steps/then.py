from commons.error_codes import resolve_error_name


@then(u'agent returns error {error}')
def step_impl(context, error):
    current_error_code = resolve_error_name(error)
    assert context.result_code == current_error_code, "expected code {} and result {} are not equals".format(current_error_code, context.result_code)
	
@then(u'account returns error {error}')
def step_impl(context, error):
    current_error_code = resolve_error_name(error)
    assert context.result_code == current_error_code, "expected code {} and result {} are not equals".format(current_error_code, context.result_code)