Feature: Email Agent

    Scenario: Email Agent receives message by subject
    Given account with tag general
    When agent receives message with subject testSubject
    Then agent returns message testMessage