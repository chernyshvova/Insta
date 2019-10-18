from commons.account import Account

@given(u'account with tag {tag}')
def step_impl(context, tag):
    context.account = Account(tag, "config.json")
