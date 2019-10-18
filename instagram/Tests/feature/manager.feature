Feature: Testing account API

    Scenario: Sign in
    Given account with tag i_valid_account
    When account trying to singin
    Then account returns error UNKNOWN

    Scenario: Like post
    Given account with tag i_valid_account
    When account trying to like post 
    Then account returns error UNKNOWN