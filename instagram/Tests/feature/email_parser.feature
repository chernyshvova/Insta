Feature: Email Agent

    Scenario: Email Agent receives message by empty account
    Given account with tag empty
    When agent receives message with subject testSubject and sender sender
    Then agent returns error E_DATA_NOT_FOUND

    Scenario: Email Agent receives message by invalid account
    Given account with tag invalid
    When agent receives message with subject testSubject and sender sender
    Then agent returns error E_INVALID_EMAIL

    Scenario: Email Agent receives message by invalid mail provider
    Given account with tag invalid_provider
    When agent receives message with subject testSubject and sender chernyshvova@gmail.com
    Then agent returns error E_EMAIL_PROVIDER_NOT_FOUND

    Scenario: Email Agent receives message and parse instagramm challenge
    Given account with tag gmail
    When agent receives message with subject testSubject and sender chernyshvova@gmail.com
    Then agent returns error S_OK
    