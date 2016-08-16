Feature: General
	In order to ovoid releasing broken apps
	As a developer
	I want to be be able to unit test my app

Scenario: Get the test value
	Given I am on the main view
	Then I can see a Label with text ""
	When I click on the button
	Then I can see a Label with text "TestValue"
