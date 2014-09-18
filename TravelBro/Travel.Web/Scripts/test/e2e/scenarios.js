'use strict';

/* http://docs.angularjs.org/guide/dev_guide.e2e-testing */

describe('TravelBro App', function() {

    // utils
    function CheckUrl(expectedUrl) {
        browser.getLocationAbsUrl().then(function (url) {
            expect(url.split('#')[1]).toBe(expectedUrl);
        });
    }

    function CloseAlert(type) {
        var btn = element.all(by.css('div.alert-' + type + '.alert-dismissable button'));
        expect(btn.count()).toBeGreaterThan(0);
        btn.first().click();
    }

    it('should maximize browser window', function() {
        browser.driver.manage().window().maximize();
    });
    
    it('should redirect #/trips', function() {
        browser.get('/');
        CheckUrl('/trips');
    });
    
    var userName = 'protractorUser' + (new Date).valueOf() + '@test.com';
    var userPwd = 'Password.123';
    describe('Sign Up', function () {
        var driver = protractor.WebDriver;

        it('should navigate to SignUp page', function() {
            browser.get('#/sign-up');
            CheckUrl('/sign-up');
        });
        
        it('should sign up', function() {
            element(by.model('mail')).sendKeys(userName);
            element(by.model('password')).sendKeys(userPwd);
            element(by.model('confirmPassword')).sendKeys(userPwd);

            driver.call(function () {
                alert('message is abc?');
            });

            var regBtn = element(by.partialButtonText('Register'));
            expect(regBtn.isPresent());
            regBtn.click();
            browser.waitForAngular();
            CheckUrl('/trips');
        });
    });

    var suffix = (new Date).valueOf();
    var newTripName = 'protractor_trip_' + suffix;
    var newTripCount = 2;
    describe('Create, filter and remove trips', function () {
        
        function createTrip(name, dateFrom, dateTo, desc, isPrivate) {
            browser.get('#/trips/new');

            element(by.model('trip.Name')).sendKeys(name);
            element(by.model('trip.DateFrom')).sendKeys(dateFrom);
            element(by.model('trip.DateFrom')).sendKeys(dateTo);
            element(by.model('trip.Description')).sendKeys(desc);

            var saveBtn = element(by.partialButtonText('Save'));
            expect(saveBtn.isPresent());
            saveBtn.click();
            CloseAlert('info');

            browser.waitForAngular();
            CheckUrl('/trips');
        }

        it('should create a trip named test+id', function() {
            createTrip(
                'test' + suffix,
                '08/12/2013',
                '09/16/2014',
                'Protractor created trip.',
                true);
        });
        var currIndex = 0;
        for (var i = 0; i < newTripCount; i++) {
            it('should create a trip', function () {
                createTrip(
                    newTripName + '-' + currIndex++,
                    '08/12/2013',
                    '09/16/2014',
                    'Protractor created trip.',
                    false);
            });
        }
        
        it('should filter trips', function() {
            CheckUrl('/trips');

            var trips = element.all(by.repeater('trip in trips'));

            expect(trips.count()).toBeGreaterThan(newTripCount);

            //trips = element.all(by.repeater('trip in trips'));
            element(by.model('headerFilter')).sendKeys(newTripName);
            expect(trips.count()).toEqual(newTripCount);

            //trips = element.all(by.repeater('trip in trips'));
            element(by.model('headerFilter')).clear();
            element(by.model('headerFilter')).sendKeys(newTripName + '-0');
            expect(trips.count()).toEqual(1);

            element(by.model('headerFilter')).clear();
        });

        it('should delete created trips', function () {
            var delTrip = function (name) {
                element(by.model('headerFilter')).clear();
                element(by.model('headerFilter')).sendKeys(name);

                element(by.partialLinkText(name)).click();

                var delBtn = element(by.partialButtonText('delete'));
                expect(delBtn.isPresent());
                delBtn.click();
                CloseAlert('info');
            }

            delTrip('test' + suffix);
            for (var i = 0; i < newTripCount; i++) {
                delTrip(newTripName + '-' + i);
            }
            element(by.model('headerFilter')).clear();
        });
    });
});
