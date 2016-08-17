Feature: General
	In order to ovoid releasing broken apps
	As a developer
	I want to be be able to unit test my app

Scenario: Get the test value
	Given I am on the main page
	Then I can see a Label with text ""
	When I click on the text button
	Then I can see a Label with text "TestValue"
	When I click on the goforward button
	Then I am redirected to the page "Another"
	When I click on the goback button
	Then I am redirected to the page "Main"