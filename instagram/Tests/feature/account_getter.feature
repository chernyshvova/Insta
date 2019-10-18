Feature: Getting account from database

    Scenario: Agent trying get NOT existing account
    Given account with tag i_not_existing_invalid
    When account recive accountname
    Then account returns error E_DB_DATA_NOT_FOUND

    Scenario: Agent trying get existing but invalid account
    Given account with tag i_existing_invalid
    When account recive accountname
    Then account returns error UNKNOWN





