Feature: Smoke validation for Recru

    Background: login validation
        Given "Admin" is provided with Recru "https://webhrappqa.azurewebsites.net/#/login"
        When "Admin" should see Recru Logo
        And "Admin" enters UserName "kevin.zatel@softvision.com" and Password "99dcd4465a6017371ce6c231db4ad223" and clicks on login button
        Then "Admin" should check the displayed "kevin.zatel@softvision.com" in HomePage

    Scenario Outline: Invalid login validation for Role : "<Role>"
        Given "<Role>" is provided with the URL "https://webhrappqa.azurewebsites.net/#/login"
        When "<Role>" should see Recru Logo
        And "<Role>" enters UserName "<username>" and Password "<password>" and clicks on login button
        Then "<Role>" validates the error message for "<context>"

        Examples:
            | username                   | password                         | context        | Role  |
            | kevin.zatel@softvision.com | 31e902722fb200e8f9ddc9b591bf85dd | Wrong Password | Admin |
            | kevin                      | 99dcd4465a6017371ce6c231db4ad223 | Wrong Username | Admin |
            |                            | 99dcd4465a6017371ce6c231db4ad223 | No Username    | Admin |
            | kevin.zatel@softvision.com |                                  | No Password    | Admin |

    Scenario Outline: Validate the Apps in app navigation bar for Role : "<Role>"
        Given "<Role>" is present in Recru homepage
        Then "<Role>" validates the Apps in app navigation bar
            | Apps      |
            | Dashboard |
            | Processes |
            | People    |
            | Tasks     |
            | Days off  |
            | Referrals |
            | Reports   |
            | Settings  |
        Examples:
            | Role  |
            | Admin |

    Scenario Outline: Validate page attributes in Dashboard page for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Dashboard" in App navigator
        Then "<Role>" validates page attributes in "Dashboard" page
        Examples:
            | Role  |
            | Admin |

    Scenario Outline: Validate page attributes in Processes page for Role:"<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Processes" in App navigator
        Then "<Role>" validates page attributes in "Processes" page
        Examples:
            | Role  |
            | Admin |

    Scenario Outline: Validate grid headers in "<Tab>" tab for Role:"<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Processes" in App navigator
        Then "<Role>" validate grid headers in "<Tab>" tab
            | Headers       |
            | Candidate     |
            | Profile       |
            | Community     |
            | Status        |
            | Current Stage |
            | Recruiter     |
            | Actions       |
        Examples:
            | Role  | Tab          |
            | Admin | My Processes |
            | Admin | BA Studio    |

    Scenario Outline: Validate page attributes in "<Stage>" stage - New candidate for Role | "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Processes" in App navigator
        And "<Role>" click on new candidate button
        Then "<Role>" validate page attributes in "<Stage>" stage
        Examples:
            | Role  | Stage     |
            | Admin | HR        |
            | Admin | Technical |
            | Admin | Client    |
            | Admin | PreOffer  |
            | Admin | Offer     |

    Scenario Outline: Validate page attributes in "<App>" page for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "<App>" in App navigator
        Then "<Role>" validates page attributes in "<App>" page
        Examples:
            | Role  | App    |
            | Admin | People |

    Scenario Outline: Validate page attributes in "<App>" page for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "<App>" in App navigator
        Then "<Role>" validates page attributes in "<App>" page
        Examples:
            | Role  | App     |
            | Admin | DaysOff |

    Scenario Outline: Validate attributes in Add New Day Off popup for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "<App>" in App navigator
        And "<Role>" clicks on "Add new daysOff" button
        Then "<Role>" validate attributes in "Add new daysOff" popup
        Examples:
            | Role  | App     |
            | Admin | DaysOff |

    Scenario Outline: Validate page attributes in "<App>" page for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "<App>" in App navigator
        Then "<Role>" validates page attributes in "<App>" page
        Examples:
            | Role  | App   |
            | Admin | Tasks |

    Scenario Outline: Validate attributes in New Tasks popup for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "<App>" in App navigator
        And "<Role>" clicks on "New Tasks" button
        Then "<Role>" validate attributes in "New Tasks" popup
        Examples:
            | Role  | App   |
            | Admin | Tasks |

    Scenario Outline: Validate page attributes in "<App>" page for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "<App>" in App navigator
        Then "<Role>" validates page attributes in "<App>" page
        Examples:
            | Role  | App     |
            | Admin | Reports |

    Scenario Outline: Validate page attributes in <Section> section for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "<App>" in App navigator
        And "<Role>" clicks on "<Section>"
        Then "<Role>" validates page attributes in "<Section>" section
        Examples:
            | Role  | App     | Section          |
            | Admin | Reports | Candidate Filter |
            | Admin | Reports | Processes report |
            | Admin | Reports | Skills Report    |

    Scenario Outline: Validate page attributes in "<App>" page for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "<App>" in App navigator
        Then "<Role>" validates page attributes in "<App>" page
        Examples:
            | Role  | App       |
            | Admin | Referrals |

    Scenario Outline: Validate grid headers in "<Tab>" tab for Role:"<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Referrals" in App navigator
        Then "<Role>" validate grid headers in "<Tab>" section
            | Headers        |
            | Candidate      |
            | Source         |
            | Referral Date  |
            | Community      |
            | Current Stage  |
            | Position Title |
            | Actions        |
        Examples:
            | Role  | Tab       |
            | Admin | REFERRALS |

    Scenario Outline: Validate grid headers in "<Tab>" tab for Role:"<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Referrals" in App navigator
        Then "<Role>" validate grid headers in "<Tab>" section
            | Headers     |
            | Title       |
            | Seniority   |
            | Studio      |
            | Community   |
            | Mark as HOT |
            | Actions     |
        Examples:
            | Role  | Tab            |
            | Admin | OPEN POSITIONS |

    Scenario Outline: validate attributes in Add Position popup for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Referrals" in App navigator
        And "<Role>" click on "OPEN POSITIONS" section
        And "<Role>" clicks on "<Button>" button
        Then "<Role>" validates the attributes in the "<Button>" popup
        Examples:
            | Role  | Tab            | Button        |
            | Admin | OPEN POSITIONS | Add Position  |
            | Admin | OPEN POSITIONS | Edit Position |

    Scenario Outline: Validate attributes in Settings for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Settings" in App navigator
        Then "<Role>" validates attributes and the submenu
            | SubMenu                  |
            | Hire Project Data        |
            | Employee Casualties Data |
            | Skills List              |
            | Skill Types              |
            | Candidate Profiles       |
            | Communities              |
            | Offices                  |
            | Rooms                    |
            | Roles                    |
            | Festivities Reminders    |
            | Declining Reasons        |
            | Reasons                  |
            | Reasons categories       |
        Examples:
            | Role  |
            | Admin |

    Scenario Outline: Validate page attributes in "<SubMenu>" for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Settings" in App navigator
        And "<Role>" click on "<SubMenu>" submenu
        Then "<Role>" validate attributes and grid headers in "<SubMenu>"
            | gridHeaders |
            | Value       |
            | Month       |
            | Year        |
            | Action      |
        Examples:
            | Role  | SubMenu           |
            | Admin | Hire Project Data |

    Scenario Outline: Validate attributes in "<Popup>" in "<SubMenu>" for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Settings" in App navigator
        And "<Role>" click on "<SubMenu>" submenu
        And "<Role>" click on "<Popup>" button in "<SubMenu>"
        Then "<Role>" validate attributes in "<Popup>" pop up
            | gridHeaders |
            | Value       |
            | Month       |
            | Year        |
            | Action      |
        Examples:
            | Role  | SubMenu            | Popup                     |
            | Admin | Hire Project Data  | Add new projection        |
            | Admin | Candidate Profiles | Add new Candidate Profile |
            | Admin | Roles              | Add new Role              |

    Scenario Outline: Validate page attributes in "<SubMenu>" submenu for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Settings" in App navigator
        And "<Role>" click on "<SubMenu>" submenu
        Then "<Role>" validate attributes and grid headers in "<SubMenu>"
            | gridHeaders |
            | Name        |
            | Description |
            | Action      |
        Examples:
            | Role  | SubMenu            |
            | Admin | Candidate Profiles |

    Scenario Outline: Validate page attributes in "<SubMenu>" submenu for Role: "<Role>"
        Given "<Role>" is present in Recru homepage
        When "<Role>" clicks on "Settings" in App navigator
        And "<Role>" click on "<SubMenu>" submenu
        Then "<Role>" validate attributes and grid headers in "<SubMenu>"
            | gridHeaders |
            | Name        |
            | Status      |
            | Action      |
        Examples:
            | Role  | SubMenu |
            | Admin | Roles   |

